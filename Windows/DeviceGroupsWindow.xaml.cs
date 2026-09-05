using NanoTwitchLeafs.Controller;
using NanoTwitchLeafs.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NanoTwitchLeafs.Windows
{
    public partial class DeviceGroupsWindow : Window
    {
        private readonly AppSettings _appSettings;
        private readonly AppSettingsController _settingsController;
        private NanoleafDeviceGroup _selectedGroup;
        private bool _isEmbedded;
        private Action _closeRequested;
        private Action _helpRequested;

        public DeviceGroupsWindow(AppSettings appSettings, AppSettingsController settingsController)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _settingsController = settingsController ?? throw new ArgumentNullException(nameof(settingsController));
            InitializeComponent();
			Title = Text("Window_DeviceGroups_Title");
			Heading_TextBlock.Text = Text("Window_DeviceGroups_Title");
			Groups_GroupBox.Header = Text("Window_DeviceGroups_Groups");
			EditGroup_GroupBox.Header = Text("Window_DeviceGroups_Edit");
			GroupName_Label.Content = Text("Window_DeviceGroups_Name");
			GroupDevices_Label.Content = Text("Window_DeviceGroups_Devices");
			NewGroup_Button.Content = Text("Window_DeviceGroups_New");
			SaveGroup_Button.Content = Text("Window_DeviceGroups_Save");
			DeleteGroup_Button.Content = Text("Window_DeviceGroups_Delete");
			Help_Button.Content = Text("General_Button_Help");
			Close_Button.Content = Text("Window_DeviceGroups_Close");

            if (_appSettings.NanoSettings.DeviceGroups == null)
                _appSettings.NanoSettings.DeviceGroups = new List<NanoleafDeviceGroup>();

            RefreshGroups();
            BuildDeviceCheckboxes();
        }

        public void RefreshContent()
        {
            RefreshGroups();
            BuildDeviceCheckboxes(_selectedGroup?.DeviceNames);
        }

        private void RefreshGroups()
        {
            Groups_ListBox.ItemsSource = null;
            Groups_ListBox.ItemsSource = _appSettings.NanoSettings.DeviceGroups.OrderBy(group => group.Name).ToList();
        }

        private void BuildDeviceCheckboxes(IEnumerable<string> selectedDeviceNames = null)
        {
            var selected = new HashSet<string>(selectedDeviceNames ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            GroupDevices_ItemsControl.Items.Clear();
            foreach (var device in _appSettings.NanoSettings.NanoLeafDevices)
            {
                GroupDevices_ItemsControl.Items.Add(new CheckBox
                {
                    Content = device.PublicName,
                    Tag = device.DeviceName,
                    IsChecked = selected.Contains(device.DeviceName),
                    Margin = new Thickness(3)
                });
            }
        }

        private void Groups_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedGroup = Groups_ListBox.SelectedItem as NanoleafDeviceGroup;
            GroupName_TextBox.Text = _selectedGroup?.Name ?? string.Empty;
            BuildDeviceCheckboxes(_selectedGroup?.DeviceNames);
        }

        private void NewGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            _selectedGroup = null;
            Groups_ListBox.SelectedItem = null;
            GroupName_TextBox.Clear();
            BuildDeviceCheckboxes();
            GroupName_TextBox.Focus();
        }

        private void SaveGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            string name = GroupName_TextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(Text("Code_DeviceGroups_MessageBox_NameRequired"), Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var deviceNames = GroupDevices_ItemsControl.Items.OfType<CheckBox>()
                .Where(checkBox => checkBox.IsChecked == true)
                .Select(checkBox => checkBox.Tag as string)
                .Where(deviceName => !string.IsNullOrWhiteSpace(deviceName))
                .ToList();
            if (deviceNames.Count == 0)
            {
                MessageBox.Show(Text("Code_DeviceGroups_MessageBox_DeviceRequired"), Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_appSettings.NanoSettings.DeviceGroups.Any(group => group != _selectedGroup &&
                string.Equals(group.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(Text("Code_DeviceGroups_MessageBox_Duplicate"), Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_selectedGroup == null)
            {
                _selectedGroup = new NanoleafDeviceGroup();
                _appSettings.NanoSettings.DeviceGroups.Add(_selectedGroup);
            }

            _selectedGroup.Name = name;
            _selectedGroup.DeviceNames = deviceNames;
            _settingsController.SaveSettings(_appSettings);
            RefreshGroups();
            Groups_ListBox.SelectedItem = Groups_ListBox.Items.Cast<NanoleafDeviceGroup>().First(group => group == _selectedGroup);
        }

        private void DeleteGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGroup == null)
                return;

            if (MessageBox.Show(string.Format(Text("Code_DeviceGroups_MessageBox_DeleteConfirm"), _selectedGroup.Name), Title,
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            _appSettings.NanoSettings.DeviceGroups.Remove(_selectedGroup);
            _selectedGroup = null;
            _settingsController.SaveSettings(_appSettings);
            RefreshGroups();
            NewGroup_Button_Click(sender, e);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isEmbedded) _closeRequested?.Invoke();
            else Close();
        }

        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isEmbedded) _helpRequested?.Invoke();
            else if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                Hide();
                mainWindow.ShowHelp(MainWindow.HelpTopic.Devices, () => { Show(); Activate(); });
            }
        }

        public void ConfigureEmbedded(Action closeRequested, Action helpRequested)
        {
            _isEmbedded = true;
            _closeRequested = closeRequested;
            _helpRequested = helpRequested;
        }

		private static string Text(string key) => Properties.Resources.ResourceManager.GetString(key);
    }
}
