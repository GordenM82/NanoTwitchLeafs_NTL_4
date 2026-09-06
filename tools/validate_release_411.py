from pathlib import Path
import sys


ROOT = Path(__file__).resolve().parents[1]


def read(path):
    return (ROOT / path).read_text(encoding="utf-8")


def main():
    errors = []
    assembly = read("Properties/AssemblyInfo.cs")
    project = read("NanoTwitchLeafs.Modern.csproj")
    constants = read("Constants.cs")
    main_xaml = read("Windows/MainWindow.xaml")
    main_code = read("Windows/MainWindow.xaml.cs")
    twitch_code = read("Controller/TwitchController.cs")
    link_xaml = read("Windows/TwitchLinkWindow.xaml")
    link_code = read("Windows/TwitchLinkWindow.xaml.cs")
    workflow = read(".github/workflows/build-release.yml")
    readme = read("README.md")
    changelog = read("CHANGELOG.md")

    if 'AssemblyInformationalVersion("4.1.1")' not in assembly:
        errors.append("informational version is not 4.1.1")
    if "NTL4_LAYOUT_PREVIEW" in project or "NanoTwitchLeafs-4-Layout-Preview" in constants:
        errors.append("preview data path remains enabled")
    for marker in ("PREVIEW", "Layout-Vorschau", "Layout Preview"):
        if marker in main_xaml or marker in main_code:
            errors.append(f"visible preview marker remains: {marker}")
    if "NanoTwitchLeafs-4.1.1-win-x64" not in workflow:
        errors.append("4.1.1 workflow artifact is missing")
    if "4.1.1" not in readme or "NanoTwitchLeafs 4.1.1" not in changelog:
        errors.append("4.1.1 release documentation is incomplete")
    if "Dispatcher.CheckAccess()" not in main_code or "BeginInvoke(new Action(MarkSettingsDirty))" not in main_code:
        errors.append("settings dirty tracking is not dispatcher-safe")
    for marker in ("OnChatConnectionChanged", "OnChatConnectionFailed", "Twitch chat connected, but EventSub could not be started"):
        if marker not in twitch_code:
            errors.append(f"Twitch resilience marker missing: {marker}")
    if "chatStatus_TextBlock" not in main_xaml or "P411_Chat_ConnectionFailed" not in main_code:
        errors.append("chat connection status UI is incomplete")
    if 'Visibility="Collapsed"' not in link_xaml or 'Header="{x:Static p:Resources.Window_TwitchLink_Tab_Bot}"' not in link_xaml:
        errors.append("optional bot tab is not hidden for single-account setup")
    if 'Height="300"' not in link_xaml or 'Height="480"' not in link_xaml:
        errors.append("Twitch connection test layout fix is missing")
    if '$"Connected to Twitch Channel' in link_code:
        errors.append("hard-coded English Twitch status remains")

    if errors:
        print("4.1.1 release validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1
    print("4.1.1 release validation passed: Twitch recovery, chat state, localized wizard and release metadata verified.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
