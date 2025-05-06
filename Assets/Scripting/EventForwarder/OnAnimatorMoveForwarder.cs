namespace WasmScripting.Proxies
{
    public class OnAnimatorMoveForwarder : BaseEventForwarder
    {
        private void OnAnimatorMove()
            => targetBehaviour.ForwardedOnAnimatorMove();
    }
}