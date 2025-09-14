using Engine.ECS.Components;
using Engine.ECS.Components.Rendering;
using Engine.ECS.Archetypes;

namespace Engine.ECS.Systems.Rendering;

public sealed class TransformHierarchySystem : SystemBase
{
    private Query? _rootsQuery;
    private Query? _childrenQuery;

    private readonly Dictionary<Entity, Matrix4x4> _worldMatrixCache = [];
    private readonly List<Entity> _deferredChildren = [];

    public override void OnInitialize()
    {
        _rootsQuery = World.Builder().Without<Parent>().Build<TransformComponent>();
        _childrenQuery = World.Builder().With<Parent>().Build<TransformComponent, Parent>();
    }

    public override void OnUpdate()
    {
        _worldMatrixCache.Clear();
        _deferredChildren.Clear();

        World.Iterate<TransformComponent>(_rootsQuery!).ForEach((entity, ref transform) =>
        {
            UpdateTransform(entity, ref transform, Matrix4x4.Identity, true);
        });

        World.Iterate<TransformComponent, Parent>(_childrenQuery!).ForEach((entity, ref transform, ref parent) =>
        {
            if (_worldMatrixCache.TryGetValue(parent.Target, out var parentMatrix))
            {
                UpdateTransform(entity, ref transform, parentMatrix, false);
            }
            else
            {
                _deferredChildren.Add(entity);
            }
        });

        int pass = 0;
        const int maxPasses = 10;

        while (_deferredChildren.Count > 0 && pass < maxPasses)
        {
            int processedCount = 0;
            for (int i = _deferredChildren.Count - 1; i >= 0; i--)
            {
                var entity = _deferredChildren[i];
                if (!World.IsAlive(entity))
                {
                    _deferredChildren.RemoveAt(i);
                    continue;
                }

                ref var transform = ref World.Ref<TransformComponent>(entity);
                ref var parent = ref World.Ref<Parent>(entity);

                if (_worldMatrixCache.TryGetValue(parent.Target, out var parentMatrix))
                {
                    UpdateTransform(entity, ref transform, parentMatrix, false);
                    _deferredChildren.RemoveAt(i);
                    processedCount++;
                }
            }

            if (processedCount == 0 && _deferredChildren.Count > 0) break;
            pass++;
        }
    }

    private void UpdateTransform(Entity entity, ref TransformComponent transform, in Matrix4x4 parentMatrix, bool isRoot)
    {
        var localMatrix = Matrix4x4.CreateScale(transform.Scale) *
                          Matrix4x4.CreateFromQuaternion(transform.Rotation) *
                          Matrix4x4.CreateTranslation(transform.Position);

        var worldMatrix = isRoot ? localMatrix : localMatrix * parentMatrix;

        World.Add(entity, new LocalToWorld(worldMatrix));
        _worldMatrixCache[entity] = worldMatrix;
    }
}