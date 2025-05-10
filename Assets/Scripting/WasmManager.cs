using UnityEngine;
using Wasmtime;

namespace WasmScripting
{
	[DefaultExecutionOrder(-100)]
	public class WasmManager : MonoBehaviour
	{
		public static Config Config { get; private set; }
		public static Engine Engine { get; private set; }
		public static Linker Linker { get; private set; }

		private void Awake()
		{
			Config = new Config().WithFuelConsumption(true).WithReferenceTypes(true).WithMultiValue(true);
			Engine = new(Config);
			Linker = new Linker(Engine);

			// Ideally there would be separate linkers for: Avatars, Props, and Worlds each with there own binding set.
			BindingManager.BindMethods(Linker);
		}

		public void OnDestroy()
		{
			Linker.Dispose();
			Engine.Dispose();
			Config.Dispose();
		}
	}
}
