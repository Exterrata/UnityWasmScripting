using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WasmModule.Proxies.UnityEngine;

public class Examples
{
	public static void ArrayWriteBack(string[] strings) => internal_ArrayWriteBack(strings);

	public static void ArrayWriteBackResize(string[] strings) => internal_ArrayWriteBackResize(strings);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_ArrayWriteBack(string[] strings)
	{
		int length = strings.Length;
		char** originalStrings = (char**)Marshal.AllocHGlobal(length * sizeof(long));
		char** unmanagedStrings = (char**)Marshal.AllocHGlobal(length * sizeof(long));
		int* unmanagedLengths = (int*)Marshal.AllocHGlobal(length * sizeof(int));

		for (int i = 0; i < length; i++)
		{
			unmanagedStrings![i] = (char*)Marshal.StringToHGlobalUni(strings[i]);
			unmanagedLengths![i] = strings[i].Length;
		}

		Buffer.MemoryCopy(unmanagedStrings, originalStrings, length * sizeof(long), length * sizeof(long));

		ArrayWriteBackResizeExample((long)unmanagedLengths, (long)unmanagedStrings, length);

		for (int i = 0; i < length; i++)
		{
			strings[i] = new(unmanagedStrings![i], 0, unmanagedLengths![i]);
			Marshal.FreeHGlobal((IntPtr)unmanagedStrings![i]);
			Marshal.FreeHGlobal((IntPtr)originalStrings![i]);
		}
		Marshal.FreeHGlobal((IntPtr)unmanagedStrings);
		Marshal.FreeHGlobal((IntPtr)unmanagedLengths);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_ArrayWriteBackResize(string[] strings)
	{
		int length = strings.Length;
		char** unmanagedStrings = (char**)Marshal.AllocHGlobal(length * sizeof(long));
		int* unmanagedLengths = (int*)Marshal.AllocHGlobal(length * sizeof(int));

		char** originalStrings = unmanagedStrings;
		int* originalLengths = unmanagedLengths;
		int originalLength = length;

		for (int i = 0; i < length; i++)
		{
			unmanagedStrings![i] = (char*)Marshal.StringToHGlobalUni(strings[i]);
			unmanagedLengths![i] = strings[i].Length;
		}

		ArrayWriteBackResizeExample((long)&unmanagedLengths, (long)&unmanagedStrings, (long)&length);

		for (int i = 0; i < originalLength; i++)
		{
			Marshal.FreeHGlobal((IntPtr)originalStrings![i]);
		}
		Marshal.FreeHGlobal((IntPtr)originalStrings);
		Marshal.FreeHGlobal((IntPtr)originalLengths);

		Array.Resize(ref strings, length);
		for (int i = 0; i < length; i++)
		{
			strings[i] = new(unmanagedStrings![i], 0, unmanagedLengths![i]);
			Marshal.FreeHGlobal((IntPtr)unmanagedStrings![i]);
		}
		Marshal.FreeHGlobal((IntPtr)unmanagedStrings);
		Marshal.FreeHGlobal((IntPtr)unmanagedLengths);
	}

	[WasmImportLinkage, DllImport("UnityEngine")]
	private static extern void ArrayWriteBackExample(long stringsPointerPointerLengths, long stringsPointerPointer, int stringsLength);

	[WasmImportLinkage, DllImport("UnityEngine")]
	private static extern void ArrayWriteBackResizeExample(long stringsPointerPointerLengthsPointer, long stringsPointerPointerPointer, long stringsLengthPointer);
}
