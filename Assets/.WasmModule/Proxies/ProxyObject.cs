namespace WasmModule.Proxies;

public class ProxyObject(long id)
{
    /// <summary>
    /// The ID assigned by the WasmAccessManager.
    /// </summary>
    /// <remarks>
    /// This ID is used to identify the object in the Wasm context.
    /// This is not the same as the instance ID in Unity.
    /// </remarks>
    internal long WrappedId = id;
}