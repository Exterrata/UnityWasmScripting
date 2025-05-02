using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class ComponentBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "component_tag_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				string str = IdTo<Component>(data, objectId).tag;
				return WriteString(data, str);
			});
			
			linker.DefineFunction("unity", "component_tag_set", (Caller caller, long objectId, long strPtr, int strSize) => {
				StoreData data = GetData(caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				IdTo<Component>(data, objectId).tag = str;
			});
			
			linker.DefineFunction("unity", "component_transform_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				return IdFrom(data, IdTo<Component>(data, objectId).transform);
			});
			
			linker.DefineFunction("unity", "component_gameObject_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				return IdFrom(data, IdTo<Component>(data, objectId).gameObject);
			});

			linker.DefineFunction("unity", "component_func_getcomponent_string", (Caller Caller, long objectId, long strPtr, int strSize) => {
				StoreData data = GetData(Caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				Component component = IdTo<Component>(data, objectId).GetComponent(str);
				return IdFrom(data, component);
			});
		}
	}
}