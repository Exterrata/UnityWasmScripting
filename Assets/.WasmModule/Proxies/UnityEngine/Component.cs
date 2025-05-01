using System.Runtime.InteropServices;

namespace UnityEngine;

public class Component(long id) : Object(id) 
{
    #region Implementation
    
    public GameObject gameObject => new(component_gameObject_get(id));
    public Transform transform => new(component_transform_get(id));
    
    public string tag
    {
        get => internal_component_tag_get(WrappedId);
        set => internal_component_tag_set(WrappedId, value);
    }

    public Component GetComponent(string component) => internal_component_getcomponent_string(WrappedId, component);

    public T GetComponent<T>() where T : Component
    {
        string componentType = typeof(T).Name; // TODO: handle case where T inherits WasmBehaviour, needs custom lookup
        return GetComponent(componentType) as T;
    }
    
    #endregion Implementation

    #region Marshaling

    private static string internal_component_tag_get(long id) {
        component_tag_get(id);
        return ReadString();
    }

    private static void internal_component_tag_set(long id, string name) {
        WriteString(name);
        component_tag_set(id);
    }

    private static Component internal_component_getcomponent_string(long id, string componentType)
    {
        WriteString(componentType);
        return new(component_func_getcomponent_string(id));
    }

    #endregion Marshaling

    #region Imports
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_gameObject_get(long id);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_transform_get(long id);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern void component_tag_get(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void component_tag_set(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_func_getcomponent_string(long id);

    #endregion Imports
}