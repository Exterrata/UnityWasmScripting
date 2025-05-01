using System.Runtime.InteropServices;

namespace UnityEngine;

public struct RaycastHit
{
    #region Implementation
    
    public Collider collider => internal_raycast_collider_get(this);
    
    // public int colliderInstanceID =>
    //
    // public Vector3 point =>
    //
    // public Vector3 normal =>
    //
    // public Vector3 barycentricCoordinate =>
    //
    // public float distance =>
    //
    // public int triangleIndex => 
    //
    // public Vector2 textureCoord =>
    //
    // public Vector2 textureCoord2 =>
    //
    // public Transform transform =>
    //
    // public Rigidbody rigidbody =>
    //
    // public ArticulationBody articulationBody =>
    //
    // public Vector2 lightmapCoord =>
    
    #endregion Implementation

    #region Marshaling

    private static Collider internal_raycast_collider_get(RaycastHit hit)
    {
        WriteStruct(hit);
        return new(raycast_get_collider());
    }

    #endregion Marshaling

    #region Imports

    [WasmImportLinkage, DllImport("unity")]
    private static extern long raycast_get_collider();

    #endregion Imports
}