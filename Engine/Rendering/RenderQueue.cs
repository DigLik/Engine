using Engine.ECS.Components.Rendering;
using System.Runtime.InteropServices;

namespace Engine.Rendering;

public sealed class RenderQueue
{
    public readonly Dictionary<(MeshHandle, MaterialHandle), List<Matrix4x4>> Batches = [];

    public void Clear()
    {
        foreach (var batch in Batches.Values)
            batch.Clear();
    }

    public void Add(RenderMesh renderMesh, in Matrix4x4 transform)
    {
        var key = (renderMesh.Mesh, renderMesh.Material);
        ref var batch = ref CollectionsMarshal.GetValueRefOrAddDefault(Batches, key, out var exists);
        if (!exists || batch == null)
            batch = [];
        batch.Add(transform);
    }
}