using System.Runtime.CompilerServices;
using System.Text;
using Wasmtime;

namespace WasmScripting {
	public class WasmPassthroughBuffer {
		private readonly long _bufferBase;
		private readonly Memory _memory;

		public WasmPassthroughBuffer(Instance instance) {
			_bufferBase = instance.GetFunction<long>("scripting_alloc_passthrough_buffer")!();
			_memory = instance.GetMemory("memory");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteStruct<T>(ref T obj, int offsetBytes) where T : unmanaged {
			_memory.Write(_bufferBase + offsetBytes, obj);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T ReadStruct<T>(int offsetBytes) where T : unmanaged {
			return _memory.Read<T>(_bufferBase + offsetBytes);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void WriteString(string str, int offsetBytes) {
			_memory.WriteInt32(_bufferBase + offsetBytes, str.Length * sizeof(char));
			_memory.WriteString(_bufferBase + offsetBytes + sizeof(int), str, Encoding.Unicode);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ReadString(int offsetBytes) {
			int length = _memory.ReadInt32(_bufferBase);
			return _memory.ReadString(_bufferBase + offsetBytes + sizeof(int), length, Encoding.Unicode);
		}
	}
}