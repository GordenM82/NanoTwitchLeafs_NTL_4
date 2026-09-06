using log4net;
using NanoTwitchLeafs.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;
using ChatMessage = NanoTwitchLeafs.Objects.ChatMessage;
using MessageBox = System.Windows.MessageBox;

namespace NanoTwitchLeafs.Controller
{
	public class TwitchController
	{
		private const string TwitchApiAddress = "https://id.twitch.tv/oauth2";
		private const string AuthorizationEndpoint = "/authorize";
		private const string TokenEndpoint = "/token";
		private const string TwitchScopesBot = "scope=channel:bot user:read:chat chat:edit chat:read whispers:read whispers:edit user:manage:whispers";
		private const string TwitchScopesChannelOwner = "scope=channel:bot user:read:chat moderator:read:followers chat:edit chat:read whispers:read whispers:edit user:manage:whispers bits:read channel:read:subscriptions channel:read:hype_train channel:read:redemptions channel:manage:redemptions";

		private const string RedirectUri = "http://127.0.0.1:1234";
		
		private readonly ILog _logger = LogManager.GetLogger(typeof(TwitchController));
		public TwitchClient Client;
		private TwitchClient _broadCasterClient;
		private TwitchAPI _api;
		private AppSettings _appSettings;
		public TwitchEventSubController EventSubController;
		private readonly AppSettingsController _appSettingsController;

		public List<string> ChannelModerator { get; set; }

		private bool _firstTryToConnectBotAccount = true;
		private bool _firstTryToConnectBroadcasterAccount = true;
		public event EventHandler OnDisconnected;
		public event Action<bool> OnChatConnectionChanged;
		public event Action<string> OnChatConnectionFailed;

		public event ChatMessageReceived OnChatMessageReceived;
		
		public event CallLoadingWindow OnCallLoadingWindow;

		public TwitchController(AppSettingsController appSettingsController)
		{
			_appSettingsController = appSettingsController ?? throw new ArgumentNullException(nameof(appSettingsController));
			_appSettings = _appSettingsController.LoadSettings();
		}

		/// <summary>
		/// Connects to Twitch Services with provided AppSettings
		/// </summary>
		/// <param name="appSettings"></param>
		public void Connect(AppSettings appSettings)
		{
			Client?.Disconnect();

			_appSettings = appSettings;

			// if (string.IsNullOrEmpty(_appSettings.BotName) || string.IsNullOrEmpty(_appSettings.ChannelName) || string.IsNullOrEmpty(_appSettings.BotAuthObject.Access_Token))
			// {
			//     MessageBox.Show(Properties.Resources.Code_Twitch_MessageBox_LoginIncorrect, Properties.Resources.General_MessageBox_Error_Title);
			//     ((MainWindow)App.Current.MainWindow).sendMessage_TextBox.IsEnabled = false;
			//     ((MainWindow)App.Current.MainWindow).sendMessage_Button.IsEnabled = false;
			//     _logger.Error("Please make sure Username, Channel and AuthToken are correct.");
			//     return;
			// }

			if (string.IsNullOrWhiteSpace(_appSettings.BotName) ||
				string.IsNullOrWhiteSpace(_appSettings.ChannelName) ||
				string.IsNullOrWhiteSpace(_appSettings.BotAuthObject?.Access_Token))
			{
				const string message = "Twitch chat credentials are incomplete.";
				_logger.Warn(message);
				OnChatConnectionFailed?.Invoke(message);
				return;
			}

			EstablishTwitchConnection();
		}

		private void EstablishTwitchConnection()
		{
			if (string.IsNullOrWhiteSpace(_appSettings.BotName) || _appSettings.BotAuthObject == null)
				return;

			ConnectionCredentials credentials = new ConnectionCredentials(_appSettings.BotName.ToLower(), "oauth:" + _appSettings.BotAuthObject.Access_Token);
			var clientOptions = new ClientOptions();
			WebSocketClient customClient = new WebSocketClient(clientOptions);
			Client = new TwitchClient(customClient);
			Client.Initialize(credentials, _appSettings.ChannelName.ToLower());

			Client.OnLog += Client_OnLog;
			Client.OnConnected += Client_OnConnected;
			Client.OnJoinedChannel += Client_OnJoinedChannel;
			Client.OnMessageReceived += Client_OnMessageReceived;
			Client.OnWhisperReceived += Client_OnWhisperReceived;
			Client.OnModeratorsReceived += Client_OnModeratorsReceived;
			Client.OnDisconnected += Client_OnDisconnected;
			Client.OnIncorrectLogin += Client_OnIncorrectLogin;
			Client.Connect();
		}

		private void Client_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{
			OnDisconnected?.Invoke(this, EventArgs.Empty);
			OnChatConnectionChanged?.Invoke(false);
			_logger.Info($"Disconnected from Twitch.");
		}

		private void BroadCasterClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{
			_logger.Info($"Disconnected from Twitch.");
		}

		private async void Client_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
		{
			try
			{
				if (!_firstTryToConnectBotAccount)
				{
					const string message = "Twitch rejected the refreshed bot login.";
					_logger.Error(message);
					OnChatConnectionFailed?.Invoke(message);
					return;
				}

				_logger.Warn("Got incorrect Login Message from Twitch ... (Bot Account)");
				_logger.Warn("Try to refresh Access Tokens ... This could take a Second ... or Two ...");
				OnCallLoadingWindow?.Invoke(true);
				Disconnect(true);
				var newOauth = await PerformCodeExchange(_appSettings.BotAuthObject?.Refresh_Token, true);
				if (newOauth == null || string.IsNullOrWhiteSpace(newOauth.Access_Token))
				{
					const string message = "Twitch bot token refresh failed. Please link the Twitch account again.";
					_logger.Error(message);
					OnChatConnectionFailed?.Invoke(message);
					return;
				}
				_appSettings.BotAuthObject = newOauth;
				if (_appSettings.BotName == _appSettings.ChannelName)
					_appSettings.BroadcasterAuthObject = newOauth;
				_appSettingsController.SaveSettings(_appSettings);
				_firstTryToConnectBotAccount = false;

				EstablishTwitchConnection();
			}
			catch (Exception exception)
			{
				_logger.Error("Could not refresh the Twitch bot login.", exception);
				OnChatConnectionFailed?.Invoke(exception.Message);
			}
			finally
			{
				OnCallLoadingWindow?.Invoke(false);
			}
		}

		private async void BroadCasterClient_OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
		{
			try
			{
				if (!_firstTryToConnectBroadcasterAccount)
				{
					_logger.Error("Twitch rejected the refreshed broadcaster login.");
					return;
				}

				_logger.Warn("Got incorrect Login Message from Twitch ...(Broadcaster Account)");
				_logger.Warn("Try to refresh Access Tokens ... This could take a Second ... or Two ...");
				OnCallLoadingWindow?.Invoke(true);

				var newOauth = await PerformCodeExchange(_appSettings.BroadcasterAuthObject?.Refresh_Token, true);
				if (newOauth == null || string.IsNullOrWhiteSpace(newOauth.Access_Token))
				{
					_logger.Error("Twitch broadcaster token refresh failed. Please link the Twitch account again.");
					return;
				}
				_appSettings.BroadcasterAuthObject = newOauth;
				_appSettingsController.SaveSettings(_appSettings);

				_firstTryToConnectBroadcasterAccount = false;

				ConnectionCredentials credentials = new ConnectionCredentials(_appSettings.ChannelName.ToLower(), "oauth:" + _appSettings.BroadcasterAuthObject.Access_Token);
				_broadCasterClient = new TwitchClient();

					_broadCasterClient.OnConnected += BroadCasterClient_OnConnected;
					_broadCasterClient.OnIncorrectLogin += BroadCasterClient_OnIncorrectLogin;
					//_broadCasterClient.OnLog += Client_OnLog; //Disabled to prevent spam in the Log
					_broadCasterClient.OnDisconnected += BroadCasterClient_OnDisconnected;

				_broadCasterClient.Initialize(credentials, _appSettings.ChannelName.ToLower());
				_broadCasterClient.Connect();
			}
			catch (Exception exception)
			{
				_logger.Error("Could not refresh the Twitch broadcaster login.", exception);
			}
			finally
			{
				OnCallLoadingWindow?.Invoke(false);
			}
		}

		private async void Client_OnConnected(object sender, OnConnectedArgs e)
		{
			try
			{
				_firstTryToConnectBotAccount = true;
				_logger.Debug($"Connected to {e.AutoJoinChannel} with Account {e.BotUsername}.");
				OnCallLoadingWindow?.Invoke(false);
				if (_appSettings.BotName.ToLower() != _appSettings.ChannelName.ToLower())
				{
					_logger.Debug("Bot Account detected. Init Broadcaster Twitch Connection...");
					if (_broadCasterClient?.IsInitialized ?? false)
						_broadCasterClient.Disconnect();

					OnCallLoadingWindow?.Invoke(true);

					if (string.IsNullOrWhiteSpace(_appSettings.BroadcasterAuthObject?.Access_Token))
						throw new InvalidOperationException("Broadcaster credentials are incomplete.");
					ConnectionCredentials broadcasterCredentials = new ConnectionCredentials(_appSettings.ChannelName.ToLower(), "oauth:" + _appSettings.BroadcasterAuthObject.Access_Token);
					_broadCasterClient = new TwitchClient();
					_broadCasterClient.Initialize(broadcasterCredentials, _appSettings.ChannelName.ToLower());

				_broadCasterClient.OnConnected += BroadCasterClient_OnConnected;
				_broadCasterClient.OnIncorrectLogin += BroadCasterClient_OnIncorrectLogin;
				//_broadCasterClient.OnLog += Client_OnLog; //Disabled to prevent spam in the Log
				_broadCasterClient.OnDisconnected += BroadCasterClient_OnDisconnected;

					_broadCasterClient.Connect();
					OnCallLoadingWindow?.Invoke(false);
				}

				try
				{
					if (EventSubController != null)
						await EventSubController.StartAsync();
				}
				catch (Exception exception)
				{
					_logger.Error("Twitch chat connected, but EventSub could not be started.", exception);
				}
			}
			catch (Exception exception)
			{
				_logger.Error("Twitch post-connect initialization failed.", exception);
			}
			finally
			{
				OnCallLoadingWindow?.Invoke(false);
			}
		}

		private void BroadCasterClient_OnConnected(object sender, OnConnectedArgs e)
		{
			OnCallLoadingWindow?.Invoke(false);
			_firstTryToConnectBroadcasterAccount = true;
			_logger.Debug($"Connected to {e.AutoJoinChannel} with BroadcasterAccount {e.BotUsername}.");
		}

		private void Client_OnModeratorsReceived(object sender, OnModeratorsReceivedArgs e)
		{
			ChannelModerator = e.Moderators;
		}

		/// <summary>
		/// Disconnects from Twitch Services
		/// </summary>
		/// <param name="both"></param>
		public void Disconnect(bool both = false)
		{
			if (Client is not null)
			{
				if (Client.IsConnected)
					Client.Disconnect();
				Client.OnLog -= Client_OnLog;
				Client.OnConnected -= Client_OnConnected;
				Client.OnJoinedChannel -= Client_OnJoinedChannel;
				Client.OnMessageReceived -= Client_OnMessageReceived;
				Client.OnWhisperReceived -= Client_OnWhisperReceived;
				Client.OnModeratorsReceived -= Client_OnModeratorsReceived;
				Client.OnDisconnected -= Client_OnDisconnected;
				Client.OnIncorrectLogin -= Client_OnIncorrectLogin;
				Client = null;
			}
			
			if (both)
			{
				if (_broadCasterClient is not null)
				{
					if (_broadCasterClient.IsConnected)
						_broadCasterClient.Disconnect();
					_broadCasterClient.OnConnected -= BroadCasterClient_OnConnected;
					_broadCasterClient.OnIncorrectLogin -= BroadCasterClient_OnIncorrectLogin;
					//_broadCasterClient.OnLog -= Client_OnLog; //Disabled to prevent spam in the Log
					_broadCasterClient.OnDisconnected -= BroadCasterClient_OnDisconnected;
					_broadCasterClient = null;
				}
			}

			if (EventSubController != null)
				_ = EventSubController.StopAsync();
			OnCallLoadingWindow?.Invoke(false);
		}
		
		/// <summary>
		/// Sends Message to connected TwitchChannel
		/// </summary>
		/// <param name="message"></param>
		public void SendMessageToChat(string message)
		{
			if (Client is null || !Client.IsConnected || string.IsNullOrWhiteSpace(message))
				return;
			Client.SendMessage(_appSettings.ChannelName, message);
			_logger.Info($"-> {message}");
		}

		/// <summary>
		/// Sends Message to User
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="message"></param>
		public async void SendWhisper(string userName, string message)
		{
			if (Client is null || !Client.IsConnected)
				return;

			//Ignore Whisper when try to send to own User Account
			if (userName.ToLower() == _appSettings.ChannelName.ToLower())
			{
				_logger.Warn("Could not send Whisper to own User. Ignoring Message");
				return;
			}
			
			_api = new TwitchAPI();
			
			_api.Settings.ClientId = Constants.TWITCH_CLIENT_ID;
			_api.Settings.AccessToken = _appSettings.BotAuthObject.Access_Token;
			
			var fromUserId = await HelperClass.GetUserId(_api, _appSettings, _appSettings.BotName);
			var toUserId = await HelperClass.GetUserId(_api, _appSettings, userName);

			try
			{
				await _api.Helix.Whispers.SendWhisperAsync(fromUserId, toUserId, message, true);
				_logger.Info($"-> to {userName} - {message}");
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				_logger.Error(ex);
			}
		}

		private void Client_OnLog(object sender, OnLogArgs e)
		{
			_logger.Debug(e.Data);
		}

		private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
		{
			OnChatConnectionChanged?.Invoke(true);
			if (_appSettings.Responses.StartupMessageActive)
			{
				string message = _appSettings.Responses.StartupResponse;
				if (message != "")
				{
					Client.SendMessage(e.Channel, message);
				}
				_logger.Info($"-> {message}");
			}
			OnCallLoadingWindow?.Invoke(false);
		}

		private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
		{
			var message = new ChatMessage(e.ChatMessage.Username, e.ChatMessage.IsSubscriber, e.ChatMessage.IsModerator, e.ChatMessage.IsVip, e.ChatMessage.Message,
				ColorTranslator.FromHtml(e.ChatMessage.ColorHex));
			_logger.Info($"{message.Username} - {message.Message}");
			OnChatMessageReceived?.Invoke(message);
		}

		private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
		{
			//_client.SendWhisper(e.WhisperMessage.Username, "I'm a Bot. I'm not programmed to answer Whisper Messages!");
		}

		#region Auth Handling

		public async Task<OAuthObject> GetAuthToken(bool isBroadcaster)
		{

			string scopes = (isBroadcaster ? TwitchScopesChannelOwner : TwitchScopesBot)
				.Substring("scope=".Length);

			using var httpClient = new HttpClient();
			using var deviceRequest = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				["client_id"] = Constants.TWITCH_CLIENT_ID,
				["scopes"] = scopes
			});

			HttpResponseMessage deviceResponse;
			try
			{
				deviceResponse = await httpClient.PostAsync($"{TwitchApiAddress}/device", deviceRequest);
			}
			catch (Exception exception)
			{
				_logger.Error("Could not start Twitch device authorization.", exception);
				return null;
			}

			string deviceResponseText = await deviceResponse.Content.ReadAsStringAsync();
			if (!deviceResponse.IsSuccessStatusCode)
			{
				_logger.Error($"Twitch device authorization failed: {deviceResponseText}");
				return null;
			}

			dynamic device = JsonConvert.DeserializeObject(deviceResponseText);
			string deviceCode = device.device_code;
			string userCode = device.user_code;
			string verificationUri = device.verification_uri;
			int expiresIn = device.expires_in;
			int interval = Math.Max(1, (int)device.interval);

			Process.Start(new ProcessStartInfo
			{
				FileName = verificationUri,
				UseShellExecute = true
			});

			MessageBox.Show(
				string.Format(Properties.Resources.ResourceManager.GetString("Code_Twitch_MessageBox_DeviceCode_Text"), userCode),
				Properties.Resources.ResourceManager.GetString("Code_Twitch_MessageBox_DeviceCode_Title"),
				System.Windows.MessageBoxButton.OK,
				System.Windows.MessageBoxImage.Information);

			DateTime expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
			while (DateTime.UtcNow < expiresAt)
			{
				await Task.Delay(TimeSpan.FromSeconds(interval));

				using var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					["client_id"] = Constants.TWITCH_CLIENT_ID,
					["scopes"] = scopes,
					["device_code"] = deviceCode,
					["grant_type"] = "urn:ietf:params:oauth:grant-type:device_code"
				});

				HttpResponseMessage tokenResponse = await httpClient.PostAsync($"{TwitchApiAddress}{TokenEndpoint}", tokenRequest);
				string tokenResponseText = await tokenResponse.Content.ReadAsStringAsync();

				if (tokenResponse.IsSuccessStatusCode)
				{
					dynamic token = JsonConvert.DeserializeObject(tokenResponseText);
					return new OAuthObject
					{
						Access_Token = token.access_token,
						Refresh_Token = token.refresh_token,
						Expires_In = token.expires_in
					};
				}

				dynamic pendingResponse = JsonConvert.DeserializeObject(tokenResponseText);
				string message = pendingResponse?.message;
				if (string.Equals(message, "authorization_pending", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				if (string.Equals(message, "slow_down", StringComparison.OrdinalIgnoreCase))
				{
					interval += 5;
					continue;
				}

				_logger.Error($"Twitch device token request failed: {tokenResponseText}");
				return null;
			}

			_logger.Error("Twitch device authorization timed out.");
			return null;
		}

		private async Task<OAuthObject> PerformCodeExchange(string code, bool isRefresh = false)
		{
			if (!isRefresh || string.IsNullOrWhiteSpace(code))
			{
				return null;
			}

			var values = new Dictionary<string, string>
			{
				["refresh_token"] = code,
				["client_id"] = Constants.TWITCH_CLIENT_ID,
				["grant_type"] = "refresh_token"
			};

			using var httpClient = new HttpClient();
			using var request = new FormUrlEncodedContent(values);
			HttpResponseMessage response = await httpClient.PostAsync($"{TwitchApiAddress}{TokenEndpoint}", request);
			string responseText = await response.Content.ReadAsStringAsync();
			if (!response.IsSuccessStatusCode)
			{
				_logger.Error($"Could not refresh Twitch token: {responseText}");
				return null;
			}

			dynamic token = JsonConvert.DeserializeObject(responseText);
			return new OAuthObject
			{
				Access_Token = token.access_token,
				Refresh_Token = token.refresh_token,
				Expires_In = token.expires_in
			};
		}

		/// <summary>
		/// Pulls Avatar Url from TwitchUser
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="token"></param>
		/// <returns>Url as String</returns>
		public async Task<string> GetAvatarUrl(string userName, string token)
		{
			var api = new TwitchAPI
			{
				Settings =
				{
					ClientId = Constants.TWITCH_CLIENT_ID,
					AccessToken = token
				}
			};

			var getUsersResponse = await api.Helix.Users.GetUsersAsync(null, [userName], token);
			return getUsersResponse.Users[0].ProfileImageUrl;
		}

		#endregion
	}

	public class TwitchClientWrapper : IDisposable
	{
		private TwitchClient _twitchClient;

		public event EventHandler<OnConnectedArgs> OnConnected;

		public TwitchClientWrapper()
		{
			_twitchClient = new TwitchClient();

			_twitchClient.OnConnected += OnConnected;
		}

		#region IDisposable Support

		private bool _disposed = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					if (_twitchClient.IsConnected)
						_twitchClient.Disconnect();

					_twitchClient.OnConnected -= OnConnected;
					_twitchClient = null;
				}

				_disposed = true;
			}
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
		}

		#endregion
	}
}
