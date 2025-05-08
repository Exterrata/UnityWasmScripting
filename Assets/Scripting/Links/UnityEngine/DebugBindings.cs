using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
    public class DebugBindings : WasmBinding
    {
        public static void BindMethods(Linker linker)
        {
            linker.DefineFunction(
                "unity",
                "debug_log",
                (Caller caller, long strPtr, int strSize) =>
                {
                    StoreData data = GetData(caller);
                    string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
                    Debug.Log(str);
                }
            );

            linker.DefineFunction(
                "unity",
                "debug_logWarning",
                (Caller caller, long strPtr, int strSize) =>
                {
                    StoreData data = GetData(caller);
                    string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
                    Debug.LogWarning(str);
                }
            );

            linker.DefineFunction(
                "unity",
                "debug_logError",
                (Caller caller, long strPtr, int strSize) =>
                {
                    StoreData data = GetData(caller);
                    string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
                    Debug.LogError(str);
                }
            );

            linker.DefineFunction(
                "unity",
                "debug_logException",
                (Caller caller, long strPtr, int strSize) =>
                {
                    StoreData data = GetData(caller);
                    string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
                    Debug.LogError(str);
                }
            );
        }
    }
}
