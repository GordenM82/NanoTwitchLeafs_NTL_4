from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


def read(path):
    return (ROOT / path).read_text(encoding="utf-8")


def main():
    errors = []
    main_xaml = read("Windows/MainWindow.xaml")
    trigger_xaml = read("Windows/TriggerWindow.xaml")

    for path in ("Windows/MainWindow.xaml", "Windows/TriggerWindow.xaml"):
        try:
            ET.parse(ROOT / path)
        except ET.ParseError as error:
            errors.append(f"{path} invalid: {error}")

    if '<TranslateTransform Y="5"' not in main_xaml:
        errors.append("P29 auto-scroll fine alignment is missing")
    if '<Border BorderBrush="{DynamicResource NtlBorderBrush}" BorderThickness="1" Background="{DynamicResource NtlSurfaceBrush}">' not in trigger_xaml:
        errors.append("trigger page outer content border is missing")
    if 'ToolTip="{TemplateBinding Content}"' not in trigger_xaml:
        errors.append("column-header tooltips are missing")
    for marker in ("TriggerCell", "CommandCell", "EffectCell", "SoundCell", "FlagsCell", "TargetsCell"):
        if f'x:Key="{marker}"' not in trigger_xaml:
            errors.append(f"tooltip cell template missing: {marker}")

    if errors:
        print("Preview 29 validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1
    print("Preview 29 validation passed: auto-scroll alignment, common trigger page edge and truncated-value tooltips verified.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
