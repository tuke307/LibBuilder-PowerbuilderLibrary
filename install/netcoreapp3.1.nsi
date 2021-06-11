; LibBuilder Installer Code
;--------------------------------

!define APPNAME "LibBuilder"
!define STDPATH "C:\Users\T.meissner\source\repos\tuke307\LibBuilder\src"
!define FRAMEWORK "netcoreapp3.1"

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------

; The name of the installer
Name "${APPNAME}"

; The file to write
OutFile "${APPNAME}Installer_${FRAMEWORK}.exe"

; Request application privileges
RequestExecutionLevel admin

; The default installation directory
InstallDir "$PROGRAMFILES\${APPNAME}"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\${APPNAME}" "Install_Dir"

;--------------------------------
VIProductVersion                 "2.0.0.0"
VIAddVersionKey ProductName      "${APPNAME}"
VIAddVersionKey CompanyName      "Timeline Financials GmbH & Co. KG"
VIAddVersionKey LegalCopyright   "Timeline Financials GmbH & Co. KG"
VIAddVersionKey FileDescription  "Timeline Financials GmbH & Co. KG"

;--------------------------------
; Pages
  ;installer
  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  ;uninstaller
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "German"

;--------------------------------
; The stuff to install
Section "Installationsdateien"

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; clear INSTDIR
  RMDir /r "$INSTDIR"
  
  ; Put file there
  File /r "${STDPATH}\LibBuilder.WPF.App\bin\x86\Release\${FRAMEWORK}\publish\*"
  File /r "${STDPATH}\LibBuilder.Console.App\bin\x86\Release\${FRAMEWORK}\publish\*"
  
  ; uninstaller
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\${APPNAME}" "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoRepair" 1
SectionEnd

; shortcut creation
Section "DesktopShorcut"

 CreateShortCut "$DESKTOP\${APPNAME}.lnk" "$INSTDIR\${APPNAME}.exe" ""
  
SectionEnd

;--------------------------------
; Uninstaller

Section "Uninstall"

  ;installation directory
  Delete "$INSTDIR\${APPNAME}.exe"
  Delete "$INSTDIR\${APPNAME}.Console.exe"
  Delete "$INSTDIR\uninstall.exe"
  RMDir /r "$INSTDIR"
 
  ; shortcuts
  Delete "$DESKTOP\${APPNAME}.lnk"
  Delete "$SMPROGRAMS\${APPNAME}\*.*"
  RMDir "$SMPROGRAMS\${APPNAME}"
  
  ; Registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
  DeleteRegKey HKLM "SOFTWARE\${APPNAME}"

  ; Appdata
  RMDir /r "$APPDATA\${APPNAME}"
  
SectionEnd
