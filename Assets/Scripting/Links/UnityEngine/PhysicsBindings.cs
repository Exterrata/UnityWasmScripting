using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
    public class PhysicsBindings : WasmBinding {
        public static void BindMethods(Linker linker) 
        {
            linker.DefineFunction("unity", "physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction", 
            (
                Caller caller,
                float maxDistance, 
                int layerMask, 
                int queryTriggerInteraction) => 
            {
                StoreData data = GetData(caller);
                Vector3 origin = ReadStruct<Vector3>(data, 0);
                Vector3 direction = ReadStruct<Vector3>(data, UnsafeUtility.SizeOf<Vector3>());
                
                bool hit = Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance, layerMask, (QueryTriggerInteraction)queryTriggerInteraction);
                WriteStruct(data, ref hitInfo, 0); // Write the out parameter
                
                return hit ? 1 : 0; // Return 1 for true, 0 for false
            });
        }
    }
}