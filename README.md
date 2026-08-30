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

Es gibt keine Zusage für regelmäßige Aktualisierungen, Support oder eine dauerhafte Weiterentwicklung. Derzeit befindet sich Version 4 noch in einem privaten Funktionstest und ist noch keine fertige öffentliche Veröffentlichung.

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

Eine Anbindung an **Streamer.bot ist derzeit nicht enthalten** und kann möglicherweise später optional ergänzt werden.

### Neuerungen in Version 4

- Umstellung von .NET Framework 4.7.2 auf .NET 10
- eigenständiger Windows-x64-Build einschließlich benötigter .NET-Laufzeit
- direkt startbare `NanoTwitchLeafs.exe` ohne Build-Skript und ohne Visual Studio
- Unterstützung von Windows 10 und Windows 11 auf 64-Bit-Systemen
- Triggerverwaltung ohne dauerhaft benötigte Datenbank
- lokale Speicherung der Trigger in `triggers.json`
- automatische Sicherung als `triggers.json.backup`
- Mehrfachauswahl und Gruppen für Nanoleaf-Zielgeräte
- aktualisierte Twitch-EventSub-Abonnements einschließlich HypeTrain v2
- vergrößerte und frei skalierbare Triggerfenster
- modernisierte Soundwiedergabe ohne Abhängigkeit vom Windows Media Player COM-Modul

Der vollständige Verlauf seit der letzten ursprünglichen Locxion-Version steht in der Datei [changelog.txt](changelog.txt).

### Systemanforderungen

- Windows 10 oder Windows 11
- 64-Bit-System (`x64`)
- Netzwerkzugriff auf die verwendeten Nanoleaf-Geräte
- Twitch-Konto und die für Twitch benötigten Zugangsdaten

Windows 7, Windows 8/8.1 und 32-Bit-Systeme werden von NanoTwitchLeafs 4 nicht unterstützt.

### Installation und Start

1. Das veröffentlichte Windows-x64-ZIP vollständig in einen eigenen Ordner entpacken.
2. Benötigte Service-Credentials neben `NanoTwitchLeafs.exe` ablegen.
3. `NanoTwitchLeafs.exe` direkt starten.

Ein Aufruf von `Build_NTL.cmd`, eine Visual-Studio-Installation oder eine gesonderte .NET-Installation sind für das fertige Paket nicht erforderlich.

> **Wichtig:** Zugangsdaten, Tokens, `ServiceCredentials`, `ServiceCredentials.local` und persönliche Einstellungsdateien dürfen niemals in ein öffentliches Repository, einen Fehlerbericht oder einen Screenshot hochgeladen werden.

Die spätere öffentliche Ausgabe soll beim ersten Start vollständig leer beginnen. Eine Übernahme vorhandener Daten aus NanoTwitchLeafs 3.x erfolgt dann nicht ungefragt.

### Lokale Daten

Während der privaten Testphase verwendet NanoTwitchLeafs 4 einen getrennten Ordner:

```text
%APPDATA%\NanoTwitchLeafs-4-Test
```

Die stabile Installation von Version 3.2.0.5 und ihre Daten bleiben dadurch unverändert. Trigger von NTL 4 befinden sich in `triggers.json`; die vorherige Fassung wird als `triggers.json.backup` gesichert.

### Fehlerberichte und Vorschläge

Fehlerberichte und Vorschläge können über die GitHub-Issues dieses Repositories eingereicht werden. Bitte keine Zugangsdaten, OAuth-Tokens, Client-Secrets oder vollständigen Einstellungsdateien anhängen.

Aus einem eingereichten Vorschlag entsteht kein Anspruch auf Umsetzung oder Support.

### Ursprung und Lizenz

NanoTwitchLeafs wurde ursprünglich von **Locxion (Markus Bender)** entwickelt. Das ursprüngliche Projekt und seine Geschichte befinden sich im Repository [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs).

NanoTwitchLeafs 4 ist eine veränderte und modernisierte Fassung, die unter dem GitHub-Konto **GordenM82** erstellt wird. Sie bleibt gemäß den Bedingungen der [GNU General Public License Version 3](LICENSE) unter GPL-3.0 lizenziert.

Besonderer Dank aus dem ursprünglichen Projekt gilt Daniel Hottmeyer (`@Silverdark`) und Denis Freund (`@revyn112`).

---

## English

NanoTwitchLeafs connects Twitch with Nanoleaf lights. Chat commands and Twitch events can trigger colors, brightness levels, and effects on one or multiple Nanoleaf devices.

### Project status

NanoTwitchLeafs 4 is an experimental modernization and continuation of the original GPL-3.0 project created by **Locxion**. The current work is performed under the GitHub account **GordenM82**.

There is no commitment to regular updates, support, or long-term maintenance. Version 4 is currently undergoing private functional testing and is not yet a finished public release.

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

**Streamer.bot integration is not currently included** and may possibly be added as an optional feature later.

### What's new in version 4

- migrated from .NET Framework 4.7.2 to .NET 10
- self-contained Windows x64 build including the required .NET runtime
- directly executable `NanoTwitchLeafs.exe` without a build script or Visual Studio
- support for 64-bit Windows 10 and Windows 11
- trigger management without a permanently required database
- local trigger storage in `triggers.json`
- automatic backup to `triggers.json.backup`
- multi-device selection and Nanoleaf device groups
- updated Twitch EventSub subscriptions, including Hype Train v2
- larger and freely resizable trigger windows
- modernized sound playback without the Windows Media Player COM dependency

The complete history since the last original Locxion release is available in [changelog.txt](changelog.txt).

### System requirements

- Windows 10 or Windows 11
- 64-bit (`x64`) system
- network access to the Nanoleaf devices being used
- a Twitch account and the credentials required for Twitch

Windows 7, Windows 8/8.1, and 32-bit systems are not supported by NanoTwitchLeafs 4.

### Installation and startup

1. Extract the complete published Windows x64 ZIP into its own folder.
2. Place the required service credentials next to `NanoTwitchLeafs.exe`.
3. Start `NanoTwitchLeafs.exe` directly.

The finished package does not require `Build_NTL.cmd`, Visual Studio, or a separate .NET installation.

> **Important:** Never upload credentials, tokens, `ServiceCredentials`, `ServiceCredentials.local`, or personal settings files to a public repository, bug report, or screenshot.

The future public edition is intended to start with a completely clean configuration. Existing NanoTwitchLeafs 3.x data will not be imported without an explicit user action.

### Local data

During private testing, NanoTwitchLeafs 4 uses a separate directory:

```text
%APPDATA%\NanoTwitchLeafs-4-Test
```

The stable 3.2.0.5 installation and its data remain unchanged. NTL 4 triggers are stored in `triggers.json`; the previous state is preserved as `triggers.json.backup`.

### Bug reports and suggestions

Bug reports and suggestions may be submitted through this repository's GitHub Issues. Do not attach credentials, OAuth tokens, client secrets, or complete settings files.

Submitting a suggestion does not imply a commitment to implementation or support.

### Origin and license

NanoTwitchLeafs was originally developed by **Locxion (Markus Bender)**. The original project and its history are available at [Locxion/NanoTwitchLeafs](https://github.com/Locxion/NanoTwitchLeafs).

NanoTwitchLeafs 4 is a modified and modernized edition created under the GitHub account **GordenM82**. It remains licensed under the [GNU General Public License Version 3](LICENSE), in accordance with the original project's GPL-3.0 license.

Special thanks from the original project go to Daniel Hottmeyer (`@Silverdark`) and Denis Freund (`@revyn112`).
