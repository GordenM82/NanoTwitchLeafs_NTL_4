# NanoTwitchLeafs 4

![NanoTwitchLeafs logo](https://user-images.githubusercontent.com/16882079/214092102-4447d44f-807b-4bbb-a85c-2d295643ee6b.png)

![Windows 10/11](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D4)
![Platform x64](https://img.shields.io/badge/Platform-x64-6f42c1)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)
![License GPL-3.0](https://img.shields.io/badge/Lizenz-GPL--3.0-green)
![Status experimental](https://img.shields.io/badge/Status-experimentell-orange)

## Deutsch

> **English version below.**

NanoTwitchLeafs verbindet Twitch mit Nanoleaf-Leuchten. Chatbefehle und Twitch-Ereignisse können dadurch Farben, Helligkeiten und Effekte auf einem oder mehreren Nanoleaf-Geräten auslösen.

### Projektstatus

NanoTwitchLeafs 4 ist eine experimentelle Modernisierung und Weiterentwicklung des ursprünglichen GPL-3.0-Projekts von **Locxion**. Der aktuelle Stand wird unter dem GitHub-Konto **GordenM82** bearbeitet.

Es gibt keine Zusage für regelmäßige Aktualisierungen, Support oder eine dauerhafte Weiterentwicklung. Version 4 ist experimentelle Software und kann sich vor einer stabilen Veröffentlichung noch ändern.

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
- deutsche Benutzeroberfläche als Standard auf deutschsprachigen Windows-Systemen; Englisch bleibt auswählbar
- Twitch-Anmeldung über den offiziellen Gerätecode-Ablauf für öffentliche Desktop-Anwendungen; kein Client-Secret erforderlich
- sauberer Erststart ohne mitgelieferte Zugangsdaten oder ungefragte Übernahme vorhandener Daten
- lokale Speicherung persönlicher Daten ohne Telemetrie oder Nutzungsanalyse

Der vollständige zweisprachige Verlauf seit der letzten ursprünglichen Locxion-Version steht im [Änderungsprotokoll](CHANGELOG.md). Hinweise zur lokalen Speicherung und zu externen Verbindungen stehen unter [Datenschutz](PRIVACY.md).

### Systemanforderungen

- Windows 10 oder Windows 11
- 64-Bit-System (`x64`)
- Netzwerkzugriff auf die verwendeten Nanoleaf-Geräte
- Twitch-Konto und die für Twitch benötigten Zugangsdaten

Windows 7, Windows 8/8.1 und 32-Bit-Systeme werden von NanoTwitchLeafs 4 nicht unterstützt.

### Installation und Start

1. Das Windows-x64-ZIP vollständig in einen eigenen Ordner entpacken.
2. `NanoTwitchLeafs.exe` starten.
3. Unter **API Einstellungen** die Twitch Client-ID eintragen und speichern. Ein Client-Secret wird nicht benötigt.
4. Anschließend im Reiter **Twitch** das Twitch-Konto verbinden. Streamlabs- und HypeRate-Zugangsdaten sind nur bei Nutzung dieser Funktionen erforderlich.

Beim ersten Start verwendet NanoTwitchLeafs auf einem deutschsprachigen Windows automatisch Deutsch. Die Sprache kann unter **Einstellungen** zwischen Deutsch und Englisch umgestellt werden.

### Update von NanoTwitchLeafs 3.x

NanoTwitchLeafs 4 kann die vorhandenen Daten aus Locxions letzter 3.x-Version beim ersten Start übernehmen:

1. Die bisherige NanoTwitchLeafs-Version vollständig schließen.
2. Das NTL-4-ZIP in einen **neuen, eigenen Ordner** entpacken. Die alte Installation nicht überschreiben oder löschen.
3. `NanoTwitchLeafs.exe` starten.
4. Wenn vorhandene 3.x-Daten erkannt werden, die Frage nach der Datenübernahme mit **Ja** bestätigen.
5. NTL 4 übernimmt die bisherigen Einstellungen und konvertiert die Trigger aus `nanotwitchleafs.sqlite` in das neue Format `triggers.json`.

Vor der Übernahme wird unter `%APPDATA%\NanoTwitchLeafs-4\Migration-Backup-3.x` eine zusätzliche Sicherung angelegt. Die ursprünglichen Daten unter `%APPDATA%\NanoTwitchLeafs` werden nur gelesen und nicht verändert. Locxions bisherige Version kann deshalb weiterhin unabhängig verwendet werden.

Wird die Übernahme mit **Nein** abgelehnt, startet NTL 4 mit einer leeren Konfiguration und fragt beim nächsten Start erneut, solange noch keine eigenen NTL-4-Daten angelegt wurden.

Eine Twitch-Anwendung kann in der [Twitch Developer Console](https://dev.twitch.tv/console/apps) registriert werden. Als Client-Typ **Öffentlich** auswählen. Falls Twitch beim Anlegen eine Weiterleitungs-URL verlangt, kann `https://localhost` eingetragen werden; NanoTwitchLeafs verwendet für die Anmeldung den Gerätecode-Ablauf und benötigt diese URL nicht. Twitch verlangt für den Zugriff auf die Developer Console ein bestätigtes Konto mit aktivierter Zwei-Faktor-Authentifizierung. Die ausführliche offizielle Anleitung steht unter [Registering Your App](https://dev.twitch.tv/docs/authentication/register-app/).

> **Wichtig:** Zugangsdaten, Tokens, `ServiceCredentials` und persönliche Einstellungsdateien dürfen niemals in ein öffentliches Repository, einen Fehlerbericht oder einen Screenshot hochgeladen werden.

Die Anwendung beginnt beim ersten Start mit einer leeren Konfiguration. Persönliche Einstellungen und Trigger werden nur lokal unter `%APPDATA%\NanoTwitchLeafs-4` gespeichert. NanoTwitchLeafs 4 enthält keine Telemetrie oder Nutzungsanalyse.

### Fehlerberichte und Vorschläge

Fehlerberichte und Vorschläge können über die GitHub-Issues dieses Repositories eingereicht werden. Bitte keine Zugangsdaten, OAuth-Tokens, Client-Secrets oder vollständigen Einstellungsdateien anhängen.

Da sich das Projekt in einem experimentellen Stadium befindet und derzeit nicht feststeht, ob es dauerhaft weitergeführt wird, entsteht aus einem eingereichten Vorschlag kein Anspruch auf Umsetzung, Aktualisierungen oder Support.

### Ursprung und Lizenz

NanoTwitchLeafs wurde ursprünglich von **Locxion (Markus Bender)** entwickelt. Das ursprüngliche Projekt und seine Geschichte befinden sich im Repository [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs).

NanoTwitchLeafs 4 ist eine veränderte und modernisierte Fassung, die unter dem GitHub-Konto **GordenM82** erstellt wird. Sie bleibt gemäß den Bedingungen der [GNU General Public License Version 3](LICENSE) unter GPL-3.0 lizenziert.

Besonderer Dank aus dem ursprünglichen Projekt gilt Daniel Hottmeyer (`@Silverdark`) und Denis Freund (`@revyn112`).

---

## English

NanoTwitchLeafs connects Twitch with Nanoleaf lights. Chat commands and Twitch events can trigger colors, brightness levels, and effects on one or multiple Nanoleaf devices.

### Project status

NanoTwitchLeafs 4 is an experimental modernization and continuation of the original GPL-3.0 project created by **Locxion**. The current work is performed under the GitHub account **GordenM82**.

There is no commitment to regular updates, support, or long-term maintenance. Version 4 is experimental software and may change before a stable release.

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
- German interface by default on German-language Windows systems; English remains selectable
- Twitch authentication through the official device-code flow for public desktop applications; no Client Secret required
- clean first launch without bundled credentials or unexpected import of existing data
- local storage of personal data without telemetry or usage analytics

The complete bilingual history since the last original Locxion release is available in the [changelog](CHANGELOG.md). Details about local storage and external connections are available under [Privacy](PRIVACY.md).

### System requirements

- Windows 10 or Windows 11
- 64-bit (`x64`) system
- network access to the Nanoleaf devices being used
- a Twitch account and the credentials required for Twitch

Windows 7, Windows 8/8.1, and 32-bit systems are not supported by NanoTwitchLeafs 4.

### Installation and startup

1. Extract the complete Windows x64 ZIP into its own folder.
2. Start `NanoTwitchLeafs.exe`.
3. Under **API Settings**, enter and save the Twitch Client ID. A Client Secret is not required.
4. Then connect the Twitch account from the **Twitch** tab. Streamlabs and HypeRate credentials are only required when those features are used.

On a German-language Windows installation, NanoTwitchLeafs starts in German by default. The language can be switched between German and English under **Settings**.

### Updating from NanoTwitchLeafs 3.x

NanoTwitchLeafs 4 can import existing data from Locxion's latest 3.x version on first launch:

1. Close the previous NanoTwitchLeafs version completely.
2. Extract the NTL 4 ZIP into a **new, separate folder**. Do not overwrite or delete the old installation.
3. Start `NanoTwitchLeafs.exe`.
4. When existing 3.x data is detected, confirm the import by selecting **Yes**.
5. NTL 4 imports the existing settings and converts the triggers from `nanotwitchleafs.sqlite` to the new `triggers.json` format.

Before importing, an additional backup is created under `%APPDATA%\NanoTwitchLeafs-4\Migration-Backup-3.x`. The original data under `%APPDATA%\NanoTwitchLeafs` is read only and is not modified. Locxion's previous version can therefore continue to be used independently.

If the import is declined with **No**, NTL 4 starts with a clean configuration and asks again on the next launch as long as no NTL 4 data has been created.

A Twitch application can be registered in the [Twitch Developer Console](https://dev.twitch.tv/console/apps). Select the **Public** client type. If Twitch requires a redirect URL while creating the application, `https://localhost` may be entered; NanoTwitchLeafs uses device-code authentication and does not use this URL. Twitch requires a verified account with two-factor authentication enabled to access the Developer Console. Detailed official instructions are available under [Registering Your App](https://dev.twitch.tv/docs/authentication/register-app/).

> **Important:** Never upload credentials, tokens, `ServiceCredentials`, or personal settings files to a public repository, bug report, or screenshot.

The application starts with a clean configuration on first launch. Personal settings and triggers are stored locally under `%APPDATA%\NanoTwitchLeafs-4`. NanoTwitchLeafs 4 contains no telemetry or usage analytics.

### Bug reports and suggestions

Bug reports and suggestions may be submitted through this repository's GitHub Issues. Do not attach credentials, OAuth tokens, client secrets, or complete settings files.

Because this project is experimental and it is not yet certain whether it will be maintained permanently, submitting a suggestion does not imply any commitment to implementation, updates, or support.

### Origin and license

NanoTwitchLeafs was originally developed by **Locxion (Markus Bender)**. The original project and its history are available at [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs).

NanoTwitchLeafs 4 is a modified and modernized edition created under the GitHub account **GordenM82**. It remains licensed under the [GNU General Public License Version 3](LICENSE), in accordance with the original project's GPL-3.0 license.

Special thanks from the original project go to Daniel Hottmeyer (`@Silverdark`) and Denis Freund (`@revyn112`).
