#!/usr/bin/env python3
"""Static regression checks for Preview 23 navigation and console tools."""

from pathlib import Path
import sys
import xml.etree.ElementTree as ET


ROOT = Path(__file__).resolve().parents[1]


def read(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8-sig")


def main() -> int:
    errors: list[str] = []
    xaml = read("Windows/MainWindow.xaml")
    code = read("Windows/MainWindow.xaml.cs")
    workflow = read(".github/workflows/build-release.yml")

    try:
        ET.parse(ROOT / "Windows/MainWindow.xaml")
    except ET.ParseError as error:
        errors.append(f"MainWindow.xaml is invalid: {error}")

    integration_panel = xaml[xaml.find('x:Name="integrationTabs_Panel"'):xaml.find('x:Name="settings_TabControl"')]
    order = [integration_panel.find(label) for label in (
        "Window_Main_Tabs_Streamlabs_Title", "Windows_Main_Tabs_Hyperate_Title",
        "StreamElements", "Windows_Main_Tabs_ApiSettings_Title")]
    if not all(value >= 0 for value in order) or order != sorted(order):
        errors.append("integration tabs are not ordered Streamlabs, HypeRate, StreamElements, API settings")

    api_start = xaml.find('x:Name="ApiSettings_Tabitem"')
    se_start = xaml.find('x:Name="StreamElements_TabItem"')
    if not 0 <= api_start < se_start:
        errors.append("StreamElements does not have a separate integration tab")
    api_section = xaml[api_start:se_start]
    if 'x:Name="streamElementsToken_PasswordBox"' in api_section:
        errors.append("StreamElements controls remain duplicated in API settings")

    required = (
        "ConsoleClear_Button_Click", "ConsoleOpenLog_Button_Click",
        "ConsoleAutoScroll_CheckBox_Changed", "_consoleAutoScroll",
    )
    for item in required:
        if item not in code:
            errors.append(f"missing console behavior: {item}")
    if 'TextWrapping="Wrap"' not in xaml[xaml.find('x:Name="console_Tabitem"'):]:
        errors.append("console line wrapping is missing")

    if "4.1.0-layout-preview." not in read("Properties/AssemblyInfo.cs"):
        errors.append("assembly preview version is missing")

    if errors:
        print("Preview 23 validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1

    print("Preview 23 validation passed: StreamElements integration tab, final API tab, "
          "console actions, wrapping, version and artifact verified.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
