using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace WasmScripting {
	[DefaultExecutionOrder(50)]
    public class WasmBehaviour : MonoBehaviour {
#if UNITY_EDITOR
        public MonoScript script;
#endif
		public List<WasmVariable<int>> intVariables;
		public List<WasmVariable<bool>> boolVariables;
		public List<WasmVariable<float>> floatVariables;
		public List<WasmVariable<string>> stringVariables;
		public List<WasmVariable<Component>> componentVariables;
		public List<WasmVariable<GameObject>> gameObjectVariables;
		[SerializeField]
		internal string behaviourName;
		internal int InstanceId;
		private WasmVM _vm;
		
		private void Awake() {
			InstanceId = GetInstanceID();
			_vm = GetComponentInParent<WasmVM>();
			if (_vm.Awaked) _vm.CallMethod(InstanceId, UnityEvent.Awake);
		}

		private void Start() => _vm.CallMethod(InstanceId, UnityEvent.Start);
		private void Update() => _vm.CallMethod(InstanceId, UnityEvent.Update);
		private void LateUpdate() => _vm.CallMethod(InstanceId, UnityEvent.LateUpdate);
		private void FixedUpdate() => _vm.CallMethod(InstanceId, UnityEvent.FixedUpdate);
		private void OnEnable() => _vm.CallMethod(InstanceId, UnityEvent.OnEnable);
		private void OnDisable() => _vm.CallMethod(InstanceId, UnityEvent.OnDisable);
		private void OnDestroy() {
			if (_vm.Initialized) _vm.CallMethod(InstanceId, UnityEvent.OnDestroy);
		}
		private void OnPreCull() => _vm.CallMethod(InstanceId, UnityEvent.OnPreCull);
		private void OnPreRender() => _vm.CallMethod(InstanceId, UnityEvent.OnPreRender);
		private void OnPostRender() => _vm.CallMethod(InstanceId, UnityEvent.OnPostRender);
	}
	
	[Serializable]
	public struct WasmVariable<T> {
		public string name;
		public T value;
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(WasmBehaviour))]
	public class WasmBehaviourInspector : Editor {
		private const string ProjectRoot = @"C:\Users\Koneko\Documents\CVRProjects\Wasm\Assets";
		private const string WasmProjectPath = @"C:\Users\Koneko\Documents\CVRProjects\Wasm\Assets\.WasmModule";
		private const string BuiltModulePath = @"bin\Release\net9.0\wasi-wasm\publish\WasmModule.wasm";
		
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if (GUILayout.Button("Build Wasm Module")) {
				WasmVM vm = ((Component)target).GetComponentInParent<WasmVM>();
				WasmBehaviour[] behaviours = vm.GetComponentsInChildren<WasmBehaviour>();
				
				Directory.Delete(Path.Combine(WasmProjectPath, "Temp"), true);
				Directory.CreateDirectory(Path.Combine(WasmProjectPath, "Temp"));

				HashSet<string> scriptNames = new();
				foreach (var behaviour in behaviours) {
					MonoScript script = behaviour.script;
					behaviour.behaviourName = script.GetClass().FullName;
					
					if (scriptNames.Add(script.name)) {
						string srcPath = AssetDatabase.GetAssetPath(script);
						string dstPath = Path.Combine(WasmProjectPath, "Temp", $"{behaviour.behaviourName}.cs");
						File.Copy(srcPath, dstPath);
					}
				}

				Process buildCmd = new Process {
					StartInfo = new ProcessStartInfo {
						FileName = @"C:\Windows\System32\cmd.exe",
						Arguments = $"/c cd \"{WasmProjectPath}\" && build.bat",
						UseShellExecute = false,
						RedirectStandardOutput = true,
						CreateNoWindow = true,
						WorkingDirectory = WasmProjectPath
					}
				};

				buildCmd.Start();
				buildCmd.WaitForExit();

				File.Copy(Path.Combine(WasmProjectPath, BuiltModulePath), Path.Combine(ProjectRoot, "WasmModule.wasm"), true);
				
				const string modulePath = "Assets/WasmModule.wasm";
				AssetDatabase.ImportAsset(modulePath);
				vm.moduleAsset = AssetDatabase.LoadAssetAtPath<WasmModuleAsset>(modulePath);
			}
		}
	}
#endif
}