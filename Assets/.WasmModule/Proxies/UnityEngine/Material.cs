using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public class Material(long id) : Object(id) {
	public string[] shaderKeywords {
		get => internal_get_shaderKeywords(WrappedId);
		set => internal_set_shaderKeywords(WrappedId, value);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe string[] internal_get_shaderKeywords(long wrappedId) {
		// i would really like to put keywords first in the param list but somehow if i do that it will always be 0. how
		UnityEngineMaterial__get__shaderKeywords(wrappedId, out int* lengths, out char** keywords, out int length);

		string[] ret = new string[length];
		
		for (int i = 0; i < length; i++) {
			ret[i] = new string(keywords![i], 0, lengths![i]);
			Marshal.FreeHGlobal((IntPtr)keywords![i]);
		}
		Marshal.FreeHGlobal((IntPtr)keywords);
		Marshal.FreeHGlobal((IntPtr)lengths);

		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_set_shaderKeywords(long wrappedId, string[] value) {
		int length = value.Length;
		long* keywords = (long*)Marshal.AllocHGlobal(length * sizeof(long));
		int* lengths = (int*)Marshal.AllocHGlobal(length * sizeof(int));
		
		for (int i = 0; i < length; i++) {
			keywords![i] = (long)Marshal.StringToHGlobalUni(value[i]);
			lengths![i] = value[i].Length;
		}
		
		UnityEngineMaterial__set__shaderKeywords(wrappedId, (long)keywords, (long)lengths, length);

		for (int i = 0; i < length; i++) {
			value[i] = new string((char*)keywords![i], 0, lengths![i]);
			Marshal.FreeHGlobal((IntPtr)keywords![i]);
		}
		Marshal.FreeHGlobal((IntPtr)keywords);
		Marshal.FreeHGlobal((IntPtr)lengths);
	}
	
	[WasmImportLinkage, DllImport("UnityEngine")]
	private static extern unsafe void UnityEngineMaterial__get__shaderKeywords(long wrappedId, out int* shaderKeywordsPointerPointerLengthsPointer, out char** shaderKeywordsPointerPointerPointer, out int shaderKeywordsLengthsPointer);
	
	[WasmImportLinkage, DllImport("UnityEngine")]
	private static extern void UnityEngineMaterial__set__shaderKeywords(long wrappedId, long shaderKeywordsPointerPointer, long shaderKeywordsPointerPointerLengths, int shaderKeywordsLength);
}