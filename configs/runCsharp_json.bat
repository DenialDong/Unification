set LUBAN_DLL=..\tools\ExcelTools_Luban\DLL\Luban.dll
set OUTPUTJSON=..\client\Unity\Assets\HotfixResources\ExcelJSON
set OUTPUTCODE=..\client\Unity\Assets\Scripts\hotfix\Main\Codes\Generate\Configs
echo %cd%

dotnet %LUBAN_DLL% ^
    -t all ^
	-c cs-simple-json ^
    -d json ^
    --schemaPath ..\tools\ExcelTools_Luban\Defines\__root__.xml ^
    -x inputDataDir=%cd% ^
	-x outputCodeDir=%OUTPUTCODE% ^
    -x outputDataDir=%OUTPUTJSON%

pause
Rem ^
	Rem -x l10n.textProviderFile=TeseEn.xlsx ^
	Rem -x l10n.textFieldName=text_en ^
	Rem -x l10n.outputNotTranslatedTextFile=NotLocalized_CN.txt
pause