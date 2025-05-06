namespace WasmScripting.Proxies
{
    public class OnRenderObjectForwarder : BaseEventForwarder
    {
        private void OnRenderObject()
            => targetBehaviour.ForwardedOnRenderObject();
    }
}