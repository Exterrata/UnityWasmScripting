using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using Wasmtime;

namespace WasmScripting {
	public class WasmPassthroughStack {
		// NOTE: this stack implementation should be reworked.
		// unity side should not have to ask wasm side for the current stack pointer, and vice versa.
		
		// NOTE: it will probably be better to remove stack entirely and directly pass pointers to allocated objects to bindings.
		// it does mean manual memory management, but it should be fine if bindings are auto generated.
		// it may also be more compatible with other languages.
		
		private readonly Func<long> _incrementStackPointer;
		private readonly Func<long> _decrementStackPointer;
		private readonly Func<int, int> _allocMethod;
		private readonly Action<int> _freeMethod;
		private readonly Memory _memory;

		public WasmPassthroughStack(Instance instance) {
			instance.GetAction("scripting_alloc_passthrough_stack")!();
			_incrementStackPointer = instance.GetFunction<long>("scripting_increment_stack_pointer");
			_decrementStackPointer = instance.GetFunction<long>("scripting_decrement_stack_pointer");
			_allocMethod = instance.GetFunction<int, int>("scripting_alloc")!;
			_freeMethod = instance.GetAction<int>("scripting_free")!;
			_memory = instance.GetMemory("memory");
		}

		public void Push<T>(ref T obj) where T : unmanaged {
			int size = UnsafeUtility.SizeOf<T>();
			int address = _allocMethod(size);
			_memory.Write(address, obj);
			_memory.WriteInt32(_incrementStackPointer(), address);
		}

		public T Pop<T>() where T : unmanaged {
			int address = _memory.ReadInt32(_decrementStackPointer());
			T obj = _memory.Read<T>(address);
			_freeMethod(address);
			return obj;
		}

		public void Push(string str) {
			int length = str.Length * sizeof(char);
			int address = _allocMethod(length);
			_memory.WriteInt32(_incrementStackPointer(), address);
			_memory.WriteInt32(_incrementStackPointer(), length);
			_memory.WriteString(address, str, Encoding.Unicode);
		}
		
		public string Pop() {
			int length = _memory.ReadInt32(_decrementStackPointer());
			int address = _memory.ReadInt32(_decrementStackPointer());
			string str = _memory.ReadString(address, length, Encoding.Unicode);
			_freeMethod(address);
			return str;
		}
	}
}