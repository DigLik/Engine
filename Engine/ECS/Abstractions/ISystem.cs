using Engine.Core.Services;
using Engine.Core.Timing;

namespace Engine.ECS.Abstractions;

public interface ISystem
{
    void Initialize(IWorldApi world, IServiceRegistry services);

    void Update(IWorldApi world, TimeSnapshot time, IServiceRegistry services);
}