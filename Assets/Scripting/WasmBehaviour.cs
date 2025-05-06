using System;
using System.Collections.Generic;
using UnityEngine;

namespace WasmScripting {
	[DefaultExecutionOrder(50)]
    public class WasmBehaviour : MonoBehaviour {
	    
	    #region Versioning

	    private const int LATEST_VERSION = 1;

	    [SerializeField, HideInInspector] 
	    private int version;
	    
	    private void Reset()
	    {
		    // Assign latest version if added in inspector / reset
		    version = LATEST_VERSION; 
	    }
	    
	    #endregion Versioning
	    
	    /// <summary>
	    /// The execution order of this behaviour among the other WasmBehaviours in the same WasmVM.
	    /// </summary>
	    public int WasmExecutionOrder = 0;
	    
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
		public string behaviourName;
		internal int InstanceId; // Set by WasmVM
		
		private WasmVM _vm;

		#region Unity Events
		
		private void Awake() {
			_vm = GetComponentInParent<WasmVM>();
			if (_vm.Awakened) _vm.CallMethod(InstanceId, UnityEvent.Awake);
		}

		private void Start() => _vm.CallMethod(InstanceId, UnityEvent.Start);
		private void Update() => _vm.CallMethod(InstanceId, UnityEvent.Update);
		private void LateUpdate() => _vm.CallMethod(InstanceId, UnityEvent.LateUpdate);
		private void FixedUpdate() => _vm.CallMethod(InstanceId, UnityEvent.FixedUpdate);
		private void OnEnable() => _vm.CallMethod(InstanceId, UnityEvent.OnEnable);
		private void OnDisable() => _vm.CallMethod(InstanceId, UnityEvent.OnDisable);
		private void OnDestroy() {
			if (_vm.Initialized) _vm.CallMethod(InstanceId, UnityEvent.OnDestroy);
		}
		private void OnPreCull() => _vm.CallMethod(InstanceId, UnityEvent.OnPreCull);
		private void OnPreRender() => _vm.CallMethod(InstanceId, UnityEvent.OnPreRender);
		private void OnPostRender() => _vm.CallMethod(InstanceId, UnityEvent.OnPostRender);
		
		#endregion Unity Events

		#region Forwarded Events

		internal void ForwardedOnAnimatorIK(int layerIndex) {}
		
		internal void ForwardedOnAnimatorMove() {}
		
		internal void ForwardedOnAudioFilterRead(float[] data, int channels) {}
		
		internal void ForwardedOnCollisionStay2D(Collision2D collision) {}
		
		internal void ForwardedOnCollisionStay(Collision collision) {}
		
		internal void ForwardedOnParticleCollision(GameObject other) {}
		
		internal void ForwardedOnRenderImage(RenderTexture source, RenderTexture destination) {}
		
		internal void ForwardedOnRenderObject() {}
		
		internal void ForwardedOnTriggerStay2D(Collider2D other) {}
		
		internal void ForwardedOnTriggerStay(Collider other) {}
		
		internal void ForwardedOnWillRenderObject() {}

		#endregion Forwarded Events
	}
	
	[Serializable]
	public struct WasmVariable<T> {
		public string name;
		public T value;
	}
}