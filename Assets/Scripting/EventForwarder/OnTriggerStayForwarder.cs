using UnityEngine;

namespace WasmScripting.Proxies
{
    public class OnTriggerStayForwarder : BaseEventForwarder
    {
        private void OnTriggerStay(Collider other)
            => targetRuntimeBehaviour.ForwardedOnTriggerStay(other);
    }
}
