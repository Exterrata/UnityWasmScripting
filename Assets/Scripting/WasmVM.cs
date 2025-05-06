using System;
using System.Text;
using System.Linq;
using UnityEngine;
using Wasmtime;

namespace WasmScripting {
	[DefaultExecutionOrder(0)]
	public class WasmVM : MonoBehaviour {
		private Action<int, long, long> _createMethod;
		private Action<int, int> _callMethod;
		private Instance _instance;
		private Module _module;
		private Store _store;
		
		public WasmVMContext context = WasmVMContext.GameObject;
		public WasmModuleAsset moduleAsset;
		public ulong fuelPerFrame = 10000000;
		
		public bool Initialized { get; private set; }
		public bool Awakened { get; private set; }

		private void Awake() {
		    _module = Module.FromBytes(WasmManager.Engine, "Scripting", moduleAsset.bytes);
			_store = new(WasmManager.Engine);

			// Prevents Instantiate erroring when there's missing links (has to run before Instantiate)
			FillNonLinkedWithEmptyStubs(_module, _store);

		    _instance = WasmManager.Linker.Instantiate(_store, _module);

			_store.Fuel = fuelPerFrame;
			_instance.GetAction("_initialize")?.Invoke();
			
			_createMethod = _instance.GetAction<int, long, long>("scripting_create_instance")!;
			_callMethod = _instance.GetAction<int, int>("scripting_call")!;
			
			_store.SetData(new StoreData(gameObject, _instance));
			
			foreach (WasmBehaviour behaviour in GetComponentsInChildren<WasmBehaviour>(true)) {
				int id = behaviour.GetInstanceID();
				behaviour.InstanceId = id;
				CreateInstance(id, behaviour);
			}

			Initialized = true;
		}

		private void Start() {
			foreach (WasmBehaviour behaviour in GetComponentsInChildren<WasmBehaviour>()) {
				CallMethod(behaviour.InstanceId, UnityEvent.Awake);
			}

			Awakened = true;
		}

		private void Update() {
			_store.Fuel = fuelPerFrame;
		}

		private void CreateInstance(int id, WasmBehaviour behaviour) {
			StoreData data = (StoreData)_store.GetData()!;
			string name = behaviour.behaviourName;
			long strPtr = data.Alloc((name.Length + 1) * sizeof(char));
			data.Memory.WriteString(strPtr, name, Encoding.Unicode);
			data.Memory.WriteInt16(strPtr + name.Length * 2, 0);
			_createMethod(id, data.AccessManager.ToWrapped(behaviour).Id, strPtr);
		}

		public void CallMethod(int id, UnityEvent @event) {
			_callMethod(id, (int)@event);
		}

		private void OnDestroy() {
			Initialized = false;
			_store.Dispose();
			_module.Dispose();
		}

		/// <summary>
		/// Fill all the non-linked functions with empty stubs so they won't explode
		/// Note: If the function returns a bool, it will be false (which is amazing)
		/// </summary>
		private static void FillNonLinkedWithEmptyStubs(Module module, Store store)
		{
			foreach (var import in module.Imports)
			{
				// Ignore non-function types
				if (import is not FunctionImport funcImport) continue;

				var moduleName = import.ModuleName;
				var functionName = import.Name;

				// Skip if already defined in the linker
				if (WasmManager.Linker.GetFunction(store, moduleName, functionName) == null)
				{
					ValueKind[] paramTypes = funcImport.Parameters.ToArray();
					ValueKind[] resultTypes = funcImport.Results.ToArray();
					WasmManager.Linker.DefineFunction(moduleName, functionName, (_, _, _) => { }, paramTypes, resultTypes);
				}
			}
		}
	}

	public readonly struct StoreData {
		public readonly WasmAccessManager AccessManager;
		public readonly Func<int, long> Alloc;
		public readonly Memory Memory;
		
		public StoreData(GameObject root, Instance instance) {
			AccessManager = new(root);
			Alloc = instance.GetFunction<int, long>("scripting_alloc");
			Memory = instance.GetMemory("memory");
		}
	}

	public enum WasmVMContext {
		GameObject, // Avatars, Spawnables
		Scene		// World (entire scene)
	}
}