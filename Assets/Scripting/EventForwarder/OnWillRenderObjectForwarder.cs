namespace WasmScripting.Proxies
{
	public class OnWillRenderObjectForwarder : BaseEventForwarder
	{
		private void OnWillRenderObject() => targetRuntimeBehaviour.ForwardedOnWillRenderObject();
	}
}
