using UnityEngine;

namespace WasmScripting.Proxies
{
    public class OnTriggerStayForwarder : BaseEventForwarder
    {
        private void OnTriggerStay(Collider other)
            => targetBehaviour.ForwardedOnTriggerStay(other);
    }
}