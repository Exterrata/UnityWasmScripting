using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
    public class RaycastHitBindings : WasmBinding
    {
        public static void BindMethods(Linker linker)
        {
            linker.DefineFunction("unity", "raycast_get_collider", (Caller caller) =>
            {
                StoreData data = GetData(caller);
                RaycastHit hit = ReadStruct<RaycastHit>(data, 0);
                return IdFrom(data, hit.collider);
            });
        }
    }
}