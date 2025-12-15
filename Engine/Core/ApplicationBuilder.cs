using Engine.Core.Services;
using Engine.DataStructures;
using Engine.ECS.Abstractions;
using Engine.ECS.Archetypes;
using static System.Net.Mime.MediaTypeNames;

namespace Engine.Core;

public sealed class ApplicationBuilder
{
    private readonly List<ISystem> _systems = [];
    private IServiceRegistry? _serviceRegistry;

    private int _chunkCapacity = 256;
    private int _initialEntityCapacity = 1024;

    public ApplicationBuilder() { }

    public ApplicationBuilder AddSystem<T>() where T : ISystem, new()
    {
        _systems.Add(new T());
        return this;
    }

    public ApplicationBuilder AddService<T>(T service) where T : class
    {
        _serviceRegistry ??= new ArrayServiceContainer();
        _serviceRegistry.Register(service);
        return this;
    }

    public ApplicationBuilder UseServiceRegistry(IServiceRegistry serviceRegistry)
    {
        _serviceRegistry = serviceRegistry;
        return this;
    }

    public ApplicationBuilder WithChunkCapacity(int capacity)
    {
        _chunkCapacity = capacity;
        return this;
    }

    public ApplicationBuilder WithInitialEntityCapacity(int capacity)
    {
        _initialEntityCapacity = capacity;
        return this;
    }

    public Application Build()
    {
        var app = new Application(_serviceRegistry, application =>
            new ArchetypeWorld(
                application.Services,
                _chunkCapacity,
                _initialEntityCapacity
            )
        );
        app.AddSystems([.. _systems]);
        return app;
    }
}