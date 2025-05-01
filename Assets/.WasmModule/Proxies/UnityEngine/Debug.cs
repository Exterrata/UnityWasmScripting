using System.Runtime.InteropServices;

namespace UnityEngine;

public static class Debug 
{
	#region Implementation
	
	public static void Log(object obj) {
		WriteString(obj.ToString()!);
		debug_log();
	}
	
	public static void LogWarning(object obj) {
		WriteString(obj.ToString()!);
		debug_logWarning();
	}
	
	public static void LogError(object obj) {
		WriteString(obj.ToString()!);
		debug_logError();
	}
	
	#endregion Implementation

	#region Imports

	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_log();
    
	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_logWarning();
    
	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_logError();

	#endregion Imports
}