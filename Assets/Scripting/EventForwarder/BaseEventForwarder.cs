using UnityEngine;

namespace WasmScripting.Proxies {
	/// <summary>
	/// Event forwarder component to forward expensive events to a target WasmBehaviour.
	/// This is added alongside the WasmBehaviour only when the user script requests it.
	/// </summary>
	public abstract class BaseEventForwarder : MonoBehaviour {
		public WasmRuntimeBehaviour targetRuntimeBehaviour;

		public static T Create<T>(GameObject targetGO, WasmRuntimeBehaviour targetBehaviour)
			where T : BaseEventForwarder {
			T forwarder = targetGO.AddComponent<T>();
			forwarder.targetRuntimeBehaviour = targetBehaviour;
			return forwarder;
		}
	}
}