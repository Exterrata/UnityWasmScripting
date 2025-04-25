using System;
using System.Collections.Generic;
using UnityEngine;

namespace WasmScripting {
	[DefaultExecutionOrder(50)]
    public class WasmBehaviour : MonoBehaviour {
	    
#if UNITY_EDITOR
        public UnityEditor.MonoScript script;
#endif
	    
		public List<WasmVariable<int>> intVariables;
		public List<WasmVariable<bool>> boolVariables;
		public List<WasmVariable<float>> floatVariables;
		public List<WasmVariable<string>> stringVariables;
		public List<WasmVariable<Component>> componentVariables;
		public List<WasmVariable<GameObject>> gameObjectVariables;

		[HideInInspector] 
		public string BehaviourName;
		
		// Set by WasmVM
		internal int InstanceId;
		
		private WasmVM _vm;

		#region Unity Events
		
		private void Awake() {
			_vm = GetComponentInParent<WasmVM>();
			if (_vm.Awaked) _vm.CallMethod(InstanceId, "Awake");
		}

		private void Start() => _vm.CallMethod(InstanceId, "Start");
		private void Update() => _vm.CallMethod(InstanceId, "Update");
		private void LateUpdate() => _vm.CallMethod(InstanceId, "LateUpdate");
		private void FixedUpdate() => _vm.CallMethod(InstanceId, "FixedUpdate");
		private void OnEnable() => _vm.CallMethod(InstanceId, "OnEnable");
		private void OnDisable() => _vm.CallMethod(InstanceId, "OnDisable");
		private void OnDestroy() {
			if (_vm.Initialized) _vm.CallMethod(InstanceId, "OnDestroy");
		}
		private void OnPreCull() => _vm.CallMethod(InstanceId, "OnPreCull");
		private void OnPreRender() => _vm.CallMethod(InstanceId, "OnPreRender");
		private void OnPostRender() => _vm.CallMethod(InstanceId, "OnPostRender");
		
		#endregion Unity Events
	}
	
	[Serializable]
	public struct WasmVariable<T> {
		public string name;
		public T value;
	}
}