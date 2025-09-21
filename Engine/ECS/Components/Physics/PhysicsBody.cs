using BepuPhysics;

namespace Engine.ECS.Components.Physics;

public record struct PhysicsBody()
{
    public BodyHandle Handle { get; set; }
    public float Mass { get; set; } = 1.0f;
    public bool IsKinematic { get; set; }
}

public record struct PhysicsStatic
{
    public StaticHandle Handle { get; set; }
}