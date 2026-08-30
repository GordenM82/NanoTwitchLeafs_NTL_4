using NanoTwitchLeafs.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace NanoTwitchLeafs
{
	public static class Constants
	{
		#region Github

		public static readonly string GITHUB_OWNER = "Locxion";
		public static readonly string GITHUB_REPO = "NanoTwitchLeafs";
		// Custom builds must not replace themselves with an upstream release.
		// Set this to true only after GITHUB_OWNER points to the maintained fork.
		public static readonly bool AUTO_UPDATE_ENABLED = false;

		#endregion

		#region Paths

		public static readonly string TEMP_PATH = Path.GetTempPath();
		private static readonly string APPDATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // AppData folder
		// Während der 4.0-Migration bleiben Einstellungen und Trigger der stabilen
		// 3.2.0.5 unangetastet. Erst ein späterer, ausdrücklicher Import übernimmt Daten.
#if NTL4_PRIVATE_MIGRATION
		public static readonly string PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs-4-Test");
#elif NTL4
		public static readonly string PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs-4");
#else
		public static readonly string PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs");
#endif
		private static readonly string LOGFOLDER_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, "logs"); // Path for old Logfiles

		private static readonly string SETTINGS_FILE = "settings.txt";
		private static readonly string DEBUG_SETTINGS_FILE = "debug_settings.txt";

		public static readonly string SETTINGS_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, SETTINGS_FILE);
		public static readonly string DEBUG_SETTINGS_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, DEBUG_SETTINGS_FILE);

		public static readonly string DATABASE_FILE = "nanotwitchleafs.sqlite";
		public static readonly string DATABASE_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, DATABASE_FILE);
		public static readonly string TRIGGERS_FILE = "triggers.json";
		public static readonly string TRIGGERS_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, TRIGGERS_FILE);

		public static readonly string LOG_PATH = Path.Combine(LOGFOLDER_PATH, "nanotwichleafs.log");
		
		public static readonly List<string> DEVELOPER = new List<string> { "locxion", "silverdark", "revyn112" };
		#endregion

		#region ServiceCredentials

		public static readonly string SERVICE_CREDENTIALS_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServiceCredentials");
		public static readonly string[] SERVICE_CREDENTIALS_PATHS =
		{
			SERVICE_CREDENTIALS_PATH,
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServiceCredentials.local"),
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServiceCredential.local")
		};
		public static ServiceCredentials ServiceCredentials;

		#endregion

		#region Twitch

		public static int TwitchMessageMaxLength = 500;

		#endregion

		public static void SetCultureInfo(string languageCode)
		{
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo(languageCode);
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
		}
	}
}
