using Engine.ECS.Components.Rendering;

namespace Engine.Rendering;

public sealed class RenderQueue
{
    public readonly List<(RenderMesh Mesh, Matrix4x4 Transform)> Items = [];

    public void Clear() => Items.Clear();

    public void Add(RenderMesh renderMesh, in Matrix4x4 transform)
        => Items.Add((renderMesh, transform));
}