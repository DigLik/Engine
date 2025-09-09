namespace Engine.ECS;

public readonly struct Entity : IEquatable<Entity>
{
    public readonly uint Id;
    public readonly uint Generation;

    internal Entity(uint id, uint generation)
    {
        Id = id;
        Generation = generation;
    }

    public bool Equals(Entity other) => Id == other.Id && Generation == other.Generation;
    public override bool Equals(object? obj) => obj is Entity e && Equals(e);
    public override int GetHashCode() => HashCode.Combine(Id, Generation);

    public static bool operator ==(Entity a, Entity b) => a.Equals(b);
    public static bool operator !=(Entity a, Entity b) => !a.Equals(b);

    public override string ToString() => $"Entity(Id:{Id}, Gen:{Generation})";

    public static readonly Entity Null = new(0, 0);
}