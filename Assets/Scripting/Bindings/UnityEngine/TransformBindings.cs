using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class TransformBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "transform_position_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				Vector3 position = IdTo<Transform>(data, objectId).position;
				WriteStruct(data, ref position, 0);
			});
            
			linker.DefineFunction("unity", "transform_position_set", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				Transform transform = IdTo<Transform>(data, objectId);
				transform.position = ReadStruct<Vector3>(data, 0);
			});
		}
	}
}