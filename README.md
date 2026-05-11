# DICTIONARY-MERGER-EXPORTER
Dieser Daten-Schmelztiegel wurde genau für einen Zweck gebaut: Das Zusammenführen von beliebig vielen, teils gigantischen Wörterbüchern zu einer einzigen, makellosen Master-Datei – komplett ohne lästige Duplikate

=========================================
ANWENDER-HANDBUCH: DICTIONARY MERGER & EXPORTER
=========================================

Willkommen beim Dictionary Merger & Exporter!
Dieses Programm ist ein hochspezialisierter "Daten-Schmelztiegel". Es wurde genau für einen Zweck gebaut: Das Zusammenführen von beliebig vielen, teils gigantischen Wörterbüchern zu einer einzigen, makellosen Master-Datei – komplett ohne lästige Duplikate und ohne dass dein PC dabei einfriert.

1. GRUNDLAGEN: WAS MACHT DIE APP EIGENTLICH?
Stell dir vor, du sammelst aus dem ganzen Internet Wortlisten. Manche haben 2 Millionen Wörter, manche nur 50.000. Viele alltägliche Wörter (wie "Haus" oder "Auto") kommen in fast jeder dieser Listen vor. Wenn du diese Textdateien einfach nur aneinanderhängst, hast du am Ende ein riesiges Chaos voller doppelter Einträge. Das verschwendet nicht nur Speicherplatz, sondern zwingt jede nachgelagerte Software (wie z. B. Untertitel-Erkennungsprogramme) beim Durchsuchen in die Knie.

Genau hier greift diese App ein. Sie liest alle deine Dateien ein, zerlegt sie blitzschnell in ihre Einzelteile und schickt jedes einzelne Wort an einem gnadenlosen, digitalen "Türsteher" vorbei (in der Programmierung "HashSet" genannt). Dieser Türsteher hat ein fotografisches Gedächtnis. Nur Wörter, die er in der aktuellen Sitzung noch absolut nie zuvor gesehen hat, dürfen eintreten. 
Zusätzlich besitzt der Türsteher eine Sicherheitsbarriere: Er blockt knallhart alle Einträge ab, die ein Leerzeichen enthalten (z.B. "New York" oder versehentliche Sätze in der Datei). Das Ergebnis: Ein reines, hochkonzentriertes Konvolut aus 100% einzigartigen Einzelwörtern.

2. DAS DASHBOARD & DIE STEUERUNG
Die Benutzeroberfläche ist in zwei logische Bereiche aufgeteilt, die sich dynamisch mitbewegen, wenn du das Fenster auf deinem Monitor vergrößerst oder maximierst.

Die Historie (Linke Seite):
Hier siehst du dein internes Logbuch für die aktuelle Sitzung. Jede Datei, die du der App zum Fraß vorwirfst, wird hier mit Datum, Dateiname und Dateigröße protokolliert. Die wichtigste Spalte hierbei ist "Neu". Sie zeigt dir NICHT an, wie viele Wörter insgesamt in der jeweiligen Datei standen. Sie zeigt dir exakt die Zahl der Wörter an, die der Türsteher als echten "Neu-Fund" erkannt und in den Schmelztiegel gelassen hat. Steht hier eine "0", weißt du sofort: Diese Datei bestand zu 100% aus Duplikaten, die du ohnehin schon kanntest.

Die Arbeitsfläche & der Monitor (Rechte Seite):
- "Dateien hinzufügen": Über diesen Button öffnet sich ein Fenster, in dem du klassisch nach Dateien auf deinem Rechner suchen kannst. Du kannst hier auch direkt mehrere Dateien markieren.
- Drag & Drop (Der Profi-Weg): Du musst den Button gar nicht nutzen. Das gesamte Programmfenster ist eine Abwurf-Zone. Markiere einfach beliebig viele .txt, .json, .jsonl, .csv oder .dat-Dateien in deinem Windows-Explorer und ziehe sie mit gedrückter Maustaste direkt in die App. Das Programm reiht sie automatisch auf und arbeitet sie fehlerfrei nacheinander ab.
- "Konvolut Exportieren": Sobald du alle deine gesammelten Listen eingeworfen und vom Türsteher filtern lassen hast, brennt dieser Button dein fertiges Meisterwerk auf die Festplatte. Bevor die Datei gespeichert wird, sortiert die App das gesamte Konvolut vollautomatisch alphabetisch (A-Z).
- Export-Format (Radio-Buttons): Hier entscheidest du vor dem Klick auf Exportieren, in welchem Gewand dein Konvolut landen soll. "Line by Line" schreibt jedes Wort stur in eine neue Zeile (.txt). "JSON" verpackt die Wörter in ein maschinenlesbares, kompaktes Daten-Array (.json).

3. HINTERGRUND-LOGIKEN & PERFORMANCE (DER MOTORRAUM)
Eines der größten Probleme bei der Verarbeitung von Millionen von Daten ist das "Einfrieren" des Programms. Wenn ein normales Programm eine 30 Megabyte große Textdatei einliest, blockiert es. Du kannst das Fenster nicht mehr verschieben, es wird blass und Windows meldet "Keine Rückmeldung". 

Um das zu verhindern, arbeitet diese App asynchron. Das bedeutet: Wenn du Dateien per Drag & Drop in das Fenster ziehst, delegiert die App die Schwerstarbeit an einen unsichtbaren Arbeiter im Hintergrund ("Background Worker"). Deine Benutzeroberfläche bleibt dadurch immer flüssig und ansprechbar. Du kannst das Fenster skalieren oder scrollen, während tief im Motorraum der App Millionen von Wörtern verarbeitet werden. 

Zusätzlich verfügt der große Text-Monitor auf der rechten Seite über eine native Zoom-Funktion. Da das Lesen von rohen Datenlisten für die Augen extrem anstrengend sein kann, kannst du jederzeit die "Strg"-Taste auf deiner Tastatur gedrückt halten und das Mausrad drehen, um die Schriftgröße stufenlos zu vergrößern oder zu verkleinern.

4. DIE SUCHFUNKTION (DER SCHNELLE SCANNER)
Oben rechts im Dashboard findest du ein Suchfeld. Hier kannst du live in deinem aktuell geladenen Konvolut stöbern. 
Wichtig zu wissen: Die Suche ist bewusst als "Enthält"-Suche (Contains) konzipiert und verzichtet auf experimentelle Fehlerverzeihung (Fuzzy-Suche). Wenn du beispielsweise nach "aus" suchst, findet die App sofort "Haus", "Maus", "Ausflug" und "Rauswurf". Eine fehlertolerante Suche würde bei über 2 Millionen Einträgen den Prozessor deines Computers regelrecht zum Schmelzen bringen. Um die Anzeige nicht zu überlasten, spuckt der Monitor maximal die ersten 500 Treffer zu deinem Suchbegriff aus.

5. FAQ (HÄUFIG GESTELLTE FRAGEN)

Frage: Ich habe eine 28 MB große Textdatei in die App gezogen, aber in der Spalte "Neu" steht eine sehr kleine Zahl (z. B. 260.000). Ist die App kaputt?
Antwort: Nein, die App hat sensationell gut funktioniert! Eine 28 MB große Textdatei enthält in Wahrheit fast 2 Millionen Wörter. Die niedrige Zahl in der Spalte "Neu" bedeutet lediglich, dass der interne Türsteher der App ca. 1,7 Millionen Wörter aus dieser Datei als Duplikate identifiziert und aussortiert hat, weil sie bereits durch eine vorherige Datei in den Schmelztiegel gelangt sind. Die Zahl bei "Neu" ist dein reiner Netto-Datengewinn.

Frage: Wenn ich auf die Vorschau der "neu hinzugefügten Wörter" schaue, fehlen dort Wörter, die ganz am Anfang der Textdatei standen (z. B. "Aachen"). Wo sind die hin?
Antwort: Auch hier hat der Duplikat-Filter zugeschlagen. Wenn "Aachen" bereits durch eine frühere Datei (z. B. eine große Master-JSON) in das System geladen wurde, wird es beim Einlesen der zweiten Datei ignoriert und taucht daher nicht in der Liste der "Neuzugänge" auf.

Frage: Kann ich Bilder oder Fotos in das große Textfeld ziehen, um Notizen visuell anzureichern?
Antwort: Nein, das ist strikt blockiert. Die App ist als puristische, saubere Text-Maschine konzipiert. Das Textfeld dient ausschließlich als Status-Monitor. Würde das Programm formatierte Bilder zulassen, wäre das exportierte Wörterbuch mit unsichtbarem Formatierungs-Code und Bilddaten "verseucht". Das würde unweigerlich zu Abstürzen in den Programmen führen, die deine Liste später einlesen sollen.

Frage: Was passiert, wenn ich das Programmfenster schließe, während es minimiert ist? Verschwindet es beim nächsten Start im unsichtbaren Bereich?
Antwort: Nein. Die App verfügt über ein intelligentes Fenster-Gedächtnis (Anti-Off-Screen-Bugfix). Sie speichert beim Schließen stets die echten Koordinaten. Beim nächsten Start prüft ein Sensor gnadenlos ab, ob diese Koordinaten überhaupt auf den aktuell angeschlossenen Monitoren sichtbar sind. Ist das nicht der Fall (z. B. weil ein zweiter Monitor abgestöpselt wurde), zentriert sich das Fenster als Fallback automatisch sicher in der Mitte deines Hauptbildschirms.

6. CLI (COMMAND LINE INTERFACE) & AUTOMATISIERUNG
Du kannst diese App nicht nur per Hand bedienen, sondern sie auch von anderen Programmen (wie AutoHotkey) oder über die Windows-Eingabeaufforderung (CMD) fernsteuern. Das nennt man "Headless-Modus". Das bedeutet: Die App öffnet sich komplett unsichtbar im Hintergrund, checkt in Millisekunden, ob ein Wort existiert, und schließt sich sofort wieder. Das schont deinen Arbeitsspeicher massiv.

So rufst du die App über die Befehlszeile auf:
Öffne die Windows-Kommandozeile (CMD) im Ordner der App und gib den Namen der exe-Datei ein, gefolgt von dem Wort, das du prüfen willst (am besten in Anführungszeichen):
DictionaryMerger.exe "Köln"

Die App antwortet dem System dann mit einem versteckten Code (ExitCode):
- Code 1: Das Wort ist der Master-Datenbank bereits bekannt.
- Code 2: Das Wort ist neu! Es wurde unsichtbar in eine Warteschlange ("cli_unknown_words.txt") geschrieben. 

Wenn du Code 2 erhältst, kannst du später die App ganz normal mit sichtbarem Fenster starten und oben auf "1. CLI-Wörter sichten" klicken, um dir alle gesammelten Neu-Funde anzusehen und sie mit "2. CLI übernehmen" in deinen Master-Bestand zu mergen.

BEISPIEL FÜR EINE BATCH-DATEI (test.bat):
Du kannst dir eine einfache Textdatei erstellen, sie "test.bat" nennen und folgenden Code hineinkopieren (speichere sie im selben Ordner wie die DictionaryMerger.exe). Wenn du sie doppelt anklickst, testet sie das Wort vollautomatisch:

@echo off
echo Pruefe Wort: Klabusterbeere
DictionaryMerger.exe "Klabusterbeere"
if %errorlevel% == 1 (
    echo.
    echo ERGEBNIS: Das Wort ist der Datenbank BEREITS BEKANNT!
) else if %errorlevel% == 2 (
    echo.
    echo ERGEBNIS: Das Wort war NEU und wurde in die Warteschlange verschoben!
)
pause

[System]: 2.305.979 Wörter automatisch geladen.
