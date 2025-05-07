setlocal enabledelayedexpansion

set PROJECTS= "WasmScripting.csproj" "WasmScripting.Editor.csproj" "WasmScripting.Links.csproj" "./Assets/.WasmModule/WasmModule.csproj"

for %%P in (%PROJECTS%) do (
    dotnet format "%%~P" -v q
)

endlocal