#!/usr/bin/env python3
"""Validate the NanoTwitchLeafs 4.1.6 release-critical changes."""

from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def require(path: str, *needles: str) -> None:
    text = (ROOT / path).read_text(encoding="utf-8-sig")
    missing = [needle for needle in needles if needle not in text]
    if missing:
        raise SystemExit(f"{path}: missing release requirement(s): {missing}")


require("Properties/AssemblyInfo.cs", 'AssemblyVersion("4.1.6.0")', 'AssemblyInformationalVersion("4.1.6")')
require("Controller/UpdateController.cs", "CheckForUpdatesAsync(bool showResult)", "P416_Update_Current", "P416_Update_SourceError")
require("Windows/MainWindow.xaml", 'SelectionMode="Extended"', 'KeyDown="Console_ListBox_KeyDown"')
require("Windows/MainWindow.xaml.cs", "Environment.ProcessPath", "P416_Update_Checking", "SpecialFolder.MyDocuments", "console_ListBox.SelectAll()", "FileShare.ReadWrite | FileShare.Delete")
require("Controller/TwitchController.cs", "UsesSeparateBotAccount", '"Got incorrect Login Message from Twitch ... (Bot Account)"')
require("README.md", "NanoTwitchLeafs 4.1.6")
require("CHANGELOG.md", "NanoTwitchLeafs 4.1.6")

print("NanoTwitchLeafs 4.1.6 release validation passed.")
