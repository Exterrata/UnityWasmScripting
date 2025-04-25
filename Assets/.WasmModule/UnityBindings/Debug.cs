using System.Runtime.InteropServices;

namespace UnityEngine;
public static class Debug {
	public static void Log(object obj) {
		WriteString(obj.ToString()!, 0);
		debug_log();
	}
	
	public static void LogWarning(object obj) {
		WriteString(obj.ToString()!, 0);
		debug_logWarning();
	}
	
	public static void LogError(object obj) {
		WriteString(obj.ToString()!, 0);
		debug_logError();
	}
    
	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_log();
    
	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_logWarning();
    
	[WasmImportLinkage, DllImport("unity")]
	private static extern void debug_logError();
}