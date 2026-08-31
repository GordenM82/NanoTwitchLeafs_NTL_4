using NanoTwitchLeafs.Controller;
using NanoTwitchLeafs.Objects;
using System;
using System.Diagnostics;
using System.Windows;

namespace NanoTwitchLeafs.Windows
{
    /// <summary>
    /// Interaction logic for AppInfoWindow.xaml
    /// </summary>
    public partial class AppInfoWindow : Window
    {
        private readonly AppSettings _appSettings;

        public AppInfoWindow(AppSettings appSettings, AppSettingsController appSettingsController)
        {
            if (appSettingsController == null) throw new ArgumentNullException(nameof(appSettingsController));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            Constants.SetCultureInfo(_appSettings.Language);
            InitializeComponent();

            version_Label.Content = typeof(AppInfoWindow).Assembly.GetName().Version.ToString(3);
            originalDeveloperTitle_TextBlock.Text = GetResource("Window_AppInfo_OriginalDeveloper");
            maintainerTitle_TextBlock.Text = GetResource("Window_AppInfo_Ntl4Maintainer");
            discord_Button.Content = GetResource("Window_AppInfo_Discord");
            originalGithub_Button.Content = GetResource("Window_AppInfo_GithubOriginal");
            ntl4Github_Button.Content = GetResource("Window_AppInfo_GithubNtl4");
            originalFeedback_Button.Content = GetResource("Window_AppInfo_FeedbackOriginal");
            ntl4Feedback_Button.Content = GetResource("Window_AppInfo_FeedbackNtl4");
            aiAssistance_TextBlock.Text = GetResource("Window_AppInfo_AiAssistance");
            association_TextBlock.Text = GetResource("Window_AppInfo_Associated_Label");
        }

        private static string GetResource(string key)
        {
            return Properties.Resources.ResourceManager.GetString(key) ?? key;
        }

        private void Discord_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://discord.gg/w92xZKd");
        }

        private void OriginalGithub_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://github.com/Locxion/NanoTwitchLeafs");
        }

        private void Ntl4Github_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://github.com/GordenM82/NanoTwitchLeafs_NTL_4");
        }

        private void OriginalFeedback_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://github.com/Locxion/NanoTwitchLeafs/issues");
        }

        private void Ntl4Feedback_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/issues");
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show(
                    Properties.Resources.General_MessageBox_GeneralError_Text,
                    Properties.Resources.General_MessageBox_Error_Title,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
