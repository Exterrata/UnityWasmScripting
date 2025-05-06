using System;
using System.Collections.Generic;
using UnityEngine;

namespace WasmScripting {
	public static class TypeMap {
		private static readonly Dictionary<int, Type> IdToType = new() {
			{ 0, typeof(Component) },
			{ 1, typeof(Renderer) },
			{ 2, typeof(MeshRenderer) },
		};
	
		private static readonly Dictionary<Type, int> TypeToId = new() {
			{ typeof(Component), 0 },
			{ typeof(Renderer), 1 },
			{ typeof(MeshRenderer), 2 },
		};
		
		public static Type GetType(int id) => IdToType[id];
		public static int GetId(Type type) => TypeToId[type];
	}
}