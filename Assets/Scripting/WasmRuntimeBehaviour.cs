using System;
using System.Collections.Generic;
using UnityEngine;
using WasmScripting.Enums;

namespace WasmScripting {
	[DefaultExecutionOrder(50)]
    public class WasmRuntimeBehaviour : MonoBehaviour {
	    
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
	    
		public List<WasmVariable<int>> intVariables;
		public List<WasmVariable<bool>> boolVariables;
		public List<WasmVariable<float>> floatVariables;
		public List<WasmVariable<string>> stringVariables;
		public List<WasmVariable<Component>> componentVariables;
		public List<WasmVariable<GameObject>> gameObjectVariables;

		public string behaviourName;
		
		/// <summary>
		/// The execution order of this behaviour among the other WasmBehaviours in the same WasmVM.
		/// </summary>
		public int executionOrder;
		internal int InstanceId; // Set by WasmVM
		
		private WasmVM _vm;

		#region Unity Events
		
		private void Awake() {
			if (_vm.Awakened) _vm.CallMethod(InstanceId, UnityEvents.Awake);
		}

		private void Start() => _vm.CallMethod(InstanceId, UnityEvents.Start);
		private void OnEnable() => _vm.CallMethod(InstanceId, UnityEvents.OnEnable);
		private void OnDisable() => _vm.CallMethod(InstanceId, UnityEvents.OnDisable);
		private void OnDestroy() {
			if (_vm.Initialized) _vm.CallMethod(InstanceId, UnityEvents.OnDestroy);
		}
		
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