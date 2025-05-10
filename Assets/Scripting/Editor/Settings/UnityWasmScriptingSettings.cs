using UnityEngine;

/// <summary>
/// Settings store object for UnityWasmScripting.
/// </summary>
public class UnityWasmScriptingSettings : ScriptableObject {
	public string projectRootOverride = "";
	public string wasmModulePathOverride = "";
	public bool hideCommandPrompt;
}