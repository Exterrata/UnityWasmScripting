using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WasmScripting {
    [CustomEditor(typeof(WasmBehaviour))]
    public class WasmBehaviourInspector : Editor {
        private const string BuiltModulePath = @"bin\Release\net9.0\wasi-wasm\publish\WasmModule.wasm";
		
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            if (!GUILayout.Button("Build Wasm Module")) 
                return;
            
            WasmVM vm = ((Component)target).GetComponentInParent<WasmVM>(true);
            WasmBehaviour[] behaviours = vm.GetComponentsInChildren<WasmBehaviour>(true);
				    
            string ProjectRoot = UnityWasmScriptingSettingsManager.GetProjectRoot();
            string WasmProjectPath = UnityWasmScriptingSettingsManager.GetWasmModulePath();
                
            Directory.Delete(Path.Combine(WasmProjectPath, "Temp"), true);
            Directory.CreateDirectory(Path.Combine(WasmProjectPath, "Temp"));

            HashSet<string> scriptNames = new();
            foreach (var behaviour in behaviours) {
                MonoScript script = behaviour.script;
                behaviour.BehaviourName = script.GetClass().FullName;
					    
                if (scriptNames.Add(script.name)) {
                    string srcPath = AssetDatabase.GetAssetPath(script);
                    string dstPath = Path.Combine(WasmProjectPath, "Temp", $"{behaviour.BehaviourName}.cs");
                    File.Copy(srcPath, dstPath);
                }
            }

            Process buildCmd = CreateCmdProcess(WasmProjectPath, $"/c cd \"{WasmProjectPath}\" && build.bat");

            buildCmd.Start();
            buildCmd.WaitForExit();

            File.Copy(Path.Combine(WasmProjectPath, BuiltModulePath), Path.Combine(ProjectRoot, "WasmModule.wasm"), true);
				    
            const string modulePath = "Assets/WasmModule.wasm";
            AssetDatabase.ImportAsset(modulePath);
            vm.moduleAsset = AssetDatabase.LoadAssetAtPath<WasmModuleAsset>(modulePath);
        }

        private static Process CreateCmdProcess(string workingDirectory, string arguments)
        {
            bool hideWindow = UnityWasmScriptingSettingsManager.GetHideCommandPrompt();
            return new Process 
            {
                StartInfo = new ProcessStartInfo 
                {
                    FileName = @"C:\Windows\System32\cmd.exe",
                    Arguments = arguments,
                    UseShellExecute = !hideWindow,
                    RedirectStandardOutput = hideWindow,
                    CreateNoWindow = hideWindow,
                    WorkingDirectory = workingDirectory
                }
            };
        }
    }
}