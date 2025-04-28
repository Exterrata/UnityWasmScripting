using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace WasmScripting
{
    /// <summary>
    /// Builds the WASM asset for the provided GameObject, Scene, or all VMs.
    /// </summary>
    public static class WasmBuilder
    {
        private const string BuiltModulePath = @"bin\Release\net9.0\wasi-wasm\publish\WasmModule.wasm";
        private const string HashFilePath = "Library/WasmScripting/ScriptHashes.json";

        #region Public API

        public static void CompileWasmProgramForObject(GameObject go, bool force = false)
        {
            WasmVM vm = go.GetComponentInParent<WasmVM>(true);
            if (vm == null 
                || vm.context != WasmVM.WasmVMContext.GameObject)
                return;

            CompileWasmProgramForVM(vm, force);
        }

        public static void CompileWasmProgramForScene(bool force = false)
        {
            WasmVM sceneVM = null;
            WasmVM[] allVMs = Object.FindObjectsOfType<WasmVM>(true);
            
            foreach (WasmVM vm in allVMs)
            {
                if (vm.context != WasmVM.WasmVMContext.Scene)
                    continue;
                
                sceneVM = vm;
                break;
            }

            if (sceneVM == null)
                return;

            CompileWasmProgramForVM(sceneVM, force);
        }

        public static void CompileAllWasmPrograms()
        {
            WasmVM[] allVMs = Object.FindObjectsOfType<WasmVM>(true);
            if (allVMs.Length == 0)
                return;

            int current = 0;
            int total = allVMs.Length;

            foreach (WasmVM vm in allVMs)
            {
                if (!vm.gameObject.scene.IsValid())
                    continue; // Skip VMs not in active scene.
                
                EditorUtility.DisplayProgressBar("Compiling WASM Programs",
                    $"Compiling VM {current + 1}/{total}", (float)current / total);
                
                CompileWasmProgramForVM(vm);
                current++;
            }

            EditorUtility.ClearProgressBar();
        }

        #endregion Public API

        #region Hashing System

        [Serializable]
        private class ScriptHash
        {
            public string vmId;
            public string hash;
        }

        [Serializable]
        private class ScriptHashCollection
        {
            public List<ScriptHash> hashes = new();
        }

        private static Dictionary<string, string> LoadScriptHashes()
        {
            Dictionary<string, string> hashDict = new Dictionary<string, string>();
            
            // Ensure directory exists
            string directory = Path.GetDirectoryName(HashFilePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory ?? throw new DirectoryNotFoundException());

            // Load existing hashes if file exists
            if (!File.Exists(HashFilePath))
                return hashDict;
            
            string jsonContent = File.ReadAllText(HashFilePath);
            ScriptHashCollection collection = JsonUtility.FromJson<ScriptHashCollection>(jsonContent);

            if (collection?.hashes == null) 
                return hashDict;
            
            foreach (ScriptHash hash in collection.hashes)
                hashDict[hash.vmId] = hash.hash;

            return hashDict;
        }
        
        private static void SaveScriptHashes(Dictionary<string, string> hashDict)
        {
            ScriptHashCollection collection = new();
            
            foreach (KeyValuePair<string, string> pair in hashDict)
            {
                collection.hashes.Add(new ScriptHash 
                { 
                    vmId = pair.Key, 
                    hash = pair.Value 
                });
            }
            
            string jsonContent = JsonUtility.ToJson(collection);
            File.WriteAllText(HashFilePath, jsonContent);
        }
        
        private static string GetScriptFileHash(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;
                
            // Use file's last write time as part of the hash
            DateTime lastWriteTime = File.GetLastWriteTimeUtc(filePath);
            string timeStamp = lastWriteTime.ToBinary().ToString();
            
            // We only need to hash the timestamp and filename for changed detection
            using SHA256 sha256 = SHA256.Create();
            string contentToHash = filePath + timeStamp;
            byte[] bytes = Encoding.UTF8.GetBytes(contentToHash);
            byte[] hashBytes = sha256.ComputeHash(bytes);
                
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        
        private static string ComputeVMHash(WasmVM vm, WasmBehaviour[] behaviours)
        {
            // Sort behaviours for consistent hashing
            List<WasmBehaviour> sortedBehaviours = new List<WasmBehaviour>(behaviours);
            sortedBehaviours.Sort((a, b) => 
                string.Compare((a.script?.name ?? string.Empty), 
                    b.script?.name ?? string.Empty, StringComparison.Ordinal));

            using SHA256 sha256 = SHA256.Create();
            
            // Use memory stream for better performance with large data
            using MemoryStream memStream = new();
            using BinaryWriter writer = new(memStream);
            
            // Write VM instance ID to ensure uniqueness
            writer.Write(vm.GetInstanceID());
                    
            // Process each behaviour script
            HashSet<string> processedPaths = new HashSet<string>();
                    
            foreach (WasmBehaviour behaviour in sortedBehaviours)
            {
                if (behaviour.script == null)
                    continue;
                            
                string scriptPath = AssetDatabase.GetAssetPath(behaviour.script);
                        
                // Skip if we've already processed this script
                if (!processedPaths.Add(scriptPath))
                    continue;
                            
                // Get hash of the script file
                string scriptHash = GetScriptFileHash(scriptPath);
                writer.Write(scriptPath);
                writer.Write(scriptHash);
            }
                    
            // Compute final hash
            memStream.Position = 0;
            byte[] hashBytes = sha256.ComputeHash(memStream);
                    
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        
        private static bool HasScriptsChanged(WasmVM vm, WasmBehaviour[] behaviours)
        {
            if (behaviours.Length == 0)
                return false;
                
            string vmId = vm.GetInstanceID().ToString();
            Dictionary<string, string> hashes = LoadScriptHashes();
            string newHash = ComputeVMHash(vm, behaviours);
            
            // Check if hash exists and matches
            if (hashes.TryGetValue(vmId, out string oldHash) && oldHash == newHash)
                return false;
                
            // Update hash
            hashes[vmId] = newHash;
            SaveScriptHashes(hashes);
            return true;
        }

        #endregion Hashing System

        #region Private Methods

        private static void CompileWasmProgramForVM(WasmVM vm, bool force = false)
        {
            if (vm == null)
                return;

            WasmBehaviour[] behaviours = GetRelevantBehaviours(vm);
            if (behaviours.Length == 0)
                return;
                
            // Check if scripts have changed before compiling
            if (!force && !HasScriptsChanged(vm, behaviours))
            {
                // Skip compilation if no changes detected
                Debug.Log($"No changes detected for VM {vm.name}. Skipping compilation.");
                return;
            }

            string projectRoot = UnityWasmScriptingSettingsManager.GetProjectRoot();
            string wasmProjectPath = UnityWasmScriptingSettingsManager.GetWasmModulePath();
            string tempPath = Path.Combine(wasmProjectPath, "Temp");

            try
            {
                // Clean and recreate Temp directory
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);

                // Copy relevant scripts
                HashSet<string> scriptNames = new HashSet<string>();
                foreach (WasmBehaviour behaviour in behaviours)
                {
                    MonoScript script = behaviour.script;
                    behaviour.behaviourName = script.GetClass().FullName;

                    if (!scriptNames.Add(script.name)) 
                        continue;
                    
                    // Copy the script to the Temp directory
                    string srcPath = AssetDatabase.GetAssetPath(script);
                    string dstPath = Path.Combine(wasmProjectPath, "Temp", $"{behaviour.behaviourName}.cs");
                    File.Copy(srcPath, dstPath);
                }

                // Build the WASM module
                Process buildCmd = CreateCmdProcess(wasmProjectPath, $"cd \"{wasmProjectPath}\" && build.bat");
                buildCmd.Start();
                buildCmd.WaitForExit();

                // Copy and import module
                string outputPath = Path.Combine(wasmProjectPath, BuiltModulePath);
                string destPath = Path.Combine(projectRoot, "WasmModule.wasm");
                File.Copy(outputPath, destPath, true);
                
                // Import the module into Unity & assign it to the VM
                const string modulePath = "Assets/WasmModule.wasm";
                AssetDatabase.ImportAsset(modulePath);
                vm.moduleAsset = AssetDatabase.LoadAssetAtPath<WasmModuleAsset>(modulePath);
            }
            finally
            {
                // Clean up temp directory
                if (Directory.Exists(tempPath))
                {
                    try
                    {
                        Directory.Delete(tempPath, true);
                    }
                    catch (Exception)
                    {
                        // Swallow exception as this is cleanup code
                    }
                }
            }
        }

        private static WasmBehaviour[] GetRelevantBehaviours(WasmVM vm)
        {
            if (vm.context == WasmVM.WasmVMContext.GameObject)
                return vm.GetComponentsInChildren<WasmBehaviour>(true);

            // Scene context
            List<WasmBehaviour> sceneBehaviours = new List<WasmBehaviour>();
            WasmBehaviour[] allBehaviours = Object.FindObjectsOfType<WasmBehaviour>(true);
                
            foreach (WasmBehaviour behaviour in allBehaviours)
            {
                // Check if this behaviour is not under any GameObject VM
                WasmVM parentVM = behaviour.GetComponentInParent<WasmVM>(true);
                if (parentVM == null 
                    || parentVM.context != WasmVM.WasmVMContext.GameObject)
                    sceneBehaviours.Add(behaviour);
            }
                
            return sceneBehaviours.ToArray();
        }

        private static Process CreateCmdProcess(string workingDirectory, string arguments)
        {
            bool hideWindow = UnityWasmScriptingSettingsManager.GetHideCommandPrompt();
            return new Process 
            {
                StartInfo = new ProcessStartInfo 
                {
                    FileName = @"C:\Windows\System32\cmd.exe",
                    Arguments = $"{(hideWindow ? "/c" : "/k")} {arguments}",
                    UseShellExecute = !hideWindow,
                    RedirectStandardOutput = hideWindow,
                    CreateNoWindow = hideWindow,
                    WorkingDirectory = workingDirectory
                }
            };
        }

        #endregion Private Methods
    }
}