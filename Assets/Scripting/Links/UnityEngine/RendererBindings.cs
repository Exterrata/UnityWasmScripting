using System;
using System.Collections.Generic;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
	public class RendererBindings : WasmBinding
	{
		public static void BindMethods(Linker linker)
		{
			linker.DefineFunction(
				"unity",
				"Renderer_func_GetSharedMaterials",
				(Caller caller, long id, long outMaterialIds, long outMaterialsLength) =>
				{
					StoreData data = GetData(caller);

					Renderer renderer = IdToClass<Renderer>(data, id);
					List<Material> materials = new();
					renderer.GetSharedMaterials(materials);

					int length = materials.Count;
					int size = materials.Count * sizeof(long);

					long address = data.Alloc(size);
					Span<long> span = data.Memory.GetSpan<long>(address, size);

					for (int i = 0; i < length; i++)
					{
						span[i] = IdFrom(data, materials[i]);
					}

					data.Memory.WriteInt64(outMaterialIds, address);
					data.Memory.WriteInt32(outMaterialsLength, length);
				}
			);

			linker.DefineFunction(
				"unity",
				"Renderer_get_SharedMaterials",
				(Caller caller, long id, long outMaterialIds, long outMaterialsLength) =>
				{
					StoreData data = GetData(caller);

					Renderer renderer = IdToClass<Renderer>(data, id);
					Material[] materials = renderer.sharedMaterials;

					int length = materials.Length;
					int size = length * sizeof(long);

					long address = data.Alloc(size);
					Span<long> span = data.Memory.GetSpan<long>(address, size);

					for (int i = 0; i < length; i++)
					{
						span[i] = IdFrom(data, materials[i]);
					}

					data.Memory.WriteInt64(outMaterialIds, address);
					data.Memory.WriteInt32(outMaterialsLength, length);
				}
			);

			linker.DefineFunction(
				"unity",
				"Renderer_set_SharedMaterials",
				(Caller caller, long id, long ptrMaterialIds, int materialsLength) =>
				{
					StoreData data = GetData(caller);

					Material[] materials = new Material[materialsLength];
					for (int i = 0; i < materialsLength; i++)
					{
						long materialsId = data.Memory.ReadInt64(ptrMaterialIds + i * sizeof(long));
						materials[i] = IdToClass<Material>(data, materialsId);
					}

					Renderer renderer = IdToClass<Renderer>(data, id);
					renderer.sharedMaterials = materials;
				}
			);
		}
	}
}
