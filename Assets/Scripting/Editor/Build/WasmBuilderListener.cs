using UnityEditor;

namespace WasmScripting
{
	[InitializeOnLoad]
	public static class WasmBuilderListener
	{
		static WasmBuilderListener()
		{
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		private static void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			//if (state == PlayModeStateChange.ExitingEditMode)
			//    WasmBuilder.CompileAllWasmPrograms();
		}
	}
}
