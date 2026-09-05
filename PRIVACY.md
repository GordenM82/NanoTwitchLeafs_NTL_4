# Datenschutz / Privacy

## Deutsch

NanoTwitchLeafs 4 enthält keine Telemetrie und keine Nutzungsanalyse. Das Programm überträgt keine Statistik an das ursprüngliche NanoTwitchLeafs-Analyseangebot.

Für die gewählten Funktionen stellt die Anwendung direkte Verbindungen zu Twitch, Nanoleaf-Geräten im lokalen Netzwerk sowie optional zu Streamlabs, StreamElements und HypeRate her. Dabei gelten die Datenschutzbestimmungen dieser Dienste. Bei aktivierter StreamElements-Anbindung wird ausschließlich das Ereignisthema für abgeschlossene Kanal-Tips abonniert.

Einstellungen, OAuth-Tokens und Trigger werden lokal im Benutzerprofil unter `%APPDATA%\NanoTwitchLeafs-4` gespeichert. Geheimnisse in der Einstellungsdatei werden mit Windows DPAPI an das aktuelle Windows-Benutzerkonto gebunden verschlüsselt. Sie sollten trotzdem niemals veröffentlicht oder an Fehlerberichte angehängt werden.

## English

NanoTwitchLeafs 4 contains no telemetry or usage analytics. It does not send statistics to the original NanoTwitchLeafs analytics service.

For enabled features, the application connects directly to Twitch, Nanoleaf devices on the local network and, optionally, Streamlabs, StreamElements, and HypeRate. The privacy policies of those services apply. When StreamElements is enabled, the application subscribes only to the event topic for completed channel tips.

Settings, OAuth tokens, and triggers are stored locally in the user profile under `%APPDATA%\NanoTwitchLeafs-4`. Secrets in the settings file are encrypted with Windows DPAPI and bound to the current Windows user account. They must still never be published or attached to bug reports.
