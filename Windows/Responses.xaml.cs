using NanoTwitchLeafs.Controller;
using NanoTwitchLeafs.Objects;
using System;
using System.Windows;

namespace NanoTwitchLeafs.Windows
{
    /// <summary>
    /// Interaction logic for Responses.xaml
    /// </summary>
    public partial class Responses : Window
    {
        private readonly AppSettings _appSettings;
        private readonly AppSettingsController _appSettingsController;
        private bool _isEmbedded;
        private Action _closeRequested;
        private Action _helpRequested;

        public Responses(AppSettings appSettings, AppSettingsController appSettingsController)
        {
            _appSettings = appSettings;
            _appSettingsController = appSettingsController;
            Constants.SetCultureInfo(_appSettings.Language);
            InitializeComponent();

            LoadStrings();
        }

        #region UI Methods

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetToDefault();
        }

        private void SaveAndClose_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveStrings();
            _appSettingsController.SaveSettings(_appSettings);
            if (_isEmbedded) _closeRequested?.Invoke();
            else Close();
        }

        private void ResponseHelp_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isEmbedded)
            {
                _helpRequested?.Invoke();
                return;
            }

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                Hide();
                mainWindow.ShowHelp(MainWindow.HelpTopic.ChatResponses, () =>
                {
                    Show();
                    Activate();
                });
            }
        }

        #endregion

        #region Methods

        public void ConfigureEmbedded(Action closeRequested, Action helpRequested)
        {
            _isEmbedded = true;
            _closeRequested = closeRequested;
            _helpRequested = helpRequested;
        }

        public void RefreshContent() => LoadStrings();

        private void SaveStrings()
        {
            _appSettings.Responses.StartupResponse = connectMessage_TextBox.Text;
            _appSettings.Responses.CommandResponse = commandResponse_TextBox.Text;
            _appSettings.Responses.CommandDurationResponse = commandDurationResponse_TextBox.Text;
            _appSettings.Responses.KeywordResponse = keywordResponse_TextBox.Text;
            _appSettings.Responses.StartupMessageActive = (bool)StartupMessage_CheckBox.IsChecked;
        }

        private void LoadStrings()
        {
            connectMessage_TextBox.Text = _appSettings.Responses.StartupResponse;
            commandResponse_TextBox.Text = _appSettings.Responses.CommandResponse;
            commandDurationResponse_TextBox.Text = _appSettings.Responses.CommandDurationResponse;
            keywordResponse_TextBox.Text = _appSettings.Responses.KeywordResponse;
            StartupMessage_CheckBox.IsChecked = _appSettings.Responses.StartupMessageActive;
        }

        private void ResetToDefault()
        {
            connectMessage_TextBox.Text = Properties.Resources.Code_Responses_ConnectMessage;
            commandResponse_TextBox.Text = Properties.Resources.Code_Responses_CommandResponse;
            commandDurationResponse_TextBox.Text = Properties.Resources.Code_Responses_CommandDurationResponse;
            keywordResponse_TextBox.Text = Properties.Resources.Code_Responses_KeywordResponse;
        }

        #endregion
    }
}
