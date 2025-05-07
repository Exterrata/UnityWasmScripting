using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]
namespace WasmScripting {
	[PublicAPI]
	[DefaultExecutionOrder(-50)]
	public class WasmVMAnchor : MonoBehaviour {
		internal WasmRuntimeBehaviour[] Behaviours;
		internal WasmVMContext Context;
		
		public WasmModuleAsset moduleAsset;
		
		// this would be done by the asset loading pipeline
		private void Awake() {
			Context = WasmVMContext.GameObject;
			Behaviours = Context == WasmVMContext.Scene ? FindObjectsOfType<WasmRuntimeBehaviour>(true) : GetComponentsInChildren<WasmRuntimeBehaviour>(true);
			Setup();
		}

		internal void Setup() {
			WasmVM vm = gameObject.AddComponent<WasmVM>();

			foreach (WasmRuntimeBehaviour behaviour in Behaviours) {
				behaviour.InstanceId = behaviour.GetInstanceID();
				behaviour.VM = vm;
			}
			
			vm.Setup(moduleAsset, Behaviours);
			
			Destroy(this);
		}
	}

	public enum WasmVMContext {
		GameObject, // Avatars, Spawnables
		Scene		// World (entire scene)
	}
}