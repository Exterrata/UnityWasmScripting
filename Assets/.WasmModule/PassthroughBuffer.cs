global using static WasmModule.PassthroughBuffer;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WasmModule;
public static unsafe class PassthroughBuffer {
	private const int BufferSize = 65536;
	private static IntPtr* _bufferBase;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteStruct<T>(T obj, int offsetBytes) where T : struct => Unsafe.Write(_bufferBase + offsetBytes, obj);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ReadStruct<T>(int offsetBytes) where T : struct => Unsafe.Read<T>(_bufferBase + offsetBytes);

	public static void WriteString(string str, int offsetBytes) {
		int size = str.Length * sizeof(char);
		if (offsetBytes + size > BufferSize) throw new WasmBufferOverflowException();
		*(_bufferBase + offsetBytes++) = size;
		fixed (char* src = str) Buffer.MemoryCopy(src, _bufferBase + offsetBytes, size, size);
	}
	
	public static string ReadString(int offsetBytes) {
		int size = (int)*(_bufferBase + offsetBytes++);
		if (offsetBytes + size > BufferSize) throw new WasmBufferOverflowException();
		return new string((char*)(_bufferBase + offsetBytes), 0, size);
	}
	
	[UnmanagedCallersOnly(EntryPoint = "scripting_alloc_passthrough_buffer")]
	public static IntPtr* AllocPassthroughStack() => _bufferBase = (IntPtr*)Marshal.AllocHGlobal(BufferSize);

	public class WasmBufferOverflowException : Exception;
}