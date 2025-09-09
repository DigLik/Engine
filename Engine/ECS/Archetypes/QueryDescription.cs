namespace Engine.ECS.Archetypes;

public readonly struct QueryDescription(TypeMask withMask, TypeMask withoutMask)
{
    public readonly TypeMask WithMask { get; } = withMask;
    public readonly TypeMask WithoutMask { get; } = withoutMask;
}