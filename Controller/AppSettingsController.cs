using log4net;
using NanoTwitchLeafs.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NanoTwitchLeafs.Controller
{
    public class AppSettingsController
    {
        private const DataProtectionScope DataProtectionScope = System.Security.Cryptography.DataProtectionScope.CurrentUser;

        private readonly ILog _logger = LogManager.GetLogger(typeof(AppSettingsController));


        /// <summary>
        /// Loads Settings from Settings Path
        /// </summary>
        /// <returns>AppSettings</returns>
        public AppSettings LoadSettings()
        {
#if DEBUG
            if (File.Exists(Constants.DEBUG_SETTINGS_PATH))
            {
                return LoadSettings(Constants.DEBUG_SETTINGS_PATH);
            }

            return new AppSettings();

#else
            if (!File.Exists(Constants.SETTINGS_PATH))
            {
                _logger.Info("No Settings File found ... Load Blank Settings.");
                return new AppSettings();
            }
            else
            {
                _logger.Info("Settings File found ...");
                return LoadSettings(Constants.SETTINGS_PATH);
            }
#endif
        }

        /// <summary>
        /// Saves Settings
        /// </summary>
        /// <param name="appSettings"></param>
        public void SaveSettings(AppSettings appSettings)
        {
            try
            {
                if (appSettings == null) throw new ArgumentNullException(nameof(appSettings));
                Directory.CreateDirectory(Path.GetDirectoryName(
#if DEBUG
                    Constants.DEBUG_SETTINGS_PATH
#else
                    Constants.SETTINGS_PATH
#endif
                ));
#if DEBUG
            File.WriteAllText(Constants.DEBUG_SETTINGS_PATH, JsonConvert.SerializeObject(appSettings, Formatting.Indented));
#else
                string json = JsonConvert.SerializeObject(appSettings);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                byte[] encryptedData = ProtectedData.Protect(bytes, null, DataProtectionScope);
                string base64 = Convert.ToBase64String(encryptedData);
                _logger.Debug("Save Settings to File " + Constants.SETTINGS_PATH);
                File.WriteAllText(Constants.SETTINGS_PATH, base64);
#endif
            }
            catch (Exception ex)
            {
                _logger.Error("Could not write to Settings File!");
                _logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Loads Settings
        /// </summary>
        /// <param name="path"></param>
        /// <returns>AppSettings</returns>
        private AppSettings LoadSettings(string path)
        {
            try
            {
#if DEBUG
            return NormalizeSettings(JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(path)));
#else
                string base64 = File.ReadAllText(path);
                byte[] encryptedData = Convert.FromBase64String(base64);
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope);
                string json = Encoding.UTF8.GetString(decryptedData);
				_logger.Debug("Load Settings from File " + path);

#if NTL4
				// Older NTL versions stored System.Version as a JSON object.
				// Modern Json.NET versions expect a string. The version number is
				// not a user preference, so normalize it to the running application.
                JObject settingsJson = JObject.Parse(json);
                JToken appVersion = settingsJson[nameof(AppSettings.AppVersion)];
                if (appVersion != null && appVersion.Type == JTokenType.Object)
                {
                    settingsJson[nameof(AppSettings.AppVersion)] =
                        typeof(AppSettings).Assembly.GetName().Version.ToString();
                }

                return NormalizeSettings(settingsJson.ToObject<AppSettings>());
#else
				return NormalizeSettings(JsonConvert.DeserializeObject<AppSettings>(json));
#endif
#endif
            }
            catch (Exception ex)
            {
                _logger.Error("Could not Load Settings from File!");
                _logger.Error(ex.Message);
                _logger.Error("Loading blank Settings instead.");
                BackupInvalidSettings(path);
                return new AppSettings();
            }
        }

        internal static AppSettings NormalizeSettings(AppSettings settings)
        {
            settings ??= new AppSettings();
            settings.Responses ??= new Responses();
            settings.BotAuthObject ??= new OAuthObject();
            settings.BroadcasterAuthObject ??= new OAuthObject();
            settings.NanoSettings ??= new NanoSettings();
            settings.NanoSettings.NanoLeafDevices ??= new System.Collections.Generic.List<NanoLeafDevice>();
            settings.NanoSettings.DeviceGroups ??= new System.Collections.Generic.List<NanoleafDeviceGroup>();
            settings.Blacklist ??= new System.Collections.Generic.List<string>();
            settings.StreamlabsInformation ??= new StreamlabsInformation();
            settings.StreamElements ??= new StreamElementsSettings();
            settings.StreamElements.Token ??= string.Empty;
            settings.StreamElements.TokenType = string.IsNullOrWhiteSpace(settings.StreamElements.TokenType) ? "jwt" : settings.StreamElements.TokenType;
            settings.CommandPrefix = string.IsNullOrWhiteSpace(settings.CommandPrefix) ? "!" : settings.CommandPrefix;
            settings.Language = string.IsNullOrWhiteSpace(settings.Language) ? "en-US" : settings.Language;
            settings.Theme = string.IsNullOrWhiteSpace(settings.Theme) ? "Light" : settings.Theme;
            settings.AccentColor = string.IsNullOrWhiteSpace(settings.AccentColor) ? "TwitchPurple" : settings.AccentColor;
            if (settings.WindowWidth < 1120) settings.WindowWidth = 1440;
            if (settings.WindowHeight < 680) settings.WindowHeight = 780;
            return settings;
        }

        private void BackupInvalidSettings(string path)
        {
            try
            {
                if (!File.Exists(path)) return;
                string backup = path + $".invalid-{DateTime.Now:yyyyMMdd-HHmmss}.bak";
                File.Copy(path, backup, false);
                _logger.Warn("Invalid settings were preserved in a backup file.");
            }
            catch (Exception backupError)
            {
                _logger.Warn("Could not preserve invalid settings.", backupError);
            }
        }
    }
}
