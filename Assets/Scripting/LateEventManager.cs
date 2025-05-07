using System;
using JetBrains.Annotations;
using UnityEngine;

namespace WasmScripting
{
    /// <summary>
    /// Simple script to allow subscription to post-update events.
    /// </summary>
    [DefaultExecutionOrder(int.MaxValue)] // lazy
    public class LateEventManager : MonoBehaviour
    {
        private static LateEventManager _instance;
        
        [PublicAPI]
        public static Action OnPostFixedUpdate, OnPostUpdate, OnPostLateUpdate;

        #region Unity Events
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this); 
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this);
        }
    
        private void FixedUpdate() => OnPostFixedUpdate?.Invoke();
        private void Update() => OnPostUpdate?.Invoke();
        private void LateUpdate() => OnPostLateUpdate?.Invoke();
        
        private void OnDestroy()
        {
            OnPostFixedUpdate = null;
            OnPostUpdate = null;
            OnPostLateUpdate = null;
        }
        
        #endregion Unity Events
    }
}