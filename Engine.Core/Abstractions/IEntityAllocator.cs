namespace Engine.Core.Abstractions;

public interface IEntityAllocator : IEntityView
{
    Entity Create();
    void Destroy(Entity entity);
}