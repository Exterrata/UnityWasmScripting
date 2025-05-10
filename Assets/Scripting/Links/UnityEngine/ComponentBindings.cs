using System;
using System.Text;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
	public class ComponentBindings : WasmBinding
	{
		public static void BindMethods(Linker linker)
		{
			linker.DefineFunction(
				"unity",
				"component_tag_get",
				(Caller caller, long wrappedId) =>
				{
					StoreData data = GetData(caller);
					string str = IdTo<Component>(data, wrappedId).tag;
					return WriteString(data, str);
				}
			);

			linker.DefineFunction(
				"unity",
				"component_tag_set",
				(Caller caller, long wrappedId, long strPtr, int strSize) =>
				{
					StoreData data = GetData(caller);
					string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
					IdTo<Component>(data, wrappedId).tag = str;
				}
			);

			linker.DefineFunction(
				"unity",
				"component_transform_get",
				(Caller caller, long wrappedId) =>
				{
					StoreData data = GetData(caller);
					return IdFrom(data, IdTo<Component>(data, wrappedId).transform);
				}
			);

			linker.DefineFunction(
				"unity",
				"component_gameObject_get",
				(Caller caller, long wrappedId) =>
				{
					StoreData data = GetData(caller);
					return IdFrom(data, IdTo<Component>(data, wrappedId).gameObject);
				}
			);

			linker.DefineFunction(
				"unity",
				"component_getComponent_string",
				(Caller caller, long wrappedId, long componentStr, int componentStrLength, long outComponentType) =>
				{
					StoreData data = GetData(caller);
					Component component = IdTo<Component>(data, wrappedId);

					string componentName = data.Memory.ReadString(componentStr, componentStrLength, Encoding.Unicode);
					Component outComponent = component.GetComponent(componentName);

					data.Memory.WriteInt32(outComponentType, TypeMap.GetId(outComponent.GetType()));
					return IdFrom(data, outComponent);
				}
			);

			linker.DefineFunction(
				"unity",
				"component_getComponent_type",
				(Caller caller, long wrappedId, int componentTypeId, long outComponentType) =>
				{
					StoreData data = GetData(caller);
					Component component = IdTo<Component>(data, wrappedId);

					Type componentType = TypeMap.GetType(componentTypeId);
					Component outComponent = component.GetComponent(componentType);

					data.Memory.WriteInt32(outComponentType, TypeMap.GetId(outComponent.GetType()));
					return IdFrom(data, outComponent);
				}
			);
		}
	}
}
