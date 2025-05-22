using System;
using System.Text;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class Examples : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("UnityEngine", "ArrayWriteBackExample",
				(Caller caller, long stringsPointerPointerLengths, long stringsPointerPointer, int stringsLength) => {
					StoreData data = GetData(caller);
					Span<long> stringPointers = data.Memory.GetSpan<long>(stringsPointerPointer, stringsLength);
					Span<int> stringLengths = data.Memory.GetSpan<int>(stringsPointerPointerLengths, stringsLength);

					string[] strings = new string[stringsLength];

					for (int i = 0; i < stringsLength; i++) {
						strings[i] = data.Memory.ReadString(stringPointers[i], stringLengths[i], Encoding.Unicode);
					}

					strings[0] = "modified element";

					for (int i = 0; i < stringsLength; i++) {
						string str = strings[i];
						int length = str.Length;
						long address = data.Alloc(length * sizeof(char));
						data.Memory.WriteString(address, str, Encoding.Unicode);
						stringPointers[i] = address;
						stringLengths[i] = length;
					}
				}
			);
			
			linker.DefineFunction("UnityEngine", "ArrayWriteBackResizeExample",
				(Caller caller, long stringsPointerPointerLengthsPointer, long stringsPointerPointerPointer, long stringsLengthPointer) => {
					StoreData data = GetData(caller);
					
					int stringsLength = data.Memory.ReadInt32(stringsLengthPointer);
					long stringsPointerPointer = data.Memory.ReadInt64(stringsPointerPointerPointer);
					long stringsPointerPointerLengths = data.Memory.ReadInt64(stringsPointerPointerLengthsPointer);
					
					Span<long> stringPointers = data.Memory.GetSpan<long>(stringsPointerPointer, stringsLength);
					Span<int> stringLengths = data.Memory.GetSpan<int>(stringsPointerPointerLengths, stringsLength);
					
					string[] strings = new string[stringsLength];

					for (int i = 0; i < stringsLength; i++) {
						strings[i] = data.Memory.ReadString(stringPointers[i], stringLengths[i], Encoding.Unicode);
					}
					
					MethodThatModifiesArray(ref strings);

					stringsLength = strings.Length;
					long newStringsAddress = data.Alloc(stringsLength * sizeof(long));
					long newLengthsAddress = data.Alloc(stringsLength * sizeof(int));
					
					Span<long> newStringPointers = data.Memory.GetSpan<long>(newStringsAddress, stringsLength);
					Span<int> newStringLengths = data.Memory.GetSpan<int>(newLengthsAddress, stringsLength);
					
					for (int i = 0; i < stringsLength; i++) {
						string str = strings[i];
						int length = str.Length;
						long address = data.Alloc(length * sizeof(char));
						data.Memory.WriteString(address, str, Encoding.Unicode);
						newStringPointers[i] = address;
						newStringLengths[i] = length;
					}
					
					data.Memory.WriteInt32(stringsLengthPointer, stringsLength);
					data.Memory.WriteInt64(stringsPointerPointerPointer, newStringsAddress);
					data.Memory.WriteInt64(stringsPointerPointerLengthsPointer, newLengthsAddress);
				}
			);
		}

		private static void MethodThatModifiesArray(ref string[] strings) {
			Array.Resize(ref strings, strings.Length + 1);
			strings[^1] = "added element";
		}
	}
}