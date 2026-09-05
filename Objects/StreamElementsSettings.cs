namespace NanoTwitchLeafs.Objects
{
    public sealed class StreamElementsSettings : NotifyObject
    {
        public StreamElementsSettings()
        {
            TokenType = "jwt";
            Token = string.Empty;
            AutoConnect = true;
        }

        public bool Enabled
        {
            get { return Get(() => Enabled); }
            set { Set(() => Enabled, value); }
        }

        public bool AutoConnect
        {
            get { return Get(() => AutoConnect); }
            set { Set(() => AutoConnect, value); }
        }

        public string Token
        {
            get { return Get(() => Token); }
            set { Set(() => Token, value); }
        }

        public string TokenType
        {
            get { return Get(() => TokenType); }
            set { Set(() => TokenType, value); }
        }

        public string ConnectedChannelId
        {
            get { return Get(() => ConnectedChannelId); }
            set { Set(() => ConnectedChannelId, value); }
        }

        public string LastConnectionError
        {
            get { return Get(() => LastConnectionError); }
            set { Set(() => LastConnectionError, value); }
        }

        public System.DateTimeOffset? LastSuccessfulConnection
        {
            get { return Get(() => LastSuccessfulConnection); }
            set { Set(() => LastSuccessfulConnection, value); }
        }
    }
}
