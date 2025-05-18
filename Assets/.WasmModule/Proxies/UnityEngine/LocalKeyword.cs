using WasmModule.Proxies;

namespace UnityEngine.Rendering;

public struct LocalKeyword(int id) : IProxyObject
{
	public long WrappedId { get; set; } = id;
}
