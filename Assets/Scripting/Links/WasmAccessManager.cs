using System.Collections.Generic;
using UnityEngine;

namespace WasmScripting
{
	public class WasmAccessManager
	{
		private GameObject _accessRoot;
		private readonly Dictionary<long, WrappedObject> _idToWrapped = new();
		private readonly Dictionary<object, WrappedObject> _objectToWrapped = new();
		private long _currentId = 1; // Start at 1 because 0 is reserved for null

		public WasmAccessManager(GameObject root)
		{
			_accessRoot = root;
		}

		public WrappedObject ToWrapped(object obj) => _objectToWrapped.TryGetValue(obj, out WrappedObject wrapped) ? wrapped : CreateWrapped(obj);

		public WrappedObject ToWrapped(long id) => _idToWrapped.GetValueOrDefault(id);

		private WrappedObject CreateWrapped(object obj)
		{
			// check permissions stuff here
			WrappedObject wrapped = new(obj, _currentId);
			_idToWrapped[_currentId++] = wrapped;
			_objectToWrapped[obj] = wrapped;
			return wrapped;
		}
	}

	public readonly struct WrappedObject
	{
		public readonly object Target;
		public readonly long Id;

		// add context stuff here

		public WrappedObject(object target, long id)
		{
			Target = target;
			Id = id;
		}
	}
}
