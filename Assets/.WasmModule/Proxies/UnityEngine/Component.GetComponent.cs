// ReSharper disable UnusedTypeParameter
// ReSharper disable MemberCanBePrivate.Global

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

// In own file because I do not believe this can be automatically generated.

public partial class Component
{
    #region Implementation

    #region GetComponent

    public Component GetComponent(string component) => internal_component_getcomponent_string(WrappedId, component);
    
    public Component GetComponent(Type type) => internal_component_getcomponent_string(WrappedId, type.Name);
    
    public T GetComponent<T>() where T : Component => internal_component_getcomponent_string(WrappedId, typeof(T).Name) as T;

    #endregion GetComponent

    #region TryGetComponent
    
    public bool TryGetComponent<T>(out T component) where T : Component
    {
        component = GetComponent<T>();
        return component != null;
    }
    
    public bool TryGetComponent(Type type, out Component component)
    {
        component = GetComponent(type.FullName);
        return component != null;
    }

    #endregion TryGetComponent

    #region GetComponentInChildren

    public Component GetComponentInChildren(Type t, bool includeInactive) => internal_component_getcomponentinchildren_string(WrappedId, t.FullName, includeInactive);

    public Component GetComponentInChildren(Type t) => internal_component_getcomponentinchildren_string(WrappedId, t.FullName, false);

    public T GetComponentInChildren<T>(bool includeInactive) where T : Component => internal_component_getcomponentinchildren_string(WrappedId, typeof(T).FullName, includeInactive) as T;

    public T GetComponentInChildren<T>() where T : Component => internal_component_getcomponentinchildren_string(WrappedId, typeof(T).FullName, false) as T;

    #endregion GetComponentInChildren
    
    #region GetComponentInParent

    public Component GetComponentInParent(Type t, bool includeInactive) => internal_component_getcomponentinparent_string(WrappedId, t.FullName, includeInactive);
    
    public Component GetComponentInParent(Type t) => internal_component_getcomponentinparent_string(WrappedId, t.FullName, false);
   
    public T GetComponentInParent<T>(bool includeInactive) where T : Component => internal_component_getcomponentinparent_string(WrappedId, typeof(T).FullName, includeInactive) as T;
   
    public T GetComponentInParent<T>() where T : Component => internal_component_getcomponentinparent_string(WrappedId, typeof(T).FullName, false) as T;

    #endregion GetComponentInParent
    
    #endregion Implementation

    #region Marshaling
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe Component internal_component_getcomponent_string(long id, string componentType) {
        fixed (char* str = componentType) 
        {
            long typeNamePtr = 0;
            long componentId = component_func_getcomponent_string
            (
                id, 
                (long)Unsafe.AsPointer(ref typeNamePtr),
                (long)str, 
                componentType.Length * sizeof(char)
            );
            
            if (componentId == 0 || typeNamePtr == 0) 
                return null;
            
            // Get the actual type name from the pointer
            string typeName = new((char*)typeNamePtr);
#pragma warning disable IL2057
            Type type = Type.GetType(typeName);
#pragma warning restore IL2057
            return type != null ? RuntimeHelpers.GetUninitializedObject(type) as Component : null;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe Component internal_component_getcomponentinchildren_string(long id, string componentType, bool includeInactive) {
        fixed (char* str = componentType) 
        {
            long typeNamePtr = 0;
            long componentId = component_func_getcomponentinchildren_string
            (
                id, 
                (long)Unsafe.AsPointer(ref typeNamePtr),
                (long)str, 
                componentType.Length * sizeof(char), 
                Unsafe.As<bool, int>(ref includeInactive)
            );
            
            if (componentId == 0 || typeNamePtr == 0) 
                return null;
            
            // Get the actual type name from the pointer
            string typeName = new((char*)typeNamePtr);
#pragma warning disable IL2057
            Type type = Type.GetType(typeName);
#pragma warning restore IL2057
            return type != null ? RuntimeHelpers.GetUninitializedObject(type) as Component : null;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe Component internal_component_getcomponentinparent_string(long id, string componentType, bool includeInactive) {
        fixed (char* str = componentType) 
        {
            long typeNamePtr = 0;
            long componentId = component_func_getcomponentinparent_string
            (
                id, 
                (long)Unsafe.AsPointer(ref typeNamePtr),
                (long)str, 
                componentType.Length * sizeof(char), 
                Unsafe.As<bool, int>(ref includeInactive)
            );

            if (componentId == 0 || typeNamePtr == 0)
                return null;
            
            // Get the actual type name from the pointer
            string typeName = new((char*)typeNamePtr);
#pragma warning disable IL2057
            Type type = Type.GetType(typeName);
#pragma warning restore IL2057
            return type != null ? RuntimeHelpers.GetUninitializedObject(type) as Component : null;
        }
    }

    #endregion Marshaling

    #region Imports
    
    // TODO: These names should be special because manually linked

    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_func_getcomponent_string(long id, long typeNamePtr, long strPtr, int strSize);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_func_getcomponentinchildren_string(long id, long typeNamePtr, long strPtr, int strSize, int includeInactive);
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern long component_func_getcomponentinparent_string(long id, long typeNamePtr, long strPtr, int strSize, int includeInactive);
    
    #endregion Imports
}