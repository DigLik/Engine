namespace Engine.ECS;

public sealed class CommandBuffer
{
    private interface ICommand
    {
        void Execute(IWorldApi world);
    }

    private readonly List<ICommand> _commands = new(128);

    private readonly record struct AddComponentCommand<T>(Entity Entity, T Component) : ICommand where T : unmanaged
    {
        public void Execute(IWorldApi world) => world.Add(Entity, Component);
    }
    private readonly record struct RemoveComponentCommand<T>(Entity Entity) : ICommand where T : unmanaged
    {
        public void Execute(IWorldApi world) => world.Remove<T>(Entity);
    }
    private readonly record struct DestroyEntityCommand(Entity Entity) : ICommand
    {
        public void Execute(IWorldApi world) => world.DestroyEntity(Entity);
    }
    private readonly record struct SetParentCommand(Entity Child, Entity Parent, bool Cascade) : ICommand
    {
        public void Execute(IWorldApi world) => world.SetParent(Child, Parent, Cascade);
    }
    private readonly record struct RemoveParentCommand(Entity Child) : ICommand
    {
        public void Execute(IWorldApi world) => world.RemoveParent(Child);
    }

    public void AddComponent<T>(Entity entity, T component = default) where T : unmanaged => _commands.Add(new AddComponentCommand<T>(entity, component));
    public void RemoveComponent<T>(Entity entity) where T : unmanaged => _commands.Add(new RemoveComponentCommand<T>(entity));
    public void DestroyEntity(Entity entity) => _commands.Add(new DestroyEntityCommand(entity));
    public void SetParent(Entity child, Entity parent, bool cascade = true) => _commands.Add(new SetParentCommand(child, parent, cascade));
    public void RemoveParent(Entity child) => _commands.Add(new RemoveParentCommand(child));

    public void Playback(IWorldApi world)
    {
        foreach (var command in _commands)
            command.Execute(world);

        _commands.Clear();
    }
}