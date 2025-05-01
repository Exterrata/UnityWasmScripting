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
    
    private static string internal_object_name_get(long id) {
        object_name_get(id);
        return ReadString();
    }

    private static void internal_object_name_set(long id, string name) {
        WriteString(name);
        object_name_set(id);
    }

    private static string internal_object_toString(long id) {
        object_toString(id);
        return ReadString();
    }
    
    #endregion Marshaling

    #region Imports
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_name_get(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_name_set(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_toString(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_destroy(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void object_instantiate(long id);
    
    #endregion Imports
}