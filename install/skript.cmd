"C:\Program Files (x86)\NSIS\makensis.exe" net5.0-windows.nsi
"C:\Program Files (x86)\NSIS\makensis.exe" net472.nsi
"C:\Program Files (x86)\NSIS\makensis.exe" netcoreapp3.1.nsi
"C:\Program Files (x86)\NSIS\makensis.exe" base.nsi
pause

COPY "LibBuilderInstaller.exe" "I:\TL_Tools\LibBuilder\Visual Studio (C#)\LibBuilderInstaller.exe" /Y
COPY "AutoUpdater.xml" "I:\TL_Tools\LibBuilder\Visual Studio (C#)\AutoUpdater.xml" /Y
pause