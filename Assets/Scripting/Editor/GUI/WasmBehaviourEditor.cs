using UnityEditor;
using UnityEngine;

namespace WasmScripting 
{
    [CustomEditor(typeof(WasmRuntimeBehaviour))]
    public class WasmBehaviourInspector : Editor 
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            if (!GUILayout.Button("Build Wasm Module"))
                return;
            
            WasmBuilder.CompileWasmProgramForObject(((WasmRuntimeBehaviour)target).gameObject, true);
        }
    }
}