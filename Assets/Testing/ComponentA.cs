using UnityEngine;

namespace WasmScripting
{
    [DefaultExecutionOrder(0)]
    public class ComponentA : MonoBehaviour
    {
        private void Awake() => LogTheOrder(nameof(Awake));

        private void Start() => LogTheOrder(nameof(Start));

        private void OnEnable() => LogTheOrder(nameof(OnEnable));

        private void OnDisable() => LogTheOrder(nameof(OnDisable));

        // private void Update() => LogTheOrder(nameof(Update));
        
        private void OnDestroy() => LogTheOrder(nameof(OnDestroy));
        
        private void LogTheOrder(string eventName) => Debug.Log($"Running {eventName} in {GetType().Name} with instance id {GetInstanceID()}");
    }
}