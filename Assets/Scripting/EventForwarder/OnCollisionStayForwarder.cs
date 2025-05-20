using UnityEngine;

namespace WasmScripting.Proxies {
	public class OnCollisionStayForwarder : BaseEventForwarder {
		private void OnCollisionStay(Collision collision) => targetRuntimeBehaviour.ForwardedOnCollisionStay(collision);
	}
}