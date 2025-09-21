using Engine.ECS.Components.Physics;
using Engine.ECS.Systems;
using Engine.ECS.Systems.Rendering;
using Engine.Rendering;

namespace Engine.Core;

public static class EngineServiceExtensions
{
    public static ApplicationBuilder AddDefaultServices(this ApplicationBuilder builder)
    {
        builder.AddService(new ActiveCameraBuffer());
        builder.AddService(new RenderQueue());

        builder.AddService(new PhysicsService());
        builder.AddService(new PhysicsMap());

        return builder;
    }

    public static ApplicationBuilder AddDefaultSystems(this ApplicationBuilder builder)
    {
        builder.AddSystem<PhysicsSystem>();

        builder.AddSystem<TransformHierarchySystem>();
        builder.AddSystem<CameraSystem>();
        builder.AddSystem<BoundsInitializationSystem>();
        builder.AddSystem<VisibilitySystem>();
        builder.AddSystem<RenderBatchingSystem>();
        builder.AddSystem<RenderDispatchSystem>();

        return builder;
    }
}