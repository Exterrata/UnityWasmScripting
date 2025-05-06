using UnityEngine;

namespace WasmScripting.Proxies
{
    public class OnCollisionStay2DForwarder: BaseEventForwarder
    {
        private void OnCollisionStay2D(Collision2D other)
            => targetRuntimeBehaviour.ForwardedOnCollisionStay2D(other);
    }
}