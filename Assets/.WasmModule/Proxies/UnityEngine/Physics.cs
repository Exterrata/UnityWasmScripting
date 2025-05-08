using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public static class Physics
{
    #region Constants

    public const int IgnoreRaycastLayer = 4;
    public const int DefaultRaycastLayers = -5;
    public const int AllLayers = -1;

    #endregion Constants

    #region Implementation

    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        => internal_physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);

    #endregion Implementation

    #region Marshaling

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool internal_physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(
        Vector3 origin,
        Vector3 direction,
        out RaycastHit hitInfo,
        float maxDistance,
        int layerMask,
        QueryTriggerInteraction queryTriggerInteraction
    )
    {
        hitInfo = default;
        int result = physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(
            (long)Unsafe.AsPointer(ref origin),
            (long)Unsafe.AsPointer(ref direction),
            (long)Unsafe.AsPointer(ref hitInfo),
            maxDistance,
            layerMask,
            queryTriggerInteraction
        );
        return Unsafe.As<int, bool>(ref result);
    }

    #endregion Marshaling

    #region Imports

    [WasmImportLinkage, DllImport("unity")]
    private static extern int physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(
        long originPtr,
        long directionPtr,
        long hitInfoPtr,
        float maxDistance,
        int layerMask,
        QueryTriggerInteraction queryTriggerInteraction
    );

    #endregion Imports
}
