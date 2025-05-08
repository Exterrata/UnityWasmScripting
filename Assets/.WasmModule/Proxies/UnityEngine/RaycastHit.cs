using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine;

public struct RaycastHit
{
    #region Implementation

    internal Vector3 m_Point;
    internal Vector3 m_Normal;
    internal uint m_FaceID;
    internal float m_Distance;
    internal Vector2 m_UV;
    internal int m_Collider;

    public Collider collider => internal_raycast_collider_get(this);

    public int colliderInstanceID => m_Collider;

    public Vector3 point
    {
        get => this.m_Point;
        set => this.m_Point = value;
    }

    public Vector3 normal
    {
        get => this.m_Normal;
        set => this.m_Normal = value;
    }

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

    private static unsafe Collider internal_raycast_collider_get(RaycastHit hit)
    {
        return new(raycast_get_collider((long)Unsafe.AsPointer(ref hit)));
    }

    #endregion Marshaling

    #region Imports

    [WasmImportLinkage, DllImport("unity")]
    private static extern long raycast_get_collider(long raycastHitPtr);

    #endregion Imports
}
