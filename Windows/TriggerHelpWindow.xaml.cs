using System.Collections.Generic;
using System.Windows;

namespace NanoTwitchLeafs.Windows
{
    /// <summary>
    /// Interaction logic for TriggerHelpWindow.xaml
    /// </summary>
    public partial class TriggerHelpWindow : Window
    {
        public TriggerHelpWindow(string languageCode)
        {
            Constants.SetCultureInfo(languageCode);
            InitializeComponent();

			HelpHeading_TextBlock.Text = Text("Window_TriggerHelp_Heading");
			HelpIntro_TextBlock.Text = Text("Window_TriggerHelp_Intro");
			HelpSections_ItemsControl.ItemsSource = new List<HelpSection>
			{
				new HelpSection(Text("Window_TriggerHelp_Types_Title"), Text("Window_TriggerHelp_Types_Body")),
				new HelpSection(Text("Window_TriggerHelp_Chat_Title"), Text("Window_TriggerHelp_Chat_Body")),
				new HelpSection(Text("Window_TriggerHelp_ChannelPoints_Title"), Text("Window_TriggerHelp_ChannelPoints_Body")),
				new HelpSection(Text("Window_TriggerHelp_Effect_Title"), Text("Window_TriggerHelp_Effect_Body")),
				new HelpSection(Text("Window_TriggerHelp_Options_Title"), Text("Window_TriggerHelp_Options_Body")),
				new HelpSection(Text("Window_TriggerHelp_Targets_Title"), Text("Window_TriggerHelp_Targets_Body"))
			};
        }

		private static string Text(string key) => Properties.Resources.ResourceManager.GetString(key) ?? key;

		public sealed class HelpSection
		{
			public HelpSection(string title, string body) { Title = title; Body = body; }
			public string Title { get; }
			public string Body { get; }
		}
    }
}
