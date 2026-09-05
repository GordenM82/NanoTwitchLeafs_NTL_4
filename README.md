# NanoTwitchLeafs 4

![NanoTwitchLeafs logo](https://user-images.githubusercontent.com/16882079/214092102-4447d44f-807b-4bbb-a85c-2d295643ee6b.png)

![Windows 10/11](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D4)
![Platform x64](https://img.shields.io/badge/Platform-x64-6f42c1)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)
![License GPL-3.0](https://img.shields.io/badge/Lizenz-GPL--3.0-green)
![Status stable](https://img.shields.io/badge/Status-stabil-brightgreen)
![Version](https://img.shields.io/github/v/release/GordenM82/NanoTwitchLeafs_NTL_4?label=Version)
![Downloads total](https://img.shields.io/github/downloads/GordenM82/NanoTwitchLeafs_NTL_4/total?label=Downloads)
![Downloads latest](https://img.shields.io/github/downloads/GordenM82/NanoTwitchLeafs_NTL_4/latest/total?label=latest%20DL)

## Deutsch

> **English version below.**

NanoTwitchLeafs verbindet Twitch mit Nanoleaf-Leuchten. Chatbefehle, Twitch-Ereignisse, Spenden und HypeRate können Farben, Helligkeiten, Effekte und optionale Sounds auf einem oder mehreren Nanoleaf-Geräten auslösen.

### Projektstatus

**NanoTwitchLeafs 4.1.0** ist die stabile öffentliche Ausgabe der modernisierten Benutzeroberfläche. Sie wird unter dem GitHub-Konto **GordenM82** als Weiterentwicklung des ursprünglichen GPL-3.0-Projekts von **Locxion** gepflegt. Bei Weiterentwicklung, Fehlersuche und Dokumentation wurde KI-Unterstützung eingesetzt. Regelmäßige Aktualisierungen oder dauerhafter Support werden nicht zugesagt.

Die fertige Windows-x64-Version steht unter [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest) bereit.

### Funktionen

- Twitch-Chatbefehle und Schlüsselwörter
- Twitch-Ereignisse für Follower, Abonnements, ReSubs, Geschenk-Abos, Bits und Raids
- Einlösung benutzerdefinierter Kanalpunkte
- HypeTrain-Ereignisse über Twitch EventSub v2
- HypeRate-Anbindung
- Spendenereignisse über Streamlabs und StreamElements mit wählbarer Quelle pro Trigger
- integrierte Twitch-Benutzer-Blocklist
- Steuerung eines oder mehrerer Nanoleaf-Geräte
- freie Zielgeräteauswahl und wiederverwendbare Gerätegruppen
- Wiederherstellung des vorherigen Nanoleaf-Zustands nach einem Trigger
- optionale Soundwiedergabe

### Oberfläche und Bedienung

- zentrale Navigation für Chat/Konsole, Twitch, Nano, Trigger, Integrationen, Einstellungen, Hilfe und Info
- Verwaltung von Triggern, Antworten, Geräten, Gerätegruppen und Blocklist im Hauptfenster
- Hell-, Dunkel- und Systemdesign mit wählbarer Akzentfarbe
- skalierbare Oberfläche für 100 %, 125 % und 150 % Windows-Skalierung
- Mehrmonitorunterstützung mit Wiederherstellung von Fenstergröße, Position und maximiertem Zustand
- monitorgebundene Haupt- und Unterfenster unter Berücksichtigung der Taskleiste
- Tastatur- und Fokusverbesserungen, `Strg+F`, Escape-Rückkehr und Hinweise bei ungespeicherten Änderungen
- Toastmeldungen und erklärende Tooltips für gekürzte oder deaktivierte Funktionen

### Triggerverwaltung

- Suche über Trigger-Typ, Befehl, Effekt, Sound, Spendenanbieter und Zielgeräte
- Filter nach aktiv, inaktiv, problematisch und Trigger-Kategorie
- Trefferanzeige und gemeinsames Zurücksetzen der Filter
- Hinweise auf fehlende Nanoleaf-Geräte, Effekte oder Sounddateien
- Erstellen, Testen, Bearbeiten, Duplizieren und Löschen
- Import und Export
- lokale Speicherung in `triggers.json` mit Sicherung als `triggers.json.backup`

### Integrationen

- **Twitch:** Anmeldung mit der enthaltenen öffentlichen Desktop-Client-ID; kein Client-Secret notwendig
- **Streamlabs:** optionale Spendenereignisse
- **StreamElements:** optionale Tip-Ereignisse über die Astro-WebSocket-Verbindung, automatische Verbindung und lokale Testspende
- **HypeRate:** optionale Herzfrequenz-Ereignisse

Für StreamElements unter **Integrationen → StreamElements** die Integration aktivieren, den passenden JWT- oder Overlay-Token eintragen, speichern und verbinden. Die lokale Testspende prüft Trigger ohne echte Zahlung und ohne aktive Verbindung. Ein eigener Hilfereiter erklärt die Einrichtung. Tokens werden mit Windows DPAPI an das aktuelle Windows-Benutzerkonto gebunden verschlüsselt.

### Konsole und Support

- Suche und Filter für Information, Warnung, Fehler und Debug
- farbliche Hervorhebung, Trefferanzahl und umschaltbares automatisches Scrollen
- Konsole leeren, Logdatei öffnen und Einträge über das Kontextmenü kopieren
- bereinigtes Support-Protokoll ohne Tokens, API-Schlüssel, Benutzernamen und persönlichen Windows-Profilpfad

### Sprachen

Die Oberfläche ist auswählbar in Deutsch, Englisch, Dänisch, Spanisch, Französisch, Italienisch, Niederländisch, Polnisch, Portugiesisch (Brasilien), Slowakisch und Russisch. Englisch ist die neutrale Standardsprache; auf einem neuen deutschsprachigen Windows-Profil wird Deutsch vorausgewählt.

### Systemanforderungen

- Windows 10 oder Windows 11
- 64-Bit-System (`x64`)
- Netzwerkzugriff auf die verwendeten Nanoleaf-Geräte
- Twitch-Konto für Twitch-Funktionen

Windows 7, Windows 8/8.1, 32-Bit-Windows, Linux und ARM werden nicht unterstützt.

### Installation und erster Start

1. Das aktuelle Windows-x64-ZIP unter [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest) herunterladen.
2. Das ZIP vollständig in einen eigenen Ordner entpacken.
3. `NanoTwitchLeafs.exe` starten.
4. Unter **Twitch** das Twitch-Konto verbinden.
5. Unter **Nano** die Nanoleaf-Geräte koppeln beziehungsweise prüfen.
6. Unter **Trigger** die gewünschten Trigger einrichten.
7. Optionale Dienste unter **Integrationen** konfigurieren.

Für Twitch sind weder eine eigene Twitch-Anwendung noch eine manuell einzutragende Client-ID oder ein Client-Secret erforderlich.

> **Wichtig:** Tokens und persönliche Einstellungsdateien niemals in ein öffentliches Repository, einen Fehlerbericht oder einen Screenshot hochladen.

### Update innerhalb von NanoTwitchLeafs 4

1. NanoTwitchLeafs vollständig beenden.
2. Das neue Windows-x64-ZIP herunterladen und in einen neuen Ordner entpacken.
3. `NanoTwitchLeafs.exe` aus dem neuen Ordner starten.
4. Verbindungen, Geräte und Trigger prüfen; danach kann der alte Programmordner entfernt werden.

Benutzerdaten liegen getrennt unter `%APPDATA%\NanoTwitchLeafs-4` und werden von neueren 4.x-Versionen automatisch weiterverwendet. Vor wichtigen Updates empfiehlt sich eine Sicherung dieses Ordners. Daten früherer Entwicklungsteststände wurden absichtlich getrennt gespeichert und überschreiben die stabile Konfiguration nicht automatisch.

### Update von NanoTwitchLeafs 3.x

Beim ersten Start erkennt NTL 4 eine vorhandene Installation der letzten ursprünglichen 3.x-Version und bietet die Übernahme an. Einstellungen werden erst nach Bestätigung kopiert; Trigger werden aus der alten SQLite-Datenbank gelesen und in `triggers.json` geschrieben. Zuvor entsteht eine Sicherung unter `%APPDATA%\NanoTwitchLeafs-4\Migration-Backup-3.x`. Die ursprüngliche 3.x-Installation bleibt unverändert.

### Lokale Daten, Stabilität und Datenschutz

Einstellungen, Trigger, Gerätegruppen und Protokolle werden unter `%APPDATA%\NanoTwitchLeafs-4` gespeichert. Beschädigte Einstellungsdateien werden durch sichere Standardwerte ersetzt und als `.invalid-*.bak` erhalten. NTL enthält keine Telemetrie oder Nutzungsanalyse. Weitere Angaben stehen in [PRIVACY.md](PRIVACY.md).

### Fehlerberichte und Vorschläge

Fehlerberichte und Vorschläge können über die [GitHub-Issues](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/issues) eingereicht werden. Bitte keine Zugangsdaten, OAuth-Tokens oder vollständigen Einstellungsdateien anhängen. Ein bereinigtes Support-Protokoll kann direkt in der NTL-Konsole erstellt werden.

### Ursprung und Lizenz

NanoTwitchLeafs wurde ursprünglich von **Locxion (Markus Bender)** entwickelt. Das ursprüngliche Projekt befindet sich unter [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs). NanoTwitchLeafs 4 ist eine veränderte und modernisierte Fassung unter dem GitHub-Konto **GordenM82** und bleibt unter der [GNU General Public License Version 3](LICENSE) lizenziert.

Besonderer Dank aus dem ursprünglichen Projekt gilt Daniel Hottmeyer (`@Silverdark`) und Denis Freund (`@revyn112`). Der vollständige zweisprachige Verlauf steht im [Änderungsprotokoll](CHANGELOG.md).

---

## English

NanoTwitchLeafs connects Twitch with Nanoleaf lights. Chat commands, Twitch events, donations, and HypeRate can trigger colors, brightness levels, effects, and optional sounds on one or multiple Nanoleaf devices.

### Project status

**NanoTwitchLeafs 4.1.0** is the stable public release of the modernized user interface. It is maintained under the GitHub account **GordenM82** as a continuation of **Locxion**'s original GPL-3.0 project. AI assistance was used for continued development, troubleshooting, and documentation. Regular updates or permanent support are not guaranteed.

The ready-to-run Windows x64 package is available under [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest).

### Features

- Twitch chat commands and keywords
- Twitch events for followers, subscriptions, resubscriptions, gift subscriptions, Bits, and raids
- custom channel-point redemptions
- Hype Train events through Twitch EventSub v2
- HypeRate integration
- Streamlabs and StreamElements donations with a selectable provider per trigger
- integrated Twitch-user blocklist
- control of one or multiple Nanoleaf devices
- per-trigger target selection and reusable device groups
- restoration of the previous Nanoleaf state after a trigger
- optional trigger sounds

### Interface and operation

- central navigation for Chat/Console, Twitch, Nano, Triggers, Integrations, Settings, Help, and Info
- trigger, response, device, device-group, and blocklist management inside the main window
- Light, Dark, and System themes with a selectable accent color
- scalable interface tested at 100%, 125%, and 150% Windows scaling
- multi-monitor support with restored window size, position, and maximized state
- monitor-bound main and child windows that respect the taskbar work area
- keyboard and focus improvements, `Ctrl+F`, Escape navigation, and unsaved-change warnings
- toast messages and explanatory tooltips for truncated or disabled functions

### Trigger management

- search across trigger type, command, effect, sound, donation provider, and target devices
- active, inactive, problem, and category filters
- result count and one-click filter reset
- warnings for missing Nanoleaf devices, effects, and sound files
- create, test, edit, duplicate, and delete actions
- trigger import and export
- local storage in `triggers.json` with `triggers.json.backup`

### Integrations

- **Twitch:** sign in with the bundled public desktop client ID; no client secret required
- **Streamlabs:** optional donation events
- **StreamElements:** optional tip events through the Astro WebSocket connection, automatic connection, and local test donation
- **HypeRate:** optional heart-rate events

For StreamElements, open **Integrations → StreamElements**, enable it, enter the appropriate JWT or overlay token, save, and connect. The local test donation tests triggers without a real payment or active connection. A dedicated help tab explains the setup. Tokens are encrypted with Windows DPAPI and bound to the current Windows user account.

### Console and support

- search and filters for Information, Warning, Error, and Debug
- level colors, result count, and configurable automatic scrolling
- clear the console, open the log, and copy entries from the context menu
- sanitized support log without tokens, API keys, usernames, or the personal Windows profile path

### Languages

The interface is selectable in German, English, Danish, Spanish, French, Italian, Dutch, Polish, Brazilian Portuguese, Slovak, and Russian. English is the neutral default; German is preselected for a new profile on German-language Windows systems.

### System requirements

- Windows 10 or Windows 11
- 64-bit system (`x64`)
- network access to the Nanoleaf devices in use
- Twitch account for Twitch features

Windows 7, Windows 8/8.1, 32-bit Windows, Linux, and ARM are not supported.

### Installation and first start

1. Download the current Windows x64 ZIP from [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest).
2. Extract the complete ZIP into its own folder.
3. Start `NanoTwitchLeafs.exe`.
4. Link the Twitch account under **Twitch**.
5. Pair or verify Nanoleaf devices under **Nano**.
6. Configure the desired triggers under **Triggers**.
7. Configure optional services under **Integrations**.

Twitch does not require users to register their own application or enter a client ID or client secret manually.

> **Important:** Never upload tokens or personal settings files to a public repository, bug report, or screenshot.

### Updating within NanoTwitchLeafs 4

1. Close NanoTwitchLeafs completely.
2. Download the new Windows x64 ZIP and extract it into a new folder.
3. Start `NanoTwitchLeafs.exe` from the new folder.
4. Verify connections, devices, and triggers; then remove the old program folder if desired.

User data is stored separately under `%APPDATA%\NanoTwitchLeafs-4` and reused automatically by newer 4.x versions. Backing up this folder before important updates is recommended. Data from earlier development test builds was deliberately stored separately and does not overwrite the stable configuration automatically.

### Updating from NanoTwitchLeafs 3.x

On first start, NTL 4 detects the last original 3.x installation and offers an import. Settings are copied only after confirmation; triggers are read from the old SQLite database and written to `triggers.json`. A backup is created under `%APPDATA%\NanoTwitchLeafs-4\Migration-Backup-3.x` first. The original 3.x installation remains unchanged.

### Local data, stability, and privacy

Settings, triggers, device groups, and logs are stored under `%APPDATA%\NanoTwitchLeafs-4`. Damaged settings files are replaced by safe defaults and retained as `.invalid-*.bak`. NTL contains no telemetry or usage analytics. See [PRIVACY.md](PRIVACY.md) for details.

### Bug reports and suggestions

Bug reports and suggestions can be submitted through [GitHub Issues](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/issues). Do not attach credentials, OAuth tokens, or complete settings files. A sanitized support log can be created directly from the NTL console.

### Origin and license

NanoTwitchLeafs was originally developed by **Locxion (Markus Bender)**. The original project is available at [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs). NanoTwitchLeafs 4 is a modified and modernized edition maintained under the GitHub account **GordenM82** and remains licensed under the [GNU General Public License Version 3](LICENSE).

Special thanks from the original project go to Daniel Hottmeyer (`@Silverdark`) and Denis Freund (`@revyn112`). The complete bilingual history is available in the [changelog](CHANGELOG.md).
