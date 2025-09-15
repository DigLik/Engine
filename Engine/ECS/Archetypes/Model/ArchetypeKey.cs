namespace Engine.ECS.Archetypes.Model;

public readonly struct ArchetypeKey(TypeMask mask) : IEquatable<ArchetypeKey>
{
    public readonly TypeMask Mask = mask;

    public bool Equals(ArchetypeKey other) => Mask.Equals(other.Mask);
    public override bool Equals(object? obj) => obj is ArchetypeKey k && Equals(k);
    public override int GetHashCode() => Mask.GetHashCode();

    public static bool operator ==(ArchetypeKey a, ArchetypeKey b) => a.Equals(b);
    public static bool operator !=(ArchetypeKey a, ArchetypeKey b) => !a.Equals(b);
}