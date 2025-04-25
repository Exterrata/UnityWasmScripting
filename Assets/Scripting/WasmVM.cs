using System;
using System.Collections;
using UnityEngine;
using Wasmtime;

namespace WasmScripting {
	[DefaultExecutionOrder(0)]
	public class WasmVM : MonoBehaviour {
		private Action<int, long> _createMethod;
		private Action<int, int> _callMethod;
		private Instance _instance;
		private Module _module;
		private Store _store;
		
		public WasmModuleAsset moduleAsset;
		public ulong fuelPerFrame = 1000000000;
		
		public bool Initialized { get; private set; }
		public bool Awaked { get; private set; }

		public void Awake() {
			_module = Module.FromBytes(WasmManager.Engine, "Scripting", moduleAsset.bytes);
			
			_store = new(WasmManager.Engine);
			_instance = WasmManager.Linker.Instantiate(_store, _module);

			_store.Fuel = fuelPerFrame;
			_instance.GetAction("_initialize")?.Invoke();
			
			_createMethod = _instance.GetAction<int, long>("scripting_create_instance")!;
			_callMethod = _instance.GetAction<int, int>("scripting_call")!;
			
			_store.SetData(new StoreData(gameObject, _instance));
			
			foreach (WasmBehaviour behaviour in GetComponentsInChildren<WasmBehaviour>(true)) {
				int id = behaviour.GetInstanceID();
				behaviour.InstanceId = id;
				CreateInstance(id, behaviour);
			}

			StartCoroutine(ResetFuel());
			Initialized = true;
		}

		private void Start() {
			foreach (WasmBehaviour behaviour in GetComponentsInChildren<WasmBehaviour>()) {
				CallMethod(behaviour.InstanceId, UnityEvent.Awake);
			}

			Awaked = true;
		}

		private void CreateInstance(int id, WasmBehaviour behaviour) {
			StoreData data = (StoreData)_store.GetData()!;
			data.Buffer.WriteString(behaviour.behaviourName, 0);
			_createMethod(id, data.AccessManager.ToWrapped(behaviour).Id);
		}

		public void CallMethod(int id, UnityEvent @event) {
			_callMethod(id, (int)@event);
		}

		private void OnDestroy() {
			Initialized = false;
			_store.Dispose();
			_module.Dispose();
		}

		private IEnumerator ResetFuel() {
			YieldInstruction waitInstruction = new WaitForEndOfFrame();
			while (true) {
				_store.Fuel = fuelPerFrame;
				yield return waitInstruction;
			}
		}
	}

	public readonly struct StoreData {
		public readonly WasmAccessManager AccessManager;
		public readonly WasmPassthroughBuffer Buffer;
		
		public StoreData(GameObject root, Instance instance) {
			AccessManager = new(root);
			Buffer = new(instance);
		}
	}
}