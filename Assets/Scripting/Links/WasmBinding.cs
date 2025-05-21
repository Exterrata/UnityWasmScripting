using System.Runtime.CompilerServices;
using System.Text;
using Wasmtime;

namespace WasmScripting
{
	public abstract class WasmBinding
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static StoreData GetData(Caller caller) => (StoreData)caller.Store.GetData()!;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static T IdToClass<T>(StoreData data, long id)
			where T : class => (T)data.AccessManager.ToWrapped(id).Target;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static ref T IdToStruct<T>(StoreData data, long id)
			where T : struct => ref Unsafe.Unbox<T>(data.AccessManager.ToWrapped(id).Target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static long IdFrom(StoreData data, object obj) => obj == null ? 0 : data.AccessManager.ToWrapped(obj).Id;

		/// <summary>
		/// Writes a null terminated string
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static long WriteString(StoreData data, string str)
		{
			long strPtr = data.Alloc((str.Length + 1) * sizeof(char));
			data.Memory.WriteString(strPtr, str, Encoding.Unicode);
			data.Memory.WriteInt16(strPtr + str.Length * 2, 0);
			return strPtr;
		}
	}
}
