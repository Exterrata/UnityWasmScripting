global using static WasmModule.PassthroughBuffer;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WasmModule;
public static unsafe class PassthroughBuffer {
	private const int BufferSize = 65536;
	private static IntPtr* _bufferBase;

	public static void WriteStruct<T>(T obj) where T : struct
	{
		int offsetBytes = 0;
		WriteStruct(obj, ref offsetBytes);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteStruct<T>(T obj, ref int offsetBytes) where T : struct
	{
		Unsafe.Write((byte*)_bufferBase + offsetBytes, obj);
		offsetBytes += Unsafe.SizeOf<T>();
	}

	public static T ReadStruct<T>() where T : struct
	{
		int offsetBytes = 0;
		return ReadStruct<T>(ref offsetBytes);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ReadStruct<T>(ref int offsetBytes) where T : struct
	{
		byte* readSource = (byte*)_bufferBase + offsetBytes;
		offsetBytes += Unsafe.SizeOf<T>();
		return Unsafe.Read<T>(readSource);
	}

	public static void WriteString(string str)
	{
		int offsetBytes = 0;
		WriteString(str, ref offsetBytes);
	}

	public static void WriteString(string str, ref int offsetBytes) {
		int size = str.Length * sizeof(char);
		if (offsetBytes + size > BufferSize) throw new WasmBufferOverflowException();
		*(_bufferBase + offsetBytes++) = size;
		fixed (char* src = str) Buffer.MemoryCopy(src, _bufferBase + offsetBytes, size, size);
		offsetBytes += size;
	}

	public static string ReadString()
	{
		int offsetBytes = 0;
		return ReadString(ref offsetBytes);
	}

	public static string ReadString(ref int offsetBytes) {
		int size = (int)*(_bufferBase + offsetBytes++);
		if (offsetBytes + size > BufferSize) throw new WasmBufferOverflowException();
		var src = (char*)(_bufferBase + offsetBytes);
		offsetBytes += size;
		return new string(src, 0, size / sizeof(char));
	}
	
	[UnmanagedCallersOnly(EntryPoint = "scripting_alloc_passthrough_buffer")]
	public static long AllocPassthroughStack() => (long)(_bufferBase = (IntPtr*)Marshal.AllocHGlobal(BufferSize));

	public class WasmBufferOverflowException : Exception;
}