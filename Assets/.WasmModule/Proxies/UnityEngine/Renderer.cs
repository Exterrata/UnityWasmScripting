using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public class Renderer(long id) : Component(id) {
	#region Implementation

	public Material[] sharedMaterials {
		get => internal_Renderer_get_SharedMaterials(WrappedId);
		set => internal_Renderer_set_SharedMaterials(WrappedId, value);
	}

	public void GetSharedMaterials(List<Material> materials) => internal_Renderer_GetSharedMaterials(WrappedId, materials);

	#endregion Implementation

	#region Marshaling

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_Renderer_set_SharedMaterials(long wrappedId, Material[] materials) {
		int length = materials.Length;
		long[] materialIds = new long[length];

		for (int i = 0; i < length; i++) {
			materialIds[i] = materials[i].WrappedId;
		}

		fixed (long* ptrMaterialIds = materialIds) {
			Renderer_set_SharedMaterials(wrappedId, (long)ptrMaterialIds, length);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe Material[] internal_Renderer_get_SharedMaterials(long wrappedId) {
		long* materialIds = default;
		int length = default;

		Renderer_get_SharedMaterials(wrappedId, (long)&materialIds, (long)&length);

		Material[] materials = new Material[length];
		for (int i = 0; i < length; i++) {
			materials[i] = new(materialIds![i]);
		}

		Marshal.FreeHGlobal((IntPtr)materialIds);
		return materials;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_Renderer_GetSharedMaterials(long wrappedId, List<Material> materials) {
		long* materialIds = default;
		int length = default;

		Renderer_func_GetSharedMaterials(wrappedId, (long)&materialIds, (long)&length);

		materials.Clear();
		materials.Capacity = length;
		for (int i = 0; i < length; i++) {
			materials.Add(new(materialIds[i]));
		}

		Marshal.FreeHGlobal((IntPtr)materialIds);
	}

	#endregion Marshaling

	#region Imports

	[WasmImportLinkage, DllImport("unity")]
	private static extern void Renderer_set_SharedMaterials(long id, long ptrMaterialIds, int materialsLength);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void Renderer_get_SharedMaterials(long id, long outMaterialIds, long outMaterialsLength);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void Renderer_func_GetSharedMaterials(long id, long outMaterialIds, long outMaterialsLength);

	#endregion Imports
}