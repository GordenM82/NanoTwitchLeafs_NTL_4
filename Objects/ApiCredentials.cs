namespace NanoTwitchLeafs.Objects
{
	/// <summary>
	/// Credentials required by Twitch authentication requests.
	/// Values are always supplied by the user's application settings.
	/// </summary>
	public class TwitchApiCredentials
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }

		public TwitchApiCredentials(string clientId, string clientSecret)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
		}
	}

	/// <summary>
	/// Credentials required by Streamlabs authentication requests.
	/// Values are always supplied by the user's application settings.
	/// </summary>
	public class StreamLabsApiCedentials
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }

		public StreamLabsApiCedentials(string clientId, string clientSecret)
		{
			ClientId = clientId;
			ClientSecret = clientSecret;
		}
	}
}
