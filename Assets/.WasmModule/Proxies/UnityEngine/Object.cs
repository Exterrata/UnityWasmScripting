using System.Runtime.InteropServices;

namespace UnityEngine;
public class Object(long id) {
    protected readonly long ObjectId = id;

    public string name {
        get => internal_object_name_get(ObjectId);
        set => internal_object_name_set(ObjectId, value);
    }

    public override string ToString() => internal_object_toString(ObjectId);

    public static void Destroy(Object obj) => object_destroy(obj.ObjectId);
    public static void Instantiate(Object obj) => object_instantiate(obj.ObjectId);

    private static string internal_object_name_get(long id) {
        object_name_get(id);
        return ReadString(0);
    }

    private static void internal_object_name_set(long id, string name) {
        WriteString(name, 0);
        object_name_set(id);
    }

    private static string internal_object_toString(long id) {
        object_toString(id);
        return ReadString(0);
    }
    
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
}