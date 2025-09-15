using Engine.ECS.Components;
using Engine.ECS.Components.Rendering;
using Engine.Rendering;
using Engine.Rendering.Data;

namespace Engine.ECS.Systems.Rendering;

public sealed class CameraSystem : SystemBase
{
    private ActiveCameraBuffer _cameraBuffer = null!;

    public override void OnInitialize()
    {
        _cameraBuffer = GetService<ActiveCameraBuffer>();
    }

    public override void OnUpdate()
    {
        _cameraBuffer.HasCamera = false;

        Query<Camera, LocalToWorld>().ForEach((entity, ref cam, ref transform) =>
        {
            if (!cam.IsMain) return;

            Matrix4x4.Invert(transform.Value, out var viewMatrix);
            _cameraBuffer.ViewMatrix = viewMatrix;
            _cameraBuffer.ProjectionMatrix = CalculateProjection(cam);
            _cameraBuffer.Frustum = new BoundingFrustum(_cameraBuffer.ViewMatrix * _cameraBuffer.ProjectionMatrix);
            _cameraBuffer.HasCamera = true;
        });
    }

    private static Matrix4x4 CalculateProjection(in Camera cam)
    {
        float aspectRatio = cam.ViewportSize.X / cam.ViewportSize.Y;
        if (float.IsNaN(aspectRatio) || float.IsInfinity(aspectRatio))
            aspectRatio = 1.0f;

        return cam.ProjectionType switch
        {
            ProjectionType.Perspective => Matrix4x4.CreatePerspectiveFieldOfView(cam.FieldOfView, aspectRatio, cam.NearPlane, cam.FarPlane),
            ProjectionType.Orthographic => Matrix4x4.CreateOrthographic(cam.OrthographicSize * aspectRatio, cam.OrthographicSize, cam.NearPlane, cam.FarPlane),
            _ => Matrix4x4.Identity
        };
    }
}