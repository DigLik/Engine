using Engine.ECS.Components.Rendering;
using Engine.Rendering;

namespace Engine.ECS.Systems.Rendering;

public sealed class RenderBatchingSystem : SystemBase
{
    private RenderQueue _renderQueue = null!;

    public override void OnInitialize()
    {
        _renderQueue = GetService<RenderQueue>();
    }

    public override void OnUpdate()
    {
        _renderQueue.Clear();

        Query<RenderMesh, LocalToWorld, VisibleTag>().ForEach((ref RenderMesh mesh, ref LocalToWorld transform, ref VisibleTag tag) =>
        {
            _renderQueue.Add(mesh, transform.Value);
        });
    }
}