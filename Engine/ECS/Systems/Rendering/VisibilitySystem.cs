using Engine.ECS.Components.Rendering;
using Engine.Rendering;
using Engine.Rendering.Data;

namespace Engine.ECS.Systems.Rendering;

public sealed class VisibilitySystem : SystemBase
{
    private ActiveCameraBuffer _cameraBuffer = null!;

    public override void OnInitialize()
    {
        _cameraBuffer = GetService<ActiveCameraBuffer>();
    }

    public override void OnUpdate()
    {
        if (!_cameraBuffer.HasCamera || _cameraBuffer.Frustum == null)
        {
            Query<Visibility>().ForEach((ref visibility) =>
            {
                visibility.IsVisibleInFrustum = false;
            });
            return;
        }

        var frustum = _cameraBuffer.Frustum;

        Query<LocalToWorld, LocalBounds, Visibility>()
            .ForEach((ref ltw, ref bounds, ref visibility) =>
            {
                if (!visibility.IsEnabled)
                {
                    visibility.IsVisibleInFrustum = false;
                    return;
                }

                var sphere = CalculateWorldBoundingSphere(ltw.Value, bounds);
                visibility.IsVisibleInFrustum = frustum.Contains(sphere) != ContainmentType.Disjoint;
            });
    }

    private static BoundingSphere CalculateWorldBoundingSphere(in Matrix4x4 worldMatrix, in LocalBounds localBounds)
    {
        var localCenter = (localBounds.Min + localBounds.Max) * 0.5f;
        var localRadius = Vector3.Distance(localBounds.Max, localCenter);

        var worldCenter = Vector3.Transform(localCenter, worldMatrix);

        var scaleX = new Vector3(worldMatrix.M11, worldMatrix.M12, worldMatrix.M13).Length();
        var scaleY = new Vector3(worldMatrix.M21, worldMatrix.M22, worldMatrix.M23).Length();
        var scaleZ = new Vector3(worldMatrix.M31, worldMatrix.M32, worldMatrix.M33).Length();
        var maxScale = Math.Max(scaleX, Math.Max(scaleY, scaleZ));

        return new BoundingSphere(worldCenter, localRadius * maxScale);
    }
}