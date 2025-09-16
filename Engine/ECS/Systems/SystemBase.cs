using Engine.Core.Services;
using Engine.Core.Timing;
using Engine.ECS.Abstractions;
using Engine.ECS.Archetypes.QueryDefinition;
using Engine.ECS.Querying;

namespace Engine.ECS;

public abstract partial class SystemBase : ISystem
{
    private readonly Dictionary<int, Query> _queryCache = [];

    protected IWorldApi World { get; private set; } = null!;
    protected IServiceRegistry Services { get; private set; } = null!;
    protected TimeSnapshot Time { get; private set; }
    protected CommandBuffer CommandBuffer { get; private set; } = null!;
    protected QueryRegistry QueryRegistry { get; private set; } = null!;

    void ISystem.Initialize(IWorldApi world, IServiceRegistry services)
    {
        World = world;
        Services = services;
        CommandBuffer = services.Resolve<CommandBuffer>();
        QueryRegistry = services.Resolve<QueryRegistry>();
        OnInitialize();
    }

    void ISystem.Update(IWorldApi world, TimeSnapshot time, IServiceRegistry services)
    {
        Time = time;
        OnUpdate();
    }

    public virtual void OnInitialize() { }

    public abstract void OnUpdate();

    protected T GetService<T>() where T : class => Services.Resolve<T>();
}