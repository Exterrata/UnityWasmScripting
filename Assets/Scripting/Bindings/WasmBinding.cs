using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Wasmtime;

namespace WasmScripting {
	public abstract class WasmBinding {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static StoreData GetData(Caller caller) => (StoreData)caller.Store.GetData()!;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static T IdTo<T>(StoreData data, long id) where T : class => data.AccessManager.ToWrapped(id).Target as T;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static long IdFrom(StoreData data, object obj) => data.AccessManager.ToWrapped(obj).Id;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void WriteStruct<T>(StoreData data, ref T obj, int offsetBytes) where T : unmanaged => data.Buffer.WriteStruct(ref obj, offsetBytes);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static T ReadStruct<T>(StoreData data, int offsetBytes) where T : unmanaged => data.Buffer.ReadStruct<T>(offsetBytes);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void WriteString(StoreData data, string str, int offsetBytes) => data.Buffer.WriteString(str, offsetBytes);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static string ReadString(StoreData data, int offsetBytes) => data.Buffer.ReadString(offsetBytes);
	}
}