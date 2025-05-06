using System;
using System.Collections.Generic;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class RendererBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "renderer_getSharedMaterials", (Caller caller, long id, long outMaterialIds, long outMaterialsLength) => {
				StoreData data = GetData(caller);
				
				List<Material> materials = new();
				Renderer renderer = IdTo<Renderer>(data, id);
				renderer.GetSharedMaterials(materials);
				
				int length = materials.Count;
				int size = materials.Count * sizeof(long);
				
				long address = data.Alloc(size);
				Span<long> span = data.Memory.GetSpan<long>(address, size);
				
				for (int i = 0; i < length; i++) {
					span[i] = IdFrom(data, materials[i]);
				}
				
				data.Memory.WriteInt64(outMaterialIds, address);
				data.Memory.WriteInt32(outMaterialsLength, length);
			});
		}
	}
}