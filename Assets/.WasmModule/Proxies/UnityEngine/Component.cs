using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WasmModule;

namespace UnityEngine;

public class Component(long id) : Object(id)
{
	#region Implementation

	public GameObject gameObject => new(component_gameObject_get(WrappedId));
	public Transform transform => new(component_transform_get(WrappedId));

	public string tag
	{
		get => internal_component_tag_get(WrappedId);
		set => internal_component_tag_set(WrappedId, value);
	}

	public Component GetComponent(string name) => internal_component_getComponent_string(WrappedId, name);

	public Component GetComponent(Type type) => internal_component_getComponent_type(this);

	public T GetComponent<T>() where T : Component => internal_component_getComponent_T<T>(this);

	#endregion Implementation

	#region Marshaling

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe string internal_component_tag_get(long id)
	{
		long strPtr = component_tag_get(id);
		return new((char*)strPtr);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_component_tag_set(long id, string name)
	{
		fixed (char* str = name)
		{
			component_tag_set(id, (long)str, name.Length * sizeof(char));
		}
	}

	private static unsafe Component internal_component_getComponent_string(long wrappedId, string name)
	{
		int componentType = default;
		fixed (char* str = name)
		{
			long id = component_getComponent_string(wrappedId, (long)str, name.Length * sizeof(char), (long)&componentType);

			Component component = RuntimeHelpers.GetUninitializedObject(TypeMap.GetType(componentType)) as Component;
			component.WrappedId = id;

			return component;
		}
	}

	private static unsafe Component internal_component_getComponent_type(Component component) {
		int componentType = default;
		long id = component_getComponent_type(component.WrappedId, TypeMap.GetId(component.GetType()), (long)&componentType);

		Component ret = (Component)RuntimeHelpers.GetUninitializedObject(TypeMap.GetType(componentType));
		ret.WrappedId = id;

		return ret;
	}

	private static unsafe T internal_component_getComponent_T<T>(Component component) where T : Component {
		int componentType = default;
		long id = component_getComponent_type(component.WrappedId, TypeMap.GetId(component.GetType()), (long)&componentType);

		T ret = (T)RuntimeHelpers.GetUninitializedObject(TypeMap.GetType(componentType));
		ret.WrappedId = id;

		return ret;
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
	private static extern long component_getComponent_string(long wrappedId, long componentStr, int componentStrLength, long outComponentType);

	[WasmImportLinkage, DllImport("unity")]
	private static extern long component_getComponent_type(long wrappedId, int componentTypeId, long outComponentType);

	#endregion Imports
}
