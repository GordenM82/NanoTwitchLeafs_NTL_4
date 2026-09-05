using log4net;
using Microsoft.Win32;
using NanoTwitchLeafs.Controller;
using NanoTwitchLeafs.Enums;
using NanoTwitchLeafs.Objects;
using NanoTwitchLeafs.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NanoTwitchLeafs.Windows
{
	/// <summary>
	/// Interaction logic for TriggerDetailWindow.xaml
	/// </summary>
	public partial class TriggerDetailWindow : Window
	{
		private readonly CommandRepository _commandRepository;
		private readonly StreamlabsController _streamLabsController;
		private readonly HypeRateIOController _hypeRateIoController;
		private readonly AppSettings _appSettings;
		private readonly AppSettingsController _appSettingsController;
		private readonly ILog _logger = LogManager.GetLogger(typeof(TriggerWindow));
		private readonly TwitchEventSubController _twitchEventSubController;
		private readonly bool _saveAsCopy;
		private bool _updatingTargetDevices;

		private string _channelPointsGuid;
		private TriggerSetting TriggerSetting { get; set; }

		public TriggerDetailWindow(AppSettings appSettings, AppSettingsController appSettingsController, CommandRepository commandRepository, List<string> effectList, StreamlabsController streamLabsController, HypeRateIOController hypeRateIoController, TriggerSetting triggerSetting = null, TwitchEventSubController eventSubController = null, bool saveAsCopy = false)
		{
			_commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
			_streamLabsController = streamLabsController;
			_hypeRateIoController = hypeRateIoController ?? throw new ArgumentNullException(nameof(hypeRateIoController));
			_appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
			_appSettingsController = appSettingsController ?? throw new ArgumentNullException(nameof(appSettingsController));
			_twitchEventSubController = eventSubController;
			_saveAsCopy = saveAsCopy;

			Constants.SetCultureInfo(_appSettings.Language);
			InitializeComponent();
			if (_saveAsCopy) Title = Text("Window_TriggerDetail_Duplicate_Title");
			Closed += TriggerDetailWindow_Closed;
			AllDevices_CheckBox.Content = Text("Window_TriggerDetail_TargetDevices_All");
			DeviceGroup_Label.Content = Text("Window_TriggerDetail_TargetDevices_Group");
			ApplyDeviceGroup_Button.Content = Text("Window_TriggerDetail_TargetDevices_Apply");
			ManageDeviceGroups_Button.Content = Text("Window_TriggerDetail_TargetDevices_Manage");
			NoDeviceGroups_TextBlock.Text = Text("Window_TriggerDetail_TargetDevices_NoGroups");
			DonationProvider_Label.Content = Text("P22_DonationProvider_Label");
			if (DonationProvider_ComboBox.Items[0] is ComboBoxItem allProviders)
				allProviders.Content = Text("P22_DonationProvider_All");
			DonationProvider_ComboBox.SelectedIndex = 0;
			InitializeTargetDevices();

			if (_twitchEventSubController != null && _twitchEventSubController.IsConnected)
			{
				_twitchEventSubController.OnChannelPointsRedeemed += TwitchEventSubController_OnChannelPointsRedeemed;
				_channelPointsGuid = "{00000000-0000-0000-0000-000000000000}";
			}

			foreach (var effect in effectList)
			{
				Effect_ComboBox.Items.Add(effect);
			}

			if (triggerSetting != null)
			{
				TriggerSetting = triggerSetting;
				InitData();
			}
		}

		private void InitializeTargetDevices()
		{
			TargetDevices_ItemsControl.Items.Clear();
			if (_appSettings.NanoSettings.DeviceGroups == null)
				_appSettings.NanoSettings.DeviceGroups = new List<NanoleafDeviceGroup>();
			RefreshDeviceGroups();
			TargetDevices_GroupBox.Header = string.Format(Text("Window_TriggerDetail_TargetDevices_Header"),
				_appSettings.NanoSettings.NanoLeafDevices.Count);

			foreach (NanoLeafDevice device in _appSettings.NanoSettings.NanoLeafDevices)
			{
				var deviceCheckBox = new CheckBox
				{
					Content = device.PublicName,
					Tag = device.DeviceName,
					IsChecked = true,
					Margin = new Thickness(5, 2, 15, 2),
					MinWidth = 145
				};
				deviceCheckBox.Checked += TargetDeviceCheckBox_Changed;
				deviceCheckBox.Unchecked += TargetDeviceCheckBox_Changed;
				TargetDevices_ItemsControl.Items.Add(deviceCheckBox);
			}

			UpdateTargetDeviceControls();
		}

		private void RefreshDeviceGroups()
		{
			DeviceGroup_ComboBox.ItemsSource = null;
			DeviceGroup_ComboBox.ItemsSource = _appSettings.NanoSettings.DeviceGroups
				.OrderBy(group => group.Name)
				.ToList();
			bool hasGroups = _appSettings.NanoSettings.DeviceGroups.Count > 0;
			DeviceGroup_ComboBox.IsEnabled = hasGroups;
			ApplyDeviceGroup_Button.IsEnabled = hasGroups;
			NoDeviceGroups_TextBlock.Visibility = hasGroups ? Visibility.Collapsed : Visibility.Visible;
			if (hasGroups)
				DeviceGroup_ComboBox.SelectedIndex = 0;
		}

		private void ApplyDeviceGroup_Button_Click(object sender, RoutedEventArgs e)
		{
			var group = DeviceGroup_ComboBox.SelectedItem as NanoleafDeviceGroup;
			if (group == null)
				return;

			var groupDevices = new HashSet<string>(group.DeviceNames ?? new List<string>(), StringComparer.OrdinalIgnoreCase);
			_updatingTargetDevices = true;
			AllDevices_CheckBox.IsChecked = false;
			foreach (CheckBox checkBox in TargetDevices_ItemsControl.Items.OfType<CheckBox>())
				checkBox.IsChecked = groupDevices.Contains(checkBox.Tag as string);
			_updatingTargetDevices = false;
			SynchronizeAllDevicesCheckBox();
		}

		private void ManageDeviceGroups_Button_Click(object sender, RoutedEventArgs e)
		{
			if (Application.Current.MainWindow is MainWindow mainWindow)
			{
				Hide();
				mainWindow.ShowEmbeddedDeviceGroups(() =>
				{
					RefreshDeviceGroups();
					Show();
					Activate();
				});
				return;
			}

			var window = new DeviceGroupsWindow(_appSettings, _appSettingsController);
			WindowPlacementService.PrepareOwnedWindow(window, this);
			window.ShowDialog();

			RefreshDeviceGroups();
		}

		private void AllDevices_CheckBox_Changed(object sender, RoutedEventArgs e)
		{
			if (_updatingTargetDevices)
				return;
			UpdateTargetDeviceControls();
		}

		private void UpdateTargetDeviceControls()
		{
			if (TargetDevices_ItemsControl == null)
				return;

			if (AllDevices_CheckBox.IsChecked != true)
				return;

			_updatingTargetDevices = true;
			foreach (CheckBox checkBox in TargetDevices_ItemsControl.Items.OfType<CheckBox>())
				checkBox.IsChecked = true;
			_updatingTargetDevices = false;
		}

		private void TargetDeviceCheckBox_Changed(object sender, RoutedEventArgs e)
		{
			if (!_updatingTargetDevices)
				SynchronizeAllDevicesCheckBox();
		}

		private void SynchronizeAllDevicesCheckBox()
		{
			var deviceCheckBoxes = TargetDevices_ItemsControl.Items.OfType<CheckBox>().ToList();
			_updatingTargetDevices = true;
			AllDevices_CheckBox.IsChecked = deviceCheckBoxes.Count > 0 && deviceCheckBoxes.All(checkBox => checkBox.IsChecked == true);
			_updatingTargetDevices = false;
		}

		private string GetSelectedTargetDeviceNames()
		{
			if (AllDevices_CheckBox.IsChecked == true)
				return null;

			return string.Join("|", TargetDevices_ItemsControl.Items.OfType<CheckBox>()
				.Where(checkBox => checkBox.IsChecked == true)
				.Select(checkBox => checkBox.Tag as string)
				.Where(deviceName => !string.IsNullOrWhiteSpace(deviceName)));
		}

		private void TwitchEventSubController_OnChannelPointsRedeemed(string username, string promt, string guid)
		{
			if (!string.Equals(_appSettings.ChannelName, username, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			Dispatcher.BeginInvoke(new Action(() => channelPointsDetection_Label.Foreground = Brushes.Green));
			Dispatcher.BeginInvoke(new Action(() => channelPointsDetection_Label.Text = string.Format(Properties.Resources.Code_TriggerDetail_Label_CPGuid, guid)));
			_channelPointsGuid = guid;
		}

		public TriggerDetailWindow()
		{
			InitializeComponent();
		}

		private void InitData()
		{
			SelectDonationProvider(TriggerSetting.DonationProvider);
			var selectedDeviceNames = new HashSet<string>(
				(TriggerSetting.TargetDeviceNames ?? string.Empty)
					.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
			bool allDevices = selectedDeviceNames.Count == 0;
			_updatingTargetDevices = true;
			AllDevices_CheckBox.IsChecked = allDevices;
			foreach (CheckBox checkBox in TargetDevices_ItemsControl.Items.OfType<CheckBox>())
			{
				checkBox.IsChecked = allDevices || selectedDeviceNames.Contains(checkBox.Tag as string);
			}
			_updatingTargetDevices = false;
			UpdateTargetDeviceControls();

			// Set On/Off Slider State
			if (TriggerSetting.IsActive == true)
			{
				OnOff_Slider.Value = 0;
				OnOff_Slider.Background = Brushes.LimeGreen;
			}
			else
			{
				OnOff_Slider.Value = 1;
				OnOff_Slider.Background = Brushes.White;
			}

			// Set Selected Item in Effect Dropdown
			foreach (string effect in Effect_ComboBox.Items)
			{
				if (effect == TriggerSetting.Effect)
				{
					Effect_ComboBox.SelectedItem = effect;
				}
			}

			// Set Radio Button for Effect or Color
			if (TriggerSetting.IsColor)
			{
				Effect_RadioButton.IsChecked = false;
				Effect_ComboBox.IsEnabled = false;
				Color_RadioButton.IsChecked = true;
				Color_RadioButton.IsEnabled = true;
				ColorPicker.IsEnabled = true;
			}

			// Set Color Picker to saved Color
			Color color = new Color { R = TriggerSetting.R, G = TriggerSetting.G, B = TriggerSetting.B, A = 255 };
			ColorPicker.SelectedColor = color;

			// Fill Command/Keyword Textbox
			CommandKeyword_Textbox.Text = TriggerSetting.CMD;

			// Fill SoundEffectPath Textbox
			SoundFilePath_Textbox.Text = TriggerSetting.SoundFilePath;
			if (!string.IsNullOrWhiteSpace(TriggerSetting.SoundFilePath))
			{
				SoundFilePath_Textbox.IsEnabled = true;
			}

			// Check for HypeRate Service Connected
			if (!_hypeRateIoController._isConnected)
			{
				HypeRate_RadioButton.IsEnabled = false;
			}

			// Check for Streamlabs Websocket Connection
			if ((_streamLabsController == null || !_streamLabsController._IsSocketConnected) &&
				(_appSettings.StreamElements == null || !_appSettings.StreamElements.Enabled))
			{
				Donation_RadioButton.IsEnabled = false;
			}

			// Fill Options Texboxes and Checkboxes
			Duration_Textbox.Text = TriggerSetting.Duration.ToString();
			Brightness_Textbox.Text = TriggerSetting.Brightness.ToString();
			Amount_Textbox.Text = TriggerSetting.Amount.ToString();
			Cooldown_Textbox.Text = TriggerSetting.Cooldown.ToString();
			Volume_Textbox.Text = TriggerSetting.Volume.ToString();
			_channelPointsGuid = TriggerSetting.ChannelPointsGuid;

			Viponly_Checkbox.IsChecked = TriggerSetting.VipOnly;
			Subonly_Checkbox.IsChecked = TriggerSetting.SubscriberOnly;
			Modonly_Checkbox.IsChecked = TriggerSetting.ModeratorOnly;

			if (TriggerSetting.ChannelPointsGuid != null && TriggerSetting.ChannelPointsGuid != "{00000000-0000-0000-0000-000000000000}")
			{
				Dispatcher.BeginInvoke(new Action(() => channelPointsDetection_Label.Foreground = Brushes.Green));
				Dispatcher.BeginInvoke(new Action(() => channelPointsDetection_Label.Text = string.Format(Properties.Resources.Code_TriggerDetail_Label_CPGuidSet, TriggerSetting.ChannelPointsGuid)));
			}

			SetControlsEnabled();

			Checkbox_Click(null, null);
		}

		private void SetControlsEnabled()
		{
			// Set Controls Enabled State && Radio Buttons
			switch (TriggerSetting.Trigger)
			{
				case "Command":
					Cmd_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = true;
					Amount_Textbox.IsEnabled = false;
					Viponly_Checkbox.IsEnabled = true;
					Subonly_Checkbox.IsEnabled = true;
					Modonly_Checkbox.IsEnabled = true;
					Cooldown_Textbox.IsEnabled = true;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					if (_appSettings.NanoSettings.ChangeBackOnCommand)
					{
						Duration_Textbox.IsEnabled = true;
					}
					else
					{
						Duration_Textbox.IsEnabled = false;
					}
					break;

				case "Subscription":
					NewSub_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "ReSubscription":
					Resub_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "GiftSubscription":
					Giftsub_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "GiftBomb":
					GiftBomb_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = true;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "AnonGiftSubscription":
					AnonGiftSub_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "AnonGiftBomb":
					AnonGiftBomb_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = true;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "HypeTrain":
					HypeTrain_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = true;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "Raid":
					Raid_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "Follower":
					Follower_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "Bits":
					Bits_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = true;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					break;

				case "Keyword":
					Keyword_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = true;
					Amount_Textbox.IsEnabled = false;
					Viponly_Checkbox.IsEnabled = true;
					Subonly_Checkbox.IsEnabled = true;
					Modonly_Checkbox.IsEnabled = true;
					Cooldown_Textbox.IsEnabled = true;
					Channelpoints_Grid.Visibility = Visibility.Hidden;
					if (_appSettings.NanoSettings.ChangeBackOnKeyword)
					{
						Duration_Textbox.IsEnabled = true;
					}
					else
					{
						Duration_Textbox.IsEnabled = false;
					}
					break;

				case "ChannelPoints":
					Channelpoints_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = true;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Channelpoints_Grid.Visibility = Visibility.Visible;
					break;

				case "HypeRate":
					HypeRate_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = true;
					Duration_Textbox.IsEnabled = false;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					break;

				case "Donation":
					Donation_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = true;
					Duration_Textbox.IsEnabled = false;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					DonationProvider_Grid.Visibility = Visibility.Visible;
					break;

				case "UsernameColor":
					UserColor_RadioButton.IsChecked = true;
					CommandKeyword_Textbox.IsEnabled = false;
					Amount_Textbox.IsEnabled = false;
					Duration_Textbox.IsEnabled = false;
					Viponly_Checkbox.IsEnabled = false;
					Subonly_Checkbox.IsEnabled = false;
					Modonly_Checkbox.IsEnabled = false;
					Cooldown_Textbox.IsEnabled = false;
					Effect_RadioButton.IsEnabled = false;
					Color_RadioButton.IsEnabled = false;
					ColorPicker.IsEnabled = false;
					Effect_ComboBox.IsEnabled = false;
					break;
			}

			if (string.IsNullOrWhiteSpace(SoundFilePath_Textbox.Text))
			{
				SoundFilePath_Textbox.IsEnabled = true;
				Volume_Textbox.IsEnabled = true;
			}
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (e.NewValue == 0)
			{
				OnOff_Slider.Background = Brushes.LimeGreen;
			}
			else
			{
				OnOff_Slider.Background = Brushes.White;
			}
		}

		private void Save_Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveTrigger();
			}
			catch (Exception exception)
			{
				_logger.Error("Trigger could not be saved.", exception);
				MessageBox.Show(
					Text("Window_TriggerDetail_Save_Error"),
					Properties.Resources.General_MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SaveTrigger()
		{
			// Do Value checks before attempt to Save
			if (!ValueChecks())
			{
				return;
			}

			TryReadInt(Brightness_Textbox.Text, out int brightness);
			TryReadInt(Volume_Textbox.Text, out int volume);
			TryReadDouble(Amount_Textbox.Text, out double amount);
			TryReadDouble(Duration_Textbox.Text, out double duration);
			TryReadDouble(Cooldown_Textbox.Text, out double cooldown);

			if (AllDevices_CheckBox.IsChecked != true &&
				!TargetDevices_ItemsControl.Items.OfType<CheckBox>().Any(checkBox => checkBox.IsChecked == true))
			{
				MessageBox.Show(Text("Code_TriggerDetail_MessageBox_TargetRequired"), Properties.Resources.General_MessageBox_Error_Title,
					MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Get all Triggers
			List<TriggerSetting> triggerSettings = _commandRepository.GetList().ToList();

			// If Trigger already exists
			if (TriggerSetting != null && !_saveAsCopy)
			{
				// Search for existing Trigger and Remove it from List
				foreach (TriggerSetting setting in triggerSettings)
				{
					if (setting.ID == TriggerSetting.ID)
					{
						triggerSettings.Remove(setting);
						break;
					}
				}
			}

			string triggerType = "";
			Color color = new Color { R = 0, G = 0, B = 0, A = 255 };

			// Get Activated Radio Button
			if (Cmd_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Command.ToString();
			}
			if (Keyword_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Keyword.ToString();
			}
			if (Follower_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Follower.ToString();
			}
			if (Bits_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Bits.ToString();
			}
			if (HypeTrain_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.HypeTrain.ToString();
			}
			if (Raid_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Raid.ToString();
			}
			if (NewSub_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Subscription.ToString();
			}
			if (Resub_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.ReSubscription.ToString();
			}
			if (AnonGiftSub_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.AnonGiftSubscription.ToString();
			}
			if (Giftsub_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.GiftSubscription.ToString();
			}
			if (AnonGiftBomb_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.AnonGiftBomb.ToString();
			}
			if (GiftBomb_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.GiftBomb.ToString();
			}
			if (Channelpoints_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.ChannelPoints.ToString();
			}
			if (HypeRate_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.HypeRate.ToString();
			}
			if (Donation_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.Donation.ToString();
			}

			if (UserColor_RadioButton.IsChecked == true)
			{
				triggerType = TriggerTypeEnum.UsernameColor.ToString();
			}

			// Get Status of Trigger
			bool isActive;
			if (OnOff_Slider.Value == 0)
			{
				isActive = true;
			}
			else
			{
				isActive = false;
			}

			// Check if Effect is Color or not
			bool IsColor = false;
			string effect = "";
			if (Effect_RadioButton.IsChecked == false)
			{
				IsColor = true;
			}
			else
			{
				if (UserColor_RadioButton.IsChecked == true)
				{
					effect = "UserColor";
				}
				else
				{
					effect = Effect_ComboBox.SelectedItem?.ToString() ?? string.Empty;
				}
			}
			// Check for Invalid CP GUID
			if (triggerType == "ChannelPoints" && _channelPointsGuid == "{00000000-0000-0000-0000-000000000000}")
			{
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_NoRewardDetected, Properties.Resources.General_MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			// Get RGB Values from Color Picker
			if (ColorPicker.SelectedColor == null && IsColor)
			{
				MessageBox.Show(Properties.Resources.Window_TriggerDetail_ColorPicker_Error, Properties.Resources.General_MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			else if (ColorPicker.SelectedColor != null && IsColor)
			{
				color = (Color)ColorPicker.SelectedColor;
			}

			// Create new TriggerSetting
			TriggerSetting newTriggerSetting = new TriggerSetting
			{
				CMD = CommandKeyword_Textbox.Text,
				IsActive = isActive,
				Brightness = brightness,
				Cooldown = cooldown,
				Duration = duration,
				IsColor = IsColor,
				Effect = effect,
				R = color.R,
				G = color.G,
				B = color.B,
				Amount = amount,
				Volume = volume,
				Trigger = triggerType,
				SoundFilePath = SoundFilePath_Textbox.Text,
				VipOnly = Viponly_Checkbox.IsChecked == true,
				ModeratorOnly = Modonly_Checkbox.IsChecked == true,
				SubscriberOnly = Subonly_Checkbox.IsChecked == true,
				DonationProvider = (DonationProvider_ComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All",
				TargetDeviceNames = GetSelectedTargetDeviceNames()
			};

			if (newTriggerSetting.ChannelPointsGuid == "{00000000-0000-0000-0000-000000000000}")
			{
				newTriggerSetting.ChannelPointsGuid = _channelPointsGuid;
			}

			// Add New Trigger Setting to the existing Triggers in List
			triggerSettings.Add(newTriggerSetting);

			_commandRepository.ReplaceAll(triggerSettings);

			_logger.Info($"Saved Trigger to Database. There are currently {triggerSettings.Count} Trigger.");
			MessageBox.Show(Properties.Resources.General_MessageBox_SettingsSaved, Properties.Resources.General_MessageBox_Sucess_Title, MessageBoxButton.OK, MessageBoxImage.Information);
			Close();
		}

		private bool ValueChecks()
		{
			if (UserColor_RadioButton.IsChecked == false && Effect_RadioButton.IsChecked == true && (Effect_ComboBox.SelectedValue == null || string.IsNullOrWhiteSpace(Effect_ComboBox.SelectedValue.ToString())))
			{
				_logger.Error("Please choose an Effect for your Trigger! Can not be Empty!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_NoEffectSelected, Properties.Resources.General_MessageBox_Error_Title);
				return false;
			}

			if (string.IsNullOrWhiteSpace(CommandKeyword_Textbox.Text) && (Cmd_RadioButton.IsChecked == true || Keyword_RadioButton.IsChecked == true))
			{
				_logger.Warn("Please enter a Command/Keyword for your Trigger! Can not be Empty!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_CmdBoxEmpty, Properties.Resources.General_MessageBox_Error_Title);
				return false;
			}

			if (!TryReadInt(Brightness_Textbox.Text, out int brightness) || brightness > 100 || brightness < 0)
			{
				_logger.Warn("Please enter a Brightness Value between 0 and 100! Can not be Empty!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_BrightnessValue, Properties.Resources.General_MessageBox_Error_Title);
				Brightness_Textbox.Text = "50";
				return false;
			}

			if (!TryReadInt(Volume_Textbox.Text, out int volume) || volume > 100 || volume < 0)
			{
				_logger.Warn("Please enter a Volume Value between 0 and 100! Can not be Empty!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_VolumeValue, Properties.Resources.General_MessageBox_Error_Title);
				Volume_Textbox.Text = "50";
				return false;
			}

			if (!TryReadDouble(Amount_Textbox.Text, out double amount) || amount < 0)
			{
				_logger.Warn("Please enter a Amount Value even if you dont use it! Can not be Empty or Negative!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_AmountValue, Properties.Resources.General_MessageBox_Error_Title);
				Amount_Textbox.Text = "0";
				return false;
			}
			if (!TryReadDouble(Duration_Textbox.Text, out double duration) || duration < 0)
			{
				_logger.Warn("Please enter a Duration Value even if you dont use it! Can not be Empty or Negative!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_DurationValue, Properties.Resources.General_MessageBox_Error_Title);
				Duration_Textbox.Text = "0";
				return false;
			}
			if (!TryReadDouble(Cooldown_Textbox.Text, out double cooldown) || cooldown < 0)
			{
				_logger.Warn("Please enter a Cooldown Value even if you dont use it! Enter 0 to disable the Trigger Cooldown! Can not be Empty!");
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_CooldownValue, Properties.Resources.General_MessageBox_Error_Title);
				Cooldown_Textbox.Text = "0";
				return false;
			}

			return true;
		}

		private static bool TryReadInt(string text, out int value)
		{
			return int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out value) ||
				int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
		}

		private static bool TryReadDouble(string text, out double value)
		{
			if (string.IsNullOrWhiteSpace(text)) { value = 0; return false; }
			string normalized = text.Trim().Replace(',', '.');
			return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}

		private void TriggerHelp_Button_Click(object sender, RoutedEventArgs e)
		{
			if (Application.Current.MainWindow is MainWindow mainWindow)
			{
				Hide();
				mainWindow.ShowHelp(MainWindow.HelpTopic.Trigger, () =>
				{
					Show();
					Activate();
				});
			}
		}

		#region Ui Stuff

		private void SoundFilePath_Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(SoundFilePath_Textbox.Text))
			{
				SoundFilePath_Textbox.IsEnabled = false;
				Volume_Textbox.IsEnabled = false;
			}
			else
			{
				SoundFilePath_Textbox.IsEnabled = true;
				Volume_Textbox.IsEnabled = true;
			}
		}

		private void SelectSoundFilePath_Button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Audio (*.mp3;*.wav)|*.mp3;*.wav" };
			if (openFileDialog.ShowDialog() == true)
			{
				SoundFilePath_Textbox.Text = openFileDialog.FileName;
			}

			if (Path.GetExtension(SoundFilePath_Textbox.Text) != ".mp3" && Path.GetExtension(SoundFilePath_Textbox.Text) != ".wav" && !string.IsNullOrWhiteSpace(SoundFilePath_Textbox.Text))
			{
				MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_SoundfileFormat, Properties.Resources.General_MessageBox_Error_Title);
				SoundFilePath_Textbox.Text = "";
			}
		}

		private void brightness_TextBoxKeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				// Check for Value Prefix
				if (!TryReadInt(Brightness_Textbox.Text, out int brightness) || brightness > 100 || brightness < 0)
				{
					Brightness_Textbox.BorderBrush = Brushes.Red;
				}
				else
				{
					Brightness_Textbox.BorderBrush = Brushes.SlateGray;
				}
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
			}
		}

		private void Volume_Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			bool valid = TryReadInt(Volume_Textbox.Text, out int result) && result >= 0 && result <= 100;
			Volume_Textbox.BorderBrush = valid ? Brushes.SlateGray : Brushes.Red;
		}

		#endregion

		private static string Text(string key) => Properties.Resources.ResourceManager.GetString(key);

		private void SelectDonationProvider(string provider)
		{
			string selectedProvider = string.IsNullOrWhiteSpace(provider) ? "All" : provider;
			foreach (ComboBoxItem item in DonationProvider_ComboBox.Items)
			{
				if (string.Equals(item.Tag?.ToString(), selectedProvider, StringComparison.OrdinalIgnoreCase))
				{
					DonationProvider_ComboBox.SelectedItem = item;
					return;
				}
			}
			DonationProvider_ComboBox.SelectedIndex = 0;
		}

		private void Checkbox_Click(object sender, RoutedEventArgs e)
		{
			if (Viponly_Checkbox.IsEnabled != true)
			{
				Vipsubmod_Textbox.Text = Text("Window_TriggerDetail_RoleFilters_Only");
				return;
			}
			var titles = new List<string>();
			if (Viponly_Checkbox.IsChecked == true)
				titles.Add("VIP");
			if (Subonly_Checkbox.IsChecked == true)
				titles.Add("Sub");
			if (Modonly_Checkbox.IsChecked == true)
				titles.Add("Mod");

			if (titles.Count == 0)
				titles.Add(Properties.Resources.Code_TriggerDetail_Label_NothingSpecial);

			string joinedString = string.Join(" & ", titles);
			Vipsubmod_Textbox.Text = string.Format(Properties.Resources.Code_TriggerDetail_Label_VipSubMod, joinedString);
		}

		private void TriggerRadioButton_Click(object sender, RoutedEventArgs e)
		{
			DonationProvider_Grid.Visibility = Donation_RadioButton.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
			if (Cmd_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = true;
				Amount_Textbox.IsEnabled = false;
				Viponly_Checkbox.IsEnabled = true;
				Subonly_Checkbox.IsEnabled = true;
				Modonly_Checkbox.IsEnabled = true;
				Cooldown_Textbox.IsEnabled = true;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
				if (_appSettings.NanoSettings.ChangeBackOnCommand)
				{
					Duration_Textbox.IsEnabled = true;
				}
				else
				{
					Duration_Textbox.IsEnabled = false;
				}
			}

			if (NewSub_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (Resub_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (Giftsub_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (AnonGiftSub_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (GiftBomb_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = true;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (AnonGiftBomb_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = true;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (HypeTrain_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = true;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (Raid_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (Follower_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (Bits_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = true;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
			}

			if (Keyword_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = true;
				Amount_Textbox.IsEnabled = false;
				Viponly_Checkbox.IsEnabled = true;
				Subonly_Checkbox.IsEnabled = true;
				Modonly_Checkbox.IsEnabled = true;
				Cooldown_Textbox.IsEnabled = true;
				Channelpoints_Grid.Visibility = Visibility.Hidden;
				if (_appSettings.NanoSettings.ChangeBackOnKeyword)
				{
					Duration_Textbox.IsEnabled = true;
				}
				else
				{
					Duration_Textbox.IsEnabled = false;
				}
			}

			if (Channelpoints_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = true;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Channelpoints_Grid.Visibility = Visibility.Visible;
				if (_twitchEventSubController == null || !_twitchEventSubController.IsConnected)
				{
					MessageBox.Show(Properties.Resources.Code_TriggerDetail_MessageBox_CPNoConnection, Properties.Resources.General_MessageBox_Hint_Title, MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}

			if (HypeRate_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = true;
				Duration_Textbox.IsEnabled = false;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
			}

			if (Donation_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = true;
				Duration_Textbox.IsEnabled = false;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				DonationProvider_Grid.Visibility = Visibility.Visible;
			}

			if (UserColor_RadioButton.IsChecked == true)
			{
				CommandKeyword_Textbox.IsEnabled = false;
				Amount_Textbox.IsEnabled = false;
				Duration_Textbox.IsEnabled = false;
				Viponly_Checkbox.IsEnabled = false;
				Subonly_Checkbox.IsEnabled = false;
				Modonly_Checkbox.IsEnabled = false;
				Cooldown_Textbox.IsEnabled = false;
				Effect_RadioButton.IsEnabled = false;
				Color_RadioButton.IsEnabled = false;
				ColorPicker.IsEnabled = false;
				Effect_ComboBox.IsEnabled = false;
			}

			Checkbox_Click(null, null);
		}

		private void EffectRadioButton_Click(object sender, RoutedEventArgs e)
		{
			if (Effect_RadioButton.IsChecked == true)
			{
				Effect_ComboBox.IsEnabled = true;
				ColorPicker.IsEnabled = false;
			}
			else
			{
				Effect_ComboBox.IsEnabled = false;
				ColorPicker.IsEnabled = true;
			}
		}

		private void TriggerDetailWindow_Closed(object sender, EventArgs e)
		{
			if (_twitchEventSubController != null)
				_twitchEventSubController.OnChannelPointsRedeemed -= TwitchEventSubController_OnChannelPointsRedeemed;
		}
	}
}
