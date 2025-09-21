using Engine.ECS.Components.Rendering;

namespace Engine.ECS.Systems.Rendering;

public sealed class BoundsInitializationSystem : SystemBase
{
    public override void OnUpdate()
    {
        Query<RenderMesh>()
            .Without<LocalBounds>()
            .ForEach((entity, ref renderMesh) =>
            {
                var bounds = renderMesh.Mesh.Bounds;
                CommandBuffer.AddComponent(entity, new LocalBounds(bounds.Min, bounds.Max));
            });
    }
}