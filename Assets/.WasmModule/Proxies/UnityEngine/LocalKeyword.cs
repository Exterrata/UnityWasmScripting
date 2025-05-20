using WasmModule.Proxies;

namespace UnityEngine.Rendering;

public struct LocalKeyword(int id) : IProxyObject {
	public long WrappedId { get; set; } = id;

	public string name => internal_get_name(WrappedId);

	public LocalKeyword(Shader shader, string name) => internal_ctor_LocalKeyword(shader.WrappedId, name);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe string internal_get_name(long wrappedId) {
		UnityEngineLocalKeyword__get__name(out char* name, out int nameLength);
		return new(name, 0, nameLength);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe LocalKeyword internal_ctor_LocalKeyword(long shaderWrappedId, string name) {
		fixed (char* str = name) {
			return new(UnityEngineLocalKeyword__ctor__LocalKeyword(shaderWrappedId, str, name.length));
		}
	}
	
	[WasmImportLinkage, DllImport("UnityEngine")]
	private static extern long UnityEngineLocalKeyword__get__name(out char* name, out int nameLength);

	[WasmImportLinkage, DllImport("UnityEngine")]
	private static extern long UnityEngineLocalKeyword__ctor__LocalKeyword(long shaderWrappedId, char* name, int nameLength);
}