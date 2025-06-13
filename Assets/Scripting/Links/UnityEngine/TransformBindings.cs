using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
	public class TransformBindings : WasmBinding
	{
		public static void BindMethods(Linker linker)
		{
			linker.DefineFunction(
				"unity",
				"transform_position_get",
				(Caller caller, long objectId, long positionPtr) =>
				{
					StoreData data = GetData(caller);
					Vector3 position = IdToClass<Transform>(data, objectId).position;
					data.Memory.Write(positionPtr, position);
				}
			);

			linker.DefineFunction(
				"unity",
				"transform_position_set",
				(Caller caller, long objectId, long positionPtr) =>
				{
					StoreData data = GetData(caller);
					Transform transform = IdToClass<Transform>(data, objectId);
					transform.position = data.Memory.Read<Vector3>(positionPtr);
				}
			);
		}
	}
}
