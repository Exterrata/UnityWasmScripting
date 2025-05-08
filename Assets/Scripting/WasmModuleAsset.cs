#if UNITY_EDITOR
using System.IO;
using UnityEditor.AssetImporters;
#endif
using UnityEngine;

namespace WasmScripting
{
    public class WasmModuleAsset : ScriptableObject
    {
        public byte[] bytes;
    }

#if UNITY_EDITOR
    [ScriptedImporter(1, "wasm")]
    public class WasmModuleImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            WasmModuleAsset moduleAsset = ScriptableObject.CreateInstance<WasmModuleAsset>();
            moduleAsset.bytes = File.ReadAllBytes(ctx.assetPath);
            ctx.AddObjectToAsset("module", moduleAsset);
            ctx.SetMainObject(moduleAsset);
        }
    }
#endif
}
