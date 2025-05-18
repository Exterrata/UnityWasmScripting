using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Wasmtime;

namespace WasmScripting.UnityEngine
{
	public class GameObjectBindings : WasmBinding
	{
		public static void BindMethods(Linker linker)
		{
			linker.DefineFunction(
				"unity",
				"gameObject_ctor0",
				(Caller caller) =>
				{
					StoreData data = GetData(caller);
					return IdFrom(data, new GameObject());
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_ctor1",
				(Caller caller, long strPtr, int strSize) =>
				{
					StoreData data = GetData(caller);
					string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
					return IdFrom(data, new GameObject(str));
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_activeInHierarchy_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					return IdTo<GameObject>(data, objectId).activeInHierarchy ? 1 : 0;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_activeSelf_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					return IdTo<GameObject>(data, objectId).activeSelf ? 1 : 0;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_isStatic_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					return IdTo<GameObject>(data, objectId).isStatic ? 1 : 0;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_isStatic_set",
				(Caller caller, long objectId, int isStatic) =>
				{
					StoreData data = GetData(caller);
					IdTo<GameObject>(data, objectId).isStatic = isStatic != 0;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_layer_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					return IdTo<GameObject>(data, objectId).layer;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_layer_set",
				(Caller caller, long objectId, int layer) =>
				{
					StoreData data = GetData(caller);
					IdTo<GameObject>(data, objectId).layer = layer;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_scene_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					return IdFrom(data, IdTo<GameObject>(data, objectId).scene);
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_sceneCullingMask_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					ulong sceneCullingMask = IdTo<GameObject>(data, objectId).sceneCullingMask;
					return UnsafeUtility.As<ulong, long>(ref sceneCullingMask);
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_tag_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					string str = IdTo<GameObject>(data, objectId).tag;
					return WriteString(data, str);
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_tag_set",
				(Caller caller, long objectId, long strPtr, int strSize) =>
				{
					StoreData data = GetData(caller);
					string str = data.Memory.ReadString(strPtr, strSize, Encoding.Unicode);
					IdTo<GameObject>(data, objectId).tag = str;
				}
			);

			linker.DefineFunction(
				"unity",
				"gameObject_transform_get",
				(Caller caller, long objectId) =>
				{
					StoreData data = GetData(caller);
					return IdFrom(data, IdTo<GameObject>(data, objectId).transform);
				}
			);
		}
	}
}
