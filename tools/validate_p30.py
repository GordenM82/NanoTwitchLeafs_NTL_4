from pathlib import Path
import sys
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


def read(path):
    return (ROOT / path).read_text(encoding="utf-8")


def main():
    errors = []
    app = read("App.xaml")
    placement = read("Windows/WindowPlacementService.cs")
    main_code = read("Windows/MainWindow.xaml.cs")

    try:
        ET.parse(ROOT / "App.xaml")
    except ET.ParseError as error:
        errors.append(f"App.xaml invalid: {error}")

    if app.count('x:Name="PART_Track"') != 2:
        errors.append("vertical and horizontal PART_Track templates are required")
    for marker in ("PageUpCommand", "PageDownCommand", "PageLeftCommand", "PageRightCommand", "IsDragging", "IsMouseOver"):
        if marker not in app:
            errors.append(f"scrollbar behavior missing: {marker}")
    for marker in ("ScreenForLogicalBounds", "LogicalWorkingArea", "Forms.Screen.FromPoint", "Forms.Screen.FromHandle", "WorkingArea", "TransformFromDevice", "Clamp"):
        if marker not in placement:
            errors.append(f"monitor placement behavior missing: {marker}")
    if "SystemParameters.VirtualScreen" in main_code:
        errors.append("main placement still uses the combined virtual desktop")
    for path in ("Windows/MainWindow.xaml.cs", "Controller/NanoController.cs", "Windows/TriggerWindow.xaml.cs", "Windows/DevicesInfoWindow.xaml.cs", "Windows/TriggerDetailWindow.xaml.cs"):
        if "WindowPlacementService" not in read(path):
            errors.append(f"owned-window placement missing: {path}")
    if "monitorDescription" not in main_code or "PixelsPerInchX" not in placement:
        errors.append("monitor/DPI diagnostics are missing")
    if "4.1.0-layout-preview.30" not in read("Properties/AssemblyInfo.cs"):
        errors.append("assembly version is not Preview 30")
    if "layout-preview.30-win-x64" not in read(".github/workflows/build-release.yml"):
        errors.append("workflow artifact is not Preview 30")

    if errors:
        print("Preview 30 validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1
    print("Preview 30 validation passed: single-monitor placement, owned-window bounds and draggable scrollbar templates verified.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
