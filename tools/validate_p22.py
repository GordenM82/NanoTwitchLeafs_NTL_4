#!/usr/bin/env python3
"""Static regression checks for the Preview 22 integration surface."""

from pathlib import Path
import sys
import xml.etree.ElementTree as ET


ROOT = Path(__file__).resolve().parents[1]


def read(relative: str) -> str:
    return (ROOT / relative).read_text(encoding="utf-8-sig")


def require(condition: bool, message: str, errors: list[str]) -> None:
    if not condition:
        errors.append(message)


def main() -> int:
    errors: list[str] = []
    main_xaml = read("Windows/MainWindow.xaml")
    main_code = read("Windows/MainWindow.xaml.cs")
    trigger_xaml = read("Windows/TriggerDetailWindow.xaml")
    trigger_code = read("Windows/TriggerDetailWindow.xaml.cs")
    logic = read("Controller/TriggerLogicController.cs")
    elements = read("Controller/StreamElementsController.cs")

    for relative in ("Windows/MainWindow.xaml", "Windows/TriggerDetailWindow.xaml"):
        try:
            ET.parse(ROOT / relative)
        except ET.ParseError as error:
            errors.append(f"{relative}: invalid XML/XAML: {error}")

    # The blocklist belongs to the existing Twitch view, never to side navigation.
    twitch_start = main_xaml.find('x:Name="botLogin_TabItem"')
    twitch_end = main_xaml.find('x:Name="nanoConfig_TabItem"')
    blocklist_at = main_xaml.find('x:Name="blocklist_ListBox"')
    navigation_end = main_xaml.find('x:Name="settings_TabControl"')
    require(twitch_start < blocklist_at < twitch_end, "blocklist is not embedded in the Twitch tab", errors)
    require("blocklist" not in main_xaml[:navigation_end].lower(), "blocklist leaked into side navigation", errors)
    require("new BlacklistWindow" not in main_code, "legacy standalone blocklist window is still opened", errors)

    # StreamElements must feed the established donation path only.
    require('topic = "channel.tips"' in elements, "channel.tips subscription is missing", errors)
    require("channel.activities" not in elements, "broad activity subscription could duplicate Twitch events", errors)
    require('Provider = "StreamElements"' in elements, "normalized StreamElements provider is missing", errors)
    require("SimulateDonation" in elements and "StreamElementsTest_Button_Click" in main_code,
            "local StreamElements test event is missing", errors)
    require("Sanitize(ex.Message)" in elements and '"[redacted]"' in elements,
            "token redaction is missing from connection errors", errors)

    # Existing donation triggers remain compatible and can optionally select a source.
    require('Trigger == "Donation"' in logic, "existing Donation trigger route is missing", errors)
    require("DonationProviderMatches" in logic, "donation provider filtering is missing", errors)
    require('Tag="All"' in trigger_xaml and 'Tag="Streamlabs"' in trigger_xaml and
            'Tag="StreamElements"' in trigger_xaml, "provider choices are incomplete", errors)
    require('configuredProvider.Equals("All"' in logic, "legacy triggers do not default to all providers", errors)
    require("TriggerSetting.DonationProvider" in trigger_code, "provider selection is not persisted", errors)

    require("4.1.0-layout-preview.22" in read("Properties/AssemblyInfo.cs"),
            "assembly informational version is not Preview 22", errors)
    require("layout-preview.22-win-x64" in read(".github/workflows/build-release.yml"),
            "workflow artifact is not Preview 22", errors)

    if errors:
        print("Preview 22 validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1

    print("Preview 22 validation passed: embedded blocklist, donation-source routing, "
          "StreamElements tip subscription, local simulation, and token redaction verified.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
