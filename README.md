# NanoTwitchLeafs 4

![NanoTwitchLeafs logo](https://user-images.githubusercontent.com/16882079/214092102-4447d44f-807b-4bbb-a85c-2d295643ee6b.png)

![Windows 10/11](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D4)
![Platform x64](https://img.shields.io/badge/Platform-x64-6f42c1)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)
![License GPL-3.0](https://img.shields.io/badge/Lizenz-GPL--3.0-green)
![Status experimental](https://img.shields.io/badge/Status-experimentell-orange)
![Version](https://img.shields.io/github/v/release/GordenM82/NanoTwitchLeafs_NTL_4?label=Version)
![Downloads total](https://img.shields.io/github/downloads/GordenM82/NanoTwitchLeafs_NTL_4/total?label=Downloads)
![Downloads latest](https://img.shields.io/github/downloads/GordenM82/NanoTwitchLeafs_NTL_4/latest/total?label=latest%20DL)

## Deutsch

> **English version below.**

NanoTwitchLeafs verbindet Twitch mit Nanoleaf-Leuchten. Chatbefehle und Twitch-Ereignisse können dadurch Farben, Helligkeiten und Effekte auf einem oder mehreren Nanoleaf-Geräten auslösen.

### Projektstatus

NanoTwitchLeafs 4 ist eine experimentelle Modernisierung und Weiterentwicklung des ursprünglichen GPL-3.0-Projekts von **Locxion**. Der aktuelle Stand wird unter dem GitHub-Konto **GordenM82** bearbeitet. Bei Weiterentwicklung, Fehlersuche und Dokumentation wurde KI-Unterstützung eingesetzt.

Die öffentliche Version **4.0.1** steht unter [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest) als direkt startbares Windows-x64-Paket bereit. Trotz der Veröffentlichung bleibt das Projekt experimentell. Es gibt keine Zusage für regelmäßige Aktualisierungen, Support oder eine dauerhafte Weiterentwicklung.

### Funktionen

- Twitch-Chatbefehle und Schlüsselwörter
- Twitch-Ereignisse für Follower, Abonnements, ReSubs, Geschenk-Abos, Bits und Raids
- Einlösung benutzerdefinierter Kanalpunkte
- HypeTrain-Ereignisse über Twitch EventSub v2
- HypeRate-Anbindung
- Streamlabs-Spendenereignisse
- Steuerung mehrerer Nanoleaf-Geräte
- freie Auswahl der Zielgeräte für jeden Trigger
- beliebige Kombinationen mehrerer Nanoleafs
- wiederverwendbare Nanoleaf-Gerätegruppen
- Wiederherstellung des vorherigen Nanoleaf-Zustands nach einem Trigger
- optionale Soundwiedergabe für Trigger

### Neuerungen in Version 4

- Umstellung von .NET Framework 4.7.2 auf .NET 10
- eigenständiger Windows-x64-Build einschließlich benötigter .NET-Laufzeit
- direkt startbare `NanoTwitchLeafs.exe`
- Unterstützung von Windows 10 und Windows 11 auf 64-Bit-Systemen
- Triggerverwaltung ohne dauerhaft benötigte Datenbank
- lokale Speicherung der Trigger in `triggers.json`
- automatische Sicherung als `triggers.json.backup`
- Mehrfachauswahl und Gruppen für Nanoleaf-Zielgeräte
- aktualisierte Twitch-EventSub-Abonnements einschließlich HypeTrain v2
- vergrößerte und frei skalierbare Triggerfenster
- modernisierte Soundwiedergabe ohne Abhängigkeit vom Windows Media Player COM-Modul
- neue Programmtexte in das vorhandene Sprachressourcensystem integriert
- Englisch als neutrale Standardsprache; auf deutschsprachigen Windows-Systemen wird bei einer neuen Konfiguration automatisch Deutsch gewählt und Englisch bleibt auswählbar
- die öffentliche Twitch-Client-ID ist in der Anwendung enthalten; für die Twitch-Anmeldung genügt ein Klick auf **Verbinde Twitch Account** und ein Client-Secret wird nicht verwendet
- Streamlabs- und HypeRate-Zugangsdaten werden nur bei Nutzung dieser optionalen Dienste lokal gespeichert
- sauberer Erststart ohne ungefragte Übernahme vorhandener Daten
- frühere Telemetrie- und Nutzungsanalyse-Komponenten vollständig entfernt

Der vollständige zweisprachige Verlauf seit der letzten ursprünglichen Locxion-Version steht im [Änderungsprotokoll](CHANGELOG.md). Hinweise zur lokalen Speicherung und zu externen Verbindungen stehen unter [Datenschutz](PRIVACY.md).

### Systemanforderungen

- Windows 10 oder Windows 11
- 64-Bit-System (`x64`)
- Netzwerkzugriff auf die verwendeten Nanoleaf-Geräte
- Twitch-Konto

Windows 7, Windows 8/8.1, 32-Bit-Windows, Linux und ARM werden nicht unterstützt.

### Installation und erster Start

1. Lade das aktuelle Windows-x64-ZIP unter [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest) herunter.
2. Entpacke das ZIP vollständig in einen eigenen Ordner.
3. Starte `NanoTwitchLeafs.exe`.
4. Öffne **Twitch**, wähle **Verbinde Twitch Account** und bestätige die Anmeldung bei Twitch.
5. Öffne **Nano** und kopple beziehungsweise prüfe deine Nanoleaf-Geräte.
6. Richte anschließend unter **Nano → Trigger bearbeiten** die gewünschten Trigger ein.

Für die Twitch-Anmeldung sind weder eine eigene Twitch-Anwendung noch eine manuell einzutragende Client-ID oder ein Client-Secret erforderlich. Zugangsdaten für Streamlabs oder HypeRate müssen nur eingetragen werden, wenn der jeweilige optionale Dienst verwendet wird. Beim ersten Start verwendet NanoTwitchLeafs Englisch; auf einem deutschsprachigen Windows wird Deutsch automatisch ausgewählt. Die Sprache kann später unter **Einstellungen** geändert werden.

> **Wichtig:** Tokens und persönliche Einstellungsdateien dürfen niemals in ein öffentliches Repository, einen Fehlerbericht oder einen Screenshot hochgeladen werden.

### Update innerhalb von NanoTwitchLeafs 4

1. Beende NanoTwitchLeafs vollständig.
2. Lade das neue Windows-x64-ZIP unter [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest) herunter.
3. Entpacke es in einen neuen Ordner und starte dort `NanoTwitchLeafs.exe`.
4. Prüfe Verbindung, Geräte und Trigger. Den alten Programmordner kannst du nach einem erfolgreichen Test entfernen.

Die Benutzerdaten liegen getrennt unter `%APPDATA%\NanoTwitchLeafs-4` und werden von einer neueren 4.x-Version automatisch weiterverwendet. Vor wichtigen Updates empfiehlt sich trotzdem eine Sicherung dieses Ordners.

### Update von NanoTwitchLeafs 3.x

NanoTwitchLeafs 4 erkennt beim ersten Start eine vorhandene Installation der letzten originalen 3.x-Version und bietet die Übernahme an:

1. NanoTwitchLeafs 3.x vollständig beenden.
2. NanoTwitchLeafs 4 in einen neuen, getrennten Ordner entpacken.
3. `NanoTwitchLeafs.exe` starten.
4. Die angebotene Übernahme bestätigen.
5. Einstellungen werden in den neuen NTL-4-Datenordner kopiert.
6. Trigger werden aus der alten SQLite-Datenbank gelesen und in die lokale `triggers.json` von NTL 4 geschrieben.
7. Twitch anschließend einmal über **Twitch → Verbinde Twitch Account** anmelden.

Vor der Übernahme legt NTL 4 eine Sicherung unter `%APPDATA%\NanoTwitchLeafs-4\Migration-Backup-3.x` an. Die Daten der alten Installation werden nur gelesen und bleiben unverändert. Der Importpfad bleibt auch in späteren 4.x-Versionen erhalten, damit ein direkter Umstieg von 3.x weiterhin möglich ist.

Wird die Übernahme abgelehnt, beginnt NTL 4 mit einer leeren Konfiguration. Solange noch keine NTL-4-Daten angelegt wurden, wird beim nächsten Start erneut gefragt.

### Lokale Daten und Datenschutz

NanoTwitchLeafs speichert Einstellungen, Trigger, Gerätegruppen und weitere lokale Daten unter `%APPDATA%\NanoTwitchLeafs-4`. Ohne gefundene oder übernommene 3.x-Daten beginnt das Programm mit einer leeren Konfiguration. Es wird kein Client-Secret mitgeliefert und es werden keine Telemetriedaten versendet.

### Fehlerberichte und Vorschläge

Fehlerberichte und Vorschläge können über die GitHub-Issues dieses Repositories eingereicht werden. Bitte keine Zugangsdaten, OAuth-Tokens, Client-Secrets oder vollständigen Einstellungsdateien anhängen.

Da sich das Projekt in einem experimentellen Stadium befindet und derzeit nicht feststeht, ob es dauerhaft weitergeführt wird, entsteht aus einem eingereichten Vorschlag kein Anspruch auf Umsetzung, Aktualisierungen oder Support.

### Entwicklungstransparenz

Teile der Weiterentwicklung, Fehlersuche und Dokumentation wurden mithilfe künstlicher Intelligenz erstellt oder bearbeitet. Alle Änderungen werden vom Maintainer geprüft und praktisch getestet. Das ursprüngliche Projekt und seine wesentliche Architektur stammen von Locxion.

### Ursprung und Lizenz

NanoTwitchLeafs wurde ursprünglich von **Locxion (Markus Bender)** entwickelt. Das ursprüngliche Projekt und seine Geschichte befinden sich im Repository [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs).

NanoTwitchLeafs 4 ist eine veränderte und modernisierte Fassung, die unter dem GitHub-Konto **GordenM82** erstellt wird. Sie bleibt gemäß den Bedingungen der [GNU General Public License Version 3](LICENSE) unter GPL-3.0 lizenziert.

Besonderer Dank aus dem ursprünglichen Projekt gilt Daniel Hottmeyer (`@Silverdark`) und Denis Freund (`@revyn112`).

---

## English

NanoTwitchLeafs connects Twitch with Nanoleaf lights. Chat commands and Twitch events can trigger colors, brightness levels, and effects on one or multiple Nanoleaf devices.

### Project status

NanoTwitchLeafs 4 is an experimental modernization and continuation of **Locxion**'s original GPL-3.0 project. The current version is maintained under the GitHub account **GordenM82**. AI assistance was used for continued development, troubleshooting, and documentation.

Public version **4.0.1** is available from [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest) as a directly executable Windows x64 package. The project remains experimental despite this public release. Regular updates, support, and continued long-term development are not guaranteed.

### Features

- Twitch chat commands and keywords
- Twitch events for followers, subscriptions, resubscriptions, gift subscriptions, Bits, and raids
- custom channel-point redemptions
- Hype Train events through Twitch EventSub v2
- HypeRate integration
- Streamlabs donation events
- control of multiple Nanoleaf devices
- per-trigger target-device selection
- arbitrary combinations of multiple Nanoleaf devices
- reusable Nanoleaf device groups
- restoration of the previous Nanoleaf state after a trigger
- optional sound playback for triggers

### What's new in version 4

- migrated from .NET Framework 4.7.2 to .NET 10
- self-contained Windows x64 build including the required .NET runtime
- directly executable `NanoTwitchLeafs.exe`
- support for 64-bit Windows 10 and Windows 11
- trigger management without a permanently required database
- local trigger storage in `triggers.json`
- automatic backup to `triggers.json.backup`
- multi-device selection and Nanoleaf device groups
- updated Twitch EventSub subscriptions, including Hype Train v2
- larger and freely resizable trigger windows
- modernized sound playback without the Windows Media Player COM dependency
- integrated newly added program text into the existing language-resource system
- English as the neutral default language; German is selected automatically for a new configuration on German-language Windows systems and English remains selectable
- the public Twitch client ID is included in the application; Twitch sign-in only requires selecting **Link Twitch Account**, and no client secret is used
- Streamlabs and HypeRate credentials are stored locally only when these optional services are used
- clean first launch without unexpected import of existing data
- completely removed the former telemetry and usage-analytics components

The complete bilingual history since the last original Locxion release is available in the [changelog](CHANGELOG.md). Details about local storage and external connections are available under [Privacy](PRIVACY.md).

### System requirements

- Windows 10 or Windows 11
- 64-bit system (`x64`)
- network access to the Nanoleaf devices in use
- Twitch account

Windows 7, Windows 8/8.1, 32-bit Windows, Linux, and ARM are not supported.

### Installation and first start

1. Download the current Windows x64 ZIP from [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest).
2. Extract the complete ZIP into its own folder.
3. Start `NanoTwitchLeafs.exe`.
4. Open **Twitch**, select **Link Twitch Account**, and confirm the sign-in with Twitch.
5. Open **Nano** and pair or verify your Nanoleaf devices.
6. Then configure the desired triggers under **Nano → Edit Triggers**.

Twitch sign-in does not require users to register their own Twitch application or manually enter a client ID or client secret. Streamlabs or HypeRate credentials are only needed when the respective optional service is used. NanoTwitchLeafs starts in English; German is selected automatically on a German-language Windows installation. The language can be changed later under **Settings**.

> **Important:** Never upload tokens or personal settings files to a public repository, bug report, or screenshot.

### Updating within NanoTwitchLeafs 4

1. Close NanoTwitchLeafs completely.
2. Download the new Windows x64 ZIP from [Releases](https://github.com/GordenM82/NanoTwitchLeafs_NTL_4/releases/latest).
3. Extract it into a new folder and start `NanoTwitchLeafs.exe` there.
4. Verify the connection, devices, and triggers. You can remove the old program folder after a successful test.

User data is stored separately under `%APPDATA%\NanoTwitchLeafs-4` and is reused automatically by a newer 4.x version. Backing up this folder before important updates is still recommended.

### Updating from NanoTwitchLeafs 3.x

On first start, NanoTwitchLeafs 4 detects an existing installation of the last original 3.x release and offers to import it:

1. Close NanoTwitchLeafs 3.x completely.
2. Extract NanoTwitchLeafs 4 into a new, separate folder.
3. Start `NanoTwitchLeafs.exe`.
4. Confirm the offered import.
5. Settings are copied into the new NTL 4 data folder.
6. Triggers are read from the old SQLite database and written to NTL 4's local `triggers.json`.
7. Afterwards, sign in once through **Twitch → Link Twitch Account**.

Before the import, NTL 4 creates a backup under `%APPDATA%\NanoTwitchLeafs-4\Migration-Backup-3.x`. Data from the old installation is only read and remains unchanged. The import path stays available in later 4.x versions so that a direct upgrade from 3.x remains possible.

If the import is declined, NTL 4 starts with an empty configuration. The application asks again on the next launch while no NTL 4 data has been created.

### Local data and privacy

NanoTwitchLeafs stores settings, triggers, device groups, and other local data under `%APPDATA%\NanoTwitchLeafs-4`. Without detected or imported 3.x data, the application starts with an empty configuration. No client secret is bundled and no telemetry data is sent.

### Bug reports and suggestions

Bug reports and suggestions may be submitted through this repository's GitHub Issues. Do not attach credentials, OAuth tokens, client secrets, or complete settings files.

Because this project is experimental and it is not yet certain whether it will be maintained permanently, submitting a suggestion does not imply any commitment to implementation, updates, or support.

### Development transparency

Parts of the continued development, troubleshooting, and documentation were created or revised with the assistance of artificial intelligence. All changes are reviewed and tested in practice by the maintainer. The original project and its core architecture were created by Locxion.

### Origin and license

NanoTwitchLeafs was originally developed by **Locxion (Markus Bender)**. The original project and its history are available at [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs).

NanoTwitchLeafs 4 is a modified and modernized edition created under the GitHub account **GordenM82**. It remains licensed under the [GNU General Public License Version 3](LICENSE), in accordance with the original project's GPL-3.0 license.

Special thanks from the original project go to Daniel Hottmeyer (`@Silverdark`) and Denis Freund (`@revyn112`).
