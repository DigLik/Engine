using Engine.Core.Services;
using Engine.DataStructures;
using Engine.ECS.Abstractions;

namespace Engine.Core;

public sealed class ApplicationBuilder
{
    private readonly List<ISystem> _systems = [];
    private IServiceRegistry? _serviceRegistry;
    private TypeIndex? _typeIndex;

    public ApplicationBuilder() { }

    public ApplicationBuilder AddSystem<T>() where T : ISystem, new()
    {
        _systems.Add(new T());
        return this;
    }

    public ApplicationBuilder AddService<T>(T service) where T : class
    {
        _serviceRegistry ??= new ArrayServiceContainer(_typeIndex ??= new TypeIndex());
        _serviceRegistry.Register(service);
        return this;
    }

    public ApplicationBuilder UseTypeIndex(TypeIndex typeIndex)
    {
        _typeIndex = typeIndex;
        return this;
    }

    public ApplicationBuilder UseServiceRegistry(IServiceRegistry serviceRegistry)
    {
        _serviceRegistry = serviceRegistry;
        return this;
    }

    public Application Build()
    {
        var app = new Application(_serviceRegistry, _typeIndex);
        app.AddSystems([.. _systems]);
        return app;
    }
}