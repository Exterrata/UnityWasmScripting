using UnityEngine;

public class WasmTest : MonoBehaviour {
    private void Start() {
        Debug.Log("aaa");
        Debug.Log("aaa");
        Debug.Log("aaa");
        Debug.Log("aaa");
        Debug.Log(gameObject.ToString());
    }

    private readonly Vector3 down = new(0, -1, 0);
    
    private void Update() {
         Vector3 pos = transform.position;
         Debug.Log(pos);
         Debug.Log(down);
         
         bool hasHitGround = Physics.Raycast(
             pos,
             down,
             out RaycastHit hitInfo,
             100f,
             -1,
             QueryTriggerInteraction.UseGlobal
         );
         
         Debug.Log(hitInfo.colliderInstanceID);
         
        if (hasHitGround) {
            Debug.Log($"Hit: {hitInfo.collider.name}");
        } else {
            Debug.Log("No hit");
        }
    }
}