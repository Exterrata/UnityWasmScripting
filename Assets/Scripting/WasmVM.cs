using System;
using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting {
	[DefaultExecutionOrder(0)]
	public partial class WasmVM : MonoBehaviour {
		private Store _store;
		private Module _module;
		private Instance _instance;
		private Action<long, long, int> _createInstance;
		
		public ulong fuelPerFrame = 10000000;

		public bool IsCrashed { get; private set; }
		public bool Disposed { get; private set; }

		internal void Setup(WasmModuleAsset moduleAsset, WasmRuntimeBehaviour[] behaviours) {
		    _module = Module.FromBytes(WasmManager.Engine, "Scripting", moduleAsset.bytes);
			_store = new(WasmManager.Engine);

			// Prevents Instantiate erroring when there's missing links (has to run before Instantiate)
			BindingManager.FillNonLinkedWithEmptyStubs(_store, _module);
		    _instance = WasmManager.Linker.Instantiate(_store, _module);

			_store.Fuel = fuelPerFrame;
			_instance.GetAction("_initialize")?.Invoke();
			
			_createInstance = _instance.GetAction<long, long, int>("scripting_create_instance")!;
			InitializeEvents();
			
			_store.SetData(new StoreData(gameObject, _instance));
			
			foreach (WasmRuntimeBehaviour behaviour in behaviours) {
				CreateInstance(behaviour);
			}
		}

		private void CreateInstance(WasmRuntimeBehaviour behaviour) {
			StoreData data = (StoreData)_store.GetData()!;
			string name = behaviour.behaviourName;
			int strLength = name.Length;
			long strPtr = data.Alloc(strLength * sizeof(char));
			data.Memory.WriteString(strPtr, name, Encoding.Unicode);

			try {
				_createInstance(data.AccessManager.ToWrapped(behaviour).Id, strPtr, strLength);
			} catch (TrapException e) {
				Debug.LogError($"WasmVM threw a trap exception: {e.Message}");
				IsCrashed = true;
				enabled = false;
			} catch (Exception e) {
				Debug.LogError($"WasmVM threw an exception: {e.Message}");
			}
		}

		private void OnDestroy() {
			Disposed = true;
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