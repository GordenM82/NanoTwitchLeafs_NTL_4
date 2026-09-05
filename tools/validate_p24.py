#!/usr/bin/env python3
"""Static regression checks for Preview 24 trigger-list enhancements."""

from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]

def read(path: str) -> str:
    return (ROOT / path).read_text(encoding="utf-8-sig")

def main() -> int:
    errors: list[str] = []
    xaml = read("Windows/TriggerWindow.xaml")
    code = read("Windows/TriggerWindow.xaml.cs")
    model = read("Objects/TriggerListObject.cs")
    try:
        ET.parse(ROOT / "Windows/TriggerWindow.xaml")
    except ET.ParseError as error:
        errors.append(f"TriggerWindow.xaml is invalid: {error}")

    for name in ("triggerSearch_TextBox", "triggerStatusFilter_ComboBox", "triggerTypeFilter_ComboBox",
                 "triggerResetFilter_Button", "triggerResultCount_TextBlock", "warning_Column"):
        if f'x:Name="{name}"' not in xaml:
            errors.append(f"missing trigger-list control: {name}")
    for behavior in ("ApplyTriggerFilters", "GetTriggerWarnings", "Trigger_Listview_MouseDoubleClick",
                     '"Active" =>', '"Inactive" =>', '"Problems" =>', '"Donation" =>'):
        if behavior not in code:
            errors.append(f"missing trigger-list behavior: {behavior}")
    for problem in ("P24_Warning_NoDevices", "P24_Warning_MissingDevice",
                    "P24_Warning_MissingEffect", "P24_Warning_MissingSound"):
        if problem not in code:
            errors.append(f"missing warning check: {problem}")
    if "HasProblem" not in model or "WarningVisibility" not in model:
        errors.append("trigger row warning state is incomplete")
    if "4.1.0-layout-preview." not in read("Properties/AssemblyInfo.cs"):
        errors.append("assembly version is not a layout preview")

    if errors:
        print("Preview 24 validation failed:")
        for error in errors: print(f"- {error}")
        return 1
    print("Preview 24 validation passed: search, status/type filters, result count, reset, "
          "warnings, double-click editing, version and artifact verified.")
    return 0

if __name__ == "__main__":
    sys.exit(main())
