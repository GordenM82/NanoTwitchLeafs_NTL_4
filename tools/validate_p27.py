#!/usr/bin/env python3
"""Static checks for Preview 27 console alignment and trigger localization."""
from pathlib import Path
import sys
import xml.etree.ElementTree as ET
ROOT = Path(__file__).resolve().parents[1]
def read(path): return (ROOT / path).read_text(encoding="utf-8-sig")
def main():
    errors=[]
    main=read("Windows/MainWindow.xaml"); code=read("Windows/MainWindow.xaml.cs"); trigger=read("Windows/TriggerWindow.xaml")
    for path in ("Windows/MainWindow.xaml","Windows/TriggerWindow.xaml"):
        try: ET.parse(ROOT/path)
        except ET.ParseError as error: errors.append(f"{path} invalid: {error}")
    toolbar=main[main.find('x:Name="console_Tabitem"'):main.find('x:Name="console_ListBox"')]
    if "<WrapPanel" in toolbar or 'Height="36"' not in toolbar: errors.append("console toolbar is not a fixed-height Grid")
    if 'x:Name="consoleSearchHint_TextBlock"' not in main or "consoleSearchHint_TextBlock.Visibility" not in code: errors.append("visible console search hint is missing")
    for old in ('Header="Zielgeräte"','Content="Importieren"','Content="Exportieren"'):
        if old in trigger: errors.append(f"hard-coded trigger text remains: {old}")
    for width in ('Trigger}" Width="95"','Command}" Width="120"','delete_Column" Header="" Width="50"'):
        if width not in trigger: errors.append(f"balanced trigger width missing: {width}")
    if "4.1.0-layout-preview.27" not in read("Properties/AssemblyInfo.cs"): errors.append("assembly version is not Preview 27")
    if "layout-preview.27-win-x64" not in read(".github/workflows/build-release.yml"): errors.append("workflow artifact is not Preview 27")
    if errors:
        print("Preview 27 validation failed:")
        for error in errors: print(f"- {error}")
        return 1
    print("Preview 27 validation passed: fixed console alignment, search hint, balanced trigger widths and localized labels verified.")
    return 0
if __name__ == "__main__": sys.exit(main())
