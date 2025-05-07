namespace WasmScripting.Proxies
{
    public class OnAnimatorIKForwarder : BaseEventForwarder
    {
        private void OnAnimatorIK(int layerIndex) =>
            targetRuntimeBehaviour.ForwardedOnAnimatorIK(layerIndex);
    }
}
