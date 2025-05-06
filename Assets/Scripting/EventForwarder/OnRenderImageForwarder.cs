using UnityEngine;

namespace WasmScripting.Proxies
{
    public class OnRenderImageForwarder : BaseEventForwarder
    {
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
            => targetBehaviour.ForwardedOnRenderImage(source, destination);
    }
}