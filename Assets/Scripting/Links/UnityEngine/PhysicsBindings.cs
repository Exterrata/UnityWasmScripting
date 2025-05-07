using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
    public class PhysicsBindings : WasmBinding
    {
        public static void BindMethods(Linker linker)
        {
            linker.DefineFunction("unity", "physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction", (
                Caller caller,
                long originPtr,
                long directionPtr,
                long hitInfoPtr,
                float maxDistance,
                int layerMask,
                int queryTriggerInteraction) =>
            {
                StoreData data = GetData(caller);
                Vector3 origin = data.Memory.Read<Vector3>(originPtr);
                Vector3 direction = data.Memory.Read<Vector3>(directionPtr);

                bool hit = Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance, layerMask, (QueryTriggerInteraction)queryTriggerInteraction);
                data.Memory.Write(hitInfoPtr, hitInfo);
                Debug.Log(hitInfo);

                return UnsafeUtility.As<bool, int>(ref hit);
            });
        }
    }
}
