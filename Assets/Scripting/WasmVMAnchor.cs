using UnityEngine;

namespace WasmScripting {
	public class WasmVMAnchor : MonoBehaviour {
		public WasmVMContext context;
		public WasmModuleAsset moduleAsset;
		
		private void Start() {
			WasmVM vm = gameObject.AddComponent<WasmVM>();

			if (context == WasmVMContext.Scene) SetupScene();
			else SetupGameObject();
		}

		private void SetupScene() {
			
		}

		private void SetupGameObject() {
			WasmRuntimeBehaviour[] behaviours = GetComponentsInChildren<WasmRuntimeBehaviour>(true);
		}
	}

	public enum WasmVMContext {
		GameObject, // Avatars, Spawnables
		Scene		// World (entire scene)
	}
}