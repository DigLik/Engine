using Engine.Core.Abstractions;

namespace Engine.ECS.Abstractions;

public interface IWorld : IEntityAllocator, IComponentStore;