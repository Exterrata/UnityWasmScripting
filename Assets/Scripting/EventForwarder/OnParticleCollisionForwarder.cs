using UnityEngine;

namespace WasmScripting.Proxies
{
    public class OnParticleCollisionForwarder : BaseEventForwarder
    {
        private void OnParticleCollision(GameObject other)
            => targetRuntimeBehaviour.ForwardedOnParticleCollision(other);
    }
}
