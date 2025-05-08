using System.Runtime.InteropServices;

namespace UnityEngine;

public class Renderer(long id) : Component(id)
{
    public void GetSharedMaterials(List<Material> materials) => internal_renderer_getSharedMaterials(WrappedId, materials);

    private static unsafe void internal_renderer_getSharedMaterials(long wrappedId, List<Material> materials)
    {
        long* materialIds = default;
        int length = default;

        renderer_getSharedMaterials(wrappedId, (long)&materialIds, (long)&length);

        materials.Clear();
        materials.Capacity = length;
        for (int i = 0; i < length; i++)
        {
            materials.Add(new(materialIds[i]));
        }

        Marshal.FreeHGlobal((IntPtr)materialIds);
    }

    [WasmImportLinkage, DllImport("unity")]
    private static extern void renderer_getSharedMaterials(long id, long outMaterialIds, long outMaterialsLength);
}