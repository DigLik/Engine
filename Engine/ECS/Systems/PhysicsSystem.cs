using Engine.ECS.Components.Physics;
using Engine.ECS.Components;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace Engine.ECS.Systems;

public sealed class PhysicsSystem : SystemBase
{
    private PhysicsService _physicsService = null!;
    private PhysicsMap _physicsMap = null!;
    private Simulation _simulation = null!;

    private static readonly PhysicsMaterial DefaultMaterial = new(0.5f, 0f);

    private float _timeAccumulator;
    private const float FixedTimeStep = 1f / 60f;

    public override void OnInitialize()
    {
        _physicsService = GetService<PhysicsService>();
        _physicsMap = GetService<PhysicsMap>();
        _simulation = _physicsService.Simulation;
    }

    public override void OnUpdate()
    {
        CreateBodies();
        CleanupBodies();

        _timeAccumulator += Time.DeltaTime;
        while (_timeAccumulator >= FixedTimeStep)
        {
            _physicsService.Simulation.Timestep(FixedTimeStep, _physicsService.ThreadDispatcher);
            _timeAccumulator -= FixedTimeStep;
        }

        SyncTransformsFromPhysics();
    }

    private void CreateBody<TShape>(Entity entity, in Transform transform, in TShape shape) where TShape : unmanaged, IConvexShape
    {
        var material = World.TryGetRef<PhysicsMaterial>(entity, out var mat) ? mat : DefaultMaterial;
        var shapeIndex = _simulation.Shapes.Add(shape);
        var pose = new RigidPose(transform.Position, transform.Rotation);

        if (World.Has<StaticTag>(entity))
        {
            var staticDescription = new StaticDescription(pose, shapeIndex);
            var handle = _simulation.Statics.Add(staticDescription);
            CommandBuffer.AddComponent(entity, new PhysicsStatic { Handle = handle });
            _physicsService.Materials[new CollidableReference(handle)] = material;
        }
        else if (World.Has<KinematicTag>(entity))
        {
            var bodyDescription = BodyDescription.CreateKinematic(pose, shapeIndex, 0.01f);
            var handle = _simulation.Bodies.Add(bodyDescription);

            _physicsMap.MapBody(handle, entity);
            CommandBuffer.AddComponent(entity, new PhysicsBody { Handle = handle, IsKinematic = true });
            _physicsService.Materials[handle] = material;
        }
        else
        {
            var mass = transform.Scale.X * transform.Scale.Y * transform.Scale.Z;
            var bodyDescription = BodyDescription.CreateDynamic(pose, shape.ComputeInertia(mass), shapeIndex, 0.01f);
            var handle = _simulation.Bodies.Add(bodyDescription);

            _physicsMap.MapBody(handle, entity);
            CommandBuffer.AddComponent(entity, new PhysicsBody { Handle = handle, Mass = mass, IsKinematic = false });
            _physicsService.Materials[handle] = material;
        }
    }

    private void CreateBodies()
    {
        Query()
            .Without<PhysicsBody, PhysicsStatic>()
            .With<Transform, BoxCollider>()
            .ForEach((entity, ref transform, ref collider) =>
            {
                var scaledHalfExtents = collider.HalfExtents * transform.Scale;
                CreateBody(entity, transform, new Box(scaledHalfExtents.X * 2, scaledHalfExtents.Y * 2, scaledHalfExtents.Z * 2));
            });

        Query()
            .Without<PhysicsBody, PhysicsStatic>()
            .With<Transform, SphereCollider>()
            .ForEach((entity, ref transform, ref collider) =>
            {
                var scale = Math.Max(transform.Scale.X, Math.Max(transform.Scale.Y, transform.Scale.Z));
                CreateBody(entity, transform, new Sphere(collider.Radius * scale));
            });

        Query()
            .Without<PhysicsBody, PhysicsStatic>()
            .With<Transform, CapsuleCollider>()
            .ForEach((entity, ref transform, ref collider) =>
            {
                var radius = collider.Radius * Math.Max(transform.Scale.X, transform.Scale.Z);
                var length = collider.Length * transform.Scale.Y;
                CreateBody(entity, transform, new Capsule(radius, length));
            });

        Query()
            .Without<PhysicsBody, PhysicsStatic>()
            .With<Transform, CylinderCollider>()
            .ForEach((entity, ref transform, ref collider) =>
            {
                var radius = collider.Radius * Math.Max(transform.Scale.X, transform.Scale.Z);
                var length = collider.Length * transform.Scale.Y;
                CreateBody(entity, transform, new Cylinder(radius, length));
            });
    }

    private void CleanupBodies()
    {
        Query<PhysicsBody>().ForEach((entity, ref body) =>
        {
            if (!World.IsAlive(entity))
            {
                if (_simulation.Bodies.BodyExists(body.Handle))
                {
                    _simulation.Bodies.Remove(body.Handle);
                    _physicsMap.UnmapBody(body.Handle);
                }
            }
        });

        Query<PhysicsStatic>().ForEach((entity, ref staticBody) =>
        {
            if (!World.IsAlive(entity))
            {
                if (_simulation.Statics.StaticExists(staticBody.Handle))
                {
                    _simulation.Statics.Remove(staticBody.Handle);
                }
            }
        });
    }

    private void SyncTransformsFromPhysics()
    {
        ref var activeSet = ref _simulation.Bodies.ActiveSet;
        for (int i = 0; i < activeSet.Count; ++i)
        {
            var bodyHandle = activeSet.IndexToHandle[i];

            if (_physicsMap.TryGetEntity(bodyHandle, out var entity))
            {
                if (World.IsAlive(entity) && !World.Has<StaticTag>(entity) && World.Has<Transform>(entity))
                {
                    ref var transform = ref World.Ref<Transform>(entity);
                    var bodyLocation = _simulation.Bodies[bodyHandle].Pose;

                    transform.Position = bodyLocation.Position;
                    transform.Rotation = bodyLocation.Orientation;
                }
            }
        }
    }
}