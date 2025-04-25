using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class DebugBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "debug_log", (Caller caller) => {
				StoreData data = GetData(caller);
				Debug.Log(ReadString(data, 0));
			});
			
			linker.DefineFunction("unity", "debug_logWarning", (Caller caller) => {
				StoreData data = GetData(caller);
				Debug.LogWarning(ReadString(data, 0));
			});
			
			linker.DefineFunction("unity", "debug_logError", (Caller caller) => {
				StoreData data = GetData(caller);
				Debug.LogError(ReadString(data, 0));
			});
			
			linker.DefineFunction("unity", "debug_logException", (Caller caller) => {
				StoreData data = GetData(caller);
				Debug.LogError(ReadString(data, 0));
			});
		}
	}
}