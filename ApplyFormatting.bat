setlocal enabledelayedexpansion

if "%1"=="smudge" (
    copy /Y ".local.editorconfig" ".editorconfig" >nul
) else (
    copy /Y ".repo.editorconfig"  ".editorconfig" >nul
)

set PROJECTS= "WasmScripting.csproj" "WasmScripting.Editor.csproj" "WasmScripting.Links.csproj" "./Assets/.WasmModule/WasmModule.csproj"

for %%P in (%PROJECTS%) do (
    dotnet format "%%~P" -v q
)

endlocal