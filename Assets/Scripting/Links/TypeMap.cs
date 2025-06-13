using System;
using System.Collections.Generic;
using UnityEngine;

namespace WasmScripting
{
	public static class TypeMap
	{
		private static readonly Dictionary<int, Type> IdToType = new()
		{
			#region IdToType
			{ 0, typeof(global::UnityEngine.Component) },
			{ 1, typeof(global::UnityEngine.Renderer) },
			{ 2, typeof(global::UnityEngine.MeshRenderer) },
			#endregion IdToType
		};

		private static readonly Dictionary<Type, int> TypeToId = new()
		{
			#region TypeToId
			{ typeof(global::UnityEngine.Component), 0 },
			{ typeof(global::UnityEngine.Renderer), 1 },
			{ typeof(global::UnityEngine.MeshRenderer), 2 },
			#endregion TypeToId
		};

		public static Type GetType(int id) => IdToType[id];

		public static int GetId(Type type) => TypeToId[type];
	}
}
