@echo off
setlocal
cd /d "%~dp0"

echo =====================================================
echo NanoTwitchLeafs 3.2.0.5 - privater lokaler Build
echo =====================================================
echo.

set "NTL_CREDENTIALS="
if exist "%~dp0ServiceCredentials" set "NTL_CREDENTIALS=%~dp0ServiceCredentials"
if exist "%~dp0ServiceCredentials.local" set "NTL_CREDENTIALS=%~dp0ServiceCredentials.local"

if defined NTL_CREDENTIALS (
    echo ServiceCredentials wurde gefunden und wird nur lokal in das Testpaket kopiert.
) else (
    echo HINWEIS: Keine ServiceCredentials-Datei im Quellordner gefunden.
    echo Die EXE kann gebaut werden, aber Twitch/Streamlabs benoetigt diese Datei.
    echo Du kannst die Datei aus deiner bisherigen NTL-Installation als
    echo ServiceCredentials.local neben diese CMD kopieren.
    echo.
)

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0build-release.ps1" -ServiceCredentialsPath "%NTL_CREDENTIALS%"
set "NTL_RESULT=%ERRORLEVEL%"

echo.
if not "%NTL_RESULT%"=="0" (
    echo BUILD FEHLGESCHLAGEN.
    echo Bitte den gesamten Text dieses Fensters als Screenshot schicken.
) else (
    echo BUILD ERFOLGREICH.
    echo Das fertige Paket liegt unter:
    echo %~dp0artifacts\NanoTwitchLeafs-3.2.0.5.zip
    explorer.exe "%~dp0artifacts"
)

echo.
pause
exit /b %NTL_RESULT%
