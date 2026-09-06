# Änderungsprotokoll / Changelog

## Deutsch

> **English version below.**

## NanoTwitchLeafs 4.1.2 – Kontostatus und Speicheranzeige (6. September 2026)

- Konsolenmeldungen unterscheiden jetzt zwischen einem normalen Twitch-Konto und einem ausdrücklich eingerichteten separaten Bot-Konto
- automatische, bereits gespeicherte Twitch-Token-Erneuerungen werden nicht mehr fälschlich als ungespeicherte Benutzeränderungen angezeigt
- Python-Prüfwerkzeuge bleiben für GitHub Actions erhalten, werden aber aus der angezeigten Anwendungssprachstatistik ausgeschlossen

## NanoTwitchLeafs 4.1.1 – Twitch-Verbindungsfixes (6. September 2026)

- Absturz beim automatischen Erneuern eines abgelaufenen Twitch-Tokens behoben
- Einstellungsänderungen aus Twitch-Hintergrundthreads werden sicher an die Oberfläche weitergegeben
- fehlgeschlagene oder unvollständige Twitch-Anmeldungen schließen NTL nicht mehr
- Twitch-Chat und EventSub werden getrennt initialisiert, sodass ein EventSub-Fehler den verbundenen Chat nicht beendet
- Chatseite zeigt jetzt verständliche Zustände für getrennt, wird verbunden, verbunden und fehlgeschlagen
- Chat-Eingabe wird erst nach dem bestätigten Beitritt zum Twitch-Kanal freigeschaltet
- optioneller Bot-Konto-Reiter wird nur noch beim Doppel-Konto-Setup angezeigt
- Verbindungsassistent vergrößert und Überdeckung der unteren Schaltfläche im Verbindungstest behoben
- verbundener Twitch-Kanal wird auf der Hauptseite lokalisiert angezeigt
- Twitch-Verbindungen werden auch nach fehlgeschlagenen Anmeldeversuchen vollständig bereinigt

## NanoTwitchLeafs 4.1.0 – stabile öffentliche Version (5. September 2026)

Version 4.1.0 bringt eine vollständig modernisierte Oberfläche sowie zahlreiche Erweiterungen und Stabilitätsverbesserungen.

### Oberfläche und Bedienung

- Hauptfenster mit zentraler Navigation für Chat/Konsole, Twitch, Nano, Trigger, Integrationen, Einstellungen, Hilfe und Info neu aufgebaut
- Verwaltung von Triggern, Chatantworten, Geräten, Gerätegruppen und Twitch-Blocklist in das Hauptfenster integriert
- Hell-, Dunkel- und Systemdesign sowie wählbare Akzentfarben ergänzt
- Oberfläche, Dialoge, Scrollleisten, Fokusführung und Tastaturbedienung vereinheitlicht
- kontextbezogene Hilfeseiten, Tooltips, Toastmeldungen und Warnungen bei ungespeicherten Änderungen ergänzt
- Oberfläche für Windows-Skalierungen von 100 %, 125 % und 150 % sowie Mehrmonitorbetrieb angepasst
- Fenstergröße, Position, maximierter Zustand und zuletzt verwendeter Monitor werden gespeichert und sicher wiederhergestellt
- Haupt- und Unterfenster bleiben innerhalb der Arbeitsfläche eines Monitors und berücksichtigen die Taskleiste

### Trigger, Twitch und Nanoleaf

- Triggerübersicht um kombinierte Suche, Status- und Typfilter, Trefferanzahl und Filterrücksetzung erweitert
- Warnungen für fehlende Geräte, Effekte und Sounddateien sowie vollständige Tooltips für gekürzte Tabellenwerte ergänzt
- Trigger können erstellt, getestet, bearbeitet, dupliziert, importiert, exportiert und gelöscht werden
- freie Nanoleaf-Zielgeräteauswahl und wiederverwendbare Gerätegruppen vollständig in die neue Oberfläche übernommen
- Wiederherstellung des vorherigen Nanoleaf-Zustands für jedes angesprochene Gerät beibehalten
- Twitch-Ereignisse für Follows, Abonnements, ReSubs, Geschenk-Abos, Bits, Raids und Kanalpunkte unterstützt
- HypeTrain-Abonnements auf Twitch EventSub v2 aktualisiert
- Twitch-Anmeldung verwendet die enthaltene öffentliche Desktop-Client-ID und benötigt kein Client-Secret
- Twitch-Benutzer-Blocklist mit Aktivierung, Suche, Hinzufügen, Entfernen und vollständigem Leeren ergänzt

### Integrationen

- StreamElements-Tips über Astro WebSocket integriert
- eigener StreamElements-Reiter mit Aktivierung, JWT-/Overlay-Token, automatischer Verbindung, Status und Hilfe ergänzt
- lokale StreamElements-Testspende ermöglicht Triggerprüfungen ohne Zahlung oder aktive Verbindung
- Spenden-Trigger können auf Alle, Streamlabs oder StreamElements begrenzt werden; bestehende Trigger bleiben kompatibel
- HypeRate-Verarbeitung gegen fehlende und ungültige Daten abgesichert
- Zugangsdaten werden mit Windows DPAPI geschützt und aus Diagnoseausgaben entfernt

### Konsole, Daten und Stabilität

- Konsole um Suche, Stufenfilter, farbliche Hervorhebung, Trefferanzahl und umschaltbares automatisches Scrollen erweitert
- Funktionen zum Leeren, Öffnen der Logdatei und Kopieren über das Kontextmenü ergänzt
- bereinigtes Support-Protokoll ohne Tokens, API-Schlüssel, Benutzernamen und persönlichen Windows-Profilpfad ergänzt
- Trigger werden lokal in `triggers.json` gespeichert und vor Änderungen als `triggers.json.backup` gesichert
- beschädigte oder unvollständige Einstellungen werden normalisiert und als Sicherung erhalten
- Triggerwarteschlange, Soundwiedergabe und Oberflächenaktualisierungen gegen Threadfehler abgesichert
- Telemetrie und die nicht mehr verfügbare ursprüngliche Nutzungsanalyse bleiben entfernt
- Benutzeroberfläche vollständig über das vorhandene Sprachressourcensystem lokalisiert
- Deutsch, Englisch, Dänisch, Spanisch, Französisch, Italienisch, Niederländisch, Polnisch, Portugiesisch (Brasilien), Slowakisch und Russisch auswählbar
- selbstständiges Windows-x64-Paket für Windows 10 und Windows 11 erstellt

## NanoTwitchLeafs 4.0.1 – Fehlerbehebungen (31. August 2026)

- Abstürze beim Öffnen von Discord-, GitHub- und Feedbacklinks aus dem Informationsfenster behoben
- externe Links werden unter .NET 10 sicher über den Standardbrowser geöffnet
- Links und Angaben für ursprüngliches Projekt, NTL 4, Entwickler und Feedback eindeutig getrennt
- Hinweis zur KI-Unterstützung und Unabhängigkeit von Twitch und Nanoleaf präzisiert

## NanoTwitchLeafs 4.0.0 – öffentliche Erstveröffentlichung (31. August 2026)

- WPF-Projekt von .NET Framework 4.7.2 auf .NET 10 und Windows x64 umgestellt
- direkt startbares, selbstständiges Windows-x64-Paket eingeführt
- Twitch-Anmeldung auf den offiziellen Gerätecode-Ablauf mit öffentlicher Desktop-Client-ID umgestellt
- Twitch-Client-Secret aus Datenmodell, Oberfläche und Authentifizierung entfernt
- Mehrgeräteauswahl, Nanoleaf-Gerätegruppen und gerätebezogene Zustandswiederherstellung ergänzt
- Twitch EventSub-Abonnements korrigiert und fehlende Ereignisse ergänzt
- Triggerdaten von SQLite auf gesicherte lokale JSON-Speicherung umgestellt
- bestätigungspflichtigen Import bestehender 3.x-Einstellungen und Trigger ergänzt
- veraltete Abhängigkeiten, Telemetrie und nicht mehr verfügbare Dienste entfernt
- reproduzierbare Windows-Builds über GitHub Actions eingerichtet

---

## English

## NanoTwitchLeafs 4.1.2 – account status and save indicator (6 September 2026)

- console messages now distinguish a regular Twitch account from an explicitly configured separate bot account
- automatic Twitch-token refreshes that are already persisted no longer appear as unsaved user changes
- Python validation helpers remain available to GitHub Actions but are excluded from the displayed application-language statistics

## NanoTwitchLeafs 4.1.1 – Twitch connection fixes (6 September 2026)

- fixed a crash while automatically refreshing an expired Twitch token
- safely dispatches setting changes raised by Twitch background threads to the user interface
- failed or incomplete Twitch logins no longer terminate NTL
- initializes Twitch chat and EventSub independently, so an EventSub failure does not disconnect working chat
- added clear disconnected, connecting, connected, and failed states to the Chat page
- enables chat input only after Twitch confirms that the channel was joined
- shows the optional bot-account tab only for dual-account setup
- enlarged the connection wizard and fixed the obscured bottom button in the connection test
- localized the connected-channel status on the main Twitch page
- completely cleans up Twitch clients after failed login attempts

## NanoTwitchLeafs 4.1.0 – stable public release (5 September 2026)

Version 4.1.0 delivers a fully modernized interface together with extensive feature and stability improvements.

### Interface and usability

- rebuilt the main window around central navigation for Chat/Console, Twitch, Nano, Triggers, Integrations, Settings, Help, and Info
- embedded trigger, chat-response, device, device-group, and Twitch-blocklist management in the main window
- added Light, Dark, and System themes with selectable accent colors
- unified layouts, dialogs, scrollbars, focus behavior, and keyboard operation
- added contextual help pages, tooltips, toast messages, and unsaved-change warnings
- adapted the interface for 100%, 125%, and 150% Windows scaling and multi-monitor operation
- stores and safely restores window size, position, maximized state, and the last monitor
- keeps main and child windows inside one monitor's taskbar-aware work area

### Triggers, Twitch, and Nanoleaf

- expanded trigger management with combined search, state/type filters, result counts, and filter reset
- added missing-device, effect, and sound warnings plus full-value tooltips for truncated table cells
- retained create, test, edit, duplicate, import, export, and delete actions
- carried per-trigger Nanoleaf target selection and reusable device groups into the new interface
- retained per-device restoration of the previous Nanoleaf state
- supports Twitch follows, subscriptions, resubscriptions, gift subscriptions, Bits, raids, and channel points
- updated Hype Train subscriptions to Twitch EventSub v2
- uses the bundled public Twitch desktop client ID without requiring a client secret
- added Twitch-user blocklist activation, search, add, remove, and clear actions

### Integrations

- integrated StreamElements tips through Astro WebSocket
- added a dedicated StreamElements tab with activation, JWT/overlay token, automatic connection, status, and help
- added a local StreamElements test donation for checking triggers without payment or an active connection
- donation triggers can target All, Streamlabs, or StreamElements while existing triggers remain compatible
- hardened HypeRate handling against missing and invalid data
- protects credentials with Windows DPAPI and redacts them from diagnostics

### Console, data, and stability

- expanded the console with search, level filters, colors, result counts, and configurable automatic scrolling
- added clear, open-log-file, and context-menu copy actions
- added a sanitized support log without tokens, API keys, usernames, or personal Windows profile paths
- stores triggers locally in `triggers.json` and backs them up as `triggers.json.backup` before changes
- normalizes incomplete settings and preserves damaged files as backups
- hardened the trigger queue, sound playback, and UI updates against threading errors
- keeps telemetry and the unavailable original usage analytics removed
- fully localized the interface through the existing resource system
- supports selectable German, English, Danish, Spanish, French, Italian, Dutch, Polish, Brazilian Portuguese, Slovak, and Russian
- provides a self-contained Windows x64 package for Windows 10 and Windows 11

## NanoTwitchLeafs 4.0.1 – bug fixes (31 August 2026)

- fixed crashes when opening Discord, GitHub, and feedback links from the information page
- external links now open safely through the default browser on .NET 10
- clearly separated links and attribution for the original project, NTL 4, developers, and feedback
- clarified the AI-assistance and Twitch/Nanoleaf non-affiliation notices

## NanoTwitchLeafs 4.0.0 – initial public release (31 August 2026)

- migrated the WPF project from .NET Framework 4.7.2 to .NET 10 and Windows x64
- introduced a ready-to-run self-contained Windows x64 package
- moved Twitch sign-in to the official device-code flow using a public desktop client ID
- removed the Twitch client secret from the data model, interface, and authentication
- added multi-device selection, Nanoleaf device groups, and per-device state restoration
- corrected Twitch EventSub subscriptions and added missing event types
- migrated trigger data from SQLite to backed-up local JSON storage
- added confirmation-based import of existing 3.x settings and triggers
- removed obsolete dependencies, telemetry, and unavailable services
- added reproducible Windows builds through GitHub Actions
