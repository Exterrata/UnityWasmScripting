using System.Runtime.InteropServices;

namespace UnityEngine;

public static class Debug {
	#region Implementation

	public static unsafe void Log(object obj) {
		string str = obj.ToString();
		fixed (char* chr = str) {
			debug_log((long)chr, str.Length * sizeof(char));
		}
	}

	public static unsafe void LogWarning(object obj) {
		string str = obj.ToString();
		fixed (char* chr = str) {
			debug_logWarning((long)chr, str.Length * sizeof(char));
		}
	}

	public static unsafe void LogError(object obj) {
		string str = obj.ToString();
		fixed (char* chr = str) {
			debug_logError((long)chr, str.Length * sizeof(char));
		}
	}

	#endregion Implementation

	#region Imports

	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_log(long strPtr, int strSize);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_logWarning(long strPtr, int strSize);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_logError(long strPtr, int strSize);

	#endregion Imports
}