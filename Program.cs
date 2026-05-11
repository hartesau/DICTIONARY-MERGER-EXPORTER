using System;
#nullable disable
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictionaryMerger
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string autoSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "autoload_master.txt");
            string cliUnknownPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cli_unknown_words.txt");

            if (args.Length > 0)
            {
                string query = args[0].Trim();
                bool found = false;

                if (File.Exists(autoSavePath))
                {
                    found = File.ReadLines(autoSavePath).Any(line => line == query);
                }

                if (found)
                {
                    Environment.Exit(1);
                }
                else
                {
                    File.AppendAllText(cliUnknownPath, query + Environment.NewLine);
                    Environment.Exit(2);
                }
                return;
            }

            Application.Run(new Form1());
        }
    }

    public class Form1 : Form
    {
        private SplitContainer splitContainer;
        private ListView listViewHistory;
        private RichTextBox rtbOutput;
        private Panel topPanel;
        private Button btnLoadFiles, btnExport, btnClear, btnSearch;
        private Button btnCliReview, btnCliApprove, btnCliDiscard;
        private TextBox txtSearch;
        private RadioButton rbExportLine, rbExportJson;
        private Label lblTotalCount;
        
        private HashSet<string> masterDictionary = new HashSet<string>();
        private string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "window_config.json");
        private string autoSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "autoload_master.txt");
        private string cliUnknownPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cli_unknown_words.txt");
        private bool isDirty = false;

        private string manualText = @"=========================================
ANWENDER-HANDBUCH: DICTIONARY MERGER & EXPORTER
=========================================

Willkommen beim Dictionary Merger & Exporter!
Dieses Programm ist ein hochspezialisierter ""Daten-Schmelztiegel"". Es wurde genau für einen Zweck gebaut: Das Zusammenführen von beliebig vielen, teils gigantischen Wörterbüchern zu einer einzigen, makellosen Master-Datei – komplett ohne lästige Duplikate und ohne dass dein PC dabei einfriert.

1. GRUNDLAGEN: WAS MACHT DIE APP EIGENTLICH?
Stell dir vor, du sammelst aus dem ganzen Internet Wortlisten. Manche haben 2 Millionen Wörter, manche nur 50.000. Viele alltägliche Wörter (wie ""Haus"" oder ""Auto"") kommen in fast jeder dieser Listen vor. Wenn du diese Textdateien einfach nur aneinanderhängst, hast du am Ende ein riesiges Chaos voller doppelter Einträge. Das verschwendet nicht nur Speicherplatz, sondern zwingt jede nachgelagerte Software (wie z. B. Untertitel-Erkennungsprogramme) beim Durchsuchen in die Knie.

Genau hier greift diese App ein. Sie liest alle deine Dateien ein, zerlegt sie blitzschnell in ihre Einzelteile und schickt jedes einzelne Wort an einem gnadenlosen, digitalen ""Türsteher"" vorbei (in der Programmierung ""HashSet"" genannt). Dieser Türsteher hat ein fotografisches Gedächtnis. Nur Wörter, die er in der aktuellen Sitzung noch absolut nie zuvor gesehen hat, dürfen eintreten. 
Zusätzlich besitzt der Türsteher eine Sicherheitsbarriere: Er blockt knallhart alle Einträge ab, die ein Leerzeichen enthalten (z.B. ""New York"" oder versehentliche Sätze in der Datei). Das Ergebnis: Ein reines, hochkonzentriertes Konvolut aus 100% einzigartigen Einzelwörtern.

2. DAS DASHBOARD & DIE STEUERUNG
Die Benutzeroberfläche ist in zwei logische Bereiche aufgeteilt, die sich dynamisch mitbewegen, wenn du das Fenster auf deinem Monitor vergrößerst oder maximierst.

Die Historie (Linke Seite):
Hier siehst du dein internes Logbuch für die aktuelle Sitzung. Jede Datei, die du der App zum Fraß vorwirfst, wird hier mit Datum, Dateiname und Dateigröße protokolliert. Die wichtigste Spalte hierbei ist ""Neu"". Sie zeigt dir NICHT an, wie viele Wörter insgesamt in der jeweiligen Datei standen. Sie zeigt dir exakt die Zahl der Wörter an, die der Türsteher als echten ""Neu-Fund"" erkannt und in den Schmelztiegel gelassen hat. Steht hier eine ""0"", weißt du sofort: Diese Datei bestand zu 100% aus Duplikaten, die du ohnehin schon kanntest.

Die Arbeitsfläche & der Monitor (Rechte Seite):
- ""Dateien hinzufügen"": Über diesen Button öffnet sich ein Fenster, in dem du klassisch nach Dateien auf deinem Rechner suchen kannst. Du kannst hier auch direkt mehrere Dateien markieren.
- Drag & Drop (Der Profi-Weg): Du musst den Button gar nicht nutzen. Das gesamte Programmfenster ist eine Abwurf-Zone. Markiere einfach beliebig viele .txt, .json, .jsonl, .csv oder .dat-Dateien in deinem Windows-Explorer und ziehe sie mit gedrückter Maustaste direkt in die App. Das Programm reiht sie automatisch auf und arbeitet sie fehlerfrei nacheinander ab.
- ""Konvolut Exportieren"": Sobald du alle deine gesammelten Listen eingeworfen und vom Türsteher filtern lassen hast, brennt dieser Button dein fertiges Meisterwerk auf die Festplatte. Bevor die Datei gespeichert wird, sortiert die App das gesamte Konvolut vollautomatisch alphabetisch (A-Z).
- Export-Format (Radio-Buttons): Hier entscheidest du vor dem Klick auf Exportieren, in welchem Gewand dein Konvolut landen soll. ""Line by Line"" schreibt jedes Wort stur in eine neue Zeile (.txt). ""JSON"" verpackt die Wörter in ein maschinenlesbares, kompaktes Daten-Array (.json).

3. HINTERGRUND-LOGIKEN & PERFORMANCE (DER MOTORRAUM)
Eines der größten Probleme bei der Verarbeitung von Millionen von Daten ist das ""Einfrieren"" des Programms. Wenn ein normales Programm eine 30 Megabyte große Textdatei einliest, blockiert es. Du kannst das Fenster nicht mehr verschieben, es wird blass und Windows meldet ""Keine Rückmeldung"". 

Um das zu verhindern, arbeitet diese App asynchron. Das bedeutet: Wenn du Dateien per Drag & Drop in das Fenster ziehst, delegiert die App die Schwerstarbeit an einen unsichtbaren Arbeiter im Hintergrund (""Background Worker""). Deine Benutzeroberfläche bleibt dadurch immer flüssig und ansprechbar. Du kannst das Fenster skalieren oder scrollen, während tief im Motorraum der App Millionen von Wörtern verarbeitet werden. 

Zusätzlich verfügt der große Text-Monitor auf der rechten Seite über eine native Zoom-Funktion. Da das Lesen von rohen Datenlisten für die Augen extrem anstrengend sein kann, kannst du jederzeit die ""Strg""-Taste auf deiner Tastatur gedrückt halten und das Mausrad drehen, um die Schriftgröße stufenlos zu vergrößern oder zu verkleinern.

4. DIE SUCHFUNKTION (DER SCHNELLE SCANNER)
Oben rechts im Dashboard findest du ein Suchfeld. Hier kannst du live in deinem aktuell geladenen Konvolut stöbern. 
Wichtig zu wissen: Die Suche ist bewusst als ""Enthält""-Suche (Contains) konzipiert und verzichtet auf experimentelle Fehlerverzeihung (Fuzzy-Suche). Wenn du beispielsweise nach ""aus"" suchst, findet die App sofort ""Haus"", ""Maus"", ""Ausflug"" und ""Rauswurf"". Eine fehlertolerante Suche würde bei über 2 Millionen Einträgen den Prozessor deines Computers regelrecht zum Schmelzen bringen. Um die Anzeige nicht zu überlasten, spuckt der Monitor maximal die ersten 500 Treffer zu deinem Suchbegriff aus.

5. FAQ (HÄUFIG GESTELLTE FRAGEN)

Frage: Ich habe eine 28 MB große Textdatei in die App gezogen, aber in der Spalte ""Neu"" steht eine sehr kleine Zahl (z. B. 260.000). Ist die App kaputt?
Antwort: Nein, die App hat sensationell gut funktioniert! Eine 28 MB große Textdatei enthält in Wahrheit fast 2 Millionen Wörter. Die niedrige Zahl in der Spalte ""Neu"" bedeutet lediglich, dass der interne Türsteher der App ca. 1,7 Millionen Wörter aus dieser Datei als Duplikate identifiziert und aussortiert hat, weil sie bereits durch eine vorherige Datei in den Schmelztiegel gelangt sind. Die Zahl bei ""Neu"" ist dein reiner Netto-Datengewinn.

Frage: Wenn ich auf die Vorschau der ""neu hinzugefügten Wörter"" schaue, fehlen dort Wörter, die ganz am Anfang der Textdatei standen (z. B. ""Aachen""). Wo sind die hin?
Antwort: Auch hier hat der Duplikat-Filter zugeschlagen. Wenn ""Aachen"" bereits durch eine frühere Datei (z. B. eine große Master-JSON) in das System geladen wurde, wird es beim Einlesen der zweiten Datei ignoriert und taucht daher nicht in der Liste der ""Neuzugänge"" auf.

Frage: Kann ich Bilder oder Fotos in das große Textfeld ziehen, um Notizen visuell anzureichern?
Antwort: Nein, das ist strikt blockiert. Die App ist als puristische, saubere Text-Maschine konzipiert. Das Textfeld dient ausschließlich als Status-Monitor. Würde das Programm formatierte Bilder zulassen, wäre das exportierte Wörterbuch mit unsichtbarem Formatierungs-Code und Bilddaten ""verseucht"". Das würde unweigerlich zu Abstürzen in den Programmen führen, die deine Liste später einlesen sollen.

Frage: Was passiert, wenn ich das Programmfenster schließe, während es minimiert ist? Verschwindet es beim nächsten Start im unsichtbaren Bereich?
Antwort: Nein. Die App verfügt über ein intelligentes Fenster-Gedächtnis (Anti-Off-Screen-Bugfix). Sie speichert beim Schließen stets die echten Koordinaten. Beim nächsten Start prüft ein Sensor gnadenlos ab, ob diese Koordinaten überhaupt auf den aktuell angeschlossenen Monitoren sichtbar sind. Ist das nicht der Fall (z. B. weil ein zweiter Monitor abgestöpselt wurde), zentriert sich das Fenster als Fallback automatisch sicher in der Mitte deines Hauptbildschirms.

6. CLI (COMMAND LINE INTERFACE) & AUTOMATISIERUNG
Du kannst diese App nicht nur per Hand bedienen, sondern sie auch von anderen Programmen (wie AutoHotkey) oder über die Windows-Eingabeaufforderung (CMD) fernsteuern. Das nennt man ""Headless-Modus"". Das bedeutet: Die App öffnet sich komplett unsichtbar im Hintergrund, checkt in Millisekunden, ob ein Wort existiert, und schließt sich sofort wieder. Das schont deinen Arbeitsspeicher massiv.

So rufst du die App über die Befehlszeile auf:
Öffne die Windows-Kommandozeile (CMD) im Ordner der App und gib den Namen der exe-Datei ein, gefolgt von dem Wort, das du prüfen willst (am besten in Anführungszeichen):
DictionaryMerger.exe ""Köln""

Die App antwortet dem System dann mit einem versteckten Code (ExitCode):
- Code 1: Das Wort ist der Master-Datenbank bereits bekannt.
- Code 2: Das Wort ist neu! Es wurde unsichtbar in eine Warteschlange (""cli_unknown_words.txt"") geschrieben. 

Wenn du Code 2 erhältst, kannst du später die App ganz normal mit sichtbarem Fenster starten und oben auf ""1. CLI-Wörter sichten"" klicken, um dir alle gesammelten Neu-Funde anzusehen und sie mit ""2. CLI übernehmen"" in deinen Master-Bestand zu mergen.

BEISPIEL FÜR EINE BATCH-DATEI (test.bat):
Du kannst dir eine einfache Textdatei erstellen, sie ""test.bat"" nennen und folgenden Code hineinkopieren (speichere sie im selben Ordner wie die DictionaryMerger.exe). Wenn du sie doppelt anklickst, testet sie das Wort vollautomatisch:

@echo off
echo Pruefe Wort: Klabusterbeere
DictionaryMerger.exe ""Klabusterbeere""
if %errorlevel% == 1 (
    echo.
    echo ERGEBNIS: Das Wort ist der Datenbank BEREITS BEKANNT!
) else if %errorlevel% == 2 (
    echo.
    echo ERGEBNIS: Das Wort war NEU und wurde in die Warteschlange verschoben!
)
pause";

        public Form1()
        {
            InitializeComponent();
            LoadWindowConfig();
            this.Load += Form1_Load;
        }

        private void UpdateDataCount()
        {
            lblTotalCount.Text = $"Datenbestand: {masterDictionary.Count:N0} Wörter";
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(autoSavePath))
            {
                rtbOutput.Text = "Lade gespeichertes Master-Wörterbuch... Bitte warten.\n";
                await Task.Run(() =>
                {
                    var lines = File.ReadAllLines(autoSavePath);
                    foreach (var l in lines) masterDictionary.Add(l);
                });
                UpdateDataCount();
                rtbOutput.Text = manualText + $"\n\n[System]: {masterDictionary.Count:N0} Wörter automatisch geladen.";
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Dictionary Merger & Exporter (NDJSON & CLI Edition)";
            this.Size = new Size(1000, 750);
            this.AllowDrop = true;

            splitContainer = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 350 };
            
            listViewHistory = new ListView { Dock = DockStyle.Fill, View = View.Details, GridLines = true, FullRowSelect = true, AllowDrop = true };
            listViewHistory.Columns.Add("Datum", 120);
            listViewHistory.Columns.Add("Datei", 100);
            listViewHistory.Columns.Add("Größe", 70);
            listViewHistory.Columns.Add("Neu", 60);
            splitContainer.Panel1.Controls.Add(listViewHistory);

            topPanel = new Panel { Dock = DockStyle.Top, Height = 170 };
            
            lblTotalCount = new Label { 
                Text = "Datenbestand: 0 Wörter", 
                Location = new Point(10, 10), 
                AutoSize = true, 
                Font = new Font("Segoe UI", 16f, FontStyle.Bold), 
                ForeColor = Color.Blue 
            };

            btnLoadFiles = new Button { Text = "Dateien hinzufügen", Location = new Point(10, 50), Width = 140 };
            btnLoadFiles.Click += async (s, e) => {
                using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Text/JSON Files|*.txt;*.json;*.jsonl;*.csv;*.dat", Multiselect = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK) await ProcessFilesAsync(ofd.FileNames);
                }
            };

            btnClear = new Button { Text = "Alles leeren", Location = new Point(160, 50), Width = 100 };
            btnClear.Click += (s, e) => {
                if(MessageBox.Show("Datenbank wirklich komplett löschen?", "Achtung", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    masterDictionary.Clear();
                    listViewHistory.Items.Clear();
                    UpdateDataCount();
                    isDirty = true;
                    if(File.Exists(autoSavePath)) File.Delete(autoSavePath);
                    rtbOutput.Text = "Datenbank wurde restlos gelöscht.";
                }
            };

            txtSearch = new TextBox { Location = new Point(430, 52), Width = 150 };
            btnSearch = new Button { Text = "Suchen", Location = new Point(590, 50), Width = 80 };
            btnSearch.Click += async (s, e) => await SearchAsync();

            rbExportLine = new RadioButton { Text = "Line by Line (.txt)", Location = new Point(10, 90), Checked = true, Width = 120 };
            rbExportJson = new RadioButton { Text = "JSON Array (.json)", Location = new Point(140, 90), Width = 150 };

            btnExport = new Button { Text = "Konvolut Exportieren", Location = new Point(310, 85), Width = 140 };
            btnExport.Click += async (s, e) => await ExportAsync();

            btnCliReview = new Button { Text = "1. CLI-Wörter sichten", Location = new Point(10, 130), Width = 140 };
            btnCliReview.Click += BtnCliReview_Click;
            btnCliApprove = new Button { Text = "2. CLI übernehmen", Location = new Point(160, 130), Width = 140 };
            btnCliApprove.Click += BtnCliApprove_Click;
            btnCliDiscard = new Button { Text = "3. CLI löschen", Location = new Point(310, 130), Width = 100 };
            btnCliDiscard.Click += BtnCliDiscard_Click;

            topPanel.Controls.AddRange(new Control[] { lblTotalCount, btnLoadFiles, btnClear, rbExportLine, rbExportJson, btnExport, txtSearch, btnSearch, btnCliReview, btnCliApprove, btnCliDiscard });

            rtbOutput = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, WordWrap = true, Font = new Font("Consolas", 10f), AllowDrop = true };
            rtbOutput.Text = manualText;

            splitContainer.Panel2.Controls.Add(rtbOutput);
            splitContainer.Panel2.Controls.Add(topPanel);
            this.Controls.Add(splitContainer);

            this.DragEnter += Form_DragEnter;
            this.DragDrop += Form_DragDrop;
            listViewHistory.DragEnter += Form_DragEnter;
            listViewHistory.DragDrop += Form_DragDrop;
            rtbOutput.DragEnter += Form_DragEnter;
            rtbOutput.DragDrop += Form_DragDrop;
        }

        private void LoadWindowConfig()
        {
            if (File.Exists(configPath))
            {
                try
                {
                    var config = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(configPath));
                    int x = config.GetProperty("X").GetInt32();
                    int y = config.GetProperty("Y").GetInt32();
                    int w = config.GetProperty("Width").GetInt32();
                    int h = config.GetProperty("Height").GetInt32();
                    Rectangle rect = new Rectangle(x, y, w, h);

                    if (w > 0 && Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(rect)))
                    {
                        this.StartPosition = FormStartPosition.Manual;
                        this.Bounds = rect;
                        return;
                    }
                }
                catch { }
            }
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Rectangle bounds = this.WindowState == FormWindowState.Minimized ? this.RestoreBounds : this.Bounds;
            var config = new { X = bounds.X, Y = bounds.Y, Width = bounds.Width, Height = bounds.Height };
            File.WriteAllText(configPath, JsonSerializer.Serialize(config));

            if (isDirty)
            {
                // AutoSave ebenfalls alphabetisch sortieren!
                var sortedData = masterDictionary.OrderBy(w => w, StringComparer.OrdinalIgnoreCase).ToList();
                File.WriteAllLines(autoSavePath, sortedData);
            }
        }

        private void BtnCliReview_Click(object sender, EventArgs e)
        {
            if (File.Exists(cliUnknownPath))
                rtbOutput.Text = "Folgende Wörter wurden über CLI gesendet und waren unbekannt:\n\n" + File.ReadAllText(cliUnknownPath);
            else
                rtbOutput.Text = "Keine neuen unbekannten CLI-Wörter gefunden.";
        }

        private async void BtnCliApprove_Click(object sender, EventArgs e)
        {
            if (!File.Exists(cliUnknownPath)) return;
            var words = File.ReadAllLines(cliUnknownPath);
            int added = 0;
            
            await Task.Run(() => {
                foreach (var w in words) {
                    string c = w.Trim();
                    if (!string.IsNullOrWhiteSpace(c) && !c.Contains(" ") && masterDictionary.Add(c)) added++;
                }
            });

            isDirty = true;
            UpdateDataCount();
            File.Delete(cliUnknownPath);
            rtbOutput.Text = $"{added} unbekannte Wörter aus der CLI wurden in die Master-Datenbank gemerged!";
        }

        private void BtnCliDiscard_Click(object sender, EventArgs e)
        {
            if (File.Exists(cliUnknownPath)) File.Delete(cliUnknownPath);
            rtbOutput.Text = "Alle gemerkten CLI-Wörter wurden restlos gelöscht.";
        }

        private void Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        private async void Form_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0)
            {
                var validFiles = files.Where(f => f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".json", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".jsonl", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".dat", StringComparison.OrdinalIgnoreCase)).ToArray();
                if (validFiles.Length > 0) await ProcessFilesAsync(validFiles);
            }
        }

        private async Task ProcessFilesAsync(string[] filePaths)
        {
            btnLoadFiles.Enabled = btnExport.Enabled = false;
            List<string> lastBatchNewWords = new List<string>();
            int totalNewInBatch = 0;

            foreach (string file in filePaths)
            {
                rtbOutput.Text = $"Verarbeite {Path.GetFileName(file)}... Bitte warten.\n";
                int addedCount = 0;
                long fileSize = new FileInfo(file).Length / 1024;

                await Task.Run(() =>
                {
                    if (file.EndsWith(".json", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".jsonl", StringComparison.OrdinalIgnoreCase))
                    {
                        string firstLine = File.ReadLines(file).FirstOrDefault(l => !string.IsNullOrWhiteSpace(l))?.Trim();
                        if (firstLine != null && firstLine.StartsWith("["))
                        {
                            var incomingWords = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(file));
                            foreach (var w in incomingWords)
                            {
                                string clean = w.Trim();
                                if (!string.IsNullOrWhiteSpace(clean) && !clean.Contains(" ") && masterDictionary.Add(clean))
                                {
                                    if (lastBatchNewWords.Count < 500) lastBatchNewWords.Add(clean);
                                    addedCount++;
                                }
                            }
                        }
                        else
                        {
                            foreach (var line in File.ReadLines(file))
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                try
                                {
                                    using (JsonDocument doc = JsonDocument.Parse(line))
                                    {
                                        if (doc.RootElement.TryGetProperty("", out JsonElement wordElement))
                                        {
                                            string clean = wordElement.GetString()?.Trim();
                                            if (!string.IsNullOrWhiteSpace(clean) && !clean.Contains(" ") && masterDictionary.Add(clean))
                                            {
                                                if (lastBatchNewWords.Count < 500) lastBatchNewWords.Add(clean);
                                                addedCount++;
                                            }
                                        }

                                        if (doc.RootElement.TryGetProperty("f", out JsonElement formsElement) && formsElement.ValueKind == JsonValueKind.Array)
                                        {
                                            foreach (var fw in formsElement.EnumerateArray())
                                            {
                                                string cleanForm = fw.GetString()?.Trim();
                                                if (!string.IsNullOrWhiteSpace(cleanForm) && !cleanForm.Contains(" ") && masterDictionary.Add(cleanForm))
                                                {
                                                    if (lastBatchNewWords.Count < 500) lastBatchNewWords.Add(cleanForm);
                                                    addedCount++;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        var incomingWords = File.ReadAllLines(file).ToList();
                        foreach (var w in incomingWords)
                        {
                            string clean = w.Trim();
                            if (!string.IsNullOrWhiteSpace(clean) && !clean.Contains(" ") && masterDictionary.Add(clean))
                            {
                                if (lastBatchNewWords.Count < 500) lastBatchNewWords.Add(clean);
                                addedCount++;
                            }
                        }
                    }
                });

                totalNewInBatch += addedCount;
                if (addedCount > 0) isDirty = true;
                string sizeStr = fileSize > 1024 ? $"{fileSize / 1024} MB" : $"{fileSize} KB";
                listViewHistory.Items.Add(new ListViewItem(new[] { DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Path.GetFileName(file), sizeStr, addedCount.ToString("N0") }));
            }

            UpdateDataCount();
            rtbOutput.Text = $"Verarbeitung abgeschlossen! {totalNewInBatch:N0} neue, saubere Wörter hinzugefügt.\nAktuelle Gesamtgröße: {masterDictionary.Count:N0} Wörter.\n\nHier sind bis zu 500 Neuzugänge:\n";
            rtbOutput.AppendText(string.Join(Environment.NewLine, lastBatchNewWords));
            btnLoadFiles.Enabled = btnExport.Enabled = true;
        }

        private async Task SearchAsync()
        {
            if (masterDictionary.Count == 0) return;
            string query = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(query)) return;

            btnSearch.Enabled = false;
            rtbOutput.Text = "Suche läuft...\n";
            List<string> results = new List<string>();

            await Task.Run(() => { results = masterDictionary.Where(w => w.Contains(query)).Take(500).ToList(); });

            rtbOutput.Text = $"Suche beendet. Zeige bis zu 500 Treffer für '{query}':\n\n";
            rtbOutput.AppendText(string.Join(Environment.NewLine, results));
            btnSearch.Enabled = true;
        }

        private async Task ExportAsync()
        {
            if (masterDictionary.Count == 0) return;
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = rbExportLine.Checked ? "Text File|*.txt" : "JSON File|*.json" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    btnExport.Enabled = false;
                    rtbOutput.Text = "Exportiere Konvolut und sortiere alphabetisch... Bitte warten.\n";
                    await Task.Run(() => {
                        // Der Datensatz wird hier vor dem Export sauber alphabetisch sortiert!
                        var sortedData = masterDictionary.OrderBy(w => w, StringComparer.OrdinalIgnoreCase).ToList();
                        
                        if (rbExportLine.Checked) File.WriteAllLines(sfd.FileName, sortedData);
                        else File.WriteAllText(sfd.FileName, JsonSerializer.Serialize(sortedData));
                    });
                    rtbOutput.Text = $"Export erfolgreich abgeschlossen!\nGespeichert unter: {sfd.FileName}";
                    btnExport.Enabled = true;
                }
            }
        }
    }
}