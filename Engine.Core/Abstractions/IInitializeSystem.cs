using Engine.ECS.Abstractions;

namespace Engine.Core.Abstractions;

public interface IInitializeSystem
{
    void Initialize(IWorld world);
}