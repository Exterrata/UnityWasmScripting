using System.Runtime.CompilerServices;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe string internal_component_tag_get(long id) {
        long strPtr = component_tag_get(id);
        return new((char*)strPtr);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void internal_component_tag_set(long id, string name) {
        fixed (char* str = name) {
            component_tag_set(id, (long)str, name.Length * sizeof(char));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe Component internal_component_getcomponent_string(long id, string componentType) {
        fixed (char* str = componentType) {
            return new(component_func_getcomponent_string(id, (long)str, componentType.Length * sizeof(char)));
        }
    }

    #endregion Marshaling

    #region Imports
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_gameObject_get(long id);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_transform_get(long id);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_tag_get(long id);

    [WasmImportLinkage, DllImport("unity")]
    private static extern void component_tag_set(long id, long strPtr, int strSize);

    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_func_getcomponent_string(long id, long strPtr, int strSize);

    #endregion Imports
}