using UnityEngine;

namespace WasmScripting.Proxies
{
    /// <summary>
    /// Event forwarder component to forward expensive events to a target WasmBehaviour.
    /// This is added alongside the WasmBehaviour only when the user script requests it.
    /// </summary>
    public class BaseEventForwarder : MonoBehaviour
    {
        public WasmRuntimeBehaviour targetRuntimeBehaviour;
    }
}