using log4net;
using NanoTwitchLeafs.Windows;
using System;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using log4net.Config;

namespace NanoTwitchLeafs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void App_Startup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
#if NTL4_PRIVATE_MIGRATION
            CopyStableDataForFirstNtl4Start();
#endif
            // Create Nanoleafs directory.
            Directory.CreateDirectory(Constants.PROGRAMFILESFOLDER_PATH);

            // Initialize Logger
            GlobalContext.Properties["LogFile"] = Constants.LOG_PATH;
            string s = Path.Combine(AppContext.BaseDirectory, "log4net.config");
            XmlConfigurator.Configure(new FileInfo(s));
            
            try
            {
                var window = new MainWindow();
                window.Show();

                await window.InitializeAsync();
            }
            catch (Exception exception)
            {
                var logger = LogManager.GetLogger(typeof(App));
                logger.Error($"Error while initializing {nameof(MainWindow)}: {exception.Message}", exception);
                logger.Error(exception.Message, exception);

                CultureInfo startupCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "de"
                    ? CultureInfo.GetCultureInfo("de-DE")
                    : CultureInfo.GetCultureInfo("en-US");
                MessageBox.Show(
                    global::NanoTwitchLeafs.Properties.Resources.ResourceManager.GetString("Code_App_MessageBox_StartupError_Text", startupCulture) +
                    $"\n\n{exception.Message}\n\n" +
                    global::NanoTwitchLeafs.Properties.Resources.ResourceManager.GetString("General_LogFile_Label", startupCulture) +
                    $":\n{Constants.LOG_PATH}",
                    global::NanoTwitchLeafs.Properties.Resources.ResourceManager.GetString("Code_App_MessageBox_StartupError_Title", startupCulture),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                if (Current.MainWindow != null)
                {
                    Current.MainWindow.Close();
                }

                Shutdown(-1);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = LogManager.GetLogger(typeof(App));
            logger.Error("Unhandled WPF user-interface exception.", e.Exception);
            CultureInfo culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "de"
                ? CultureInfo.GetCultureInfo("de-DE")
                : CultureInfo.GetCultureInfo("en-US");
            MessageBox.Show(
                (culture.Name == "de-DE" ? "Ein Oberflächenfehler wurde abgefangen. NanoTwitchLeafs bleibt geöffnet. Details stehen in der Logdatei:" : "A user-interface error was caught. NanoTwitchLeafs remains open. Details are available in the log file:") +
                Environment.NewLine + Constants.LOG_PATH,
                global::NanoTwitchLeafs.Properties.Resources.ResourceManager.GetString("General_MessageBox_Error_Title", culture),
                MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

#if NTL4_PRIVATE_MIGRATION
        private static void CopyStableDataForFirstNtl4Start()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string stablePath = Path.Combine(appDataPath, "NanoTwitchLeafs");
            Directory.CreateDirectory(Constants.PROGRAMFILESFOLDER_PATH);

            if (!Directory.Exists(stablePath))
            {
                return;
            }

            // Copy user data only. The stable installation and its files are not
            // opened, modified, or deleted.
            string[] filesToCopy = { "settings.txt", "nanotwitchleafs.sqlite" };
            foreach (string fileName in filesToCopy)
            {
                string source = Path.Combine(stablePath, fileName);
                string destination = Path.Combine(Constants.PROGRAMFILESFOLDER_PATH, fileName);
                if (File.Exists(source) && !File.Exists(destination))
                {
                    File.Copy(source, destination, false);
                }
            }

            string markerPath = Path.Combine(Constants.PROGRAMFILESFOLDER_PATH, "migration-source.txt");
            if (!File.Exists(markerPath))
            {
                File.WriteAllText(
                    markerPath,
                    $"Copy from NanoTwitchLeafs 3.2.0.5 created at {DateTime.Now:yyyy-MM-dd HH:mm:ss}.{Environment.NewLine}" +
                    "The original data was not changed.");
            }
        }
#endif
    }
}
