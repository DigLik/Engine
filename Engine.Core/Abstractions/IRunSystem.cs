using Engine.Core.Timing;
using Engine.ECS.Abstractions;

namespace Engine.Core.Abstractions;

public interface IRunSystem : ISystem
{
    void Run(IWorld world, in TimeSnapshot time);
}