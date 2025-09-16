using Engine.ECS.Components;
using Engine.ECS.Components.Rendering;
using Engine.ECS.Archetypes.QueryDefinition;

namespace Engine.ECS.Systems.Rendering;

public sealed class TransformHierarchySystem : SystemBase
{
    private Query? _rootsQuery;
    private readonly Queue<(Entity Entity, Matrix4x4 ParentMatrix)> _processQueue = new();

    public override void OnInitialize()
    {
        _rootsQuery = World.Builder().Without<Parent>().Build<TransformComponent>();
    }

    public override void OnUpdate()
    {
        World.Iterate<TransformComponent>(_rootsQuery!).ForEach((entity, ref transform) =>
        {
            var worldMatrix = CalculateLocalMatrix(ref transform);
            World.Add(entity, new LocalToWorld(worldMatrix));

            _processQueue.Enqueue((entity, worldMatrix));
        });

        while (_processQueue.Count > 0)
        {
            var (parentEntity, parentMatrix) = _processQueue.Dequeue();

            foreach (var childEntity in World.GetChildren(parentEntity))
            {
                if (!World.IsAlive(childEntity) || !World.Has<TransformComponent>(childEntity))
                {
                    continue;
                }

                ref var childTransform = ref World.Ref<TransformComponent>(childEntity);

                var localMatrix = CalculateLocalMatrix(ref childTransform);
                var worldMatrix = localMatrix * parentMatrix;

                World.Add(childEntity, new LocalToWorld(worldMatrix));

                _processQueue.Enqueue((childEntity, worldMatrix));
            }
        }
    }

    private static Matrix4x4 CalculateLocalMatrix(ref TransformComponent transform)
    {
        return Matrix4x4.CreateScale(transform.Scale) *
               Matrix4x4.CreateFromQuaternion(transform.Rotation) *
               Matrix4x4.CreateTranslation(transform.Position);
    }
}