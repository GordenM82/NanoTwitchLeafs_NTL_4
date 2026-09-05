#!/usr/bin/env python3
"""Static quality and stability regression checks for Preview 28."""
from pathlib import Path
import sys
import xml.etree.ElementTree as ET
ROOT=Path(__file__).resolve().parents[1]
def read(path): return (ROOT/path).read_text(encoding="utf-8-sig")
def main():
    errors=[]
    main=read("Windows/MainWindow.xaml"); main_code=read("Windows/MainWindow.xaml.cs")
    trigger=read("Windows/TriggerWindow.xaml"); trigger_code=read("Windows/TriggerWindow.xaml.cs")
    settings=read("Controller/AppSettingsController.cs"); hype=read("Controller/HypeRateIOController.cs"); logic=read("Controller/TriggerLogicController.cs")
    for path in ("App.xaml","Windows/MainWindow.xaml","Windows/TriggerWindow.xaml"):
        try: ET.parse(ROOT/path)
        except ET.ParseError as error: errors.append(f"{path} invalid: {error}")
    if 'triggerManager_Host" Grid.Column="1" Margin="20,20,20,76"' not in main: errors.append("trigger host does not share the standard content bounds")
    if '<TranslateTransform Y="3"' not in main: errors.append("auto-scroll checkbox optical alignment is missing")
    for marker in ('ToolTip="{Binding Trigger}"','ToolTip="{Binding Command}"','ToolTip="{Binding Effect}"','ToolTip="{Binding TargetDevices}"'):
        if marker not in trigger: errors.append(f"truncated trigger tooltip missing: {marker}")
    if "NtlFocusVisualStyle" not in read("App.xaml") or "_embeddedReturnPage" not in main_code or "Keyboard.Focus(triggerSearch_TextBox)" not in trigger_code: errors.append("keyboard/focus improvements are incomplete")
    for marker in ("NormalizeSettings", "BackupInvalidSettings", "NanoLeafDevices ??=", "StreamElements.TokenType"):
        if marker not in settings: errors.append(f"settings recovery missing: {marker}")
    for marker in ("JObject.Parse", "heartRate.HasValue", "Ignored an invalid HypeRate message", "_appSettings = appSettings ??"):
        if marker not in hype: errors.append(f"HypeRate guard missing: {marker}")
    for marker in ("Dispatcher.BeginInvoke", "HasShutdownStarted", "while (_queue.TryReceive(out _))", "_queueToken?.Cancel"):
        if marker not in logic: errors.append(f"queue/thread guard missing: {marker}")
    if "4.1.0-layout-preview.28" not in read("Properties/AssemblyInfo.cs"): errors.append("assembly version is not Preview 28")
    if "layout-preview.28-win-x64" not in read(".github/workflows/build-release.yml"): errors.append("workflow artifact is not Preview 28")
    if errors:
        print("Preview 28 validation failed:")
        for error in errors: print(f"- {error}")
        return 1
    print("Preview 28 validation passed: layout alignment, tooltips, focus, settings recovery, HypeRate guards and queue UI-thread safety verified.")
    return 0
if __name__=="__main__": sys.exit(main())
