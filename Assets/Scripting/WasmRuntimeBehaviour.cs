using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WasmScripting.Enums;

namespace WasmScripting {
	[DefaultExecutionOrder(50)] // Has to run after the WasmVM.
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
		public long definedEvents;
		
		internal int InstanceId;
		internal WasmVM VM;
		
		// this should be removed
		#if UNITY_EDITOR
		public MonoScript script;
		#endif

		#region Events
		
		// Unity Events
		private void Awake() => VM.CallMethod(InstanceId, UnityEventCall.Awake);
		private void Start() => VM.CallMethod(InstanceId, UnityEventCall.Start);
		private void OnEnable() => VM.CallMethod(InstanceId, UnityEventCall.OnEnable);
		private void OnDisable() => VM.CallMethod(InstanceId, UnityEventCall.OnDisable);
		private void OnDestroy() {
			if (!VM.Disposed) VM.CallMethod(InstanceId, UnityEventCall.OnDestroy);
		}

		// Forwarded Events
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
		
		#endregion Events
	}
	
	[Serializable]
	public struct WasmVariable<T> {
		public string name;
		public T value;
	}
}