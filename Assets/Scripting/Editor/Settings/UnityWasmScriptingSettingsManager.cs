using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages the settings for UnityWasmScripting.
/// </summary>
public static class UnityWasmScriptingSettingsManager
{
    private const string ProjectName = "UnityWasmScripting";
    private const string DefaultWasmModulePath = ".WasmModule";
    
    private static UnityWasmScriptingSettings s_Instance;
    private static readonly string k_SettingsPath = $"Assets/Resources/{ProjectName}Settings.asset";

    #region Settings Access
    
    public static UnityWasmScriptingSettings GetOrCreateSettings()
    {
        if (s_Instance != null)
            return s_Instance;
            
        s_Instance = AssetDatabase.LoadAssetAtPath<UnityWasmScriptingSettings>(k_SettingsPath);
        
        if (s_Instance == null)
        {
            string directory = Path.GetDirectoryName(k_SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory ?? throw new DirectoryNotFoundException());
            
            s_Instance = ScriptableObject.CreateInstance<UnityWasmScriptingSettings>();
            AssetDatabase.CreateAsset(s_Instance, k_SettingsPath);
            AssetDatabase.SaveAssets();
        }
        
        return s_Instance;
    }
    
    public static string GetProjectRoot() => 
        string.IsNullOrEmpty(GetOrCreateSettings()?.projectRootOverride) 
            ? Application.dataPath : GetOrCreateSettings().projectRootOverride;
        
    public static string GetWasmModulePath() => 
        string.IsNullOrEmpty(GetOrCreateSettings()?.wasmModulePathOverride) 
            ? Path.Combine(GetProjectRoot(), DefaultWasmModulePath) 
            : GetOrCreateSettings().wasmModulePathOverride;
    
    public static bool GetHideCommandPrompt() => 
        GetOrCreateSettings()?.hideCommandPrompt ?? false;
    
    #endregion Settings Access
}