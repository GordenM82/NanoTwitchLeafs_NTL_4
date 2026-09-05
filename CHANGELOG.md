# Änderungsprotokoll / Changelog

## Deutsch

> **English version below.**

## NanoTwitchLeafs 4.1.0 – stabile öffentliche Version (5. September 2026)

Version 4.1.0 fasst die in 30 Entwicklungsschritten umgesetzte und praktisch geprüfte Modernisierung der vollständigen Oberfläche zusammen.

### Neue Oberfläche und Navigation

- Hauptfenster mit zentraler Seitennavigation für Chat/Konsole, Twitch, Nano, Trigger, Integrationen, Einstellungen, Hilfe und Info vollständig neu aufgebaut
- Trigger-, Antworten-, Geräte-, Gerätegruppen- und Blocklist-Verwaltung in das Hauptfenster integriert
- Hell-, Dunkel- und Systemdesign sowie wählbare Akzentfarben auf alle aktiven Fenster und Dialoge angewendet
- Bedienung, Fokusführung, Escape-Rückkehr, `Strg+F`, Toastmeldungen und Hinweise bei ungespeicherten Änderungen vereinheitlicht
- Hilfewege und Entwickler-/Projektinformationen neu strukturiert und sprachlich überarbeitet

### Trigger, Twitch und Nanoleaf

- Triggerübersicht mit kombinierter Suche, Status- und Typfiltern, Trefferanzahl und Filterrücksetzung erweitert
- Warnungen für fehlende Geräte, Effekte und Sounddateien sowie Tooltips für gekürzte Werte und Spaltenüberschriften ergänzt
- Trigger können erstellt, getestet, bearbeitet, dupliziert, importiert, exportiert und gelöscht werden
- Triggerbearbeitung und JSON-Speicherung gegen ungültige Eingaben, unvollständige Daten und fehlende Dienste abgesichert
- Nanoleaf-Mehrgeräteauswahl, Gerätegruppen und Wiederherstellung des vorherigen Zustands in die neue Oberfläche übernommen
- Twitch-Ereignisse, Kanalpunkte und HypeTrain EventSub v2 bleiben vollständig unterstützt

### Integrationen und Blocklist

- StreamElements-Tips über Astro WebSocket ohne eigene Client-ID integriert
- StreamElements-Reiter mit Aktivierung, JWT-/Overlay-Token, automatischer Verbindung, Status, Hilfe und lokaler Testspende ergänzt
- bestehender Spenden-Trigger um die Quellen Alle, Streamlabs und StreamElements erweitert
- Twitch-Blocklist mit Aktivierung, Hinzufügen, Suche, Entfernen und vollständigem Leeren direkt im Twitch-Bereich ergänzt
- StreamElements-Tokens per Windows DPAPI geschützt und aus Diagnoseausgaben entfernt

### Konsole, Fenster und Stabilität

- Konsole um Suche, Stufenfilter, Farben, Trefferanzahl, Kopieren, Leeren, Logzugriff und umschaltbares Auto-Scrollen erweitert
- bereinigtes Support-Protokoll ohne Tokens, API-Schlüssel, Benutzernamen und persönlichen Windows-Profilpfad ergänzt
- Fenstergröße, Position und maximierter Zustand werden gespeichert und monitorgebunden wiederhergestellt
- Oberfläche bei 100 %, 125 % und 150 % Windows-Skalierung sowie im Mehrmonitorbetrieb geprüft
- vertikale und horizontale Scrollleisten vollständig bedienbar und designabhängig gestaltet
- beschädigte oder unvollständige Einstellungen werden normalisiert und als Sicherung erhalten
- HypeRate-Verarbeitung und Triggerwarteschlange gegen ungültige Daten und Threadfehler abgesichert
- automatisierte Regressionstests P22 bis P30 sowie Lokalisierungs- und Windows-x64-Buildprüfungen ergänzt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 30 (5. September 2026)

- Hauptfenster beim Wiederherstellen auf die Arbeitsfläche eines einzelnen Monitors begrenzt
- ungültige Positionen nach Monitorwechsel werden auf den nächstgelegenen verfügbaren Bildschirm zurückgeführt
- aktive Unterfenster werden auf dem Monitor des Haupt- beziehungsweise Besitzerfensters zentriert und begrenzt
- eigene Scrollleisten mit korrektem WPF-`PART_Track`, ziehbaren Schiebern sowie Hover- und Ziehzuständen repariert
- erkannte Arbeitsfläche und DPI werden zur Diagnose protokolliert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 29 (5. September 2026)

- Auto-Scroll-Checkbox und Beschriftung nochmals fein ausgerichtet
- Triggerverwaltung mit einem sichtbaren äußeren Seitenrahmen an die gemeinsame obere Inhaltskante der übrigen Bereiche angeglichen
- vollständige Tooltips für Spaltenüberschriften sowie gekürzte Trigger-, Befehls-, Effekt-, Sound-, Flag- und Zielgerätewerte ergänzt
- wählbare Anzeigedichte bewusst aus der weiteren Planung entfernt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 28 (5. September 2026)

- Auto-Scroll-Checkbox optisch zentriert und Triggerbereich an die einheitlichen Seitenkanten angepasst
- Tooltips für gekürzte Triggerwerte und wichtige deaktivierte Funktionen ergänzt
- Fokusmarkierung, Startfokus und Escape-Rückkehr für eingebettete Inhalte verbessert
- Einstellungsdateien werden normalisiert; beschädigte Dateien bleiben als Sicherung erhalten
- HypeRate-Nachrichten gegen fehlende oder ungültige Daten abgesichert
- Triggerwarteschlange aktualisiert die Oberfläche asynchron und wird ohne blockierten Queue-Handler geleert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 27 (5. September 2026)

- Konsolenleiste auf ein einheitlich hohes Grid umgestellt und vollständig vertikal zentriert
- sichtbaren Suchhinweis für das leere Konsolensuchfeld ergänzt
- Trigger-Spalten nach P26 wieder ausgewogener und besser lesbar gestaltet
- Zielgeräte sowie Import-/Export-Schaltflächen ohne hart codierte deutsche Texte lokalisiert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 26 (5. September 2026)

- neue Konsolenfarben, Warnhinweise, Kontextmenüs und Scrollbalken an Hell/Dunkel/System angepasst
- „Automatisch scrollen“ und Trefferanzeige in der Konsolenleiste sauber ausgerichtet
- Twitch-Seite weiter verkürzt, um den unnötigen kleinen Scrollweg zu entfernen
- Trigger-Tabelle ohne Strukturumbau kompakter gestaltet
- Titel, Version und Entwicklerangaben der Info-Seite zentriert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 25 (5. September 2026)

- Blocklist-Verwaltung im Hauptfenster hinter einer kompakten Twitch-Zusammenfassung
- kompaktere Twitch-/Nano-Seiten mit Scrollen als Reserve bei kleinen Fenstern und Skalierung
- Konsolensuche, Stufenfilter/-farben, Leeren, Logzugriff, Auto-Scroll, Kopieren und bereinigtes Support-Protokoll
- Wiederherstellung von Fenstergröße, Position und maximiertem Zustand mit Bildschirmprüfung
- Hinweise auf ungespeicherte Änderungen, Toastmeldungen sowie Escape-/Strg+F-Navigation

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 24 (5. September 2026)

- Triggerübersicht um eine kombinierte Suche über Typ, Befehl, Effekt, Sound, Anbieter und Zielgeräte erweitert
- Filter für Aktiv, Inaktiv, problematische Trigger sowie Trigger-Kategorien ergänzt
- Trefferanzahl und Zurücksetzen aller Filter ergänzt
- orange Warnhinweise für fehlende Nanoleaf-Geräte, Effekte und Sounddateien ergänzt
- Triggerbearbeitung per Doppelklick auf eine Tabellenzeile ergänzt
- Auswahl und Filter werden nach Änderungen zuverlässig neu ausgewertet
- bestehendes Triggerfenster und Triggerdatenformat unverändert beibehalten

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 23 (5. September 2026)

- StreamElements als eigenen Reiter unter Integrationen ergänzt
- API-Einstellungen bewusst als letzten Integrationsreiter beibehalten
- StreamElements-Aktivierung, Token, Verbindungsstatus und lokale Testspende aus den API-Einstellungen in den neuen Reiter verschoben
- Konsole um Leeren, direktes Öffnen der Logdatei und umschaltbares automatisches Scrollen erweitert
- lange Konsoleneinträge werden innerhalb der verfügbaren Breite umgebrochen
- automatisierte Prüfung der P23-Navigation und Konsolenfunktionen ergänzt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 22 (5. September 2026)

- Blocklist vollständig in den Twitch-Bereich des Hauptfensters integriert, ohne eigenen Seitennavigationspunkt
- Hinzufügen, Suchen, Entfernen und vollständiges Leeren der Blocklist ergänzt
- StreamElements-Tips über die Astro-WebSocket-Verbindung integriert; eine eigene Client-ID oder ein Client-Secret ist nicht erforderlich
- StreamElements-Einstellungen mit automatischer Verbindung, Statusanzeige, sicherem Tokenfeld und lokalem Testereignis ergänzt
- vorhandenen Spenden-Trigger um die Quellenwahl Alle, Streamlabs oder StreamElements erweitert; bestehende Trigger bleiben mit „Alle“ kompatibel
- eigenen Hilfereiter für StreamElements und Blocklist-Hilfe im Twitch-Hilfebereich ergänzt
- StreamElements-Zugangsdaten bleiben Bestandteil der per Windows-DPAPI verschlüsselten NTL-Einstellungen und werden aus Diagnosefehlern entfernt
- automatisierte P22-Regressionsprüfung und Preview-22-Windows-Artefakt ergänzt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 21 (4. September 2026)

- Antworten-, Geräte- und Gerätegruppenverwaltung in das Hauptfenster eingebettet
- Rückkehrwege aus eingebetteten Verwaltungsbereichen vereinheitlicht

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 20 (4. September 2026)

- Hilfe-Rückkehr auf den tatsächlich aufrufenden Bereich korrigiert
- Grammatik, Zeichensetzung, Zielgeräte- und Befehlshilfen überarbeitet

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 19 (4. September 2026)

- vollständigen Hilfebereich mit thematischen Reitern in das Hauptfenster eingebettet
- kontextbezogene Hilfeschaltflächen mit passenden Reiterzielen verbunden

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 18 (3. September 2026)

- vollständige Designprüfung aller aktiven Fenster für Hell, Dunkel und System durchgeführt
- Eingabe-, Dialog-, Listen-, Info-, Pairing- und Anmeldefenster an das gemeinsame Farbsystem angepasst

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 17 (3. September 2026)

- Info-Seite, Navigation und externe Projekt-/Feedbackwege modernisiert
- ursprünglichen Entwickler und NTL-4-Weiterentwicklung klarer getrennt dargestellt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 16 (3. September 2026)

- Geräte- und Gerätegruppenansichten an die neue Oberfläche angepasst
- automatische Vollständigkeitsprüfung der auswählbaren Sprachressourcen eingeführt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 15 (3. September 2026)

- sprachliche Qualitätsrunde für alle vorhandenen Übersetzungen durchgeführt
- Deutsch (Österreich) und französische Fallback-Ressourcen ergänzt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 14 (2. September 2026)

- neue Oberflächen- und Triggertexte in Dänisch, Spanisch, Französisch, Italienisch, Niederländisch, Polnisch, Portugiesisch, Russisch und Slowakisch ergänzt
- Übersetzungsworkflow abgesichert und sämtliche Sprachdateien vervollständigt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 13 (2. September 2026)

- Sprachumschaltung und Triggerdarstellung weiter vereinheitlicht
- Triggertexte, Beschriftungen und Designressourcen nachgebessert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 12 (2. September 2026)

- Triggerbearbeitung gegen ungültige Zahlen, fehlende Dienste und Speicherfehler abgesichert
- lokalisierte Triggerhilfe, Gerätegruppenaktualisierung und Sounddateifilter ergänzt
- Speichern ersetzt Triggerdaten atomarer, ohne vorhandene Daten vorzeitig zu leeren

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 11 (2. September 2026)

- Triggereditor und zugehörige Hilfeseite überarbeitet
- Kanalpunktstatus, Eingabefelder und Fensteraufteilung korrigiert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 10 (2. September 2026)

- Triggerverwaltung als eigener Navigationsbereich in das Hauptfenster eingebettet
- Triggeraktionen, Zielgeräteanzeige und bewusst durch Nutzer ausgelöste Aktivumschaltung stabilisiert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 9 (2. September 2026)

- JSON-Triggerdaten, Laden, Speichern und Repository-Zugriffe stabilisiert
- Triggerbearbeitung, Duplizieren und kulturunabhängige Zahleneingaben abgesichert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 8 (2. September 2026)

- Triggerverwaltung für die neue Oberfläche vergrößert und neu strukturiert
- Testen, Bearbeiten, Duplizieren und Zielgeräteanzeige verbessert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 7 (2. September 2026)

- Haupt- und Triggeroberfläche kompakter und einheitlich themenfähig gestaltet
- Triggerdetail- und Übersichtsfenster an das moderne Design angepasst

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 6 (1. September 2026)

- Streamlabs-, HypeRate- und API-Seiten für das neue Hauptfenster neu aufgebaut
- Integrationsnavigation und Verbindungsbereiche vereinheitlicht

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 5 (1. September 2026)

- Einstellungsseite vollständig neu angeordnet
- Sprache, Design, Akzentfarbe und allgemeine Optionen übersichtlicher zusammengeführt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 4 (1. September 2026)

- linke Seitennavigation und Seitenwechsel weiterentwickelt
- Größen, Abstände und aktive Navigationszustände vereinheitlicht

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 3 (1. September 2026)

- Hauptfenster, Kopfbereich, Info-Darstellung und Navigationsdetails verfeinert
- Plattformkennzeichnung und Windows-Unterstützung präzisiert

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 2 (1. September 2026)

- moderne Oberfläche funktional mit Twitch-, Nano-, Einstellungs- und Integrationslogik verbunden
- dynamisches Designsystem, Akzentfarben und erste neue Info-Seite ergänzt

## NanoTwitchLeafs 4.1.0 Layout-Vorschau 1 (1. September 2026)

- erste funktionale Vorschau der vollständig modernisierten WPF-Oberfläche erstellt
- Kopfbereich, moderne Bedienelemente und separate Testdatenhaltung eingeführt

## NanoTwitchLeafs 4.0.1 – Fehlerbehebungen (31. August 2026)

- Abstürze beim Öffnen von NTL-Discord, GitHub und Feedback aus dem Informationsfenster behoben
- externe Links werden unter .NET 10 sicher über den Standardbrowser geöffnet
- getrennte Links für Locxions ursprüngliches GitHub-Repository und das NTL-4-Repository ergänzt
- getrennte Feedback-Links für die ursprüngliche Version und NTL 4 ergänzt
- ursprünglichen Entwickler Locxion und Weiterentwicklung durch GordenM82 getrennt ausgewiesen
- Hinweis zur KI-Unterstützung und präzisierter Unabhängigkeitshinweis ergänzt
- Informationsfenster auf Deutsch und Englisch aktualisiert

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

## NanoTwitchLeafs 4.1.0 – stable public release (5 September 2026)

Version 4.1.0 delivers the complete interface modernization developed and practically tested across 30 layout previews.

### New interface and navigation

- rebuilt the main window around central navigation for Chat/Console, Twitch, Nano, Triggers, Integrations, Settings, Help, and Info
- embedded trigger, response, device, device-group, and blocklist management in the main window
- applied Light, Dark, and System themes plus selectable accent colors to all active windows and dialogs
- unified keyboard operation, focus, Escape navigation, `Ctrl+F`, toast messages, and unsaved-change warnings
- restructured contextual help and project/developer information

### Triggers, integrations, and support

- added combined trigger search, status/category filters, result counts, missing-resource warnings, and complete-value tooltips
- retained create, test, edit, duplicate, import, export, and delete actions with safer JSON persistence and input handling
- integrated StreamElements tips through Astro WebSocket with token protection, automatic connection, help, and a local test donation
- extended donation triggers with All, Streamlabs, and StreamElements provider choices
- embedded complete Twitch-user blocklist management in the Twitch section
- expanded the console with search, level filters/colors, copy, clear, log access, auto-scroll, and a sanitized support log

### Windows, scaling, and stability

- stores and restores window size, position, maximized state, and the selected monitor
- constrains main and child windows to one monitor's taskbar-aware work area
- tested the interface at 100%, 125%, and 150% Windows scaling and in multi-monitor use
- repaired draggable vertical and horizontal scrollbars with theme-aware interaction states
- normalizes incomplete settings, preserves damaged files as backups, and hardens HypeRate and trigger-queue processing
- added automated P22–P30 regression, localization, and Windows x64 build validation

## NanoTwitchLeafs 4.1.0 Layout Preview 30 (5 September 2026)

- constrained restored main windows and active child windows to a single taskbar-aware monitor work area
- restored removed-monitor positions to the nearest available screen and added monitor/DPI diagnostics
- repaired vertical and horizontal WPF scrollbars with proper `PART_Track`, drag, hover, and pressed behavior

## NanoTwitchLeafs 4.1.0 Layout Preview 29 (5 September 2026)

- fine-tuned automatic-scroll alignment and aligned the trigger page with the shared outer content edge
- added full tooltips for column headers and truncated trigger-table values

## NanoTwitchLeafs 4.1.0 Layout Preview 28 (5 September 2026)

- improved focus indication, initial focus, Escape return, disabled-control explanations, and trigger content bounds
- normalized incomplete settings and preserved invalid settings as backups
- hardened malformed HypeRate messages and trigger-queue UI-thread updates

## NanoTwitchLeafs 4.1.0 Layout Preview 27 (5 September 2026)

- aligned the console toolbar, added its visible search hint, and balanced trigger columns
- localized target-device and import/export labels

## NanoTwitchLeafs 4.1.0 Layout Preview 26 (5 September 2026)

- applied Light/Dark/System styling to console colors, warnings, context menus, and scrollbars
- compacted Twitch and trigger content and centered the information-page title and developer details

## NanoTwitchLeafs 4.1.0 Layout Preview 25 (5 September 2026)

- moved blocklist management behind a compact Twitch summary in the main window
- added complete console search/filter/support actions, window-placement persistence, dirty warnings, toasts, Escape, and `Ctrl+F`

## NanoTwitchLeafs 4.1.0 Layout Preview 24 (5 September 2026)

- added combined trigger search across type, command, effect, sound, provider, and target devices
- added filters for active, inactive, problematic triggers, and trigger categories
- added a result count and filter reset action
- added orange warnings for missing Nanoleaf devices, effects, and sound files
- added trigger editing by double-clicking a table row
- reliably reapplies selection and filters after changes
- preserved the existing trigger window and trigger data format

## NanoTwitchLeafs 4.1.0 Layout Preview 23 (5 September 2026)

- added StreamElements as its own tab within Integrations
- deliberately kept API Settings as the final integration tab
- moved StreamElements activation, token, connection state, and local test donation from API Settings into the new tab
- added clear, open-log-file, and configurable auto-scroll controls to the console
- long console entries now wrap within the available width
- added automated validation for the P23 navigation and console functionality

## NanoTwitchLeafs 4.1.0 Layout Preview 22 (5 September 2026)

- embedded the complete blocklist in the main window's Twitch section without adding a side-navigation entry
- added blocklist add, search, remove, and clear-all actions
- integrated StreamElements tips through the Astro WebSocket connection without requiring a custom client ID or client secret
- added StreamElements settings with automatic connection, status display, protected token entry, and a local test event
- extended the existing donation trigger with All, Streamlabs, and StreamElements source choices while keeping existing triggers compatible through the All default
- added dedicated StreamElements help and blocklist guidance in the Twitch help section
- kept StreamElements credentials inside the Windows-DPAPI-encrypted NTL settings and redacted them from diagnostic errors
- added automated P22 regression checks and the Preview 22 Windows artifact

## NanoTwitchLeafs 4.1.0 Layout Preview 21 (4 September 2026)

- embedded response, device, and device-group management in the main window
- unified return navigation from embedded management areas

## NanoTwitchLeafs 4.1.0 Layout Preview 20 (4 September 2026)

- corrected contextual help return targets and revised grammar, punctuation, target-device, and command guidance

## NanoTwitchLeafs 4.1.0 Layout Preview 19 (4 September 2026)

- embedded the complete tabbed help area in the main window and connected contextual help buttons

## NanoTwitchLeafs 4.1.0 Layout Preview 18 (3 September 2026)

- completed the Light, Dark, and System theme audit across active input, list, info, pairing, and sign-in windows

## NanoTwitchLeafs 4.1.0 Layout Preview 17 (3 September 2026)

- modernized Info, navigation, external project links, and separate original/continued-development credits

## NanoTwitchLeafs 4.1.0 Layout Preview 16 (3 September 2026)

- adapted device and device-group views and introduced automatic selectable-language resource validation

## NanoTwitchLeafs 4.1.0 Layout Preview 15 (3 September 2026)

- completed a language-quality pass and added Austrian German and French fallback resources

## NanoTwitchLeafs 4.1.0 Layout Preview 14 (2 September 2026)

- completed Danish, Spanish, French, Italian, Dutch, Polish, Portuguese, Russian, and Slovak interface resources
- secured the translation workflow and resource completeness

## NanoTwitchLeafs 4.1.0 Layout Preview 13 (2 September 2026)

- unified language switching, trigger presentation, labels, and theme resources

## NanoTwitchLeafs 4.1.0 Layout Preview 12 (2 September 2026)

- hardened trigger editing against invalid numbers, missing services, and save failures
- added localized trigger help, immediate device-group refresh, sound filters, and safer replacement saves

## NanoTwitchLeafs 4.1.0 Layout Preview 11 (2 September 2026)

- revised the trigger editor/help and corrected channel-point status, inputs, and window layout

## NanoTwitchLeafs 4.1.0 Layout Preview 10 (2 September 2026)

- embedded trigger management as its own navigation area and stabilized user-requested activation changes

## NanoTwitchLeafs 4.1.0 Layout Preview 9 (2 September 2026)

- stabilized JSON trigger loading, saving, repository access, duplication, and culture-independent numeric input

## NanoTwitchLeafs 4.1.0 Layout Preview 8 (2 September 2026)

- enlarged and restructured trigger management and improved test, edit, duplicate, and target-device actions

## NanoTwitchLeafs 4.1.0 Layout Preview 7 (2 September 2026)

- compacted the main and trigger interfaces and applied the shared modern theme

## NanoTwitchLeafs 4.1.0 Layout Preview 6 (1 September 2026)

- rebuilt Streamlabs, HypeRate, API, integration-navigation, and connection pages

## NanoTwitchLeafs 4.1.0 Layout Preview 5 (1 September 2026)

- rebuilt Settings around language, theme, accent, and general options

## NanoTwitchLeafs 4.1.0 Layout Preview 4 (1 September 2026)

- developed the left navigation, page switching, spacing, sizes, and active states

## NanoTwitchLeafs 4.1.0 Layout Preview 3 (1 September 2026)

- refined the main header, Info presentation, navigation details, and Windows platform declaration

## NanoTwitchLeafs 4.1.0 Layout Preview 2 (1 September 2026)

- connected the modern interface to Twitch, Nano, Settings, and integration logic
- added dynamic themes, accent colors, and the first new Info page

## NanoTwitchLeafs 4.1.0 Layout Preview 1 (1 September 2026)

- created the first functional preview of the fully modernized WPF interface
- introduced the header, modern controls, and separate preview data storage

## NanoTwitchLeafs 4.0.1 – bug fixes (31 August 2026)

- fixed crashes when opening NTL Discord, GitHub, or feedback links from the information window
- external links now open safely through the default browser on .NET 10
- added separate links to Locxion's original GitHub repository and the NTL 4 repository
- added separate feedback links for the original version and NTL 4
- distinguished original developer Locxion from NTL 4 development by GordenM82
- added the AI-assistance notice and clarified the non-affiliation statement
- updated the information window in German and English

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
