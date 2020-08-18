# LibBuilder
Bauen und Regenerien von [Powerbuilder](https://www.appeon.com/products/powerbuilder)-Objects 

## Erforderliche Software
Das Programm wurde für Windows 10 - 32Bit getestet. Programmiert wurde in C# mit dem  .NET Core Framework, dieses wird benötigt um das Programm auszuführen. Falls das Framework noch nicht installiert ist, wird beim Start darauf hingewiesen. 
(Download x86 [hier](https://dotnet.microsoft.com/download/dotnet-core/current/runtime))

Zum verwenden der ORCA Funktionen benötigt man die ORCA DLL, diese wird beim Installieren des Powerbuilders automatisch mit installiert. Für jede Powerbuilder Version gibt es eine andere DLL.
Normalerweise unter “C:\Program Files (x86)\Appeon\Shared\PowerBuilder\”
Die Versionen werden vom LibBuilder unterstützt. Weitere Versionen können natürlich hinzugefügt werden.
| Name              | Orca-DLL      |
| ------------------|:-------------:|
| PowerBuilder 10.5 | PBORC105.DLL  | 
| PowerBuilder 12.5 | PBORC125.DLL  | 
| PowerBuilder 17.0 | PBORC170.DLL  |
| PowerBuilder 19.0 | PBORC190.DLL  |

## Dateien
Die zweite Version des LibBuilders erstellt keine zusätzlichen Dateien wie in der ersten Version. Im Installationsverzeichnis befinden sich lediglich 2 Dateien.
Die Datenbank befindet sich im Roaming Verzeichnis; „C:\Users\%currentUser%\AppData\Roaming\LibBuilder\libbuilder.db“

## Powerbuilder
Es kann zu Problemen mit dem Powerbuilder kommen während das Target dort geöffnet ist. 

## Mehrere Prozeduren
Es können mehrere Prozeduren auf einmal ausgeführt werden. Der LibBuilder muss lediglich ein zweites, drittes, viertes, … Mal ausgeführt werden. 
