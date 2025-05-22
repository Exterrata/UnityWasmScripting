using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class PhysicsBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction(
				"unity",
				"Physics_func_SphereCastNonAlloc_vector3_float_vector3_raycasthit_int_float_int_querytriggerinteraction",
				(Caller caller, long originPtr, float radius, long directionPtr, long resultsPtr, int resultsLength, float maxDistance, int layerMask, int queryTriggerInteraction) => {
					StoreData data = GetData(caller);
					Vector3 origin = data.Memory.Read<Vector3>(originPtr);
					Vector3 direction = data.Memory.Read<Vector3>(directionPtr);

					RaycastHit[] results = new RaycastHit[resultsLength];

					int ret = Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask, (QueryTriggerInteraction)queryTriggerInteraction);

					int size = UnsafeUtility.SizeOf<RaycastHit>();
					for (int i = 0; i < resultsLength; i++) {
						data.Memory.Write(resultsPtr + size * i, results[i]);
					}

					return ret;
				}
			);

			linker.DefineFunction(
				"unity",
				"Physics_func_Raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction",
				(Caller caller, long originPtr, long directionPtr, long hitInfoPtr, float maxDistance, int layerMask, int queryTriggerInteraction) => {
					StoreData data = GetData(caller);
					Vector3 origin = data.Memory.Read<Vector3>(originPtr);
					Vector3 direction = data.Memory.Read<Vector3>(directionPtr);

					bool hit = Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance, layerMask, (QueryTriggerInteraction)queryTriggerInteraction);
					data.Memory.Write(hitInfoPtr, hitInfo);

					return UnsafeUtility.As<bool, int>(ref hit);
				}
			);
		}
	}
}