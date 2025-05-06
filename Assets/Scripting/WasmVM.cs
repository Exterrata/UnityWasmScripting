using System;
using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting {
	[DefaultExecutionOrder(0)]
	public class WasmVM : MonoBehaviour {
		private Action<int, long, long, int> _createMethod;
		private Action<int, int> _callMethod;
		private Instance _instance;
		private Module _module;
		private Store _store;
		
		public ulong fuelPerFrame = 10000000;
		
		public bool Initialized { get; private set; }
		public bool Awakened { get; private set; }

		private void Setup(WasmModuleAsset moduleAsset) {
		    _module = Module.FromBytes(WasmManager.Engine, "Scripting", moduleAsset.bytes);
			_store = new(WasmManager.Engine);

			// Prevents Instantiate erroring when there's missing links (has to run before Instantiate)
			BindingManager.FillNonLinkedWithEmptyStubs(_module, _store);

		    _instance = WasmManager.Linker.Instantiate(_store, _module);

			_store.Fuel = fuelPerFrame;
			_instance.GetAction("_initialize")?.Invoke();
			
			_createMethod = _instance.GetAction<int, long, long, int>("scripting_create_instance")!;
			_callMethod = _instance.GetAction<int, int>("scripting_call")!;
			
			_store.SetData(new StoreData(gameObject, _instance));
			
			foreach (WasmRuntimeBehaviour behaviour in GetComponentsInChildren<WasmRuntimeBehaviour>(true)) {
				int id = behaviour.GetInstanceID();
				behaviour.InstanceId = id;
				CreateInstance(id, behaviour);
			}

			Initialized = true;
		}

		private void Start() {
			foreach (WasmRuntimeBehaviour behaviour in GetComponentsInChildren<WasmRuntimeBehaviour>()) {
				CallMethod(behaviour.InstanceId, UnityEvent.Awake);
			}

			Awakened = true;
		}

		private void Update() {
			_store.Fuel = fuelPerFrame;
		}

		private void CreateInstance(int id, WasmRuntimeBehaviour runtimeBehaviour) {
			StoreData data = (StoreData)_store.GetData()!;
			string name = runtimeBehaviour.behaviourName;
			int strLength = name.Length;
			long strPtr = data.Alloc(strLength * sizeof(char));
			data.Memory.WriteString(strPtr, name, Encoding.Unicode);
			_createMethod(id, data.AccessManager.ToWrapped(runtimeBehaviour).Id, strPtr, strLength);
		}

		public void CallMethod(int id, UnityEvent @event) {
			_callMethod(id, (int)@event);
		}

		private void OnDestroy() {
			Initialized = false;
			_store.Dispose();
			_module.Dispose();
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
}