using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WasmModule.Proxies;

namespace UnityEngine;

public class Object(long id) : IProxyObject {
	public long WrappedId { get; set; } = id;

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
		char* outString = default;
		int outSize = default;

		object_name_get(id, (long)&outString, (long)&outSize);
		string str = new(outString, 0, outSize);
		Marshal.FreeHGlobal((IntPtr)outString);
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
		char* strPtr = default;
		int strSize = default;

		object_toString(id, (long)&strPtr, (long)&strSize);
		string str = new(strPtr, 0, strSize);
		Marshal.FreeHGlobal((IntPtr)strPtr);
		return str;
	}

	#endregion Marshaling

	#region Imports

	[WasmImportLinkage, DllImport("unity")]
	private static extern void object_name_get(long id, long outString, long outSize);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void object_name_set(long id, long strPtr, int strSize);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void object_toString(long id, long outString, long outSize);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void object_destroy(long id);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void object_instantiate(long id);

	#endregion Imports
}