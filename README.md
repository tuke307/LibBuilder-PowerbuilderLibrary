[German](README.de-DE.md)

[English](README.en-EN.md)

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

## Runtime
| Success              | Error      |
| ------------------|:-------------:|
| ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/run%20without%20errors.png "process success") | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/run%20with%20errors.png "process with error")  | 

## Multiple procedures
Multiple procedures can be run at once. All you have to do is select a different target and start the process. The process of the target is added as a further tab.
![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/multiple%20processes.gif "multiple processes")

## Command line
It can be started from the command line. The order of the parameter specification as well as the upper and lower case is irrelevant.

| parameter (short) | parameter (long)   | description                        | type                     | input                   |
|------------------|--------------------|-------------------------------------|--------------------------|-------------------------|
| -w               | -Workspace         | name or path of workspace           | string                   |                         | 
| -t               | -Target            | name or path of target              | string                   |                         | 
| -v               | -Version           | Powerbuilder Version des Workspace  | enum                     | PB105 = 105,  PB125 = 125,  PB170 = 170,  PB190 = 190  | 
| -b               | -Build             | build of Librarys                   | boolean 	               | true oder false         | 
| -r               | -Regenerate        | regenerate of library-objects       | boolean	                 | true oder false         | 
| -l               | -Librarys          | choices of librarys                 | list of strings          |                         | 
| -x               | -RebuildType       | type of rebuild                     | enum                     | PBORCA_FULL_REBUILD = 1,  PBORCA_INCREMENTAL_REBUILD = 2,  PBORCA_MIGRATE = 3,  PBORCA_3PASS = 4         | 



**Samples:** 

execute last saved procedure;  
libbuilder.exe -w kunden.pbw

Full Build of Target „tlfi_lokal.pbt“;  
libbuilder.exe -w kunden.pbw -t tlfi_lokal.pbt -x 1

regenerate all objects of library „client_elinv.pbl“ and „client_tlfiutils.pbl“ of target „tlfi_lokal.pbt“: 
libbuilder.exe -w kunden.pbw -t tlfi_lokal.pbt -r true -l client_elinv.pbl;client_tlfiutils.pbl

First time working of a workspace and a complete goal;  
libbuilder.exe -w C:\db\Workspaces\kunden.pbw -t C:\tl_kunden\TL_13_Suedguss\fakt3_v13.pbt -v 170 –r true –b true

show version;  
libbuilder.exe --version

show help;  
libbuilder.exe --help

![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/cmd%20example.gif "cmd-example")

## Features
| application color/theme              | process history      | AutoUpdater      |
| ------------------|:-------------:|:-------------:|
| ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/colors.png "colors") | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/history.png "history")  | ![alt text](https://github.com/tuke307/LibBuilder/blob/master/Screenshots/update.png "update")  | 

