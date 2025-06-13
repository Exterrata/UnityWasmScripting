using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public class Transform(long id) : Component(id)
{
	#region Implementation

	public Vector3 position
	{
		get => internal_transform_position_get(WrappedId);
		set => internal_transform_position_set(WrappedId, value);
	}

	#endregion Implementation

	#region Marshaling

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe Vector3 internal_transform_position_get(long id)
	{
		Vector3 position = default;
		transform_position_get(id, (long)Unsafe.AsPointer(ref position));
		return position;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void internal_transform_position_set(long id, Vector3 position)
	{
		transform_position_set(id, (long)Unsafe.AsPointer(ref position));
	}

	#endregion Marshaling

	#region Imports

	[WasmImportLinkage, DllImport("unity")]
	private static extern void transform_position_get(long id, long positionPtr);

	[WasmImportLinkage, DllImport("unity")]
	private static extern void transform_position_set(long id, long positionPtr);

	#endregion Imports
}
