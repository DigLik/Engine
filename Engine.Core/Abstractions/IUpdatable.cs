using Engine.Core.Timing;
using Engine.ECS.Abstractions;

namespace Engine.Core.Abstractions;

public interface IUpdatable
{
    void Update(IWorld world, in TimeSnapshot time);
}