using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class ObjectBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "object_name_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				string str = IdTo<Object>(data, objectId).name;
				return WriteString(data, str);
			});
			
			linker.DefineFunction("unity", "object_name_set", (Caller caller, long objectId, long strPtr, int strSize) => {
				StoreData data = GetData(caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				IdTo<Object>(data, objectId).name = str;
			});
			
			linker.DefineFunction("unity", "object_toString", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				string str = IdTo<Object>(data, objectId).ToString();
				return WriteString(data, str);
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