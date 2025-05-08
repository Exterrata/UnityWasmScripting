using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WasmModule;

public static partial class Module
{
    [UnmanagedCallersOnly(EntryPoint = "scripting_call_awake")]
    public static unsafe void CallAwake(long* wrappedIds, int wrappedIdsCount)
    {
        SortByExecutionOrder(wrappedIds, wrappedIdsCount);
        for (int i = 0; i < wrappedIdsCount; i++)
        {
            MonoBehaviour behaviour = Behaviours[wrappedIds[i]];
            Callbacks[behaviour.GetType()][ScriptEvent.Awake].Invoke(behaviour, null);
        }
        Marshal.FreeHGlobal((IntPtr)wrappedIds);
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call_start")]
    public static unsafe void CallStart(long* wrappedIds, int wrappedIdsCount)
    {
        SortByExecutionOrder(wrappedIds, wrappedIdsCount);
        for (int i = 0; i < wrappedIdsCount; i++)
        {
            MonoBehaviour behaviour = Behaviours[wrappedIds[i]];
            Callbacks[behaviour.GetType()][ScriptEvent.Start].Invoke(behaviour, null);
        }
        Marshal.FreeHGlobal((IntPtr)wrappedIds);
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call_update")]
    public static void CallUpdate()
    {
        foreach (MonoBehaviour behaviour in UpdateSortedBehaviours.Values)
        {
            if (Callbacks[behaviour.GetType()].TryGetValue(ScriptEvent.Update, out MethodInfo method)) method.Invoke(behaviour, null);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call_late_update")]
    public static void CallLateUpdate()
    {
        foreach (MonoBehaviour behaviour in UpdateSortedBehaviours.Values)
        {
            if (Callbacks[behaviour.GetType()].TryGetValue(ScriptEvent.LateUpdate, out MethodInfo method)) method.Invoke(behaviour, null);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call_fixed_update")]
    public static void CallFixedUpdate()
    {
        foreach (MonoBehaviour behaviour in UpdateSortedBehaviours.Values)
        {
            if (Callbacks[behaviour.GetType()].TryGetValue(ScriptEvent.FixedUpdate, out MethodInfo method)) method.Invoke(behaviour, null);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "scripting_call_event")]
    public static void CallEvent(long wrappedId, int @event)
    {
        MonoBehaviour behaviour = Behaviours[wrappedId];
        Callbacks[behaviour.GetType()][(ScriptEvent)@event].Invoke(behaviour, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SortByExecutionOrder(long* ids, int count)
    {
        Span<long> span = new(ids, count);
        Span<int> keys = stackalloc int[count];
        for (int i = 0; i < count; i++) keys[i] = UpdateOrderByBehaviour[ids[i]];
        keys.Sort(span);
    }
}
