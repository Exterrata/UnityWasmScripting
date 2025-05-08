namespace WasmScripting.Proxies
{
    public class OnAnimatorMoveForwarder : BaseEventForwarder
    {
        private void OnAnimatorMove()
            => targetRuntimeBehaviour.ForwardedOnAnimatorMove();
    }
}
