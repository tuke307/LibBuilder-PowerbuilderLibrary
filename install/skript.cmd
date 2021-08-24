dotnet publish ..\src\LibBuilder.WPF.Core\LibBuilder.WPF.Core.csproj -f net472 -p:PublishProfile="net472" -o ..\src\LibBuilder.WPF.App\bin\x86\Release\net472\publish\
dotnet publish ..\src\LibBuilder.WPF.Core\LibBuilder.WPF.Core.csproj -f netcoreapp3.1 -p:PublishProfile="netcoreapp3.1" -o ..\src\LibBuilder.WPF.App\bin\x86\Release\netcoreapp3.1\publish\
dotnet publish ..\src\LibBuilder.WPF.Core\LibBuilder.WPF.Core.csproj -f net5.0-windows -p:PublishProfile="net5.0-windows" -o ..\src\LibBuilder.WPF.App\bin\x86\Release\net5.0-windows\publish\

dotnet publish ..\src\LibBuilder.Console.App\LibBuilder.Console.App.csproj -f net472 -p:PublishProfile="net472" -o ..\src\LibBuilder.Console.App\bin\x86\Release\net472\publish\
dotnet publish ..\src\LibBuilder.Console.App\LibBuilder.Console.App.csproj -f netcoreapp3.1 -p:PublishProfile="netcoreapp3.1" -o ..\src\LibBuilder.Console.App\bin\x86\Release\netcoreapp3.1\publish\
dotnet publish ..\src\LibBuilder.Console.App\LibBuilder.Console.App.csproj -f net5.0-windows -p:PublishProfile="net5.0-windows" -o ..\src\LibBuilder.Console.App\bin\x86\Release\net5.0-windows\publish\

"C:\Program Files (x86)\NSIS\makensis.exe" net5.0-windows.nsi
"C:\Program Files (x86)\NSIS\makensis.exe" net472.nsi
"C:\Program Files (x86)\NSIS\makensis.exe" netcoreapp3.1.nsi
"C:\Program Files (x86)\NSIS\makensis.exe" base.nsi
pause

COPY "LibBuilderInstaller.exe" "I:\TL_Tools\LibBuilder\Visual Studio (C#)\LibBuilderInstaller.exe" /Y
COPY "AutoUpdater.xml" "I:\TL_Tools\LibBuilder\Visual Studio (C#)\AutoUpdater.xml" /Y
pause