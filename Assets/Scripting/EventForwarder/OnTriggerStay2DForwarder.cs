using UnityEngine;

namespace WasmScripting.Proxies
{
    public class OnTriggerStay2DForwarder : BaseEventForwarder
    {
        private void OnTriggerStay2D(Collider2D other)
            => targetBehaviour.ForwardedOnTriggerStay2D(other);
    }
}