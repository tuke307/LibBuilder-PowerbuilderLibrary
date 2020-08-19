# LibBuilder
Bauen und Regenerien von [Powerbuilder](https://www.appeon.com/products/powerbuilder)-Objects 
![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/workaround.gif "workaround")

## Erforderliche Software
Das Programm wurde für Windows 10 - 32Bit getestet und entwickelt. Programmiert wurde in C# mit dem  .NET Core Framework, dieses wird benötigt um das Programm auszuführen. Falls das Framework noch nicht installiert ist, wird beim Start darauf hingewiesen.  
(Download x86 [hier](https://dotnet.microsoft.com/download/dotnet-core/current/runtime))

Zum verwenden der ORCA Funktionen benötigt man die ORCA DLL, diese wird beim Installieren des Powerbuilders automatisch mit installiert. Für jede Powerbuilder Version gibt es eine andere DLL.  
Normalerweise unter *“C:\Program Files (x86)\Appeon\Shared\PowerBuilder\”*  
Die Versionen werden vom LibBuilder unterstützt. Weitere Versionen können natürlich hinzugefügt werden.
| Name              | Orca-DLL      |
| ------------------|---------------|
| PowerBuilder 10.5 | PBORC105.DLL  | 
| PowerBuilder 12.5 | PBORC125.DLL  | 
| PowerBuilder 17.0 | PBORC170.DLL  |
| PowerBuilder 19.0 | PBORC190.DLL  |

## Dateien
Die zweite Version des LibBuilders erstellt keine zusätzlichen Dateien, alles wird in einer Datenbank gespeichert. Im Installationsverzeichnis befinden sich lediglich 2 Dateien; 
Die Datenbank befindet sich im Roaming Verzeichnis; *„C:\Users\\%currentUser%\\AppData\Roaming\LibBuilder\libbuilder.db“*


## Powerbuilder
Es kann zu Problemen mit dem Powerbuilder kommen während das Target dort geöffnet ist. 

## Ausführung
| Erfolgreich              | Fehler      |
| ------------------|:-------------:|
| ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/run%20without%20errors.png "process success") | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/run%20with%20errors.png "process with error")  | 

## Mehrere Prozeduren
Es können mehrere Prozeduren auf einmal ausgeführt werden. Der LibBuilder muss lediglich ein zweites, drittes, viertes, … Mal ausgeführt werden. 
![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/multiple%20processes.gif "multiple processes")

## Kommandozeile
Der Start über die Kommandozeile ist möglich. Die Reihenfolge der Parameterangabe sowie die Groß-und Kleinschreibung ist egal.

| Parameter (kurz) | Parameter (lang)   | Erklärung                           | Typ                     | Eingabe                 |
|------------------|--------------------|-------------------------------------|-------------------------|-------------------------|
| -w               | -Workspace         | Name oder Pfad des Workspace        | Zeichenkette            |                         | 
| -t               | -Target            | Name oder Pfad des Targets          | Zeichenkette            |                         | 
| -v               | -Version           | Powerbuilder Version des Workspace  | Nummer                  | 105, 125, 170 oder 190  | 
| -b               | -Build             | Build der Librarys                  | Boolescher Wert	        | true oder false         | 
| -r               | -Regenerate        | Regenerate der Library Objects      | Boolescher Wert	        | true oder false         | 
| -l               | -Librarys          | Auswahl der Librarys                | Liste von Zeichenketten |                         | 
| -a               | -Application       | Ausführung über Fenster-Applikation | Boolescher Wert         | true oder false         | 

**Beispiele:** 

Letzte gespeicherte Prozedur erneut ausführen;  
libbuilder.exe -w kunden.pbw

Full Build des Targets „tlfi_lokal.pbt“;  
libbuilder.exe -w kunden.pbw -t tlfi_lokal.pbt -b true -r true

Regenerieren aller Objekte der Librarys „client_elinv.pbl“ und „client_tlfiutils.pbl“ des Target „tlfi_lokal.pbt“: 
libbuilder.exe -w kunden.pbw -t tlfi_lokal.pbt -b true -l client_elinv.pbl;client_tlfiutils.pbl

Erstmaliges Hinzufügen eines Workspace und ausführen eines Full Build eines Targets;  
libbuilder.exe -w C:\db\Workspaces\kunden.pbw -t C:\tl_kunden\TL_13_Suedguss\fakt3_v13.pbt -v 170 –r true –b true

Anzeige der Version;  
libbuilder.exe --version

Anzeige der Hilfe;  
libbuilder.exe --help

![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/cmd%20example.gif "cmd-example")

## Features
| Applikations Farbe und Theme              | Prozess-History      | AutoUpdater      |
| ------------------|:-------------:|:-------------:|
| ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/colors.png "colors") | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/history.png "history")  | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/update.png "update")  | 
