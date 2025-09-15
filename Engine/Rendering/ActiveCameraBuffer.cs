using Engine.Rendering.Data;

namespace Engine.Rendering;

public sealed class ActiveCameraBuffer
{
    public Matrix4x4 ViewMatrix { get; set; }
    public Matrix4x4 ProjectionMatrix { get; set; }
    public BoundingFrustum? Frustum { get; set; }
    public bool HasCamera { get; set; }
}