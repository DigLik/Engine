using Engine.Core.Timing;
using Engine.ECS.Abstractions;

namespace Engine.Core.Abstractions;

public interface IFixedRunSystem : ISystem
{
    void FixedRun(IWorld world, in TimeSnapshot time);
}