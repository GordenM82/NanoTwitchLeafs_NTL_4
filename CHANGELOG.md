# Änderungsprotokoll / Changelog

## Deutsch

> **English version below.**

## NanoTwitchLeafs 4.0.0 – Veröffentlichungskandidat

NanoTwitchLeafs 4 basiert auf Locxions letzter ursprünglicher Version 3.1.5.0 und den anschließend getesteten Zwischenständen. Version 4.0.0 ist derzeit ein Veröffentlichungskandidat und noch als experimentell einzustufen.

### Technische Grundlage und Verteilung

- WPF-Projekt von .NET Framework 4.7.2 auf .NET 10 umgestellt
- Zielplattform auf Windows 10 und Windows 11 x64 festgelegt
- eigenständiger Windows-x64-Build einschließlich benötigter .NET-Laufzeit
- direkt startbare `NanoTwitchLeafs.exe`
- reproduzierbare EXE-Erstellung über GitHub Actions eingerichtet
- veraltete beziehungsweise verwundbare Pakete aktualisiert
- Windows Media Player COM-Abhängigkeit durch den WPF-Audioplayer ersetzt
- Audioplayer-Zugriffe für die Verarbeitung in der Triggerwarteschlange threadsicher umgesetzt
- automatische Aktualisierung durch fremde ursprüngliche Releases deaktiviert

### Nanoleaf-Mehrgerätesteuerung

- freie Auswahl der reagierenden Nanoleaf-Geräte für jeden einzelnen Trigger
- beliebige Kombinationen mehrerer Nanoleafs unterstützt
- wiederverwendbare Nanoleaf-Gerätegruppen ergänzt
- Gruppenverwaltung direkt im Triggereditor erreichbar
- Gruppenauswahl wird nach dem Bearbeiten unmittelbar aktualisiert
- Geräteliste zeigt sämtliche gekoppelten Nanoleafs sowie deren Anzahl
- vorhandene Trigger bleiben kompatibel und steuern standardmäßig weiterhin alle Geräte
- Zielgeräte werden anhand ihres festen Gerätenamens statt ihrer veränderlichen IP-Adresse gespeichert
- nicht erreichbare Geräte blockieren die übrigen ausgewählten Nanoleafs nicht mehr
- fehlende oder entfernte Zielgeräte werden übersprungen und protokolliert
- vorheriger Zustand wird für jedes angesprochene Gerät getrennt gespeichert und wiederhergestellt

### Twitch EventSub

- HypeTrain-Abonnements von der entfernten EventSub-Version 1 auf Version 2 aktualisiert
- fehlende EventSub-Abonnements für Geschenk-Abos und ReSubs ergänzt
- Bedingungen für Abonnements, Cheers, Kanalpunkte und HypeTrains korrigiert
- ein von Twitch abgelehntes Abonnement verhindert nicht mehr die Anmeldung der übrigen Ereignisse
- Ergebnis jedes einzelnen EventSub-Abonnements wird protokolliert
- Anzahl der erfolgreich aktiven Abonnements wird zusammengefasst angezeigt

### Triggerverwaltung und Bedienung

- Triggerübersicht standardmäßig deutlich vergrößert
- frühere feste maximale Fensterbreite entfernt
- Triggerübersicht und Trigger-Detailfenster frei skalierbar gemacht
- vollständige Zielgeräteliste im Trigger-Detailfenster sichtbar gemacht
- Hauptfenster wird beim manuellen Start zuverlässig in den Vordergrund geholt
- unvollständiger Programmstart zeigt die konkrete Ursache und den Logpfad an
- Trigger-Soundwiedergabe unter .NET 10 korrigiert
- Threadfehler beim Beenden eines Sounds nach Ablauf eines Triggers behoben

### Datenhaltung, Ersteinrichtung und Datenschutz

- Trigger werden lokal in `triggers.json` statt dauerhaft in einer Datenbank gespeichert
- vor jeder Änderung wird `triggers.json.backup` als Sicherung erzeugt
- alte verschlüsselte Einstellungen mit einem als JSON-Objekt gespeicherten Versionswert werden unter .NET 10 korrekt gelesen
- öffentliche Builds starten ohne mitgelieferte Zugangsdaten und ohne ungefragte Datenübernahme
- beim ersten Start werden vorhandene Daten aus Locxions NanoTwitchLeafs 3.x erkannt und nur nach ausdrücklicher Bestätigung übernommen
- alte Einstellungen werden in den getrennten NTL-4-Datenordner kopiert und SQLite-Trigger sicher nach `triggers.json` konvertiert
- vor der Migration wird eine zusätzliche Sicherung erstellt; die ursprüngliche 3.x-Installation bleibt unverändert und weiterhin nutzbar
- eigene Twitch-, Streamlabs- und HypeRate-Zugangsdaten können über die API-Einstellungen eingerichtet werden
- deutschsprachige Windows-Systeme starten standardmäßig mit deutscher Oberfläche; Englisch bleibt auswählbar
- API-Einstellungen enthalten eine Kurzanleitung und einen direkten Link zur Twitch Developer Console
- Twitch-Anmeldung auf den offiziellen Gerätecode-Ablauf für öffentliche Desktop-Clients umgestellt; kein Client-Secret und keine lokale Redirect-Verbindung erforderlich
- API-Einstellungen neu angeordnet und scrollbar gemacht, damit Hilfetext und Schaltflächen auch bei kleiner Fensterhöhe nicht überlappen
- Verbindungstest auf lesbare, umgebrochene Statusmeldungen reduziert; technische IRC-Rohdaten bleiben nur im Debugprotokoll
- ein nicht verfügbares Twitch-Profilbild verwendet unauffällig das Standardbild und gilt nicht mehr als Programmfehler
- Einstellungen und Trigger werden unter `%APPDATA%\NanoTwitchLeafs-4` gespeichert
- Telemetrie und die ursprüngliche Nutzungsanalyse sind in NanoTwitchLeafs 4 deaktiviert
- die GitHub-Action erstellt ausschließlich ein bereinigtes Windows-x64-ZIP ohne Zugangsdaten

---

## English

## NanoTwitchLeafs 4.0.0 – release candidate

NanoTwitchLeafs 4 is based on Locxion's last original version 3.1.5.0 and the subsequently tested intermediate builds. Version 4.0.0 is currently a release candidate and should still be considered experimental.

### Technical foundation and distribution

- migrated the WPF project from .NET Framework 4.7.2 to .NET 10
- set the target platform to 64-bit Windows 10 and Windows 11
- added a self-contained Windows x64 build including the required .NET runtime
- added a directly executable `NanoTwitchLeafs.exe`
- added reproducible EXE creation through GitHub Actions
- updated outdated or vulnerable packages
- replaced the Windows Media Player COM dependency with the WPF media player
- made media-player access safe for background trigger-queue processing
- disabled automatic updates from unrelated original releases

### Multi-device Nanoleaf control

- added per-trigger selection of the Nanoleaf devices that should react
- added support for arbitrary combinations of multiple Nanoleaf devices
- added reusable Nanoleaf device groups
- made group management directly accessible from the trigger editor
- group selection now refreshes immediately after editing
- the device list displays every paired Nanoleaf and the total device count
- existing triggers remain compatible and continue to target all devices by default
- targets are stored by stable device name instead of a changeable IP address
- one unreachable device no longer prevents the remaining selected devices from reacting
- missing or removed targets are skipped and logged
- the previous state is stored and restored separately for every targeted device

### Twitch EventSub

- updated Hype Train subscriptions from the removed EventSub version 1 to version 2
- added missing EventSub subscriptions for gift subscriptions and resubscriptions
- corrected subscription conditions for subscriptions, cheers, channel points, and Hype Trains
- one subscription rejected by Twitch no longer prevents the remaining events from subscribing
- the result of every individual EventSub subscription is logged
- the number of successfully active subscriptions is displayed as a summary

### Trigger management and user interface

- significantly increased the default trigger-overview size
- removed the previous fixed maximum window width
- made the trigger overview and trigger-details window freely resizable
- made the complete target-device list visible in the trigger-details window
- the main window now reliably comes to the foreground after a manual start
- incomplete startup now displays the specific cause and log path
- corrected trigger sound playback under .NET 10
- fixed the cross-thread error when stopping sound after a trigger ends

### Data storage, initial setup, and privacy

- triggers are stored locally in `triggers.json` instead of requiring a permanent database
- `triggers.json.backup` is created before each change
- old encrypted settings containing a version stored as a JSON object are read correctly under .NET 10
- public builds start without bundled credentials and without importing existing data unexpectedly
- existing data from Locxion's NanoTwitchLeafs 3.x is detected on first launch and imported only after explicit confirmation
- previous settings are copied into the separate NTL 4 data folder and SQLite triggers are safely converted to `triggers.json`
- an additional backup is created before migration; the original 3.x installation remains unchanged and usable
- personal Twitch, Streamlabs, and HypeRate credentials can be configured through API Settings
- German-language Windows systems now start with the German interface by default; English remains selectable
- API Settings include brief instructions and a direct link to the Twitch Developer Console
- changed Twitch authentication to the official device-code flow for public desktop clients; no Client Secret or local redirect listener is required
- rearranged API Settings and made the page scrollable so help text and buttons do not overlap at smaller window heights
- reduced the connection test to readable wrapped status messages; raw IRC traffic remains in the debug log only
- an unavailable Twitch profile image now quietly uses the default image instead of being treated as an application error
- settings and triggers are stored under `%APPDATA%\NanoTwitchLeafs-4`
- telemetry and the original usage analytics are disabled in NanoTwitchLeafs 4
- GitHub Actions produces a sanitized Windows x64 ZIP without credentials
