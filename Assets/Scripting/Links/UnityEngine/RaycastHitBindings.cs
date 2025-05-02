using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
    public class RaycastHitBindings : WasmBinding
    {
        public static void BindMethods(Linker linker)
        {
            linker.DefineFunction("unity", "raycast_get_collider", (Caller caller, long raycastHitPtr) =>
            {
                StoreData data = GetData(caller);
                RaycastHit hit = data.Memory.Read<RaycastHit>(raycastHitPtr);
                return IdFrom(data, hit.collider);
            });
        }
    }
}