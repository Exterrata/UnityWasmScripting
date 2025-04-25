using UnityEditor;

/// <summary>
/// Creates the settings provider so the user can modify the settings via the Unity Project Settings window.
/// </summary>
public class UnityWasmScriptingSettingsProvider : SettingsProvider
{
    private const string ProjectName = "UnityWasmScripting";
    private const string DefaultProjectRoot = "Assets";
    private const string DefaultWasmModulePath = ".WasmModule";
    
    private SerializedObject m_Settings;

    private UnityWasmScriptingSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
        : base(path, scope) {}
    
    #region Settings Management
    
    private SerializedObject GetSerializedSettings() => m_Settings ??= new SerializedObject(UnityWasmScriptingSettingsManager.GetOrCreateSettings());
    
    #endregion Settings Management

    #region GUI Methods
    
    public override void OnGUI(string searchContext)
    {
        SerializedObject settings = GetSerializedSettings();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"{ProjectName} Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Default Paths:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Project Root: {DefaultProjectRoot}");
        EditorGUILayout.LabelField($"WASM Module Path: {DefaultProjectRoot}/{DefaultWasmModulePath}");
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Path Overrides:", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        
        SerializedProperty prop = settings.GetIterator();
        if (prop.NextVisible(true))
        {
            do
            {
                if (prop.propertyPath == "m_Script") continue;
                EditorGUILayout.PropertyField(prop, true);
            }
            while (prop.NextVisible(false));
        }
        
        if (EditorGUI.EndChangeCheck()) settings.ApplyModifiedProperties();
    }
    
    #endregion GUI Methods

    #region Provider Registration
    
    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        UnityWasmScriptingSettingsProvider provider = new($"Project/{ProjectName}")
        {
            keywords = GetSearchKeywordsFromSerializedObject(
                new SerializedObject(UnityWasmScriptingSettingsManager.GetOrCreateSettings()))
        };
        return provider;
    }
    
    #endregion Provider Registration
}