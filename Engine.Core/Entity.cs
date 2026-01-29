namespace Engine.Core;

public readonly record struct Entity(uint Id, uint Generation)
{
    public static readonly Entity None = new(0, 0);
    public bool IsNone => Id == 0;
}