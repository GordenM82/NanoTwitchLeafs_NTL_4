using log4net;
using NanoTwitchLeafs.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Authentication;
using System.Windows;
using WebSocket4Net;

namespace NanoTwitchLeafs.Controller
{
    public delegate void OnHeartRateRecieved(int heartRate);

    public delegate void OnHypeRateDisconnected();

    public delegate void OnHypeRateConnected();

    public class HypeRateIOController
    {
        private readonly AppSettings _appSettings;

        public event OnHeartRateRecieved OnHeartRateRecieved;

        public event OnHypeRateConnected OnHypeRateConnected;

        public event OnHypeRateDisconnected OnHypeRateDisconnected;

        private readonly ILog _logger = LogManager.GetLogger(typeof(NanoController));

		private readonly string _apiKey;
		private readonly string _websocketUrl;
        private WebSocket _webSocket;
        public bool _isConnected = false;

		public HypeRateIOController(AppSettings appSettings)
		{
			_appSettings = appSettings ?? new AppSettings();
			_apiKey = _appSettings.HypeRateApiKey ?? string.Empty;
			_websocketUrl = $"wss://app.hyperate.io/socket/websocket?token={Uri.EscapeDataString(_apiKey)}";
			_webSocket = new WebSocket(_websocketUrl);
            _webSocket.Opened += _webSocket_Opened;
            _webSocket.Error += _webSocket_Error;
            _webSocket.Closed += _webSocket_Closed;
            _webSocket.MessageReceived += _webSocket_MessageReceived;
            _webSocket.AutoSendPingInterval = 25;
            _webSocket.EnableAutoSendPing = true;
            _webSocket.Security.EnabledSslProtocols = SslProtocols.Tls12;
        }

        private void _webSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            try
            {
                JObject message = JObject.Parse(e?.Message ?? "{}");
                if (!string.Equals(message.Value<string>("event"), "hr_update", StringComparison.Ordinal)) return;
                int? heartRate = message["payload"]?.Value<int?>("hr");
                if (!heartRate.HasValue) { _logger.Warn("HypeRate update did not contain a heart rate."); return; }
                _logger.Debug($"Received HeartRate {heartRate.Value}");
                OnHeartRateRecieved?.Invoke(heartRate.Value);
            }
            catch (Exception ex)
            {
                _logger.Warn("Ignored an invalid HypeRate message.", ex);
            }
        }

        private void _webSocket_Closed(object sender, EventArgs e)
        {
            _logger.Debug("Closed Connection to HypeRate Server");
            _isConnected = false;
            OnHypeRateDisconnected?.Invoke();
        }

        private void _webSocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            _logger.Error(e.Exception);
            _logger.Error(e.Exception.Message);
            MessageBox.Show(Properties.Resources.General_MessageBox_HypeRate_Error_Message, Properties.Resources.General_MessageBox_Error_Title,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void _webSocket_Opened(object sender, EventArgs e)
        {
            _isConnected = true;
            OnHypeRateConnected?.Invoke();
            _logger.Debug("Connect to HypeRate Server");
            var message = $"{{\r\n  \"topic\": \"hr:{_appSettings.HypeRateId}\",\r\n  \"event\": \"phx_join\",\r\n  \"payload\": {{}},\r\n  \"ref\": 0\r\n}}";
            _logger.Debug("Send 'Join Channel' Message");
            _webSocket.Send(message);
        }

        /// <summary>
        /// Connets to Websocket with HypeRateId from Appsettings
        /// </summary>
		public void StartListener()
		{
			if (string.IsNullOrWhiteSpace(_apiKey))
			{
				_logger.Warn("No HypeRate API key configured ... skip Connection");
				MessageBox.Show(
					Properties.Resources.ResourceManager.GetString("Code_HypeRate_MessageBox_MissingApiKey"),
					Properties.Resources.General_MessageBox_Hint_Title,
					MessageBoxButton.OK,
					MessageBoxImage.Information);
				return;
			}

			if (string.IsNullOrWhiteSpace(_appSettings.HypeRateId))
            {
                _logger.Warn("No HypeRateIO ID ... skip Connection");
                return;
            }
            _webSocket.Open();
        }

        /// <summary>
        /// Disconnects from Websocket
        /// </summary>
        public void Disconnect()
        {
            if (_webSocket != null && _webSocket.State != WebSocketState.Closed)
                _webSocket.Close();
        }
    }
}
