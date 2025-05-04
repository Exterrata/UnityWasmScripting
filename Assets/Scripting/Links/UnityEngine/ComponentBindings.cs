using System;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine {
	public class ComponentBindings : WasmBinding {
		public static void BindMethods(Linker linker) {
			linker.DefineFunction("unity", "component_tag_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				string str = IdTo<Component>(data, objectId).tag;
				return WriteString(data, str);
			});
			
			linker.DefineFunction("unity", "component_tag_set", (Caller caller, long objectId, long strPtr, int strSize) => {
				StoreData data = GetData(caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				IdTo<Component>(data, objectId).tag = str;
			});
			
			linker.DefineFunction("unity", "component_transform_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				return IdFrom(data, IdTo<Component>(data, objectId).transform);
			});
			
			linker.DefineFunction("unity", "component_gameObject_get", (Caller caller, long objectId) => {
				StoreData data = GetData(caller);
				return IdFrom(data, IdTo<Component>(data, objectId).gameObject);
			});

			linker.DefineFunction("unity", "component_func_getcomponent_string", (Caller Caller, long objectId, long typeNamePtr, long strPtr, int strSize) => {
				StoreData data = GetData(Caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				Component component = IdTo<Component>(data, objectId).GetComponent(str);
				
				if (component == null)
					return 0;
				 
				// Get both the component ID and actual type name
				string actualTypeName = component.GetType().FullName;
				long actualTypeNamePtr = WriteString(data, actualTypeName);
				data.Memory.WriteInt64(typeNamePtr, actualTypeNamePtr);
				
				return IdFrom(data, component);
			});
			
			linker.DefineFunction("unity", "component_func_getcomponentinchildren_string", (Caller Caller, long objectId, long typeNamePtr, long strPtr, int strSize, int includeInactive) => {
				StoreData data = GetData(Caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				
				Type type = Type.GetType(str); // TODO: fix this
				
				Component component = IdTo<Component>(data, objectId).GetComponentInChildren(type, UnsafeUtility.As<int, bool>(ref includeInactive));
				
				if (component == null)
					return 0;
				
				// Get both the component ID and actual type name
				string actualTypeName = component.GetType().FullName;
				long actualTypeNamePtr = WriteString(data, actualTypeName);
				data.Memory.WriteInt64(typeNamePtr, actualTypeNamePtr);
				
				return IdFrom(data, component);
			});
			
			linker.DefineFunction("unity", "component_func_getcomponentinparent_string", (Caller Caller, long objectId, long typeNamePtr, long strPtr, int strSize, int includeInactive) => {
				StoreData data = GetData(Caller);
				string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
				
				Type type = Type.GetType(str); // TODO: fix this
				
				Component component = IdTo<Component>(data, objectId).GetComponentInParent(type, UnsafeUtility.As<int, bool>(ref includeInactive));
				
				if (component == null)
					return 0;
				
				// Get both the component ID and actual type name
				string actualTypeName = component.GetType().FullName;
				long actualTypeNamePtr = WriteString(data, actualTypeName);
				data.Memory.WriteInt64(typeNamePtr, actualTypeNamePtr);
				
				return IdFrom(data, component);
			});
		}
	}
}