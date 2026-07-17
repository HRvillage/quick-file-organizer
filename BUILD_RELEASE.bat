@echo off
setlocal
cd /d "%~dp0"

set "PROJECT=%~dp0src\QuickFileOrganizer\QuickFileOrganizer.csproj"
set "OUTPUT=%~dp0release"

if not exist "%PROJECT%" (
  echo Cannot find QuickFileOrganizer.csproj.
  pause
  exit /b 1
)

if exist "%OUTPUT%" rmdir /s /q "%OUTPUT%"
mkdir "%OUTPUT%"

echo Building Quick File Organizer v1.0.0...
dotnet publish "%PROJECT%" ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:EnableCompressionInSingleFile=false ^
  -p:PublishTrimmed=false ^
  -p:DebugType=none ^
  -p:DebugSymbols=false ^
  -p:PublishDir="%OUTPUT%\"

if errorlevel 1 (
  echo.
  echo Build failed.
  pause
  exit /b 1
)

mkdir "%OUTPUT%\Data" 2>nul
copy /y "%~dp0PORTABLE_README.txt" "%OUTPUT%\README.txt" >nul

echo.
echo Build completed successfully.
start "" "%OUTPUT%"
pause
