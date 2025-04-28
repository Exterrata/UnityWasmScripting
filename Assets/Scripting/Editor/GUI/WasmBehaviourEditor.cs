using UnityEditor;
using UnityEngine;

namespace WasmScripting 
{
    [CustomEditor(typeof(WasmBehaviour))]
    public class WasmBehaviourInspector : Editor 
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            if (!GUILayout.Button("Build Wasm Module"))
                return;
            
            WasmBuilder.CompileWasmProgramForObject(((WasmBehaviour)target).gameObject, true);
        }
    }
}