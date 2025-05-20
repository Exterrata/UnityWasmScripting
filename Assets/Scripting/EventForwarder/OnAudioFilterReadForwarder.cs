namespace WasmScripting.Proxies {
	public class OnAudioFilterReadForwarder : BaseEventForwarder {
		private void OnAudioFilterRead(float[] data, int channels) => targetRuntimeBehaviour.ForwardedOnAudioFilterRead(data, channels);
	}
}