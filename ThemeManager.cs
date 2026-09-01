using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace NanoTwitchLeafs
{
    internal static class ThemeManager
    {
        private static readonly IReadOnlyDictionary<string, string> AccentColors =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["TwitchPurple"] = "#9146FF",
                ["NanoleafGreen"] = "#22A447",
                ["WindowsBlue"] = "#0067C0",
                ["Coral"] = "#D83B6A"
            };

        public static void Apply(string theme, string accentColor)
        {
            bool dark = string.Equals(theme, "Dark", StringComparison.OrdinalIgnoreCase) ||
                (string.Equals(theme, "System", StringComparison.OrdinalIgnoreCase) && IsSystemDark());
            string accent = AccentColors.TryGetValue(accentColor ?? string.Empty, out string color)
                ? color
                : AccentColors["TwitchPurple"];

            SetBrush("NtlBackgroundBrush", dark ? "#0F141C" : "#F3F5F8");
            SetBrush("NtlNavigationBrush", dark ? "#111722" : "#FFFFFF");
            SetBrush("NtlSurfaceBrush", dark ? "#181F2A" : "#FFFFFF");
            SetBrush("NtlSurfaceRaisedBrush", dark ? "#222B39" : "#F7F8FA");
            SetBrush("NtlInputBrush", dark ? "#111720" : "#FFFFFF");
            SetBrush("NtlBorderBrush", dark ? "#394455" : "#D7DCE3");
            SetBrush("NtlTextBrush", dark ? "#F4F6FA" : "#18202B");
            SetBrush("NtlMutedTextBrush", dark ? "#B6C0CF" : "#596574");
            SetBrush("NtlDisabledTextBrush", dark ? "#778294" : "#8A94A2");
            SetBrush("NtlAccentBrush", accent);
            SetBrush("NtlAccentHoverBrush", Lighten(accent));
            SetBrush("NtlAccentSurfaceBrush", dark ? "#332452" : "#EEE5FF");
        }

        private static bool IsSystemDark()
        {
            try
            {
                object value = Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    "AppsUseLightTheme", 1);
                return value is int setting && setting == 0;
            }
            catch
            {
                return false;
            }
        }

        private static void SetBrush(string key, string color)
        {
            Application.Current.Resources[key] = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(color));
        }

        private static string Lighten(string hex)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hex);
            byte Blend(byte channel) => (byte)Math.Min(255, channel + 28);
            return $"#{Blend(color.R):X2}{Blend(color.G):X2}{Blend(color.B):X2}";
        }
    }
}
