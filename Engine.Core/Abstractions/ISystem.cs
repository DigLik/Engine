using Engine.Core.Abstractions;

namespace Engine.ECS.Abstractions;

public interface ISystem : IInitializeSystem, IUpdatable;