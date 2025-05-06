using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WasmModule;
public static class Program {
    private static readonly Dictionary<long, MonoBehaviour> Behaviours = new();
    private static readonly Dictionary<Type, Dictionary<UnityEvent, MethodInfo>> Callbacks = new();
    
	[UnmanagedCallersOnly(EntryPoint = "scripting_create_instance")]
	public static unsafe void CreateInstance(int id, long objectId, long strPtr) {
        string name = new((char*)strPtr);
        Marshal.FreeHGlobal((IntPtr)strPtr);
        try {
            Type type = Type.GetType(name);
            MonoBehaviour obj = Activator.CreateInstance(type) as MonoBehaviour;
            obj!.WrappedId = objectId;
            Behaviours[id] = obj;
            if (Callbacks.ContainsKey(type)) return;
            Dictionary<UnityEvent, MethodInfo> callbacks = new();
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods) {
                if (Enum.TryParse(method.Name, out UnityEvent @event)) callbacks[@event] = method;
            }
            Callbacks[type] = callbacks;
        } catch (Exception e) {
            Debug.LogError($"Error Creating WasmBehaviour `{name}`: {e}");
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call")]
    public static void Call(int id, int @event) {
        try {
            MonoBehaviour behaviour = Behaviours[id];
            if (Callbacks[behaviour.GetType()].TryGetValue((UnityEvent)@event, out MethodInfo method)) method.Invoke(behaviour, null);
        } catch (Exception e) {
            Debug.LogError($"Error Calling Method `{(UnityEvent)@event}`: {e}");
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_alloc")]
    public static long Alloc(int length) => Marshal.AllocHGlobal(length);
    
    public enum UnityEvent {
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