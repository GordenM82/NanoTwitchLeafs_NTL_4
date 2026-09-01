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

            bool german = string.Equals(_appSettings.Language, "de-DE", StringComparison.OrdinalIgnoreCase);
            importCmd_Button.Content = german ? "Importieren" : "Import";
            exportCmd_Button.Content = german ? "Exportieren" : "Export";

            SafeLoadTrigger();
        }

        private bool IsGerman => string.Equals(_appSettings.Language, "de-DE", StringComparison.OrdinalIgnoreCase);

        private void ShowError(string germanMessage, string englishMessage, Exception exception = null)
        {
            if (exception != null) _logger.Error(IsGerman ? germanMessage : englishMessage, exception);
            MessageBox.Show(IsGerman ? germanMessage : englishMessage,
                Properties.Resources.General_MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SafeLoadTrigger()
        {
            try { LoadTrigger(); }
            catch (Exception ex) { ShowError("Die Trigger konnten nicht geladen werden.", "The triggers could not be loaded.", ex); }
        }

        private void LoadTrigger()
        {
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

                TriggerListItems.Add(triggerListObject);
                _logger.Debug($"Loading Trigger with id {triggerSetting.ID}.");
            }
            TriggerListItems = TriggerListItems.OrderBy(x => x.Trigger).ToList();
            Trigger_Listview.ItemsSource = TriggerListItems;
            _logger.Info($"Loaded {triggerSettings.Count} Triggers from Database.");
        }

        private void OnOffSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded || sender is not Slider slider || !TryGetTrigger(slider, out TriggerSetting triggerSetting)) return;
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
            }
            catch (Exception ex) { ShowError("Der Trigger konnte nicht geändert werden.", "The trigger could not be changed.", ex); SafeLoadTrigger(); }
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
            catch (Exception ex) { ShowError("Der Trigger konnte nicht gelöscht werden.", "The trigger could not be deleted.", ex); }
        }


        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement element || !TryGetTrigger(element, out TriggerSetting triggerSetting)) return;
            try { _triggerLogicController.AddToQueue(new QueueObject(triggerSetting, "Test")); }
            catch (Exception ex) { ShowError("Der Trigger konnte nicht getestet werden.", "The trigger could not be tested.", ex); }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && TryGetTrigger(element, out TriggerSetting triggerSetting)) OpenTriggerDetails(triggerSetting);
        }

        private void NewCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenTriggerDetails();
        }

        private async void OpenTriggerDetails(TriggerSetting triggerSetting = null)
        {
            try
            {
            if (_appSettings.NanoSettings?.NanoLeafDevices == null || _appSettings.NanoSettings.NanoLeafDevices.Count == 0)
            {
                ShowError("Bitte zuerst ein Nanoleaf-Gerät verbinden.", "Please connect a Nanoleaf device first.");
                return;
            }

            var effectList = await _nanoController.GetEffectList(_appSettings.NanoSettings.NanoLeafDevices[0]);

            if (effectList == null)
            {
                _logger.Error("Connection failed! Couldn't get Effect List!");
                System.Windows.MessageBox.Show(Properties.Resources.Code_Trigger_MessageBox_EffectList, Properties.Resources.General_MessageBox_Error_Title);
                return;
            }

            Window triggerDetailWindow = new TriggerDetailWindow(_appSettings, _appSettingsController, _commandRepository, effectList, _streamlabsController, _hypeRateIoController, triggerSetting, _twitchEventSubController)
            {
                Owner = this
            };
            triggerDetailWindow.Closed += TriggerDetailWindow_Closed;
            triggerDetailWindow.Show();
            }
            catch (Exception ex) { ShowError("Die Trigger-Details konnten nicht geöffnet werden.", "The trigger details could not be opened.", ex); }
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
                if (dialog.ShowDialog(this) != true) return;
                File.WriteAllText(dialog.FileName, JsonConvert.SerializeObject(_commandRepository.GetList(), Formatting.Indented));
                MessageBox.Show(IsGerman ? "Trigger wurden exportiert." : "Triggers were exported.");
            }
            catch (Exception ex) { ShowError("Die Trigger konnten nicht exportiert werden.", "The triggers could not be exported.", ex); }
        }

        private void ImportCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog { Filter = "JSON (*.json)|*.json" };
                if (dialog.ShowDialog(this) != true) return;
                var imported = JsonConvert.DeserializeObject<List<TriggerSetting>>(File.ReadAllText(dialog.FileName));
                if (imported == null || imported.Count == 0 || imported.Any(item => item == null || string.IsNullOrWhiteSpace(item.Trigger)))
                    throw new InvalidDataException(IsGerman ? "Die Datei enthält keine gültigen Trigger." : "The file contains no valid triggers.");

                if (File.Exists(Constants.TRIGGERS_PATH))
                    File.Copy(Constants.TRIGGERS_PATH, Constants.TRIGGERS_PATH + $".before-import-{DateTime.Now:yyyyMMdd-HHmmss}.backup", false);

                MessageBoxResult mode = MessageBox.Show(
                    IsGerman ? "Ja: mit vorhandenen Triggern zusammenführen\nNein: vorhandene Trigger ersetzen" : "Yes: merge with existing triggers\nNo: replace existing triggers",
                    IsGerman ? "Trigger importieren" : "Import triggers", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (mode == MessageBoxResult.Cancel) return;

                if (mode == MessageBoxResult.No) _commandRepository.ClearAll();
                foreach (TriggerSetting item in imported)
                {
                    TriggerSetting existing = mode == MessageBoxResult.Yes ? _commandRepository.GetList().FirstOrDefault(current =>
                        string.Equals(current.Trigger, item.Trigger, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(current.CMD ?? "", item.CMD ?? "", StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(current.ChannelPointsGuid ?? "", item.ChannelPointsGuid ?? "", StringComparison.OrdinalIgnoreCase)) : null;
                    if (existing != null) { item.ID = existing.ID; _commandRepository.Update(item); }
                    else { item.ID = 0; _commandRepository.Insert(item); }
                }
                SafeLoadTrigger();
                MessageBox.Show(IsGerman ? $"{imported.Count} Trigger wurden importiert." : $"{imported.Count} triggers were imported.");
            }
            catch (Exception ex) { ShowError("Die Trigger konnten nicht importiert werden.", "The triggers could not be imported.", ex); }
        }

        private void ClearCmd_Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(IsGerman ? "Wirklich alle Trigger löschen?" : "Delete all triggers?",
                IsGerman ? "Trigger leeren" : "Clear triggers", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try { _commandRepository.ClearAll(); SafeLoadTrigger(); }
            catch (Exception ex) { ShowError("Die Trigger konnten nicht gelöscht werden.", "The triggers could not be deleted.", ex); }
        }

    }
}
