using Engine.ECS.Archetypes.Model;

namespace Engine.ECS.Archetypes.QueryDefinition;

public readonly struct QueryDescription(TypeMask withMask, TypeMask withoutMask)
{
    public readonly TypeMask WithMask { get; } = withMask;
    public readonly TypeMask WithoutMask { get; } = withoutMask;
}