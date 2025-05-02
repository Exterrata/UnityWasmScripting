using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WasmModule.Proxies;

namespace UnityEngine;

public class Object(long id) : ProxyObject(id)
{
    #region Implementation
    
    public string name {
        get => internal_object_name_get(WrappedId);
        set => internal_object_name_set(WrappedId, value);
    }

    public override string ToString() => internal_object_toString(WrappedId);

    public static void Destroy(Object obj) => object_destroy(obj.WrappedId);
    public static void Instantiate(Object obj) => object_instantiate(obj.WrappedId);
    
    #endregion Implementation

    #region Marshaling
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe string internal_object_name_get(long id) {
        long strPtr = object_name_get(id);
        string str = new((char*)strPtr);
        Marshal.FreeHGlobal((IntPtr)strPtr);
        return str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void internal_object_name_set(long id, string name) {
        fixed (char* str = name) {
            object_name_set(id, (long)str, name.Length * sizeof(char));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe string internal_object_toString(long id) {
        long strPtr = object_toString(id);
        string str = new((char*)strPtr);
        Marshal.FreeHGlobal((IntPtr)strPtr);
        return str;
    }
    
    #endregion Marshaling

    #region Imports
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long object_name_get(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_name_set(long id, long strPtr, int strSize);

    [WasmImportLinkage, DllImport("unity")]
    private static extern long object_toString(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_destroy(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_instantiate(long id);
    
    #endregion Imports
}