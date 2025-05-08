using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace WasmScripting
{
    [InitializeOnLoad]
    public partial class WasmBindingGenerator
    {
        private const string WhitelistPath = @"Scripting\Links\Whitelist.json";
        private static ScriptingWhitelist ScriptingWhitelist;
        private static readonly List<string> AllSets = new() { "Avatar", "World", "Prop" };
        private static readonly List<string> Assemblies = new()
        {
            "UnityEngine.CoreModule",
            "UnityEngine.PhysicsModule",
            "UnityEngine.AnimationModule",
            "UnityEngine.AudioModule",
        };
        private static readonly List<Type> Types = new();
        private readonly List<string> _members = new();
        private readonly List<Type> _shownTypes = new();
        private readonly List<string> _shownMembers = new();
        private readonly List<string> _activeSets = new();
        private Type _selectedType;

        [MenuItem("Wasm/BindingGenerator")]
        public static void OpenWindow() =>
            GetWindow<WasmBindingGenerator>("Wasm Binding Generator");

        static WasmBindingGenerator()
        {
            foreach (string assemblyName in Assemblies)
            {
                Assembly assembly = Assembly.Load(assemblyName);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsPublic)
                        Types.Add(type);
                }
            }

            FieldInfo info = typeof(EditorApplication).GetField(
                "globalEventHandler",
                BindingFlags.Static | BindingFlags.NonPublic
            );
            info!.SetValue(
                null,
                (EditorApplication.CallbackFunction)info!.GetValue(null) + SaveWhitelist
            );

            LoadWhitelist();
        }

        private static void SearchList<T>(IEnumerable<T> all, List<T> matching, string pattern)
        {
            matching.Clear();
            if (string.IsNullOrEmpty(pattern))
            {
                matching.AddRange(all);
                return;
            }

            Regex regex = new(pattern, RegexOptions.IgnoreCase);
            matching.AddRange(all.Where(item => regex.IsMatch(item.ToString())));
        }

        private static void SaveWhitelist()
        {
            if (!Event.current.control || Event.current.keyCode != KeyCode.S)
                return;

            if (HasOpenInstances<WasmBindingGenerator>())
                GetWindow<WasmBindingGenerator>().SaveChanges();
        }

        public override void SaveChanges()
        {
            if (!hasUnsavedChanges)
                return;
            string projectPath = UnityWasmScriptingSettingsManager.GetProjectRoot();
            string whitelistPath = Path.Combine(projectPath, WhitelistPath);
            File.WriteAllText(
                whitelistPath,
                JsonConvert.SerializeObject(ScriptingWhitelist, Formatting.Indented)
            );
            base.SaveChanges();
        }

        public override void DiscardChanges()
        {
            LoadWhitelist();
            base.DiscardChanges();
        }

        private static void LoadWhitelist()
        {
            string projectPath = UnityWasmScriptingSettingsManager.GetProjectRoot();
            string whitelistPath = Path.Combine(projectPath, WhitelistPath);
            if (File.Exists(whitelistPath))
            {
                string json = File.ReadAllText(whitelistPath);
                ScriptingWhitelist = JsonConvert.DeserializeObject<ScriptingWhitelist>(json);
            }
            else
                File.Create(whitelistPath);
            ScriptingWhitelist ??= new(Types, AllSets.ToArray());
        }
    }
}
