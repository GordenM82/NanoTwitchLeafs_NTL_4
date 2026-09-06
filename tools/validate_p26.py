#!/usr/bin/env python3
"""Static regression checks for regression 26 theme and layout polish."""
from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
def read(path): return (ROOT / path).read_text(encoding="utf-8-sig")

def main():
    errors = []
    app = read("App.xaml")
    theme = read("ThemeManager.cs")
    main = read("Windows/MainWindow.xaml")
    trigger = read("Windows/TriggerWindow.xaml")
    info = read("Windows/AppInfoWindow.xaml")
    for path in ("App.xaml", "Windows/MainWindow.xaml", "Windows/TriggerWindow.xaml", "Windows/AppInfoWindow.xaml"):
        try: ET.parse(ROOT / path)
        except ET.ParseError as error: errors.append(f"{path} is invalid: {error}")
    for key in ("NtlWarningBrush", "NtlErrorBrush", "NtlDebugBrush"):
        if key not in app or f'SetBrush("{key}"' not in theme: errors.append(f"theme color missing: {key}")
    if 'TargetType="ScrollBar"' not in app or ('horizontalTrack' not in app and 'NtlHorizontalScrollBarTemplate' not in app): errors.append("theme-aware scrollbar style is missing")
    if 'Foreground="DarkOrange"' in main + trigger: errors.append("hard-coded warning color remains")
    if 'Height="36"' not in main or 'consoleAutoScroll_CheckBox' not in main: errors.append("console auto-scroll alignment is missing")
    if 'TwitchLinkAvatar_Image" Height="105"' not in main: errors.append("Twitch layout was not compacted")
    if 'test_Column" Header="" Width="44"' not in trigger: errors.append("trigger action columns were not compacted")
    if info.count('TextAlignment="Center"') < 7: errors.append("information heading/developer block is not centered")
    if 'AssemblyInformationalVersion("4.1.' not in read("Properties/AssemblyInfo.cs"): errors.append("assembly version is not 4.1.x")
    if errors:
        print("Regression 26 validation failed:")
        for error in errors: print(f"- {error}")
        return 1
    print("Regression 26 validation passed: theme colors, scrollbars, console alignment, compact Twitch/trigger layouts and centered information view verified.")
    return 0

if __name__ == "__main__": sys.exit(main())
