using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class ObjectBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "object_name_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				WriteString(data, IdTo<Object>(data, objectId).name, 0);
			});
			
			linker.DefineFunction("unity", "object_name_set", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				IdTo<Object>(data, objectId).name = ReadString(data, 0);
			});
			
			linker.DefineFunction("unity", "object_toString", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				WriteString(data, IdTo<Object>(data, objectId).ToString(), 0);
			});
			
			linker.DefineFunction("unity", "object_destroy", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				Object.Destroy(IdTo<Object>(data, objectId));
			});
			
			linker.DefineFunction("unity", "object_instantiate", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				Object.Instantiate(IdTo<Object>(data, objectId));
			});
		}
	}
}