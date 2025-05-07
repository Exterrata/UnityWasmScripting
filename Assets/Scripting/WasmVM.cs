using System;
using System.Text;
using UnityEngine;
using WasmScripting.Enums;
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

		public bool IsCrashed { get; private set; }
		public bool Disposed { get; private set; }

		internal void Setup(WasmModuleAsset moduleAsset, WasmRuntimeBehaviour[] behaviours) {
		    _module = Module.FromBytes(WasmManager.Engine, "Scripting", moduleAsset.bytes);
			_store = new(WasmManager.Engine);

			// Prevents Instantiate erroring when there's missing links (has to run before Instantiate)
			BindingManager.FillNonLinkedWithEmptyStubs(_store, _module);
		    _instance = WasmManager.Linker.Instantiate(_store, _module);

			_store.Fuel = fuelPerFrame;
			// _instance.GetAction("_initialize")?.Invoke();
			
			_createMethod = _instance.GetAction<int, long, long, int>("scripting_create_instance")!;
			_callMethod = _instance.GetAction<int, int>("scripting_call")!;
			
			_store.SetData(new StoreData(gameObject, _instance));
			
			foreach (WasmRuntimeBehaviour behaviour in behaviours) {
				CreateInstance(behaviour.InstanceId, behaviour);
				behaviour.enabled = true; // behaviours should be set disabled at this point and enabling them should trigger there Awake() method.
			}
		}

		private void CreateInstance(int id, WasmRuntimeBehaviour behaviour) {
			StoreData data = (StoreData)_store.GetData()!;
			string name = behaviour.behaviourName;
			int strLength = name.Length;
			long strPtr = data.Alloc(strLength * sizeof(char));
			data.Memory.WriteString(strPtr, name, Encoding.Unicode);

			try
			{
				_createMethod(id, data.AccessManager.ToWrapped(behaviour).Id, strPtr, strLength);
			}
			catch (TrapException e)
			{
				Debug.LogError($"WasmVM threw a trap exception: {e.Message}");
				CrashVM();
			}
			catch (Exception e)
			{
				Debug.LogError($"WasmVM threw an exception: {e.Message}");
			}
		}

		public void CallMethod(int id, UnityEventCall unityEvent) {
			
			if (IsCrashed) return;
			
			try
			{
				_callMethod(id, (int)unityEvent);
			}
			catch (TrapException e)
			{
				Debug.LogError($"WasmVM threw a trap exception: {e.Message}");
				CrashVM();
			}
			catch (Exception e)
			{
				Debug.LogError($"WasmVM threw an exception: {e.Message}");
			}
		}

		private void Update() {
			_store.Fuel = fuelPerFrame;
		}

		private void LateUpdate() {
			
		}

		private void FixedUpdate() {
			
		}

		private void OnDestroy() {
			Disposed = true;
			_store.Dispose();
			_module.Dispose();
		}

		private void CrashVM()
		{
			enabled = false;
			IsCrashed = true;
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