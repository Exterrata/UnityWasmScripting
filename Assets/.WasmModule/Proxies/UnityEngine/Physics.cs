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

    private static bool internal_physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(
        Vector3 origin,
        Vector3 direction,
        out RaycastHit hitInfo,
        float maxDistance,
        int layerMask,
        QueryTriggerInteraction queryTriggerInteraction
    )
    {
        int offsetBytesWrite = 0;
        WriteStruct(origin, ref offsetBytesWrite);
        WriteStruct(direction, ref offsetBytesWrite);
        WriteStruct(hitInfo, ref offsetBytesWrite);
        int result = physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction(
            maxDistance,
            layerMask,
            queryTriggerInteraction
        );
        hitInfo = ReadStruct<RaycastHit>();
        return result != 0;
    }

    #endregion Marshaling
    
    #region Imports
    
    [WasmImportLinkage, DllImport("unity")]
    private static extern int physics_func_raycast_vector3_vector3_raycasthit_float_int_querytriggerinteraction
    (
        float maxDistance,
        int layerMask,
        QueryTriggerInteraction queryTriggerInteraction
    );
    
    #endregion Imports
}