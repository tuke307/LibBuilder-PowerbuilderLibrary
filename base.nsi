;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  !include sections.nsh

;--------------------------------
; The name of the installer
Name "LibBuilder"

; The file to write
OutFile "LibBuilderInstaller.exe"

; Request application privileges
RequestExecutionLevel admin

;--------------------------------
VIProductVersion                 "2.0.0.0"
VIAddVersionKey ProductName      "LibBuilder"
VIAddVersionKey CompanyName      "Timeline Financials GmbH & Co. KG"
VIAddVersionKey LegalCopyright   "Timeline Financials GmbH & Co. KG"
VIAddVersionKey FileDescription  "Timeline Financials GmbH & Co. KG"

;--------------------------------
; Pages
  ;installer
  !insertmacro MUI_PAGE_WELCOME
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "German"

;--------------------------------
Section "basierend auf .Net Framework 4.7.2" P1
File "/oname=$pluginsdir\Setup.exe" "LibBuilderInstaller_net472.exe"
SectionEnd

Section /o "basierend auf .Net Core 3.1" P2
File "/oname=$pluginsdir\Setup.exe" "LibBuilderInstaller_netcoreapp3.1.exe"
SectionEnd

Section /o "basierend auf .Net 5.0" P3
File "/oname=$pluginsdir\Setup.exe" "LibBuilderInstaller_net5.0-windows.exe"
SectionEnd

Section ; Hidden section that runs the show
DetailPrint "Installing selected application..."
SetDetailsPrint none
ExecWait '"$pluginsdir\Setup.exe"'
SetDetailsPrint lastused
SectionEnd

Function .onInit
Initpluginsdir ; Make sure $pluginsdir exists
StrCpy $1 ${P2} ;The default
FunctionEnd

Function .onSelChange
!insertmacro StartRadioButtons $1
    !insertmacro RadioButton ${P1}
    !insertmacro RadioButton ${P2}
	!insertmacro RadioButton ${P3}
!insertmacro EndRadioButtons
FunctionEnd
