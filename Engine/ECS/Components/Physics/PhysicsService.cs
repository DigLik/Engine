using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;

namespace Engine.ECS.Components.Physics;

public sealed class PhysicsService : IDisposable
{
    public Simulation Simulation { get; }
    public BufferPool BufferPool { get; }
    public ThreadDispatcher ThreadDispatcher { get; }
    public CollidableProperty<PhysicsMaterial> Materials { get; }

    public PhysicsService()
    {
        BufferPool = new BufferPool();
        Materials = new CollidableProperty<PhysicsMaterial>();

        var narrowPhaseCallbacks = new NarrowPhaseCallbacks(Materials);
        var poseIntegratorCallbacks = new PoseIntegratorCallbacks(
            new Vector3(0, -9.81f, 0),
            angularDamping: 0.5f,
            sleepThreshold: 0.01f);

        var solveDescription = new SolveDescription(16, 4);

        Simulation = Simulation.Create(
            BufferPool,
            narrowPhaseCallbacks,
            poseIntegratorCallbacks,
            solveDescription);

        int threadCount = Math.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : 1);
        ThreadDispatcher = new ThreadDispatcher(threadCount);
    }

    public void Dispose()
    {
        Simulation.Dispose();
        BufferPool.Clear();
        ThreadDispatcher.Dispose();
    }

    private readonly struct NarrowPhaseCallbacks(CollidableProperty<PhysicsMaterial> materials) : INarrowPhaseCallbacks
    {
        private readonly CollidableProperty<PhysicsMaterial> _materials = materials;

        public void Initialize(Simulation simulation)
        {
            _materials.Initialize(simulation);
        }

        public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b, ref float speculativeMargin)
        {
            return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
        }

        public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB) => true;

        public unsafe bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial)
            where TManifold : unmanaged, IContactManifold<TManifold>
        {
            var materialA = _materials[pair.A];
            var materialB = _materials[pair.B];

            pairMaterial.FrictionCoefficient = (materialA.Friction + materialB.Friction) * 0.5f;
            pairMaterial.MaximumRecoveryVelocity = MathF.Max(materialA.Bounciness, materialB.Bounciness);

            pairMaterial.SpringSettings = new SpringSettings(
                (materialA.SpringSettings.Frequency + materialB.SpringSettings.Frequency) * 0.5f,
                (materialA.SpringSettings.DampingRatio + materialB.SpringSettings.DampingRatio) * 0.5f);

            return true;
        }

        public unsafe bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold) => true;

        public readonly void Dispose() { }
    }


    private struct PoseIntegratorCallbacks(Vector3 gravity, float linearDamping = 0.03f, float angularDamping = 0.03f, float sleepThreshold = 0.01f) : IPoseIntegratorCallbacks
    {
        private readonly Vector3 _gravity = gravity;
        private readonly float _linearDamping = linearDamping;
        private readonly float _angularDamping = angularDamping;

        public readonly float SleepThreshold => sleepThreshold;

        private Vector3Wide _gravityWide = default;
        private Vector<float> _linearDampingWide = default;
        private Vector<float> _angularDampingWide = default;

        public readonly AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;
        public readonly bool AllowSubstepsForUnconstrainedBodies => false;
        public readonly bool IntegrateVelocityForKinematics => false;

        public void Initialize(Simulation simulation)
        {
            _gravityWide = Vector3Wide.Broadcast(_gravity);
            _linearDampingWide = new Vector<float>(_linearDamping);
            _angularDampingWide = new Vector<float>(_angularDamping);
        }

        public readonly void PrepareForIntegration(float dt)
        {
        }

        public readonly void IntegrateVelocity(
            Vector<int> bodyIndices,
            Vector3Wide position,
            QuaternionWide orientation,
            BodyInertiaWide localInertia,
            Vector<int> integrationMask,
            int workerIndex,
            Vector<float> dt,
            ref BodyVelocityWide velocity)
        {
            velocity.Linear += _gravityWide * dt;
            velocity.Linear *= Vector<float>.One / (Vector<float>.One + dt * _linearDampingWide);
            velocity.Angular *= Vector<float>.One / (Vector<float>.One + dt * _angularDampingWide);
        }
    }
}