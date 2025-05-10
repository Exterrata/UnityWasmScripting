using WasmScripting.UnityEngine;
using Wasmtime;

namespace WasmScripting {
	public static class BindingManager {
		public static void BindMethods(Linker linker) {
			WasiStubs.DefineWasiFunctions(linker);
			DebugBindings.BindMethods(linker);
			ObjectBindings.BindMethods(linker);
			GameObjectBindings.BindMethods(linker);
			ComponentBindings.BindMethods(linker);
			TransformBindings.BindMethods(linker);
			ColliderBindings.BindMethods(linker);
			PhysicsBindings.BindMethods(linker);
			ColliderBindings.BindMethods(linker);
			RaycastHitBindings.BindMethods(linker);
			RendererBindings.BindMethods(linker);
		}

		/// <summary>
		/// Fill all the non-linked functions with empty stubs so they won't explode
		/// Note: If the function returns a bool, it will be false (which is amazing)
		/// </summary>
		public static void FillNonLinkedWithEmptyStubs(this Linker linker, Store store, Module module) {
			foreach (Import import in module.Imports) {
				// Ignore non-function types
				if (import is not FunctionImport funcImport)
					continue;

				string moduleName = import.ModuleName;
				string functionName = import.Name;

				// Skip if already defined in the linker
				if (linker.GetFunction(store, moduleName, functionName) == null)
					linker.DefineFunction(moduleName, functionName, (_, _, _) => { }, funcImport.Parameters, funcImport.Results);
			}
		}
	}
}