using Engine.Core.Services;

namespace Engine.ECS;

public interface ISystem
{
    void Initialize(IWorldApi world, IServiceRegistry services);

    void Update(IWorldApi world, TimeSnapshot time, IServiceRegistry services);
}