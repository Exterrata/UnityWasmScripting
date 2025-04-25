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
		}
	}
}