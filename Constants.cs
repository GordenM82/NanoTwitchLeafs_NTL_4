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
		// Keep the stable 3.x settings and triggers untouched during migration.
		// This compatibility path must remain available in later releases unless an
		// equivalent direct importer is provided.
#if NTL4_PRIVATE_MIGRATION
		public static readonly string PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs-4-Test");
#elif NTL4
		public static readonly string PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs-4");
#else
		public static readonly string PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs");
#endif
		private static readonly string LOGFOLDER_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, "logs"); // Path for old Logfiles

		// Read-only source paths used by the optional first-start migration from Locxion\'s 3.x release.
		public static readonly string LEGACY_PROGRAMFILESFOLDER_PATH = Path.Combine(APPDATA_PATH, "NanoTwitchLeafs");
		public static readonly string LEGACY_SETTINGS_PATH = Path.Combine(LEGACY_PROGRAMFILESFOLDER_PATH, "settings.txt");
		public static readonly string LEGACY_DATABASE_PATH = Path.Combine(LEGACY_PROGRAMFILESFOLDER_PATH, "nanotwitchleafs.sqlite");
		public static readonly string LEGACY_MIGRATION_ACCEPTED_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, ".migration-3x-accepted");
		public static readonly string LEGACY_MIGRATION_COMPLETED_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, ".migration-3x-completed");
		public static readonly string LEGACY_MIGRATION_BACKUP_PATH = Path.Combine(PROGRAMFILESFOLDER_PATH, "Migration-Backup-3.x");

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
