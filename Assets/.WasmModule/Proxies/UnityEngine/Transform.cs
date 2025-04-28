using System.Runtime.InteropServices;

namespace UnityEngine;

public class Transform(long id) : Component(id)
{
	#region Implementation
	
	public Vector3 position {
		get => internal_transform_position_get(WrappedId);
		set => internal_transform_position_set(WrappedId, value);
	}
	
	#endregion Implementation

	#region Marshaling
	
	private static Vector3 internal_transform_position_get(long id) {
		transform_position_get(id);
		return ReadStruct<Vector3>(0);
	}

	private static void internal_transform_position_set(long id, Vector3 position) {
		WriteStruct(position, 0);
		transform_position_set(id);
	}
	
	#endregion Marshaling
	
	#region Imports
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern void transform_position_get(long id);
    
	[WasmImportLinkage, DllImport("unity")]
	private static extern void transform_position_set(long id);
	
	#endregion Imports
}