using UnityEngine;

public class WasmTest : MonoBehaviour {
    private void Start() {
        // Log the full qualified name of Component
        // Debug.Log(typeof(SkinnedMeshRenderer).AssemblyQualifiedName);
        
        Component component = GetComponent<Renderer>();
        Debug.Log(component != null ? $"Component found: {component.GetType().Name}" : "Component not found");
        
        Collider collider = GetComponentInParent<Collider>();
        Debug.Log(collider != null ? $"Collider found: {collider.GetType().Name}" : "Collider not found");
    }
}