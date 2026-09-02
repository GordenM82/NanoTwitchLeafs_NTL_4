import re
import time
import xml.etree.ElementTree as ET
from pathlib import Path

from deep_translator import GoogleTranslator


ROOT = Path(__file__).resolve().parents[1]
PROPERTIES = ROOT / "Properties"
SOURCE = PROPERTIES / "Resources.resx"
TARGETS = {
    "da": ("Resources.da-DK.resx", False),
    "es": ("Resources.es-ES.resx", True),
    "fr": ("Resources.fr-FR.resx", False),
    "it": ("Resources.it-IT.resx", True),
    "nl": ("Resources.nl-NL.resx", True),
    "pl": ("Resources.pl-PL.resx", True),
    "pt": ("Resources.pt-BR.resx", False),
    "ru": ("Resources.ru-RU.resx", False),
    "sk": ("Resources.sk-SK.resx", False),
}
PLACEHOLDER = re.compile(r"(https?://\S+|\{\d+(?::[^}]+)?\}|%\w|\\[nrt])")


def protect(text):
    values = []

    def replace(match):
        values.append(match.group(0))
        return f"ZXQPH{len(values) - 1}QXZ"

    return PLACEHOLDER.sub(replace, text), values


def restore(text, values):
    for index, value in enumerate(values):
        text = text.replace(f"ZXQPH{index}QXZ", value)
        text = text.replace(f"ZXQPH {index} QXZ", value)
    return text


def translate(translator, text):
    if not text.strip():
        return text
    protected, values = protect(text)
    for attempt in range(5):
        try:
            return restore(translator.translate(protected), values)
        except Exception:
            if attempt == 4:
                raise
            time.sleep(2 ** attempt)


def values_by_name(tree):
    return {node.attrib["name"]: node.find("value") for node in tree.getroot().findall("data")}


source_tree = ET.parse(SOURCE)
source_values = values_by_name(source_tree)
ET.register_namespace("", "")

for language, (filename, create) in TARGETS.items():
    target_path = PROPERTIES / filename
    tree = ET.parse(SOURCE if create else target_path)
    target_values = values_by_name(tree)
    translator = GoogleTranslator(source="en", target=language)

    for name, source_value in source_values.items():
        if source_value is None:
            continue
        target_value = target_values.get(name)
        if target_value is None:
            source_node = next(node for node in source_tree.getroot().findall("data") if node.attrib["name"] == name)
            target_node = ET.SubElement(tree.getroot(), "data", source_node.attrib)
            target_value = ET.SubElement(target_node, "value")
            target_values[name] = target_value
        elif not create and (target_value.text or "").strip():
            continue

        target_value.text = translate(translator, source_value.text or "")

    ET.indent(tree, space="  ")
    tree.write(target_path, encoding="utf-8", xml_declaration=True)
    print(f"Completed {filename}")
