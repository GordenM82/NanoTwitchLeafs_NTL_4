from pathlib import Path
import sys

ROOT = Path(__file__).resolve().parents[1]


def read(path):
    return (ROOT / path).read_text(encoding="utf-8-sig")


def main():
    errors = []
    assembly = read("Properties/AssemblyInfo.cs")
    project = read("NanoTwitchLeafs.Modern.csproj")
    constants = read("Constants.cs")
    main_xaml = read("Windows/MainWindow.xaml")
    main_code = read("Windows/MainWindow.xaml.cs")
    workflow = read(".github/workflows/build-release.yml")
    readme = read("README.md")
    changelog = read("CHANGELOG.md")

    if 'AssemblyInformationalVersion("4.1.0")' not in assembly:
        errors.append("final informational version is not 4.1.0")
    if "NTL4_LAYOUT_PREVIEW" in project or "NanoTwitchLeafs-4-Layout-Preview" in constants:
        errors.append("preview data-folder build flag remains enabled")
    for marker in ("PREVIEW", "Layout-Vorschau", "Layout Preview"):
        if marker in main_xaml or marker in main_code:
            errors.append(f"user-facing preview marker remains: {marker}")
    if "NanoTwitchLeafs-4.1.0-win-x64" not in workflow:
        errors.append("final workflow artifact name is missing")
    if "4.1.0" not in readme or "NanoTwitchLeafs 4.1.0" not in changelog:
        errors.append("release documentation is incomplete")

    if errors:
        print("4.1.0 release validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1
    print("4.1.0 release validation passed: stable data path, final branding, artifact and documentation verified.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
