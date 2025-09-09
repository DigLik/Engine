namespace Engine.ECS.Components;

public record struct TransformComponent
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public Quaternion Rotation { get; set; } = Quaternion.Identity;

    public TransformComponent() { }
    public TransformComponent(Vector3 position)
    {
        Position = position;
    }
    public TransformComponent(Vector3 position, Vector3 scale)
    {
        Position = position;
        Scale = scale;
    }
    public TransformComponent(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
    public TransformComponent(Vector3 position, Vector3 scale, Quaternion rotation)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
    }
}