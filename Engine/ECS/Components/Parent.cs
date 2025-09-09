namespace Engine.ECS.Components;

internal readonly struct Parent
{
    internal readonly Entity Target;
    internal readonly bool CascadeDelete;

    internal Parent(Entity target, bool cascadeDelete)
    {
        Target = target;
        CascadeDelete = cascadeDelete;
    }
}