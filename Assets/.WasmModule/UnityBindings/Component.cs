using System.Runtime.InteropServices;

namespace UnityEngine;
public class Component(long id) : Object(id) {

    public GameObject gameObject => new(component_gameObject_get(id));
    public Transform transform => new(component_transform_get(id));
    
    public string tag {
        get => internal_component_tag_get(ObjectId);
        set => internal_component_tag_set(ObjectId, value);
    }

    private static string internal_component_tag_get(long id) {
        component_tag_get(id);
        return ReadString(0);
    }

    private static void internal_component_tag_set(long id, string name) {
        WriteString(name, 0);
        component_tag_set(id);
    }
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_gameObject_get(long id);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_transform_get(long id);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern void component_tag_get(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void component_tag_set(long id);
}