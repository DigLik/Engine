namespace Engine.ECS.Components.Rendering;

public record struct Camera
{
    public ProjectionType ProjectionType { get; set; }
    public float FieldOfView { get; set; }
    public float OrthographicSize { get; set; }
    public float NearPlane { get; set; }
    public float FarPlane { get; set; }
    public Vector2 ViewportSize { get; set; }
    public bool IsMain { get; set; }
}