# LibBuilder
Building and Regenerating from [Powerbuilder](https://www.appeon.com/products/powerbuilder)-Objects 
![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/workaround.gif "workaround")

## Required software
The program was tested and developed for Windows 10 - 32Bit. Programming was done in C # with WPF. The [.NET Framework](https://dotnet.microsoft.com/download/dotnet-framework)(>= v.4.72) or the [.NET Core Framework](https://dotnet.microsoft.com/download/dotnet-core/current/runtime)(>= v.3.1) is required depending on the installation to run the program. If the framework has not yet been installed, this is indicated at the start.

To use the ORCA functions you need the ORCA DLL, this is automatically installed when you install the Powerbuilder. There is a different DLL for each Powerbuilder version.
Usually under “C: \ Program Files (x86) \ Appeon \ Shared \ PowerBuilder \”
The versions are supported by the LibBuilder. Further versions can of course be added.
| Name              | Orca-DLL      |
| ------------------|---------------|
| PowerBuilder 10.5 | PBORC105.DLL  | 
| PowerBuilder 12.5 | PBORC125.DLL  | 
| PowerBuilder 17.0 | PBORC170.DLL  |
| PowerBuilder 19.0 | PBORC190.DLL  |

## Files
The second version of the LibBuilder does not create any additional files, everything is stored in a database. There are only 2 files in the installation directory;
The database is located in the roaming directory; *„C:\Users\\%currentUser%\\AppData\Roaming\LibBuilder\libbuilder.db“*


## Powerbuilder
Problems with the Powerbuilder can occur while the target is open there.

## Ausführung
| Success              | Error      |
| ------------------|:-------------:|
| ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/run%20without%20errors.png "process success") | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/run%20with%20errors.png "process with error")  | 

## Mehrere Prozeduren
Multiple procedures can be run at once. All you have to do is select a different target and start the process. The process of the target is added as a further tab.
![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/multiple%20processes.gif "multiple processes")

## Command line
It can be started from the command line. The order of the parameter specification as well as the upper and lower case is irrelevant.

| Parameter (kurz) | Parameter (lang)   | Erklärung                           | Typ                     | Eingabe                 |
|------------------|--------------------|-------------------------------------|-------------------------|-------------------------|
| -w               | -Workspace         | Name oder Pfad des Workspace        | Zeichenkette            |                         | 
| -t               | -Target            | Name oder Pfad des Targets          | Zeichenkette            |                         | 
| -v               | -Version           | Powerbuilder Version des Workspace  | Enumerischer Wert       | PB105 = 105,  PB125 = 125,  PB170 = 170,  PB190 = 190  | 
| -b               | -Build             | Build der Librarys                  | Boolescher Wert	        | true oder false         | 
| -r               | -Regenerate        | Regenerate der Library Objects      | Boolescher Wert	        | true oder false         | 
| -l               | -Librarys          | Auswahl der Librarys                | Liste von Zeichenketten |                         | 
| -x               | -RebuildType       | Typ des Rebuild                     | Enumerischer Wert       | PBORCA_FULL_REBUILD = 1,  PBORCA_INCREMENTAL_REBUILD = 2,  PBORCA_MIGRATE = 3,  PBORCA_3PASS = 4         | 



**Beispiele:** 

Letzte gespeicherte Prozedur erneut ausführen;  
libbuilder.exe -w kunden.pbw

Full Build des Targets „tlfi_lokal.pbt“;  
libbuilder.exe -w kunden.pbw -t tlfi_lokal.pbt -x 1

Regenerieren aller Objekte der Librarys „client_elinv.pbl“ und „client_tlfiutils.pbl“ des Target „tlfi_lokal.pbt“: 
libbuilder.exe -w kunden.pbw -t tlfi_lokal.pbt -r true -l client_elinv.pbl;client_tlfiutils.pbl

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
