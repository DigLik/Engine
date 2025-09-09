namespace Engine.ECS.Components;

public record struct CameraComponent
{
    public ProjectionType ProjectionType { get; private set; }
    public Vector2 ViewportSize { get; private set; }
    public float NearPlane { get; private set; }
    public float FarPlane { get; private set; }
    public bool IsMainCamera { get; set; }

    public float PerspectiveFieldOfView { get; private set; }
    public float OrthographicSize { get; private set; }

    public Matrix4x4 ViewMatrix { get; set; }

    public readonly Matrix4x4 ProjectionMatrix
    {
        get
        {
            float width = Math.Max(1e-6f, ViewportSize.X);
            float height = Math.Max(1e-6f, ViewportSize.Y);
            float aspectRatio = width / height;

            float nearZ = Math.Max(1e-4f, NearPlane);
            float farZ = Math.Max(nearZ + 1e-4f, FarPlane);

            return ProjectionType switch
            {
                ProjectionType.Perspective => Matrix4x4.CreatePerspectiveFieldOfView(
                    Math.Clamp(PerspectiveFieldOfView, 1e-3f, MathF.PI - 1e-3f),
                    aspectRatio, nearZ, farZ),

                ProjectionType.Orthographic => Matrix4x4.CreateOrthographic(
                    Math.Max(1e-4f, OrthographicSize) * aspectRatio,
                    Math.Max(1e-4f, OrthographicSize),
                    nearZ, farZ),

                _ => Matrix4x4.Identity
            };
        }
    }

    public static CameraComponent CreateDefault()
        => new()
        {
            ProjectionType = ProjectionType.Perspective,
            ViewportSize = new Vector2(800, 600),
            NearPlane = 0.1f,
            FarPlane = 1000f,
            IsMainCamera = true,
            PerspectiveFieldOfView = MathF.PI / 2,
            OrthographicSize = 10f,
            ViewMatrix = Matrix4x4.Identity
        };

    public void SetViewportSize(float width, float height)
    {
        if (width > 0 && height > 0)
            ViewportSize = new Vector2(width, height);
    }

    public void SetPerspective(float fieldOfView, float nearPlane, float farPlane)
    {
        ProjectionType = ProjectionType.Perspective;
        PerspectiveFieldOfView = fieldOfView;
        NearPlane = nearPlane;
        FarPlane = farPlane;
    }

    public void SetOrthographic(float size, float nearPlane, float farPlane)
    {
        ProjectionType = ProjectionType.Orthographic;
        OrthographicSize = size;
        NearPlane = nearPlane;
        FarPlane = farPlane;
    }
}