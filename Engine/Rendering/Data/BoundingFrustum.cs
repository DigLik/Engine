namespace Engine.Rendering.Data;

public sealed class BoundingFrustum
{
    private readonly Plane[] _planes = new Plane[6];

    public BoundingFrustum(Matrix4x4 viewProjection)
    {
        _planes[0] = Plane.Normalize(new Plane(
            viewProjection.M14 + viewProjection.M11,
            viewProjection.M24 + viewProjection.M21,
            viewProjection.M34 + viewProjection.M31,
            viewProjection.M44 + viewProjection.M41));

        _planes[1] = Plane.Normalize(new Plane(
            viewProjection.M14 - viewProjection.M11,
            viewProjection.M24 - viewProjection.M21,
            viewProjection.M34 - viewProjection.M31,
            viewProjection.M44 - viewProjection.M41));

        _planes[2] = Plane.Normalize(new Plane(
            viewProjection.M14 + viewProjection.M12,
            viewProjection.M24 + viewProjection.M22,
            viewProjection.M34 + viewProjection.M32,
            viewProjection.M44 + viewProjection.M42));

        _planes[3] = Plane.Normalize(new Plane(
            viewProjection.M14 - viewProjection.M12,
            viewProjection.M24 - viewProjection.M22,
            viewProjection.M34 - viewProjection.M32,
            viewProjection.M44 - viewProjection.M42));

        _planes[4] = Plane.Normalize(new Plane(
            viewProjection.M13,
            viewProjection.M23,
            viewProjection.M33,
            viewProjection.M43));

        _planes[5] = Plane.Normalize(new Plane(
            viewProjection.M14 - viewProjection.M13,
            viewProjection.M24 - viewProjection.M23,
            viewProjection.M34 - viewProjection.M33,
            viewProjection.M44 - viewProjection.M43));
    }

    public ContainmentType Contains(BoundingSphere sphere)
    {
        bool allIn = true;
        foreach (var plane in _planes)
        {
            float distance = Plane.DotCoordinate(plane, sphere.Center);
            if (distance < -sphere.Radius)
                return ContainmentType.Disjoint;
            if (distance < sphere.Radius)
                allIn = false;
        }
        return allIn ? ContainmentType.Contains : ContainmentType.Intersects;
    }
}

public enum ContainmentType { Disjoint, Contains, Intersects }

public readonly record struct BoundingSphere(Vector3 Center, float Radius);