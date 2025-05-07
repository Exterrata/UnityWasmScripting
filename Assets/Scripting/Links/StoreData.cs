using System;
using UnityEngine;
using Wasmtime;

namespace WasmScripting
{
    public readonly struct StoreData
    {
        public readonly WasmAccessManager AccessManager;
        public readonly Func<int, long> Alloc;
        public readonly Memory Memory;

        public StoreData(GameObject root, Instance instance)
        {
            AccessManager = new(root);
            Alloc = instance.GetFunction<int, long>("scripting_alloc");
            Memory = instance.GetMemory("memory");
        }
    }
}
