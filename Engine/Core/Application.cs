using Engine.Core.Services;
using Engine.DataStructures;
using Engine.ECS;
using Engine.ECS.Archetypes;

namespace Engine.Core;

public sealed class Application : IApplication, IDisposable
{
    private readonly List<ISystem> _systems = [];
    private readonly Time _time = new();
    private readonly CommandBuffer _commandBuffer = new();
    private bool _shouldClose;

    public IServiceRegistry Services { get; }
    public IWorldApi World { get; }

    internal Application(
        IServiceRegistry? services = null,
        TypeIndex? typeIndex = null)
    {
        var types = typeIndex ?? new TypeIndex();
        Services = services ?? new ArrayServiceContainer(types);

        World = new ArchetypeWorld(types, Services);

        Services.Register<IApplication>(this);
        Services.Register(World);
        Services.Register(_time);
        Services.Register(_commandBuffer);
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

        _time.Update(deltaTime);
        var timeSnapshot = TimeSnapshot.FromTime(_time);

        foreach (var system in _systems)
            system.Update(World, timeSnapshot, Services);

        _commandBuffer.Playback(World);

        return !_shouldClose;
    }

    public void RequestClose() => _shouldClose = true;

    public void Dispose() => (World as IDisposable)?.Dispose();
}