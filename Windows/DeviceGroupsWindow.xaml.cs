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

        public DeviceGroupsWindow(AppSettings appSettings, AppSettingsController settingsController)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _settingsController = settingsController ?? throw new ArgumentNullException(nameof(settingsController));
            InitializeComponent();

            if (_appSettings.NanoSettings.DeviceGroups == null)
                _appSettings.NanoSettings.DeviceGroups = new List<NanoleafDeviceGroup>();

            RefreshGroups();
            BuildDeviceCheckboxes();
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
                MessageBox.Show("Bitte einen Gruppennamen eingeben.", "Gerätegruppen", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var deviceNames = GroupDevices_ItemsControl.Items.OfType<CheckBox>()
                .Where(checkBox => checkBox.IsChecked == true)
                .Select(checkBox => checkBox.Tag as string)
                .Where(deviceName => !string.IsNullOrWhiteSpace(deviceName))
                .ToList();
            if (deviceNames.Count == 0)
            {
                MessageBox.Show("Bitte mindestens ein Nanoleaf auswählen.", "Gerätegruppen", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_appSettings.NanoSettings.DeviceGroups.Any(group => group != _selectedGroup &&
                string.Equals(group.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Eine Gruppe mit diesem Namen existiert bereits.", "Gerätegruppen", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            if (MessageBox.Show($"Gruppe '{_selectedGroup.Name}' wirklich löschen?", "Gerätegruppen",
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
            Close();
        }
    }
}
