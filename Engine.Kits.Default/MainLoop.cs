using Engine.Core.Abstractions;
using Engine.ECS.Abstractions;
using System.Diagnostics;

namespace Engine.Kits.Default;

public sealed class MainLoop(IWorld world, ITimeProvider time)
{
    private readonly List<ISystem> _systems = [];

    private readonly List<IRunSystem> _runSystems = [];
    private readonly List<IFixedRunSystem> _fixedSystems = [];

    private bool _running;
    private TimeSpan _accumulator;

    public void AddSystem(ISystem system)
    {
        _systems.Add(system);

        if (system is IRunSystem rs)
            _runSystems.Add(rs);
        if (system is IFixedRunSystem fs)
            _fixedSystems.Add(fs);

        if (system is IInitializeSystem initSys)
            initSys.Initialize(world);
    }

    public void Run()
    {
        _running = true;
        time.Reset();
        var stopwatch = Stopwatch.StartNew();

        while (_running)
        {
            var rawDelta = stopwatch.Elapsed;
            stopwatch.Restart();

            time.Tick(rawDelta);
            var snapshot = time.GetSnapshot();

            var dt = rawDelta * snapshot.TimeScale;
            if (dt > time.MaxDeltaTime)
                dt = time.MaxDeltaTime;

            _accumulator += dt;
            while (_accumulator >= time.FixedStep)
            {
                var fixedSnap = snapshot with { DeltaTime = time.FixedStep };

                foreach (var system in _fixedSystems)
                    system.FixedRun(world, fixedSnap);

                _accumulator -= time.FixedStep;
            }

            var frameSnap = snapshot with { DeltaTime = dt };
            foreach (var system in _runSystems)
                system.Run(world, frameSnap);
        }
    }

    public void Stop() => _running = false;
}