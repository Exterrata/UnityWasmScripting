using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using WasmScripting.Enums;
using WasmScripting.Proxies;

namespace WasmScripting 
{
    [DefaultExecutionOrder(50)]
    public class WasmBehaviour : MonoBehaviour 
    {
        #region Profile Markers

        private static readonly ProfilerMarker s_ProcessExternalEventsMarker = new($"{nameof(WasmBehaviour)}.{nameof(ProcessExternalEvents)}");
        private static readonly ProfilerMarker s_CallUnityEventMarker = new($"{nameof(WasmBehaviour)}.{nameof(CallUnityEvent)}");

        #endregion Profile Markers
        
        #region Versioning

        private const int LATEST_VERSION = 1;

        [SerializeField, HideInInspector] 
#pragma warning disable CS0414 // Field is assigned but its value is never used
        private int version;
#pragma warning restore CS0414 // Field is assigned but its value is never used
        
        private void Reset() => version = LATEST_VERSION;
        
        #endregion Versioning
        
        #region Serialized Fields

#if UNITY_EDITOR
        public UnityEditor.MonoScript script;
#endif
        
        [HideInInspector] public string behaviourName;
        [HideInInspector] public long unityEvents;
        
        public List<WasmVariable<int>> intVariables;
        public List<WasmVariable<bool>> boolVariables;
        public List<WasmVariable<float>> floatVariables;
        public List<WasmVariable<string>> stringVariables;
        public List<WasmVariable<Component>> componentVariables;
        public List<WasmVariable<GameObject>> gameObjectVariables;
        
        #endregion Serialized Fields

        #region Private Variables
        
        private readonly List<BaseEventForwarder> _eventForwarders = new();
        
        internal int InstanceId; // Set by WasmVM
        private WasmVM _vm;

        #endregion Private Variables

        #region Event Handling
        
        internal void CallUnityEvent(UnityEvents flag)
        {
            using (s_CallUnityEventMarker.Auto())
            {
                if (!_vm.Initialized)
                    return;
                
                if (!UnityEventsUtils.HasEvent(unityEvents, flag)) 
                    return;
                
                if (!isActiveAndEnabled && 
                    flag != UnityEvents.OnEnable 
                    && flag != UnityEvents.OnDisable
                    && flag != UnityEvents.OnDestroy)
                    return;

                _vm.CallUnityEvent(InstanceId, flag);
            }
        }
        
        #endregion Event Handling

        #region Unity Events
        
        private void Awake() 
        {
            _vm = GetComponentInParent<WasmVM>();

            
            // The VM will invoke Awake on all behaviours in its own Awake, so if it already did that,
            // we will do it ourselves here.
            if (_vm.Awakened) CallUnityEvent(UnityEvents.Awake);
        }

        private void Start()
        {
            ProcessExternalEvents();
            CallUnityEvent(UnityEvents.Start);
        }

        private void Update() => CallUnityEvent(UnityEvents.Update);
        private void LateUpdate() => CallUnityEvent(UnityEvents.LateUpdate);
        private void FixedUpdate() => CallUnityEvent(UnityEvents.FixedUpdate);
        
        private void OnEnable()
        {
            CallUnityEvent(UnityEvents.OnEnable);
            SetEventForwardersState(true);
        }
        private void OnDisable()
        {
            CallUnityEvent(UnityEvents.OnDisable);
            SetEventForwardersState(false);
        }
        private void OnDestroy()
        {
            CallUnityEvent(UnityEvents.OnDestroy); 
            CleanupEventForwarders();
        }
        
        private void OnPreCull() => CallUnityEvent(UnityEvents.OnPreCull);
        private void OnPreRender() => CallUnityEvent(UnityEvents.OnPreRender);
        private void OnPostRender() => CallUnityEvent(UnityEvents.OnPostRender);
        
        #endregion Unity Events

        #region Forwarded Events Processing
        
        private void ProcessExternalEvents()
        {
            using (s_ProcessExternalEventsMarker.Auto())
            {
                GameObject ourGameObject = gameObject;

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnAnimatorIK))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnAnimatorIKForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnAnimatorMove))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnAnimatorMoveForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnAudioFilterRead))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnAudioFilterReadForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnCollisionStay2D))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnCollisionStay2DForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnCollisionStay))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnCollisionStayForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnParticleCollision))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnParticleCollisionForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnRenderImage))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnRenderImageForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnRenderObject))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnRenderObjectForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnTriggerStay2D))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnTriggerStay2DForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnTriggerStay))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnTriggerStayForwarder>(ourGameObject, this));

                if (UnityEventsUtils.HasEvent(unityEvents, UnityEvents.OnWillRenderObject))
                    _eventForwarders.Add(BaseEventForwarder.Create<OnWillRenderObjectForwarder>(ourGameObject, this));
            }
        }
        
        private void SetEventForwardersState(bool state)
        {
            foreach (BaseEventForwarder forwarder in _eventForwarders)
                forwarder.enabled = state;
        }

        private void CleanupEventForwarders()
        {
            foreach (BaseEventForwarder forwarder in _eventForwarders) Destroy(forwarder);
            _eventForwarders.Clear();
        }
        
        #endregion Forwarded Events Processing

        #region Forwarded Event Handlers
        
        internal void ForwardedOnAnimatorIK(int layerIndex) => CallUnityEvent(UnityEvents.OnAnimatorIK);
        internal void ForwardedOnAnimatorMove() => CallUnityEvent(UnityEvents.OnAnimatorMove);
        internal void ForwardedOnAudioFilterRead(float[] data, int channels) => CallUnityEvent(UnityEvents.OnAudioFilterRead);
        internal void ForwardedOnCollisionStay2D(Collision2D collision) => CallUnityEvent(UnityEvents.OnCollisionStay2D);
        internal void ForwardedOnCollisionStay(Collision collision) => CallUnityEvent(UnityEvents.OnCollisionStay);
        internal void ForwardedOnParticleCollision(GameObject other) => CallUnityEvent(UnityEvents.OnParticleCollision);
        internal void ForwardedOnRenderImage(RenderTexture source, RenderTexture destination) => CallUnityEvent(UnityEvents.OnRenderImage);
        internal void ForwardedOnRenderObject() => CallUnityEvent(UnityEvents.OnRenderObject);
        internal void ForwardedOnTriggerStay2D(Collider2D other) => CallUnityEvent(UnityEvents.OnTriggerStay2D);
        internal void ForwardedOnTriggerStay(Collider other) => CallUnityEvent(UnityEvents.OnTriggerStay);
        internal void ForwardedOnWillRenderObject() => CallUnityEvent(UnityEvents.OnWillRenderObject);

        #endregion Forwarded Event Handlers
    }
    
    [Serializable]
    public struct WasmVariable<T> {
        public string name;
        public T value;
    }
}