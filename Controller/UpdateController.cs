using AutoUpdaterDotNET;
using log4net;
using NanoTwitchLeafs.Objects;
using NanoTwitchLeafs.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace NanoTwitchLeafs.Controller
{
	public class UpdateController
	{
		private readonly ILog _logger = LogManager.GetLogger(typeof(UpdateController));
		private readonly AppSettings _appSettings;

		private sealed class UpdateCandidate
		{
			public string SourceId { get; init; }
			public string Publisher { get; init; }
			public Version Version { get; init; }
			public GithubReleaseInfo Release { get; init; }
			public Asset DownloadAsset { get; init; }
		}

		public UpdateController(AppSettings appSettings)
		{
			_appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
			AutoUpdater.Synchronous = true;
			AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
			AutoUpdater.ShowRemindLaterButton = true;
			AutoUpdater.ShowSkipButton = false;
			AutoUpdater.RemindLaterAt = 1;
			AutoUpdater.UpdateFormSize = new System.Drawing.Size(1000, 600);
			AutoUpdater.HttpUserAgent = "NanoTwitchLeafs AutoUpdater";
			AutoUpdater.DownloadPath = Constants.TEMP_PATH;
			AutoUpdater.RunUpdateAsAdmin = true;
			AutoUpdater.ReportErrors = true;
			AutoUpdater.InstallationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if !DEBUG
			CheckForUpdates();
#endif
		}

		public async void CheckForUpdates()
		{
			try
			{
				Version currentVersion = typeof(AppInfoWindow).Assembly.GetName().Version;
				List<Task<UpdateCandidate>> checks = new List<Task<UpdateCandidate>>();

				if (_appSettings.UpdateSource != Constants.UPDATE_SOURCE_ORIGINAL)
				{
					checks.Add(GetUpdateCandidate(Constants.UPDATE_SOURCE_NTL4, "GordenM82 / NTL 4",
						Constants.NTL4_GITHUB_OWNER, Constants.NTL4_GITHUB_REPO));
				}
				if (_appSettings.UpdateSource != Constants.UPDATE_SOURCE_NTL4)
				{
					checks.Add(GetUpdateCandidate(Constants.UPDATE_SOURCE_ORIGINAL, "Locxion / Original",
						Constants.ORIGINAL_GITHUB_OWNER, Constants.ORIGINAL_GITHUB_REPO));
				}

				UpdateCandidate[] results = await Task.WhenAll(checks);
				List<UpdateCandidate> available = results
					.Where(candidate => candidate != null && candidate.Version > currentVersion)
					.OrderByDescending(candidate => candidate.Version)
					.ToList();

				if (available.Count == 0)
				{
					_logger.Info("No update available from the selected update source(s).");
					return;
				}

				UpdateCandidate selected = available[0];
				if (available.Count > 1)
				{
					selected = SelectCandidate(available[0], available[1]);
					if (selected == null) return;
				}

				if (selected.SourceId == Constants.UPDATE_SOURCE_ORIGINAL)
				{
					string switchText = string.Format(Text("P415_Update_SwitchWarning"),
						selected.Publisher, FormatVersion(selected.Version));
					if (MessageBox.Show(switchText, Text("P415_Update_Title"), MessageBoxButton.YesNo,
						MessageBoxImage.Warning) != MessageBoxResult.Yes)
					{
						return;
					}
				}

				ShowUpdate(selected, currentVersion);
			}
			catch (Exception ex)
			{
				_logger.Error("Could not check for updates.", ex);
			}
		}

		private UpdateCandidate SelectCandidate(UpdateCandidate first, UpdateCandidate second)
		{
			string message = string.Format(Text("P415_Update_Choose"),
				first.Publisher, FormatVersion(first.Version), second.Publisher, FormatVersion(second.Version));
			MessageBoxResult result = MessageBox.Show(message, Text("P415_Update_Title"),
				MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
			return result switch
			{
				MessageBoxResult.Yes => first,
				MessageBoxResult.No => second,
				_ => null
			};
		}

		private void ShowUpdate(UpdateCandidate candidate, Version currentVersion)
		{
			var args = new UpdateInfoEventArgs
			{
				CurrentVersion = FormatVersion(candidate.Version),
				ChangelogURL = candidate.Release.html_url,
				DownloadURL = candidate.DownloadAsset.browser_download_url,
				IsUpdateAvailable = true,
				InstalledVersion = currentVersion,
			};
			_logger.Info($"Update found from {candidate.Publisher}: v{args.CurrentVersion}.");
			AutoUpdater.ShowUpdateForm(args);
		}

		private async Task<UpdateCandidate> GetUpdateCandidate(string sourceId, string publisher,
			string owner, string repository)
		{
			string githubUrl = $"https://api.github.com/repos/{owner}/{repository}/releases/latest";
			_logger.Debug($"Getting GitHub release info from {owner}/{repository}.");
			try
			{
				using var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("NanoTwitchLeafs-AutoUpdater/4.1.5");
				var response = await httpClient.GetAsync(githubUrl);
				if (response.StatusCode != HttpStatusCode.OK)
				{
					_logger.Warn($"Could not get release info from {owner}/{repository}: {response.StatusCode}.");
					return null;
				}

				GithubReleaseInfo release = JsonConvert.DeserializeObject<GithubReleaseInfo>(
					await response.Content.ReadAsStringAsync());
				if (release == null || release.draft || release.prerelease ||
					!TryParseVersion(release.tag_name, out Version version))
				{
					_logger.Warn($"Release information from {owner}/{repository} is incomplete or invalid.");
					return null;
				}

				Asset asset = FindWindowsZip(release.assets, version);
				if (asset == null)
				{
					_logger.Warn($"No compatible Windows ZIP found in {owner}/{repository} release {release.tag_name}.");
					return null;
				}

				return new UpdateCandidate
				{
					SourceId = sourceId,
					Publisher = publisher,
					Version = version,
					Release = release,
					DownloadAsset = asset,
				};
			}
			catch (Exception ex)
			{
				_logger.Warn($"Update source {owner}/{repository} could not be checked.", ex);
				return null;
			}
		}

		private static bool TryParseVersion(string tag, out Version version)
		{
			version = null;
			if (string.IsNullOrWhiteSpace(tag)) return false;
			return Version.TryParse(tag.Trim().TrimStart('v', 'V'), out version);
		}

		private static Asset FindWindowsZip(IEnumerable<Asset> assets, Version version)
		{
			if (assets == null) return null;
			string number = FormatVersion(version);
			string[] preferredNames =
			{
				$"NanoTwitchLeafs-{number}-win-x64.zip",
				$"NanoTwitchLeafs-{number}.zip",
			};
			return preferredNames
				.Select(name => assets.FirstOrDefault(asset =>
					string.Equals(asset.name, name, StringComparison.OrdinalIgnoreCase)))
				.FirstOrDefault(asset => asset != null)
				?? assets.FirstOrDefault(asset =>
					asset.name?.Contains(number, StringComparison.OrdinalIgnoreCase) == true &&
					asset.name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));
		}

		private static string FormatVersion(Version version) =>
			version.Build >= 0 ? version.ToString(3) : version.ToString(2);

		private static string Text(string key) => Properties.Resources.ResourceManager.GetString(key) ?? key;
	}
}
