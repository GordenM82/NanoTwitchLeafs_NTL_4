using log4net;
using NanoTwitchLeafs.Colors;
using NanoTwitchLeafs.Controller;
using NanoTwitchLeafs.Enums;
using NanoTwitchLeafs.Objects;
using NanoTwitchLeafs.Repositories;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace NanoTwitchLeafs.Windows
{
    public partial class TriggerWindow : Window
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(TriggerWindow));
        private readonly CommandRepository _commandRepository;
        private readonly NanoController _nanoController;
        private readonly AppSettings _appSettings;
        private readonly AppSettingsController _appSettingsController;
        private readonly StreamlabsController _streamlabsController;
        private readonly HypeRateIOController _hypeRateIoController;
        private readonly TriggerLogicController _triggerLogicController;
        public readonly TwitchEventSubController _twitchEventSubController;
        private bool _isLoadingTriggers;
        private bool _sliderChangeRequestedByUser;
        private List<TriggerListObject> _allTriggerItems = new List<TriggerListObject>();

        public TriggerWindow(CommandRepository commandRepository, NanoController nanoController, AppSettings appSettings, AppSettingsController appSettingsController, StreamlabsController streamlabsController, HypeRateIOController hypeRateIoController, TriggerLogicController triggerLogicController, TwitchEventSubController twitchEventSubController = null)
        {
            _commandRepository = commandRepository ?? throw new ArgumentNullException(nameof(commandRepository));
            _nanoController = nanoController ?? throw new ArgumentNullException(nameof(nanoController));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _appSettingsController = appSettingsController ?? throw new ArgumentNullException(nameof(appSettingsController));
            _streamlabsController = streamlabsController;
            _hypeRateIoController = hypeRateIoController;
            _triggerLogicController = triggerLogicController ?? throw new ArgumentNullException(nameof(triggerLogicController));
            _twitchEventSubController = twitchEventSubController;
            Constants.SetCultureInfo(_appSettings.Language);
            InitializeComponent();
			triggerSearch_TextBox.SetResourceReference(FocusVisualStyleProperty, "NtlFocusVisualStyle");
			Trigger_Listview.SetResourceReference(FocusVisualStyleProperty, "NtlFocusVisualStyle");

            triggerHeading_TextBlock.Text = Text("Window_Trigger_Heading");
            triggerDescription_TextBlock.Text = Text("Window_Trigger_Description");
            targetDevices_Column.Header = Text("Window_Trigger_Header_TargetDevices");
            test_Column.Header = Text("Window_Trigger_Action_Test");
            duplicate_Column.Header = Text("Window_Trigger_Action_Copy");
            edit_Column.Header = Text("Window_Trigger_Action_Edit");
            delete_Column.Header = Text("Window_Trigger_Action_Delete");
            importCmd_Button.Content = Text("Window_Trigger_Button_Import");
            exportCmd_Button.Content = Text("Window_Trigger_Button_Export");
            triggerSearch_TextBox.ToolTip = Text("P24_Trigger_Search");
            triggerResetFilter_Button.Content = Text("P24_Trigger_Reset");
            SetFilterTexts();
            triggerStatusFilter_ComboBox.SelectedIndex = 0;
            triggerTypeFilter_ComboBox.SelectedIndex = 0;

            SafeLoadTrigger();
            Dispatcher.BeginInvoke(new Action(() => { triggerSearch_TextBox.Focus(); Keyboard.Focus(triggerSearch_TextBox); }));
        }

        private static string Text(string key) => Properties.Resources.ResourceManager.GetString(key) ?? key;

        private void ShowError(string resourceKey, Exception exception = null)
        {
            string message = Text(resourceKey);
            if (exception != null) _logger.Error(message, exception);
            MessageBox.Show(message,
                Properties.Resources.General_MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SafeLoadTrigger()
        {
            _isLoadingTriggers = true;
            try { LoadTrigger(); }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Load", ex); }
            finally { _isLoadingTriggers = false; }
        }

        public void RefreshTriggerList() => SafeLoadTrigger();

        private void LoadTrigger()
        {
            string selectedId = (Trigger_Listview.SelectedItem as TriggerListObject)?.ID;
            List<TriggerSetting> triggerSettings = _commandRepository.GetList().ToList();
            triggerSettings.OrderBy(x => x.ID);
            List<TriggerListObject> TriggerListItems = new List<TriggerListObject>();
            foreach (TriggerSetting triggerSetting in triggerSettings)
            {
                int OnOffSliderValue = 0;
                var OnOffSliderBackground = Brushes.White;
                if (triggerSetting.IsActive.HasValue && triggerSetting.IsActive.Value)
                {
                    OnOffSliderValue = 0;
                    OnOffSliderBackground = Brushes.LimeGreen;
                }
                else
                {
                    OnOffSliderValue = 1;
                    OnOffSliderBackground = Brushes.White;
                }

                string soundEffect = "X";
                if (!string.IsNullOrWhiteSpace(triggerSetting.SoundFilePath))
                {
                    string[] soundEffectArray = triggerSetting.SoundFilePath.Split('\\');

                    soundEffect = soundEffectArray[soundEffectArray.Length - 1];
                }

                string vipsubmod = "Vip[N] Sub[N] Mod[N]";
                if (triggerSetting.VipOnly)
                {
                    vipsubmod = vipsubmod.Replace("Vip[N]", "Vip[Y]");
                }
                if (triggerSetting.SubscriberOnly)
                {
                    vipsubmod = vipsubmod.Replace("Sub[N]", "Sub[Y]");
                }
                if (triggerSetting.ModeratorOnly)
                {
                    vipsubmod = vipsubmod.Replace("Mod[N]", "Mod[Y]");
                }

                string targetDevices = Properties.Resources.ResourceManager.GetString("Window_Trigger_TargetDevices_All");
                if (!string.IsNullOrWhiteSpace(triggerSetting.TargetDeviceNames))
                {
                    var selectedDeviceNames = new HashSet<string>(
                        triggerSetting.TargetDeviceNames.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries),
                        StringComparer.OrdinalIgnoreCase);
                    var publicNames = (_appSettings.NanoSettings?.NanoLeafDevices ?? new List<NanoLeafDevice>())
                        .Where(device => selectedDeviceNames.Contains(device.DeviceName))
                        .Select(device => device.PublicName)
                        .ToList();
                    targetDevices = publicNames.Count > 0
						? string.Join(", ", publicNames)
						: Properties.Resources.ResourceManager.GetString("Window_Trigger_TargetDevices_Unavailable");
                }

                Button editButton = new Button
                {
                    Width = 40,
                    Height = 22,
                    Margin = new Thickness(12, 3, 0, 3),
                    Name = "TriggerEdit_Button_" + triggerSetting.ID.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = Properties.Resources.Window_Trigger_Button_Edit
                };

                editButton.Click += EditButton_Click;

                Button delete_Button = new Button
                {
                    Width = 40,
                    Height = 22,
                    Margin = new Thickness(12, 3, 0, 3),
                    Name = "TriggerDelete_Button_" + triggerSetting.ID.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Content = Properties.Resources.Window_Trigger_Button_Delete
                };

                delete_Button.Click += DeleteButton_Click;

                TriggerListObject triggerListObject = new TriggerListObject
                {
                    OnOffSliderValue = OnOffSliderValue,
                    OnOffSliderBackground = OnOffSliderBackground,
                    ID = triggerSetting.ID.ToString(),
                    Trigger = triggerSetting.Trigger,
                    Command = triggerSetting.CMD,
                    Sound = soundEffect,
                    Duration = triggerSetting.Duration.ToString(),
                    Brightness = triggerSetting.Brightness.ToString(),
                    Amount = triggerSetting.Amount.ToString(),
                    Cooldown = triggerSetting.Cooldown.ToString(),
                    VipSubMod = vipsubmod,
                    TargetDevices = targetDevices,
                    DuplicateText = Text("Window_Trigger_Action_Copy"),
                    IsActive = triggerSetting.IsActive == true,
                };

                if (triggerListObject.Trigger != TriggerTypeEnum.Command.ToString() && triggerListObject.Trigger != TriggerTypeEnum.Keyword.ToString())
                {
                    triggerListObject.Command = "/";
                }

                var color = ColorConverting.RgbToMediacolor(new RgbColor(triggerSetting.R, triggerSetting.G, triggerSetting.B, 255));
                if (!triggerSetting.IsColor)
                {
                    triggerListObject.Effect = triggerSetting.Effect;
                }
                else
                {
                    triggerListObject.Background = new SolidColorBrush(color);
                }

                List<string> warnings = GetTriggerWarnings(triggerSetting);
                triggerListObject.HasProblem = warnings.Count > 0;
                triggerListObject.WarningText = string.Join(Environment.NewLine, warnings);
                triggerListObject.SearchText = string.Join(" ", new[]
                {
                    triggerListObject.Trigger, triggerListObject.Command, triggerListObject.Effect,
                    triggerListObject.Sound, triggerListObject.TargetDevices, triggerSetting.DonationProvider
                }.Where(value => !string.IsNullOrWhiteSpace(value)));

                TriggerListItems.Add(triggerListObject);
                _logger.Debug($"Loading Trigger with id {triggerSetting.ID}.");
            }
            _allTriggerItems = TriggerListItems.OrderBy(x => x.Trigger).ToList();
            ApplyTriggerFilters(selectedId);
            _logger.Info($"Loaded {triggerSettings.Count} Triggers from Database.");
        }

        private void SetFilterTexts()
        {
            SetComboText(triggerStatusFilter_ComboBox, "All", Text("P24_Filter_All"));
            SetComboText(triggerStatusFilter_ComboBox, "Active", Text("P24_Filter_Active"));
            SetComboText(triggerStatusFilter_ComboBox, "Inactive", Text("P24_Filter_Inactive"));
            SetComboText(triggerStatusFilter_ComboBox, "Problems", Text("P24_Filter_Problems"));
            SetComboText(triggerTypeFilter_ComboBox, "All", Text("P24_Filter_AllTypes"));
            SetComboText(triggerTypeFilter_ComboBox, "Chat", Text("P24_Filter_Chat"));
            SetComboText(triggerTypeFilter_ComboBox, "Twitch", Text("P24_Filter_Twitch"));
            SetComboText(triggerTypeFilter_ComboBox, "ChannelPoints", Text("P24_Filter_ChannelPoints"));
            SetComboText(triggerTypeFilter_ComboBox, "Donation", Text("P24_Filter_Donations"));
            SetComboText(triggerTypeFilter_ComboBox, "HypeRate", "HypeRate");
        }

        private static void SetComboText(ComboBox comboBox, string tag, string text)
        {
            foreach (ComboBoxItem item in comboBox.Items)
                if (string.Equals(item.Tag?.ToString(), tag, StringComparison.OrdinalIgnoreCase)) item.Content = text;
        }

        private List<string> GetTriggerWarnings(TriggerSetting trigger)
        {
            var warnings = new List<string>();
            var devices = _appSettings.NanoSettings?.NanoLeafDevices ?? new List<NanoLeafDevice>();
            if (devices.Count == 0)
                warnings.Add(Text("P24_Warning_NoDevices"));
            else if (!string.IsNullOrWhiteSpace(trigger.TargetDeviceNames))
            {
                var knownNames = new HashSet<string>(devices.Select(device => device.DeviceName), StringComparer.OrdinalIgnoreCase);
                if (trigger.TargetDeviceNames.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Any(name => !knownNames.Contains(name)))
                    warnings.Add(Text("P24_Warning_MissingDevice"));
            }
            if (!trigger.IsColor && string.IsNullOrWhiteSpace(trigger.Effect))
                warnings.Add(Text("P24_Warning_MissingEffect"));
            if (!string.IsNullOrWhiteSpace(trigger.SoundFilePath) && !File.Exists(trigger.SoundFilePath))
                warnings.Add(Text("P24_Warning_MissingSound"));
            return warnings;
        }

        private void TriggerFilter_Changed(object sender, RoutedEventArgs e)
        {
            if (!_isLoadingTriggers) ApplyTriggerFilters();
        }

        private void TriggerResetFilter_Button_Click(object sender, RoutedEventArgs e)
        {
            triggerSearch_TextBox.Clear();
            triggerStatusFilter_ComboBox.SelectedIndex = 0;
            triggerTypeFilter_ComboBox.SelectedIndex = 0;
            ApplyTriggerFilters();
        }

        private void ApplyTriggerFilters(string selectedId = null)
        {
            if (Trigger_Listview == null || triggerStatusFilter_ComboBox == null || triggerTypeFilter_ComboBox == null) return;
            string search = triggerSearch_TextBox.Text?.Trim() ?? string.Empty;
            string status = (triggerStatusFilter_ComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All";
            string type = (triggerTypeFilter_ComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All";
            IEnumerable<TriggerListObject> filtered = _allTriggerItems;
            if (!string.IsNullOrWhiteSpace(search))
                filtered = filtered.Where(item => item.SearchText?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);
            filtered = status switch
            {
                "Active" => filtered.Where(item => item.IsActive),
                "Inactive" => filtered.Where(item => !item.IsActive),
                "Problems" => filtered.Where(item => item.HasProblem),
                _ => filtered
            };
            filtered = type switch
            {
                "Chat" => filtered.Where(item => item.Trigger is "Command" or "Keyword"),
                "Twitch" => filtered.Where(item => item.Trigger is "Follower" or "Subscription" or "ReSubscription" or "GiftSubscription" or "AnonGiftSubscription" or "GiftBomb" or "AnonGiftBomb" or "Bits" or "Raid" or "HypeTrain" or "UsernameColor"),
                "ChannelPoints" => filtered.Where(item => item.Trigger == "ChannelPoints"),
                "Donation" => filtered.Where(item => item.Trigger == "Donation"),
                "HypeRate" => filtered.Where(item => item.Trigger == "HypeRate"),
                _ => filtered
            };
            List<TriggerListObject> results = filtered.ToList();
            Trigger_Listview.ItemsSource = results;
            triggerResultCount_TextBlock.Text = string.Format(Text("P24_Trigger_Count"), results.Count, _allTriggerItems.Count);
            if (!string.IsNullOrWhiteSpace(selectedId))
                Trigger_Listview.SelectedItem = results.FirstOrDefault(item => item.ID == selectedId);
        }

        private void Trigger_Listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not DependencyObject source || FindVisualParent<Button>(source) != null) return;
            ListViewItem clickedItem = FindVisualParent<ListViewItem>(source);
            if (clickedItem?.DataContext is TriggerListObject row && int.TryParse(row.ID, out int id))
                OpenTriggerDetails(_commandRepository.GetList().FirstOrDefault(item => item.ID == id));
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T match) return match;
                child = child is ContentElement content
                    ? ContentOperations.GetParent(content) ?? (content as FrameworkContentElement)?.Parent
                    : VisualTreeHelper.GetParent(child);
            }
            return null;
        }

        private void OnOffSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isLoadingTriggers || !_sliderChangeRequestedByUser || sender is not Slider slider || !slider.IsLoaded || !TryGetTrigger(slider, out TriggerSetting triggerSetting)) return;
            _sliderChangeRequestedByUser = false;
            try
            {
            bool IsActive;
            if (e.NewValue == 0)
            {
                slider.Background = Brushes.LimeGreen;
                IsActive = true;
            }
            else
            {
                slider.Background = Brushes.White;
                IsActive = false;
            }

            triggerSetting.IsActive = IsActive;
            _commandRepository.Update(triggerSetting);
            _logger.Info($"Trigger with the ID {triggerSetting.ID} is now updated to IsActive: {IsActive}.");
            SafeLoadTrigger();
            }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Change", ex); SafeLoadTrigger(); }
        }

        private void OnOffSlider_PreviewMouseInput(object sender, MouseButtonEventArgs e) => _sliderChangeRequestedByUser = true;

        private void OnOffSlider_PreviewMouseInputFinished(object sender, MouseButtonEventArgs e) => _sliderChangeRequestedByUser = false;

        private void OnOffSlider_PreviewKeyInput(object sender, KeyEventArgs e)
        {
            _sliderChangeRequestedByUser = e.Key is Key.Left or Key.Right or Key.Up or Key.Down or Key.Home or Key.End or Key.PageUp or Key.PageDown;
        }

        private bool TryGetTrigger(FrameworkElement element, out TriggerSetting triggerSetting)
        {
            triggerSetting = null;
            if (element?.DataContext is not TriggerListObject row || !int.TryParse(row.ID, out int triggerId)) return false;
            triggerSetting = _commandRepository.GetList().FirstOrDefault(item => item.ID == triggerId);
            return triggerSetting != null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element || !TryGetTrigger(element, out TriggerSetting triggerSetting)) return;
            try { _commandRepository.Delete(triggerSetting); SafeLoadTrigger(); }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Delete", ex); }
        }


        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element || !TryGetTrigger(element, out TriggerSetting triggerSetting)) return;
            try { _triggerLogicController.AddToQueue(new QueueObject(triggerSetting, "Test")); }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Test", ex); }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && TryGetTrigger(element, out TriggerSetting triggerSetting)) OpenTriggerDetails(triggerSetting);
        }

        private void DuplicateButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && TryGetTrigger(element, out TriggerSetting triggerSetting))
                OpenTriggerDetails(triggerSetting, true);
        }

        private void NewCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenTriggerDetails();
        }

        private async void OpenTriggerDetails(TriggerSetting triggerSetting = null, bool saveAsCopy = false)
        {
            try
            {
            if (_appSettings.NanoSettings?.NanoLeafDevices == null || _appSettings.NanoSettings.NanoLeafDevices.Count == 0)
            {
                ShowError("Window_Trigger_Error_ConnectDevice");
                return;
            }

            var effectList = await _nanoController.GetEffectList(_appSettings.NanoSettings.NanoLeafDevices[0]);

            if (effectList == null)
            {
                _logger.Error("Connection failed! Couldn't get Effect List!");
                System.Windows.MessageBox.Show(Properties.Resources.Code_Trigger_MessageBox_EffectList, Properties.Resources.General_MessageBox_Error_Title);
                return;
            }

            Window triggerDetailWindow = new TriggerDetailWindow(_appSettings, _appSettingsController, _commandRepository, effectList, _streamlabsController, _hypeRateIoController, triggerSetting, _twitchEventSubController, saveAsCopy);
            Window hostWindow = Window.GetWindow(Trigger_Listview) ?? Application.Current?.MainWindow;
            if (hostWindow != null) triggerDetailWindow.Owner = hostWindow;
            triggerDetailWindow.Closed += TriggerDetailWindow_Closed;
            triggerDetailWindow.Show();
            }
            catch (Exception ex) { ShowError("Window_Trigger_Error_OpenDetails", ex); }
        }

        private void TriggerDetailWindow_Closed(object sender, EventArgs e)
        {
            SafeLoadTrigger();
        }

        private void ExportCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new SaveFileDialog { Filter = "JSON (*.json)|*.json", FileName = $"NanoTwitchLeafs-triggers-{DateTime.Now:yyyy-MM-dd}.json" };
                if (dialog.ShowDialog(Window.GetWindow(Trigger_Listview)) != true) return;
                File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(_commandRepository.GetList(), Formatting.Indented));
                MessageBox.Show(Text("Window_Trigger_Export_Success"));
            }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Export", ex); }
        }

        private void ImportCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog { Filter = "JSON (*.json)|*.json" };
                if (dialog.ShowDialog(Window.GetWindow(Trigger_Listview)) != true) return;
                var imported = JsonConvert.DeserializeObject<List<TriggerSetting>>(File.ReadAllText(dialog.FileName));
                if (imported == null || imported.Count == 0 || imported.Any(item => item == null || string.IsNullOrWhiteSpace(item.Trigger)))
                    throw new InvalidDataException(Text("Window_Trigger_Import_Invalid"));

                MessageBoxResult mode = MessageBox.Show(
                    Text("Window_Trigger_Import_Mode"), Text("Window_Trigger_Import_Title"),
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (mode == MessageBoxResult.Cancel) return;

                if (File.Exists(Constants.TRIGGERS_PATH))
                    File.Copy(Constants.TRIGGERS_PATH, Constants.TRIGGERS_PATH + $".before-import-{DateTime.Now:yyyyMMdd-HHmmss}.backup", false);

                List<TriggerSetting> replacement = mode == MessageBoxResult.Yes
                    ? _commandRepository.GetList()
                    : new List<TriggerSetting>();
                foreach (TriggerSetting item in imported)
                {
                    TriggerSetting existing = mode == MessageBoxResult.Yes ? replacement.FirstOrDefault(current =>
                        string.Equals(current.Trigger, item.Trigger, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(current.CMD ?? "", item.CMD ?? "", StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(current.ChannelPointsGuid ?? "", item.ChannelPointsGuid ?? "", StringComparison.OrdinalIgnoreCase)) : null;
                    if (existing != null) replacement[replacement.IndexOf(existing)] = item;
                    else replacement.Add(item);
                }
                _commandRepository.ReplaceAll(replacement);
                SafeLoadTrigger();
                MessageBox.Show(string.Format(Text("Window_Trigger_Import_Success"), imported.Count));
            }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Import", ex); }
        }

        private void ClearCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            int triggerCount = _commandRepository.GetList().Count;
            if (MessageBox.Show(string.Format(Text("Window_Trigger_Clear_Confirm"), triggerCount),
                Text("Window_Trigger_Clear_Title"), MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try { _commandRepository.ReplaceAll(Array.Empty<TriggerSetting>()); SafeLoadTrigger(); }
            catch (Exception ex) { ShowError("Window_Trigger_Error_Clear", ex); }
        }

    }
}
