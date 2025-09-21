namespace Engine.ECS.Components.Physics;

public record struct BoxCollider()
{
    public Vector3 HalfExtents { get; set; } = new(0.5f);
}

public record struct SphereCollider()
{
    public float Radius { get; set; } = 0.5f;
}

public record struct CapsuleCollider()
{
    public float Radius { get; set; } = 0.5f;
    public float Length { get; set; } = 1.0f;
}

public record struct CylinderCollider()
{
    public float Radius { get; set; } = 0.5f;
    public float Length { get; set; } = 1.0f;
}
