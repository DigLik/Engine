namespace Engine.ECS.Components.Physics;

public record struct BoxCollider()
{
    public Vector3 HalfExtents = new(0.5f);
}

public record struct SphereCollider()
{
    public float Radius = 0.5f;
}

public record struct CapsuleCollider()
{
    public float Radius = 0.5f;
    public float Length = 1.0f;
}

public record struct CylinderCollider()
{
    public float Radius = 0.5f;
    public float Length = 1.0f;
}
