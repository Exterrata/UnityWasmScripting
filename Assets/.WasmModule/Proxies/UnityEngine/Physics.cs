using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public static class Physics {
	#region Constants

	public const int IgnoreRaycastLayer = 4;
	public const int DefaultRaycastLayers = -5;
	public const int AllLayers = -1;

	#endregion Constants
	
	#region Implementation
	
	public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) 
		=> internal_Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
	
	public static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) 
		=> internal_SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
	
	#endregion Implementation
	
	#region Marshaling
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe int internal_SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction) {
		fixed (RaycastHit* resultsPtr = results) {
			return Physics_func_SphereCastNonAlloc_vector3_float_vector3_raycasthit_int_float_int_querytriggerinteraction(
				(long)Unsafe.AsPointer(ref origin),
				radius,
				(long)Unsafe.AsPointer(ref direction),
				(long)resultsPtr,
				results.Length,
				maxDistance,
				layerMask,
				(int)queryTriggerInteraction
			);
		}
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe bool internal_Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction) {
		hitInfo = default;
		int result = Physics_func_Raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(
			(long)Unsafe.AsPointer(ref origin),
			(long)Unsafe.AsPointer(ref direction),
			(long)Unsafe.AsPointer(ref hitInfo),
			maxDistance,
			layerMask,
			(int)queryTriggerInteraction
		);
		
		return Unsafe.As<int, bool>(ref result);
	}
	
	#endregion Marshaling
	
	#region Imports
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern int Physics_func_SphereCastNonAlloc_vector3_float_vector3_raycasthit_int_float_int_querytriggerinteraction(long originPtr, float radius, long directionPtr, long resultsPtr, int resultsLength, float maxDistance, int layerMask, int queryTriggerInteraction);
	
	[WasmImportLinkage, DllImport("unity")]
	private static extern int Physics_func_Raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(long originPtr, long directionPtr, long hitInfoPtr, float maxDistance, int layerMask, int queryTriggerInteraction);
	
	#endregion Imports
}