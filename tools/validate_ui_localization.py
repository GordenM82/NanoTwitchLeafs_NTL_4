#!/usr/bin/env python3
"""Fail the build when selectable UI cultures have incomplete resources."""

from __future__ import annotations

from collections import Counter
from pathlib import Path
import re
import sys
import xml.etree.ElementTree as ET


ROOT = Path(__file__).resolve().parents[1]
PROPERTIES = ROOT / "Properties"
SELECTABLE_CULTURES = [
    "de-DE", "en-US", "da-DK", "es-ES", "fr-FR", "it-IT",
    "nl-NL", "pl-PL", "pt-BR", "sk-SK", "ru-RU",
]
RESOURCE_CULTURES = [culture for culture in SELECTABLE_CULTURES if culture != "en-US"] + ["de-AT", "fr"]
PLACEHOLDER = re.compile(r"\{\d+(?:[^}]*)?\}")
XAML_RESOURCE = re.compile(r"x:Static\s+p:Resources\.([A-Za-z0-9_]+)")
CS_RESOURCE = re.compile(r'(?:ResourceManager\.GetString|\bText)\(\s*"([A-Za-z0-9_]+)"')
LANGUAGE_ENTRY = re.compile(r'ComboBoxItem\s*\{\s*Content\s*=\s*"[^"]+",\s*Tag\s*=\s*"([^"]+)"')


def read_resx(path: Path) -> dict[str, str]:
    root = ET.parse(path).getroot()
    result: dict[str, str] = {}
    duplicates: list[str] = []
    for data in root.findall("data"):
        name = data.get("name")
        if not name:
            continue
        if name in result:
            duplicates.append(name)
        value = data.find("value")
        result[name] = "" if value is None or value.text is None else value.text
    if duplicates:
        raise ValueError(f"{path.name}: duplicate keys: {', '.join(sorted(duplicates))}")
    return result


def main() -> int:
    errors: list[str] = []
    neutral = read_resx(PROPERTIES / "Resources.resx")
    neutral_keys = set(neutral)

    wpf_namespace = "{http://schemas.microsoft.com/winfx/2006/xaml/presentation}"
    for path in sorted((ROOT / "Windows").glob("*.xaml")):
        window = ET.parse(path).getroot()
        if window.tag != f"{wpf_namespace}Window":
            continue
        if window.get("Background") != "{DynamicResource NtlBackgroundBrush}":
            errors.append(f"{path.name}: window background is not theme-aware")
        if window.get("Foreground") != "{DynamicResource NtlTextBrush}":
            errors.append(f"{path.name}: window foreground is not theme-aware")

    for culture in RESOURCE_CULTURES:
        path = PROPERTIES / f"Resources.{culture}.resx"
        if not path.exists():
            errors.append(f"{culture}: resource file is missing")
            continue
        localized = read_resx(path)
        missing = sorted(neutral_keys - set(localized))
        extra = sorted(set(localized) - neutral_keys)
        empty = sorted(key for key, value in localized.items() if not value.strip())
        if missing:
            errors.append(f"{culture}: {len(missing)} missing keys: {', '.join(missing[:8])}")
        if extra:
            errors.append(f"{culture}: {len(extra)} unknown keys: {', '.join(extra[:8])}")
        if empty:
            errors.append(f"{culture}: {len(empty)} empty values: {', '.join(empty[:8])}")
        for key in neutral_keys & set(localized):
            expected = Counter(PLACEHOLDER.findall(neutral[key]))
            actual = Counter(PLACEHOLDER.findall(localized[key]))
            if expected != actual:
                errors.append(f"{culture}/{key}: placeholders {dict(actual)} != {dict(expected)}")

    references: set[str] = set()
    for path in ROOT.rglob("*.xaml"):
        references.update(XAML_RESOURCE.findall(path.read_text(encoding="utf-8-sig")))
    for path in ROOT.rglob("*.cs"):
        if ".Designer.cs" not in path.name:
            references.update(CS_RESOURCE.findall(path.read_text(encoding="utf-8-sig")))
    unknown_references = sorted(references - neutral_keys)
    if unknown_references:
        errors.append(f"resource references without neutral value: {', '.join(unknown_references)}")

    designer = (PROPERTIES / "Resources.Designer.cs").read_text(encoding="utf-8-sig")
    missing_designer_properties = sorted(key for key in XAML_RESOURCE.findall(
        "\n".join(path.read_text(encoding="utf-8-sig") for path in ROOT.rglob("*.xaml"))
    ) if not re.search(rf"public static string\s+{re.escape(key)}\b", designer))
    if missing_designer_properties:
        errors.append(
            "XAML static resources without generated property: "
            + ", ".join(sorted(set(missing_designer_properties)))
        )

    main_window_code = (ROOT / "Windows" / "MainWindow.xaml.cs").read_text(encoding="utf-8-sig")
    actual_cultures = LANGUAGE_ENTRY.findall(main_window_code)
    if actual_cultures != SELECTABLE_CULTURES:
        errors.append(f"language selector order differs: expected {SELECTABLE_CULTURES}, got {actual_cultures}")

    if errors:
        print("UI localization validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1

    print(
        f"UI localization validation passed: {len(SELECTABLE_CULTURES)} selectable cultures, "
        f"{len(RESOURCE_CULTURES)} satellite files, {len(neutral_keys)} keys each, "
        f"{len(references)} referenced UI keys."
    )
    return 0


if __name__ == "__main__":
    sys.exit(main())
