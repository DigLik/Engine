using Engine.ECS.Abstractions;

namespace Engine.Core.Abstraction;

public interface IScene
{
    void Initialize(IWorld world);
}