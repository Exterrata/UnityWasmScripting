using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public class GameObject(long id) : Object(id) 
{
	#region Implementation
	
	public GameObject() : this(gameObject_ctor0()) {}
	public GameObject(string name) : this(internal_gameObject_ctor1(name)) {}

	public bool activeInHierarchy => gameObject_activeInHierarchy_get(WrappedId) != 0;
	public bool activeSelf => gameObject_activeSelf_get(WrappedId) != 0;
	public Transform transform => new(gameObject_transform_get(WrappedId));
	//public Scene scene => new(gameObject_scene_get(ObjectId));
	public ulong sceneCullingMask => internal_gameObject_sceneCullingMask_get(WrappedId);

	public bool isStatic {
		get => gameObject_isStatic_get(WrappedId) != 0;
		set => gameObject_isStatic_set(WrappedId, value ? 1 : 0);
	}
	
	public int layer {
		get => gameObject_layer_get(WrappedId);
		set => gameObject_layer_set(WrappedId, value);
	}
	
	public string tag {
		get => internal_gameObject_tag_get(WrappedId);
		set => internal_gameObject_tag_set(WrappedId, value);
	}
	
	#endregion Implementation
	
	#region Marshaling

	private static long internal_gameObject_ctor1(string name) {
		WriteString(name, 0);
		return gameObject_ctor1();
	}

	private static string internal_gameObject_tag_get(long objectId) {
		gameObject_tag_get(objectId);
		return ReadString(0);
	}

	private static void internal_gameObject_tag_set(long objectId, string tag) {
		WriteString(tag, 0);
		gameObject_tag_set(objectId);
	}

	private static ulong internal_gameObject_sceneCullingMask_get(long objectId) {
		long sceneCullingMask = gameObject_sceneCullingMask_get(objectId);
		return Unsafe.As<long, ulong>(ref sceneCullingMask);
	}
	
	#endregion Marshaling

	#region Imports
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern long gameObject_ctor0();
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern long gameObject_ctor1();
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern int gameObject_activeInHierarchy_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern int gameObject_activeSelf_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern int gameObject_isStatic_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern void gameObject_isStatic_set(long id, int isStatic);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern int gameObject_layer_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern void gameObject_layer_set(long id, int layer);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern long gameObject_scene_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern long gameObject_sceneCullingMask_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern void gameObject_tag_get(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern void gameObject_tag_set(long id);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern long gameObject_transform_get(long id);
	
	#endregion Imports
}