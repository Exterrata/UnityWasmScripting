@echo off
setlocal enabledelayedexpansion

set "SDK_VERSION=25.0"
set "TARGET_DIR=wasi-sdk"
set "ARCHIVE_NAME=wasi-sdk-%SDK_VERSION%-x86_64-windows.tar.gz"
set "VC_REDIST_URL=https://aka.ms/vs/17/release/vc_redist.x64.exe"
set "VC_REDIST_EXE=%TEMP%\vc_redist.x64.exe"

reg query "HKLM\SOFTWARE\Microsoft\VisualStudio\17.0\VC\Runtimes\x64" >nul 2>&1
if %errorlevel% neq 0 (
    echo Installing Visual C++ Redistributable

    powershell -Command "$ProgressPreference = 'SilentlyContinue'; (New-Object Net.WebClient).DownloadFile('%VC_REDIST_URL%', '%VC_REDIST_EXE%')"
    if errorlevel 1 (
        echo ERROR: Failed to download vc_redist.x64.exe
        echo Manual download: !VC_REDIST_URL!
        pause & exit /b 1
    )

    %VC_REDIST_EXE% /install /quiet /norestart
    if errorlevel 1 (
        echo ERROR: vc_redist.x64.exe installation failed
        pause & exit /b 1
    )
    echo Visual C++ Redistributable installed successfully.
)

set "SCRIPT_DIR=%~dp0"
if "!SCRIPT_DIR:~-1!"=="\" set "SCRIPT_DIR=!SCRIPT_DIR:~0,-1!"
set "WASI_SDK_PATH=!SCRIPT_DIR!\!TARGET_DIR!"

for /f "tokens=1 delims=." %%a in ("%SDK_VERSION%") do set "MAJOR_VER=%%a"
set "DOWNLOAD_URL=https://github.com/WebAssembly/wasi-sdk/releases/download/wasi-sdk-!MAJOR_VER!/%ARCHIVE_NAME%"

if not exist "!WASI_SDK_PATH!" (
    echo Installing WASI SDK v%SDK_VERSION%...
    
    echo Downloading %ARCHIVE_NAME%
    powershell -Command "$ProgressPreference = 'SilentlyContinue'; (New-Object Net.WebClient).DownloadFile('!DOWNLOAD_URL!', '!SCRIPT_DIR!\%ARCHIVE_NAME%')"
    if errorlevel 1 (
        echo ERROR: Download failed
        echo Manual download: !DOWNLOAD_URL!
        pause & exit /b 1
    )

    echo Extracting files...
    tar -xzf "!SCRIPT_DIR!\%ARCHIVE_NAME%" -C "!SCRIPT_DIR!"
    if errorlevel 1 (
        echo ERROR: Extraction failed
        pause & exit /b 1
    )

    move "!SCRIPT_DIR!\wasi-sdk-%SDK_VERSION%-x86_64-windows" "!WASI_SDK_PATH!" >nul || (
        echo ERROR: Failed to create directory
        pause & exit /b 1
    )
    del "!SCRIPT_DIR!\%ARCHIVE_NAME%"
    echo Success: Installed to !WASI_SDK_PATH!
)

dotnet publish -c Release