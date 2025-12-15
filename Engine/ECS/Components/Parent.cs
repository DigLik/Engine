namespace Engine.ECS.Components;

internal record struct Parent(Entity Target, bool CascadeDelete);