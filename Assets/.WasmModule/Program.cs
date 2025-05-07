using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WasmModule;
public static class Program {
    private static readonly Dictionary<long, MonoBehaviour> Behaviours = new();
    private static readonly Dictionary<Type, Dictionary<UnityEventCall, MethodInfo>> Callbacks = new();
    
	[UnmanagedCallersOnly(EntryPoint = "scripting_create_instance")]
	public static unsafe void CreateInstance(int id, long objectId, long strPtr, int strLength) {
        string name = new((char*)strPtr, 0, strLength);
        Marshal.FreeHGlobal((IntPtr)strPtr);
        try {
            Type type = Type.GetType(name);
            MonoBehaviour obj = Activator.CreateInstance(type) as MonoBehaviour;
            obj!.WrappedId = objectId;
            Behaviours[id] = obj;
            if (Callbacks.ContainsKey(type)) return;
            Dictionary<UnityEventCall, MethodInfo> callbacks = new();
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods) {
                if (Enum.TryParse(method.Name, out UnityEventCall unityEvent)) callbacks[unityEvent] = method;
            }
            Callbacks[type] = callbacks;
        } catch (Exception e) {
            Debug.LogError($"Error Creating WasmBehaviour `{name}`: {e}");
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call")]
    public static void Call(int id, int unityEvent) {
        try {
            MonoBehaviour behaviour = Behaviours[id];
            if (Callbacks[behaviour.GetType()].TryGetValue((UnityEventCall)unityEvent, out MethodInfo method)) method.Invoke(behaviour, null);
        } catch (Exception e) {
            Debug.LogError($"Error Calling Method `{(UnityEventCall)unityEvent}`: {e}");
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_alloc")]
    public static long Alloc(int length) => Marshal.AllocHGlobal(length);
    
    public enum AvailableUnityEvents : long {
        Awake = 1L << 0,
        Start = 1L << 1,
        Update = 1L << 2,
        LateUpdate = 1L << 3,
        FixedUpdate = 1L << 4,
        OnEnable = 1L << 5,
        OnDisable = 1L << 6,
        OnDestroy = 1L << 7,
        OnPreCull = 1L << 8,
        OnPreRender = 1L << 9,
        OnPostRender = 1L << 10,
        OnRenderImage = 1L << 11,
        OnRenderObject = 1L << 12,
        OnWillRenderObject = 1L << 13,
        OnBecameVisible = 1L << 14,
        OnBecameInvisible = 1L << 15,
        OnTriggerEnter = 1L << 16,
        OnTriggerEnter2D = 1L << 17,
        OnTriggerStay = 1L << 18,
        OnTriggerStay2D = 1L << 19,
        OnTriggerExit = 1L << 20,
        OnTriggerExit2D = 1L << 21,
        OnParticleTrigger = 1L << 22,
        OnCollisionEnter = 1L << 23,
        OnCollisionEnter2D = 1L << 24,
        OnCollisionStay = 1L << 25,
        OnCollisionStay2D = 1L << 26,
        OnCollisionExit = 1L << 27,
        OnCollisionExit2D = 1L << 28,
        OnControllerColliderHit = 1L << 29,
        OnTransformChildrenChanged = 1L << 30,
        OnTransformParentChanged = 1L << 31,
        OnJointBreak = 1L << 32,
        OnJointBreak2D = 1L << 33,
        OnParticleCollision = 1L << 34,
        OnMouseEnter = 1L << 35,
        OnMouseOver = 1L << 36,
        OnMouseExit = 1L << 37,
        OnMouseDown = 1L << 38,
        OnMouseUp = 1L << 39,
        OnMouseUpAsButton = 1L << 40,
        OnMouseDrag = 1L << 41,
        OnAnimatorMove = 1L << 42,
        OnAnimatorIK = 1L << 43,
        OnAudioFilterRead = 1L << 44,
        // Up to 63: 1L << 63 is max
    }
    
    public enum UnityEventCall {
        Awake,
        Start,
        Update,
        LateUpdate,
        FixedUpdate,
        OnEnable,
        OnDisable,
        OnDestroy,
        OnPreCull,
        OnPreRender,
        OnPostRender,
        OnRenderImage,
        OnRenderObject,
        OnWillRenderObject,
        OnBecameVisible,
        OnBecameInvisible,
        OnTriggerEnter,
        OnTriggerEnter2D,
        OnTriggerStay,
        OnTriggerStay2D,
        OnTriggerExit,
        OnTriggerExit2D,
        OnParticleTrigger,
        OnCollisionEnter,
        OnCollisionEnter2D,
        OnCollisionStay,
        OnCollisionStay2D,
        OnCollisionExit,
        OnCollisionExit2D,
        OnControllerColliderHit,
        OnTransformChildrenChanged,
        OnTransformParentChanged,
        OnJointBreak,
        OnJointBreak2D,
        OnParticleCollision,
        OnMouseEnter,
        OnMouseOver,
        OnMouseExit,
        OnMouseDown,
        OnMouseUp,
        OnMouseUpAsButton,
        OnMouseDrag,
        OnAnimatorMove,
        OnAnimatorIK,
        OnAudioFilterRead,
    }
}