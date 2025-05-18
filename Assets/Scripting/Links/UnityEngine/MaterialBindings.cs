using System;
using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
	public class MaterialBindings : WasmBinding
	{
		public static void BindMethods(Linker linker)
		{
			linker.DefineFunction(
				"UnityEngine",
				"UnityEngineMaterial__get__shaderKeywords",
				(Caller caller, long wrappedId, int shaderKeywordsPointerPointerLengthsPointer, int shaderKeywordsPointerPointerPointer, int shaderKeywordsLengthsPointer) =>
				{
					StoreData data = GetData(caller);
					Material selfObject = IdTo<Material>(data, wrappedId);
					string[] keywords = selfObject.shaderKeywords;

					int length = keywords.Length;
					long shaderKeywordsPointerPointer = data.Alloc(length * sizeof(long));
					long shaderKeywordsPointerPointerLengths = data.Alloc(length * sizeof(int));

					data.Memory.WriteInt64(shaderKeywordsPointerPointerPointer, shaderKeywordsPointerPointer);
					data.Memory.WriteInt64(shaderKeywordsPointerPointerLengthsPointer, shaderKeywordsPointerPointerLengths);
					data.Memory.WriteInt32(shaderKeywordsLengthsPointer, length);

					for (int i = 0; i < length; i++)
					{
						int length2 = keywords[i].Length;
						long address = data.Alloc(length2 * sizeof(char));
						data.Memory.WriteString(address, keywords[i], Encoding.Unicode);
						data.Memory.WriteInt64(shaderKeywordsPointerPointer + i * sizeof(long), address);
						data.Memory.WriteInt32(shaderKeywordsPointerPointerLengths + i * sizeof(int), length2);
					}
				}
			);

			linker.DefineFunction(
				"UnityEngine",
				"UnityEngineMaterial__set__shaderKeywords",
				(Caller caller, long wrappedId, long shaderKeywordsPointerPointer, long shaderKeywordsPointerPointerLengths, int shaderKeywordsLength) =>
				{
					StoreData data = GetData(caller);
					Material selfObject = IdTo<Material>(data, wrappedId);

					Span<long> shaderKeywordsPointerSpan = data.Memory.GetSpan<long>(shaderKeywordsPointerPointer, shaderKeywordsLength);
					Span<int> shaderKeywordsLengthsSpan = data.Memory.GetSpan<int>(shaderKeywordsPointerPointerLengths, shaderKeywordsLength);

					string[] shaderKeywords = new string[shaderKeywordsPointerPointerLengths];
					for (int i = 0; i < shaderKeywordsLength; i++)
					{
						shaderKeywords[i] = data.Memory.ReadString(shaderKeywordsPointerSpan[i], shaderKeywordsLengthsSpan[i] * sizeof(char), Encoding.Unicode);
					}

					selfObject.shaderKeywords = shaderKeywords;
				}
			);
		}
	}
}
