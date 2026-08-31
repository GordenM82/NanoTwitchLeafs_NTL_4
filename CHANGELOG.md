# Änderungsprotokoll / Changelog

## Deutsch

> **English version below.**

## NanoTwitchLeafs 4.0.0 – öffentliche Erstveröffentlichung (31. August 2026)

Version 4.0.0 ist die erste öffentliche Veröffentlichung der modernisierten NanoTwitchLeafs-Version.

- öffentliche Twitch-Client-ID der Desktop-Anwendung integriert, sodass Nutzer keine eigene Twitch-Anwendung mehr registrieren und keine Client-ID mehr eintragen müssen
- Twitch-Anmeldung auf **Verbinde Twitch Account** und die Bestätigung bei Twitch vereinfacht
- Twitch-Client-Secret vollständig aus Datenmodell, Oberfläche, Authentifizierungsmethoden und Token-Aktualisierung entfernt
- bisherige Twitch-Credential-Hilfsklasse sowie die zugehörigen Eingabefelder, Ersteinrichtungsmeldungen und Hilfetexte entfernt
- Twitch API, EventSub und Profilabfragen verwenden einheitlich die öffentliche Anwendungskennung
- README für öffentliche Veröffentlichung, direkten Start, manuelle 4.x-Aktualisierung und dauerhaften Import aus 3.x überarbeitet
- Versions- sowie Download-Badges für die öffentliche GitHub-Veröffentlichung ergänzt
- Windows-x64-Paket als stabile Version 4.0.0 veröffentlicht
- Projekt bleibt experimentell; regelmäßige Aktualisierungen, Support und dauerhafte Weiterentwicklung sind nicht zugesagt

## NanoTwitchLeafs 4.0.0-rc.2 – privater Veröffentlichungskandidat (31. August 2026)

Dieser zweite private Veröffentlichungskandidat setzt die Rückmeldungen aus der Quellcodeprüfung nach RC1 um.

- neue Programmtexte aus NTL 4 in das vorhandene Sprachressourcensystem überführt; keine fest eincodierten deutsch/englischen Meldungen mehr in der Hauptfensterlogik
- Englisch bleibt die neutrale Standardsprache; auf deutschsprachigem Windows wird bei einer neuen Konfiguration zuverlässig Deutsch vorausgewählt
- Telemetrie, Analytics-Controller und zugehörige Datenmodelle vollständig entfernt, da der frühere Server nicht mehr verfügbar ist
- mitgelieferte `ServiceCredentials`-Logik vollständig entfernt; Twitch-, Streamlabs- und HypeRate-Zugangsdaten werden ausschließlich aus den lokalen Benutzereinstellungen gelesen
- direkten Importpfad von 3.x-Einstellungen und SQLite-Triggern als dauerhafte NTL-4-Kompatibilitätsfunktion festgelegt
- KI-Unterstützung bei Weiterentwicklung, Fehlersuche und Dokumentation transparent in der README ausgewiesen

## NanoTwitchLeafs 4.0.0-rc.1 – privater Veröffentlichungskandidat (31. August 2026)

NanoTwitchLeafs 4 basiert auf Locxions letzter ursprünglicher Version 3.1.5.0 und den anschließend getesteten Zwischenständen. `v4.0.0-rc.1` ist der erste private Veröffentlichungskandidat für eingeladene Tester und noch als experimentell einzustufen.

### Technische Grundlage und Verteilung

- WPF-Projekt von .NET Framework 4.7.2 auf .NET 10 umgestellt
- Zielplattform auf Windows 10 und Windows 11 x64 festgelegt
- eigenständiger Windows-x64-Build einschließlich benötigter .NET-Laufzeit
- direkt startbare `NanoTwitchLeafs.exe`
- reproduzierbare EXE-Erstellung über GitHub Actions eingerichtet
- privates Pre-Release `v4.0.0-rc.1` mit bereinigtem Windows-x64-ZIP und zweisprachigen Veröffentlichungshinweisen erstellt
- veraltete .NET-Framework-Projekt-, Installer-, Build- und Konfigurationsdateien aus dem NTL-4-Quellbaum entfernt
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
- Telemetrie und die ursprüngliche Nutzungsanalyse sind in NanoTwitchLeafs 4 nicht aktiv
- die GitHub-Action erstellt ausschließlich ein bereinigtes Windows-x64-ZIP ohne Zugangsdaten

---

## English

## NanoTwitchLeafs 4.0.0 – initial public release (31 August 2026)

Version 4.0.0 is the first public release of the modernized NanoTwitchLeafs application.

- integrated the desktop application's public Twitch client ID so users no longer need to register their own Twitch application or enter a client ID
- simplified Twitch sign-in to selecting **Link Twitch Account** and confirming the request with Twitch
- completely removed the Twitch client secret from the data model, user interface, authentication methods, and token refresh handling
- removed the former Twitch credential helper class together with its input fields, initial-setup prompts, and help text
- made Twitch API, EventSub, and profile requests use the same public application identifier
- revised the README for the public release, direct startup, manual 4.x updates, and the permanent import path from 3.x
- added version and download badges for the public GitHub release
- published the Windows x64 package as stable version 4.0.0
- the project remains experimental; regular updates, support, and continued long-term development are not guaranteed

## NanoTwitchLeafs 4.0.0-rc.2 – private release candidate (31 August 2026)

This second private release candidate implements the source-review feedback received after RC1.

- moved new NTL 4 user-facing text into the existing language-resource system; the main-window logic no longer contains hard-coded German/English messages
- kept English as the neutral default language while reliably preselecting German for new configurations on German-language Windows systems
- completely removed telemetry, the analytics controller, and related data models because the former server is no longer available
- completely removed bundled `ServiceCredentials` handling; Twitch, Streamlabs, and HypeRate credentials are read exclusively from local user settings
- established direct import of 3.x settings and SQLite triggers as a permanent NTL 4 compatibility feature
- disclosed the use of AI assistance for continued development, troubleshooting, and documentation in the README

## NanoTwitchLeafs 4.0.0-rc.1 – private release candidate (31 August 2026)

NanoTwitchLeafs 4 is based on Locxion's last original version 3.1.5.0 and the subsequently tested intermediate builds. `v4.0.0-rc.1` is the first private release candidate for invited testers and should still be considered experimental.

### Technical foundation and distribution

- migrated the WPF project from .NET Framework 4.7.2 to .NET 10
- set the target platform to 64-bit Windows 10 and Windows 11
- added a self-contained Windows x64 build including the required .NET runtime
- added a directly executable `NanoTwitchLeafs.exe`
- added reproducible EXE creation through GitHub Actions
- created private pre-release `v4.0.0-rc.1` with a sanitized Windows x64 ZIP and bilingual release notes
- removed obsolete .NET Framework project, installer, build, and configuration files from the NTL 4 source tree
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
- telemetry and the original usage analytics are not active in NanoTwitchLeafs 4
- GitHub Actions produces a sanitized Windows x64 ZIP without credentials
