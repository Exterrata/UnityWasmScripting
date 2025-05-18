using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WasmModule;

public static partial class Module
{
	private static readonly Dictionary<long, MonoBehaviour> Behaviours = new();
	private static readonly Dictionary<long, int> UpdateOrderByBehaviour = new();
	private static readonly SortedList<int, MonoBehaviour> UpdateSortedBehaviours = new();
	private static readonly Dictionary<Type, Dictionary<ScriptEvent, MethodInfo>> Callbacks = new();

	[UnmanagedCallersOnly(EntryPoint = "scripting_create_instance")]
	public static unsafe void CreateInstance(long wrappedId, long strPtr, int strLength)
	{
		string name = new((char*)strPtr, 0, strLength);
		Marshal.FreeHGlobal((IntPtr)strPtr);

		Type type = Type.GetType(name);
		MonoBehaviour behaviour = (MonoBehaviour)Activator.CreateInstance(type);
		behaviour!.WrappedId = wrappedId;
		Behaviours[wrappedId] = behaviour;

		int order = type.GetCustomAttribute<DefaultExecutionOrderAttribute>()?.Order ?? 0;
		UpdateSortedBehaviours.Add(order, behaviour);
		UpdateOrderByBehaviour.Add(wrappedId, order);

		if (Callbacks.ContainsKey(type))
			return;

		Dictionary<ScriptEvent, MethodInfo> callbacks = new();
		foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
		{
			if (Enum.TryParse(method.Name, out ScriptEvent unityEvent))
				callbacks[unityEvent] = method;
		}

		Callbacks[type] = callbacks;
	}

	[UnmanagedCallersOnly(EntryPoint = "scripting_alloc")]
	public static long Alloc(int length) => Marshal.AllocHGlobal(length);
}

[AttributeUsage(AttributeTargets.Class)]
public class DefaultExecutionOrderAttribute(int order) : Attribute
{
	public int Order = order;
}
