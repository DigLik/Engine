namespace Engine.ECS.Components.Rendering;

public record struct Camera
{
    public ProjectionType ProjectionType;
    public float FieldOfView;
    public float OrthographicSize;
    public float NearPlane;
    public float FarPlane;
    public Vector2 ViewportSize;
    public bool IsMain;
}