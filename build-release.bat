@echo off
rem Builds Pause and packs a Forge-ready archive.
rem Usage: build-release.bat [path-to-SPT]   (default: C:\SPT)
setlocal

set "VERSION=1.5.0"
set "SPT=%~1"
if "%SPT%"=="" set "SPT=C:\SPT"
set "ROOT=%~dp0"
set "OUT=%ROOT%release"
set "PKG=%OUT%\pkg"
set "MODDIR=%PKG%\BepInEx\plugins\netVnum-Pause"
set "ZIP=%OUT%\netVnum-Pause-%VERSION%.zip"

if not exist "%SPT%\BepInEx\plugins\spt\spt-reflection.dll" (
  echo [ERROR] SPT not found at "%SPT%". Pass the SPT folder as the first argument.
  goto :fail
)

echo === Building against "%SPT%" ===
dotnet build "%ROOT%source\Pause.csproj" -c Release -p:SptPath="%SPT%" || goto :fail

echo === Packing ===
if exist "%OUT%" rmdir /s /q "%OUT%"
mkdir "%MODDIR%" || goto :fail
copy /y "%ROOT%source\bin\Release\net481\netVnum.Pause.dll" "%MODDIR%\" >nul || goto :fail
copy /y "%ROOT%LICENSE" "%MODDIR%\LICENSE.txt" >nul || goto :fail
copy /y "%ROOT%README.md" "%MODDIR%\README.md" >nul || goto :fail

rem tar (built into Windows 10+) writes forward-slash paths; Compress-Archive in Windows PowerShell 5.1 writes backslashes
tar -a -c -f "%ZIP%" -C "%PKG%" BepInEx || goto :fail

echo.
echo Done: %ZIP%
echo Archive layout: BepInEx\plugins\netVnum-Pause\netVnum.Pause.dll (+ LICENSE.txt, README.md)
exit /b 0

:fail
echo.
echo Build FAILED.
exit /b 1
