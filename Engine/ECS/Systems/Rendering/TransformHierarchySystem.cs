using Engine.ECS.Components;
using Engine.ECS.Components.Rendering;
using System.Diagnostics;

namespace Engine.ECS.Systems.Rendering;

public sealed class TransformHierarchySystem : SystemBase
{
    private readonly Dictionary<Entity, Matrix4x4> _worldMatrixCache = [];

    public override void OnUpdate()
    {
        _worldMatrixCache.Clear();

        var rootsQuery = World.Builder().Without<Parent>().Build<TransformComponent>();
        World.Iterate<TransformComponent>(rootsQuery).ForEach((entity, ref transform) =>
        {
            var matrix = Matrix4x4.CreateScale(transform.Scale) *
                         Matrix4x4.CreateFromQuaternion(transform.Rotation) *
                         Matrix4x4.CreateTranslation(transform.Position);

            if (World.Has<LocalToWorld>(entity))
                World.Ref<LocalToWorld>(entity).Value = matrix;
            else
                World.Add(entity, new LocalToWorld(matrix));

            _worldMatrixCache[entity] = matrix;
        });

        Query<TransformComponent, Parent>().ForEach((entity, ref transform, ref parent) =>
        {
            if (_worldMatrixCache.TryGetValue(parent.Target, out var parentMatrix))
            {
                var localMatrix = Matrix4x4.CreateScale(transform.Scale) *
                                  Matrix4x4.CreateFromQuaternion(transform.Rotation) *
                                  Matrix4x4.CreateTranslation(transform.Position);

                var worldMatrix = localMatrix * parentMatrix;

                if (World.Has<LocalToWorld>(entity))
                    World.Ref<LocalToWorld>(entity).Value = worldMatrix;
                else
                    World.Add(entity, new LocalToWorld(worldMatrix));

                _worldMatrixCache[entity] = worldMatrix;
            }
            else
            {
                Debug.WriteLine($"Parent {parent.Target} of entity {entity} not processed yet. Skipping.");
            }
        });
    }
}