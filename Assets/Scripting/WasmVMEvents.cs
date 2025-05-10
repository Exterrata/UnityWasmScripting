using System;
using WasmScripting.Enums;

namespace WasmScripting {
	public partial class WasmVM {
		private Action<long, int> _callEvent;
		private Action<long, int> _callAwake;
		private Action<long, int> _callStart;
		private Action _callUpdate;
		private Action _callLateUpdate;
		private Action _callFixedUpdate;

		private void InitializeEvents() {
			_callEvent = _instance.GetAction<long, int>("scripting_call_event")!;
			_callAwake = _instance.GetAction<long, int>("scripting_call_awake")!;
			_callStart = _instance.GetAction<long, int>("scripting_call_start")!;
			_callUpdate = _instance.GetAction("scripting_call_update")!;
			_callLateUpdate = _instance.GetAction("scripting_call_late_update")!;
			_callFixedUpdate = _instance.GetAction("scripting_call_fixed_update")!;
		}

		private void Update() {
			if (IsCrashed)
				return;
			_store.Fuel = fuelPerFrame;
			_callUpdate();
		}

		private void LateUpdate() {
			if (IsCrashed)
				return;
			_callLateUpdate();
		}

		private void FixedUpdate() {
			if (IsCrashed)
				return;
			_callFixedUpdate();
		}

		public void CallScriptEvent(WasmRuntimeBehaviour behaviour, ScriptEvent scriptEvent) {
			if (IsCrashed)
				return;

			StoreData data = (StoreData)_store.GetData()!;
			long id = data.AccessManager.ToWrapped(behaviour).Id;
			_callEvent(id, (int)scriptEvent);
		}
	}
}