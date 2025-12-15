using BepuPhysics;

namespace Engine.ECS.Components.Physics;

public record struct PhysicsBody()
{
    public BodyHandle Handle;
    public float Mass = 1.0f;
    public bool IsKinematic;
}

public record struct PhysicsStatic
{
    public StaticHandle Handle;
}