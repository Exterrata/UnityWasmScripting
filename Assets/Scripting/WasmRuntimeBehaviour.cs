using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WasmScripting.Enums;

namespace WasmScripting
{
	[DefaultExecutionOrder(50)] // Has to execute after WasmVM.
	public class WasmRuntimeBehaviour : MonoBehaviour
	{
		#region Versioning

		private const int LatestVersion = 1;

		[SerializeField, HideInInspector]
		private int version;

		private void Reset()
		{
			// Assign latest version if added in inspector / reset
			version = LatestVersion;
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

		internal WasmVM VM;

		// this should be removed and instead replace WasmBehaviour scripts with WasmRuntimeBehaviour at build time
#if UNITY_EDITOR
		public MonoScript script;
#endif

		#region Events

		// Unity Events
		private void Awake()
		{
			if (UnityEventsUtils.HasEvent(definedEvents, UnityEvents.Awake))
				VM.CallScriptEvent(this, ScriptEvent.Awake);
		}

		private void Start()
		{
			if (UnityEventsUtils.HasEvent(definedEvents, UnityEvents.Start))
				VM.CallScriptEvent(this, ScriptEvent.Start);
		}

		private void OnEnable()
		{
			if (UnityEventsUtils.HasEvent(definedEvents, UnityEvents.OnEnable))
				VM.CallScriptEvent(this, ScriptEvent.OnEnable);
		}

		private void OnDisable()
		{
			if (UnityEventsUtils.HasEvent(definedEvents, UnityEvents.OnDisable))
				VM.CallScriptEvent(this, ScriptEvent.OnDisable);
		}

		private void OnDestroy()
		{
			if (UnityEventsUtils.HasEvent(definedEvents, UnityEvents.OnDestroy) && !VM.Disposed)
				VM.CallScriptEvent(this, ScriptEvent.OnDestroy);
		}

		// Forwarded Events
		internal void ForwardedOnAnimatorIK(int layerIndex) { }

		internal void ForwardedOnAnimatorMove() { }

		internal void ForwardedOnAudioFilterRead(float[] data, int channels) { }

		internal void ForwardedOnCollisionStay2D(Collision2D collision) { }

		internal void ForwardedOnCollisionStay(Collision collision) { }

		internal void ForwardedOnParticleCollision(GameObject other) { }

		internal void ForwardedOnRenderImage(RenderTexture source, RenderTexture destination) { }

		internal void ForwardedOnRenderObject() { }

		internal void ForwardedOnTriggerStay2D(Collider2D other) { }

		internal void ForwardedOnTriggerStay(Collider other) { }

		internal void ForwardedOnWillRenderObject() { }

		#endregion Events
	}

	[Serializable]
	public struct WasmVariable<T>
	{
		public string name;
		public T value;
	}
}
