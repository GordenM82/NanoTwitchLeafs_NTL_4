using log4net;
using NanoTwitchLeafs.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NanoTwitchLeafs.Controller
{
    public sealed class StreamElementsController : IDisposable
    {
        private static readonly Uri Endpoint = new Uri("wss://astro.streamelements.com/");
        private readonly ILog _logger = LogManager.GetLogger(typeof(StreamElementsController));
        private readonly StreamElementsSettings _settings;
        private readonly HashSet<string> _seenEvents = new HashSet<string>(StringComparer.Ordinal);
        private readonly object _sync = new object();
        private ClientWebSocket _socket;
        private CancellationTokenSource _lifetime;

        public event OnDonationRecieved OnDonationRecieved;
        public event Action<bool, string> ConnectionStateChanged;

        public bool IsConnected => _socket?.State == WebSocketState.Open;

        public StreamElementsController(AppSettings appSettings)
        {
            _settings = appSettings.StreamElements ??= new StreamElementsSettings();
        }

        public async Task<bool> ConnectAsync()
        {
            if (IsConnected)
                return true;
            if (!_settings.Enabled || string.IsNullOrWhiteSpace(_settings.Token))
            {
                SetState(false, "not-configured");
                return false;
            }

            await DisconnectAsync().ConfigureAwait(false);
            _lifetime = new CancellationTokenSource();
            _socket = new ClientWebSocket();
            try
            {
                await _socket.ConnectAsync(Endpoint, _lifetime.Token).ConfigureAwait(false);
                _ = ReceiveLoopAsync(_lifetime.Token);
                await SubscribeAsync(_lifetime.Token).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                _settings.LastConnectionError = Sanitize(ex.Message);
                _logger.Error("StreamElements connection failed: " + _settings.LastConnectionError);
                SetState(false, _settings.LastConnectionError);
                await DisconnectAsync().ConfigureAwait(false);
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            ClientWebSocket socket = _socket;
            _socket = null;
            try { _lifetime?.Cancel(); } catch { }
            if (socket != null)
            {
                try
                {
                    if (socket.State == WebSocketState.Open)
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NTL shutdown", CancellationToken.None).ConfigureAwait(false);
                }
                catch (Exception ex) { _logger.Debug("StreamElements socket close: " + Sanitize(ex.Message)); }
                socket.Dispose();
            }
            _lifetime?.Dispose();
            _lifetime = null;
            SetState(false, "disconnected");
        }

        public void SimulateDonation(double amount, string username, string currency = "EUR")
        {
            OnDonationRecieved?.Invoke(new DonationEvent
            {
                EventId = "test-" + Guid.NewGuid().ToString("N"),
                Provider = "StreamElements",
                Amount = amount,
                Currency = currency,
                Username = string.IsNullOrWhiteSpace(username) ? "Test" : username,
                Message = "NTL test donation"
            });
        }

        private async Task SubscribeAsync(CancellationToken token)
        {
            var request = new
            {
                type = "subscribe",
                nonce = Guid.NewGuid().ToString("N"),
                data = new { topic = "channel.tips", token = _settings.Token.Trim(), token_type = NormalizeTokenType(_settings.TokenType) }
            };
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            await _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            var buffer = new byte[16384];
            try
            {
                while (!token.IsCancellationRequested && _socket?.State == WebSocketState.Open)
                {
                    using var stream = new MemoryStream();
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), token).ConfigureAwait(false);
                        if (result.MessageType == WebSocketMessageType.Close)
                            return;
                        stream.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Text)
                        ProcessMessage(Encoding.UTF8.GetString(stream.ToArray()));
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                _settings.LastConnectionError = Sanitize(ex.Message);
                _logger.Warn("StreamElements connection interrupted: " + _settings.LastConnectionError);
            }
            finally
            {
                SetState(false, "disconnected");
                if (_settings.Enabled && _settings.AutoConnect && !token.IsCancellationRequested)
                    _ = ReconnectAsync();
            }
        }

        internal void ProcessMessage(string json)
        {
            JObject message;
            try { message = JObject.Parse(json); }
            catch (JsonException) { _logger.Warn("Ignored malformed StreamElements message."); return; }

            string type = message.Value<string>("type");
            if (type == "response")
            {
                string error = message.Value<string>("error");
                if (string.IsNullOrWhiteSpace(error))
                {
                    string room = message.SelectToken("data.room")?.ToString() ?? string.Empty;
                    _settings.ConnectedChannelId = room;
                    _settings.LastConnectionError = string.Empty;
                    _settings.LastSuccessfulConnection = DateTimeOffset.UtcNow;
                    SetState(true, room);
                }
                else
                {
                    _settings.LastConnectionError = error;
                    SetState(false, error);
                }
                return;
            }

            if (type != "message" || message.Value<string>("topic") != "channel.tips")
                return;

            string id = message.SelectToken("data._id")?.ToString() ?? message.Value<string>("id") ?? string.Empty;
            lock (_sync)
            {
                if (!string.IsNullOrEmpty(id) && !_seenEvents.Add(id))
                {
                    _logger.Info("Ignored duplicate StreamElements tip " + id);
                    return;
                }
                if (_seenEvents.Count > 500)
                    _seenEvents.Clear();
            }

            JToken donation = message.SelectToken("data.donation");
            if (donation == null)
                return;
            string status = message.SelectToken("data.status")?.ToString();
            string approved = message.SelectToken("data.approved")?.ToString();
            if (!string.IsNullOrEmpty(status) && !status.Equals("success", StringComparison.OrdinalIgnoreCase))
                return;
            if (!string.IsNullOrEmpty(approved) && !approved.Equals("allowed", StringComparison.OrdinalIgnoreCase))
                return;

            string username = donation.SelectToken("user.username")?.ToString();
            var tip = new DonationEvent
            {
                EventId = id,
                Provider = "StreamElements",
                Amount = donation.Value<double?>("amount") ?? 0,
                Currency = donation.Value<string>("currency") ?? string.Empty,
                Username = string.IsNullOrWhiteSpace(username) ? "Anonymous" : username,
                IsAnonymous = string.IsNullOrWhiteSpace(username),
                Message = donation.Value<string>("message") ?? string.Empty
            };
            _logger.Info($"Received StreamElements tip: {tip.Amount:0.##} {tip.Currency}; donor={(tip.IsAnonymous ? "anonymous" : "provided")}");
            OnDonationRecieved?.Invoke(tip);
        }

        private async Task ReconnectAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            await ConnectAsync().ConfigureAwait(false);
        }

        private void SetState(bool connected, string detail)
        {
            ConnectionStateChanged?.Invoke(connected, detail ?? string.Empty);
        }

        private string Sanitize(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(_settings.Token)) return value ?? string.Empty;
            return value.Replace(_settings.Token, "[redacted]", StringComparison.Ordinal);
        }

        private static string NormalizeTokenType(string value) =>
            string.Equals(value, "apikey", StringComparison.OrdinalIgnoreCase) ? "apikey" : "jwt";

        public void Dispose()
        {
            try { DisconnectAsync().GetAwaiter().GetResult(); } catch { }
        }
    }
}
