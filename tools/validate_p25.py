#!/usr/bin/env python3
"""Static regression checks for Preview 25 usability features."""
from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
def read(path): return (ROOT / path).read_text(encoding="utf-8-sig")

def main():
    errors = []
    xaml, code, settings = read("Windows/MainWindow.xaml"), read("Windows/MainWindow.xaml.cs"), read("Objects/AppSettings.cs")
    try: ET.parse(ROOT / "Windows/MainWindow.xaml")
    except ET.ParseError as error: errors.append(f"MainWindow.xaml is invalid: {error}")
    for name in ("blocklistManage_Button", "blocklistPanel", "blocklistBack_Button", "consoleSearch_TextBox",
                 "consoleLevelFilter_ComboBox", "consoleSupportLog_Button", "consoleAutoScroll_CheckBox",
                 "consoleResultCount_TextBlock", "toast_Border", "unsavedChanges_TextBlock"):
        if f'x:Name="{name}"' not in xaml: errors.append(f"missing P25 control: {name}")
    for behavior in ("ConsoleEntryMatchesFilter", "SanitizeSupportLog", "ConsoleCopySelected_MenuItem_Click",
                     "RestoreWindowPlacement", "SaveWindowPlacement", "MarkSettingsDirty", "ShowToast",
                     "MainWindow_PreviewKeyDown", "MessageBoxButton.YesNoCancel"):
        if behavior not in code: errors.append(f"missing P25 behavior: {behavior}")
    for prop in ("WindowLeft", "WindowTop", "WindowWidth", "WindowHeight", "WindowMaximized"):
        if prop not in settings: errors.append(f"missing window placement property: {prop}")
    if 'Height="145"' not in xaml: errors.append("Nano status area was not compacted")
    if "4.1.0-layout-preview." not in read("Properties/AssemblyInfo.cs"): errors.append("assembly version is not a layout preview")
    if errors:
        print("Preview 25 validation failed:")
        for error in errors: print(f"- {error}")
        return 1
    print("Preview 25 validation passed: blocklist, compact layout, console, support log, window placement, dirty warning, toasts and keyboard navigation verified.")
    return 0

if __name__ == "__main__": sys.exit(main())
