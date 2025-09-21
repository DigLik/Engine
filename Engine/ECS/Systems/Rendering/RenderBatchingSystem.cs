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

        Query<RenderMesh, LocalToWorld, Visibility>()
            .ForEach((ref mesh, ref transform, ref visibility) =>
            {
                if (visibility.IsEnabled && visibility.IsVisibleInFrustum)
                {
                    _renderQueue.Add(mesh, transform.Value);
                }
            });
    }
}