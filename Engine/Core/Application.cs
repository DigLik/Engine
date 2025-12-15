using Engine.Core.Memory;
using Engine.Core.Services;
using Engine.Core.Timing;
using Engine.ECS;
using Engine.ECS.Abstractions;
using Engine.ECS.Archetypes;
using Engine.ECS.Querying;
using Engine.Input;

namespace Engine.Core;

public sealed class Application : IApplication, IDisposable
{
    private readonly List<ISystem> _systems = [];
    private readonly Time _time = new();
    private readonly CommandBuffer _commandBuffer = new();
    private readonly LinearAllocator _frameAllocator = new(16 * 1024 * 1024); // 16 MB
    private bool _shouldClose;

    private IInputService? _inputService;
    public IServiceRegistry Services { get; }
    public IWorldApi World { get; }

    internal Application(
        IServiceRegistry? services = null,
        Func<Application, IWorldApi>? worldFactory = null)
    {
        Services = services ?? new ArrayServiceContainer();

        Services.Register<IApplication>(this);

        World = worldFactory?.Invoke(this) ?? new ArchetypeWorld(Services);

        Services.Register(World);
        Services.Register(_time);
        Services.Register(_commandBuffer);
        Services.Register(new QueryRegistry(World));
        Services.Register(_frameAllocator);
    }

    public static ApplicationBuilder CreateBuilder() => new();

    public void AddSystems(params ISystem[] systems)
    {
        foreach (var system in systems)
        {
            system.Initialize(World, Services);
            _systems.Add(system);
        }
    }

    public bool Tick(float deltaTime)
    {
        if (_shouldClose)
            return false;

        if (deltaTime < 0f)
            deltaTime = 0f;

        _frameAllocator.Reset();
        _time.Update(deltaTime);
        var timeSnapshot = TimeSnapshot.FromTime(_time);

        foreach (var system in _systems)
            system.Update(World, timeSnapshot, Services);

        _commandBuffer.Playback(World);

        if (_inputService == null)
            Services.TryResolve(out _inputService);

        _inputService?.Update();

        return !_shouldClose;
    }

    public void RequestClose() => _shouldClose = true;

    public void Dispose()
    {
        _frameAllocator.Dispose();
        (World as IDisposable)?.Dispose();
    }
}