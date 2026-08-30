# Änderungsprotokoll / Changelog

## Deutsch

> **English version below.**

## NanoTwitchLeafs 4.0.0.2 – privater Teststand

NanoTwitchLeafs 4 basiert auf Locxions letzter ursprünglicher Version 3.1.5.0 und dem privat getesteten Zwischenstand 3.2.0.5. Version 4 ist noch keine fertige öffentliche Veröffentlichung.

### Technische Grundlage und Verteilung

- WPF-Projekt von .NET Framework 4.7.2 auf .NET 10 umgestellt
- Zielplattform auf Windows 10 und Windows 11 x64 festgelegt
- eigenständiger Windows-x64-Build einschließlich benötigter .NET-Laufzeit
- direkt startbare `NanoTwitchLeafs.exe` ohne `Build_NTL.cmd`, Visual Studio oder gesonderte .NET-Installation
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

### Datenhaltung und private Migration

- Trigger werden lokal in `triggers.json` statt dauerhaft in einer Datenbank gespeichert
- vor jeder Änderung wird `triggers.json.backup` als Sicherung erzeugt
- vorhandene Trigger werden beim privaten Test ausschließlich aus einer Kopie der 3.2.0.5-Datenbank importiert
- alte verschlüsselte Einstellungen mit einem als JSON-Objekt gespeicherten Versionswert werden unter .NET 10 korrekt gelesen
- `ServiceCredentials`, `ServiceCredentials.local` und `ServiceCredential.local` werden im privaten Test akzeptiert
- Version 4 verwendet während des Tests `%APPDATA%\NanoTwitchLeafs-4-Test`
- die stabile Installation 3.2.0.5 und ihre Originaldaten werden nicht verändert
- die spätere öffentliche Ausgabe soll vollständig leer starten und keine Daten ungefragt übernehmen

### Noch nicht enthalten

- keine Streamer.bot-Anbindung
- keine Unterstützung für Windows 7, Windows 8/8.1 oder 32-Bit-Systeme
- noch keine fertige öffentliche Credential-Ersteinrichtung

---

## English

## NanoTwitchLeafs 4.0.0.2 – private test build

NanoTwitchLeafs 4 is based on Locxion's last original version 3.1.5.0 and the privately tested intermediate build 3.2.0.5. Version 4 is not yet a finished public release.

### Technical foundation and distribution

- migrated the WPF project from .NET Framework 4.7.2 to .NET 10
- set the target platform to 64-bit Windows 10 and Windows 11
- added a self-contained Windows x64 build including the required .NET runtime
- added a directly executable `NanoTwitchLeafs.exe` without `Build_NTL.cmd`, Visual Studio, or a separate .NET installation
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

### Data storage and private migration

- triggers are stored locally in `triggers.json` instead of requiring a permanent database
- `triggers.json.backup` is created before each change
- during private testing, existing triggers are imported exclusively from a copy of the 3.2.0.5 database
- old encrypted settings containing a version stored as a JSON object are read correctly under .NET 10
- `ServiceCredentials`, `ServiceCredentials.local`, and `ServiceCredential.local` are accepted during private testing
- version 4 uses `%APPDATA%\NanoTwitchLeafs-4-Test` during testing
- the stable 3.2.0.5 installation and its original data are not modified
- the future public edition is intended to start completely clean and will not import data without explicit consent

### Not currently included

- no Streamer.bot integration
- no support for Windows 7, Windows 8/8.1, or 32-bit systems
- no finished public first-run credential setup yet

---

## Ursprünglicher Versionsverlauf / Original version history

Der unveränderte historische Versionsverlauf des ursprünglichen Projekts und der privaten Zwischenstände bleibt in [changelog.txt](changelog.txt) erhalten.

The unchanged historical version history of the original project and the private intermediate builds remains available in [changelog.txt](changelog.txt).
