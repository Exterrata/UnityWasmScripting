using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace WasmScripting {
	[PublicAPI]
	public class ScriptingWhitelist {
		public List<BindingSet> BindingSets = new();

		public BindingSet this[string key] => BindingSets.FirstOrDefault(context => context.Name == key);

		public ScriptingWhitelist(List<Type> types, string[] sets) {
			foreach (string set in sets) {
				BindingSets.Add(new(types, set));
			}
		}
		
		public ScriptingWhitelist() { }

		public void ForEach(IEnumerable<string> sets, Type type, IEnumerable<string> members, Action<BoundMember> op) {
			HashSet<string> setNames = new(sets);
			HashSet<string> memberNames = new(members);

			foreach (BoundType boundType in BindingSets.Where(s => setNames.Contains(s.Name)).Select(s => s.BoundTypes[type])) {
				foreach (BoundMember boundMember in boundType.BoundMembers.Values) {
					if (memberNames.Contains(boundMember.Name)) op(boundMember);
				}
			}
		}

		public IEnumerable<BoundMember> Members(string set, Type type) => BindingSets.Find(s => s.Name == set).BoundTypes[type].BoundMembers.Values;
	}

	[PublicAPI]
	public class BindingSet {
		public string Name;
		public Dictionary<Type, BoundType> BoundTypes = new();

		public BindingSet(List<Type> types, string name) {
			Name = name;
			foreach (Type type in types) {
				BoundTypes.Add(type, new(type));
			}
		}
		
		public BindingSet() { }

		public BoundType this[Type key] => BoundTypes[key];

		public bool Contains(Type key) => BoundTypes.ContainsKey(key);
	}

	[PublicAPI]
	public class BoundType {
		public Type Type;
		public Dictionary<string, BoundMember> BoundMembers = new();

		public BoundType(Type type) {
			Type = type;
			foreach (MemberInfo memberInfo in type.GetMembers((BindingFlags)30)) {
				if (memberInfo is FieldInfo fieldInfo) {
					string nameGet = ScriptingWhitelistUtils.GetMemberId(memberInfo);
					BoundMembers.Add(nameGet, new(nameGet, memberInfo));
					if (!fieldInfo.IsLiteral && !fieldInfo.IsInitOnly) {
						string nameSet = ScriptingWhitelistUtils.GetMemberId(memberInfo, true);
						BoundMembers.Add(nameSet, new(nameSet, memberInfo));
					}
				} else if (memberInfo is PropertyInfo property) {
					if (property.CanRead) {
						string name = ScriptingWhitelistUtils.GetMemberId(memberInfo);
						BoundMembers.Add(name, new(name, memberInfo));
					}

					if (property.CanWrite) {
						string name = ScriptingWhitelistUtils.GetMemberId(memberInfo, true);
						BoundMembers.Add(name, new(name, memberInfo));
					}
				} else if (memberInfo is not (System.Type or EventInfo)) {
					string name = ScriptingWhitelistUtils.GetMemberId(memberInfo);
					BoundMembers.Add(name, new(name, memberInfo));
				}
			}
		}
		
		public BoundMember this[string key] => BoundMembers[key];
		
		public bool Contains(string key) => BoundMembers.ContainsKey(key);
	}

	[PublicAPI]
	public class BoundMember {
		public bool IsBound;
		public string Name;
		public string OverrideName;
		public Type DeclaringType;
		public MemberType MemberType;
		public OwnerContext OwnerContext;
		public ScopeContext ScopeContext;

		public BoundMember(string name, MemberInfo memberInfo) {
			DeclaringType = memberInfo.DeclaringType;
			Name = name;
		}
		
		public BoundMember() { }
		
		public string GetName() => OverrideName ?? Name;
	}

	public static class ScriptingWhitelistUtils {
		public static string GetMemberId(MemberInfo memberInfo, bool setter = false) {
			string declaringType = TypeToIdentifier(memberInfo.DeclaringType);
			List<string> parts = new();
			if (memberInfo is ConstructorInfo ctor) {
				parts.Add("ctor");
				parts.AddRange(ctor.GetParameters().Select(p => TypeToIdentifier(p.ParameterType)));
			}
			else if (memberInfo is MethodInfo func) {
				parts.Add("func");
				parts.Add(func.IsGenericMethod ? $"{memberInfo.Name}_{func.GetGenericArguments().Length}" : memberInfo.Name);
				parts.AddRange(func.GetParameters().Select(p => TypeToIdentifier(p.ParameterType)));
				parts.Add(TypeToIdentifier(func.ReturnType));
			}
			else if (memberInfo is FieldInfo fieldInfo) {
				if (fieldInfo.IsLiteral) parts.Add("const");
				else parts.Add(setter ? "set" : "get");
				parts.Add(memberInfo.Name);
			} else if (memberInfo is PropertyInfo property) {
				parts.Add(setter ? "set" : "get");
				parts.Add(memberInfo.Name);
				parts.AddRange(property.GetIndexParameters().Select(p => TypeToIdentifier(p.ParameterType)));
			}

			return $"{declaringType}__{string.Join("__", parts.Where(p => p != null))}";
		}

		private static string TypeToIdentifier(Type type) {
			if (type.IsGenericType) {
				string ns = type.Namespace!.Replace('.', '_');
				string name = type.Name;
				int index = name.LastIndexOf('`');
				if (index < 0) index = name.Length;
				IEnumerable<string> arguments = type.GetGenericArguments().Select(TypeToIdentifier);
				return $"{ns}_{name[..index]}_{string.Join('_', arguments)}";
			}

			return (type.FullName ?? type.Name).Replace('.', '_').Replace("&", "");
		}
	}

	[PublicAPI]
	public enum MemberType {
		Method,
		Constructor,
		FieldGetter,
		FieldSetter,
		PropertyGetter,
		PropertySetter
	}

	[PublicAPI, Flags]
	public enum OwnerContext {
		None = 0,
		Self = 1,
		Other = 2,
		All = Self | Other
	}

	[PublicAPI, Flags]
	public enum ScopeContext {
		None = 0,
		Self = 1,
		ExternalContent = 2,
		GameInternal = 4,
		NotAvailable = 8,
		All = Self | ExternalContent | GameInternal | NotAvailable
	}
}