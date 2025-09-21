using Engine.Rendering.Data;

namespace TestGame;

internal static class MeshGenerator
{
    public static void CreateCube(float size, out Vertex[] vertices, out uint[] indices)
    {
        float s = size / 2f;

        vertices =
        [
            new Vertex(new Vector3(-s, -s, s), new Vector3(0, 0, 1), new Vector2(0, 1)),
            new Vertex(new Vector3(s, -s, s), new Vector3(0, 0, 1), new Vector2(1, 1)),
            new Vertex(new Vector3(s, s, s), new Vector3(0, 0, 1), new Vector2(1, 0)),
            new Vertex(new Vector3(-s, s, s), new Vector3(0, 0, 1), new Vector2(0, 0)),
            new Vertex(new Vector3(s, -s, -s), new Vector3(0, 0, -1), new Vector2(0, 1)),
            new Vertex(new Vector3(-s, -s, -s), new Vector3(0, 0, -1), new Vector2(1, 1)),
            new Vertex(new Vector3(-s, s, -s), new Vector3(0, 0, -1), new Vector2(1, 0)),
            new Vertex(new Vector3(s, s, -s), new Vector3(0, 0, -1), new Vector2(0, 0)),
            new Vertex(new Vector3(-s, -s, -s), new Vector3(-1, 0, 0), new Vector2(0, 1)),
            new Vertex(new Vector3(-s, -s, s), new Vector3(-1, 0, 0), new Vector2(1, 1)),
            new Vertex(new Vector3(-s, s, s), new Vector3(-1, 0, 0), new Vector2(1, 0)),
            new Vertex(new Vector3(-s, s, -s), new Vector3(-1, 0, 0), new Vector2(0, 0)),
            new Vertex(new Vector3(s, -s, s), new Vector3(1, 0, 0), new Vector2(0, 1)),
            new Vertex(new Vector3(s, -s, -s), new Vector3(1, 0, 0), new Vector2(1, 1)),
            new Vertex(new Vector3(s, s, -s), new Vector3(1, 0, 0), new Vector2(1, 0)),
            new Vertex(new Vector3(s, s, s), new Vector3(1, 0, 0), new Vector2(0, 0)),
            new Vertex(new Vector3(-s, s, s), new Vector3(0, 1, 0), new Vector2(0, 1)),
            new Vertex(new Vector3(s, s, s), new Vector3(0, 1, 0), new Vector2(1, 1)),
            new Vertex(new Vector3(s, s, -s), new Vector3(0, 1, 0), new Vector2(1, 0)),
            new Vertex(new Vector3(-s, s, -s), new Vector3(0, 1, 0), new Vector2(0, 0)),
            new Vertex(new Vector3(-s, -s, -s), new Vector3(0, -1, 0), new Vector2(0, 1)),
            new Vertex(new Vector3(s, -s, -s), new Vector3(0, -1, 0), new Vector2(1, 1)),
            new Vertex(new Vector3(s, -s, s), new Vector3(0, -1, 0), new Vector2(1, 0)),
            new Vertex(new Vector3(-s, -s, s), new Vector3(0, -1, 0), new Vector2(0, 0))
        ];

        indices = new uint[36];
        for (uint i = 0; i < 6; i++)
        {
            uint offset = i * 4;
            indices[i * 6 + 0] = offset + 0;
            indices[i * 6 + 1] = offset + 1;
            indices[i * 6 + 2] = offset + 2;
            indices[i * 6 + 3] = offset + 0;
            indices[i * 6 + 4] = offset + 2;
            indices[i * 6 + 5] = offset + 3;
        }
    }

    public static void CreateCylinder(float radius, float length, int segments, out Vertex[] vertices, out uint[] indices)
    {
        var vertexList = new List<Vertex>();
        var indexList = new List<uint>();
        float halfLength = length / 2;

        var topCenter = new Vector3(0, halfLength, 0);
        var bottomCenter = new Vector3(0, -halfLength, 0);

        vertexList.Add(new Vertex(topCenter, Vector3.UnitY, new Vector2(0.5f, 0.5f)));
        uint topCenterIndex = (uint)vertexList.Count - 1;
        vertexList.Add(new Vertex(bottomCenter, -Vector3.UnitY, new Vector2(0.5f, 0.5f)));
        uint bottomCenterIndex = (uint)vertexList.Count - 1;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 2.0f * MathF.PI / segments;
            float x = MathF.Cos(angle) * radius;
            float z = MathF.Sin(angle) * radius;
            var normal = Vector3.Normalize(new Vector3(x, 0, z));

            vertexList.Add(new Vertex(new Vector3(x, halfLength, z), normal, new Vector2((float)i / segments, 0)));
            vertexList.Add(new Vertex(new Vector3(x, -halfLength, z), normal, new Vector2((float)i / segments, 1)));

            vertexList.Add(new Vertex(new Vector3(x, halfLength, z), Vector3.UnitY, new Vector2(x / (2 * radius) + 0.5f, z / (2 * radius) + 0.5f)));
            vertexList.Add(new Vertex(new Vector3(x, -halfLength, z), -Vector3.UnitY, new Vector2(x / (2 * radius) + 0.5f, z / (2 * radius) + 0.5f)));
        }

        uint baseIndex = 2;
        for (uint i = 0; i < segments; i++)
        {
            uint i0 = baseIndex + i * 4;
            uint i1 = baseIndex + (i + 1) * 4;

            indexList.Add(i0); indexList.Add(i1); indexList.Add(i0 + 1);
            indexList.Add(i1); indexList.Add(i1 + 1); indexList.Add(i0 + 1);

            indexList.Add(topCenterIndex); indexList.Add(i1 + 2); indexList.Add(i0 + 2);
            indexList.Add(bottomCenterIndex); indexList.Add(i0 + 3); indexList.Add(i1 + 3);
        }

        vertices = [.. vertexList];
        indices = [.. indexList];
    }

    public static void CreateCapsule(float radius, float length, int segments, int rings, out Vertex[] vertices, out uint[] indices)
    {
        var vertexList = new List<Vertex>();
        var indexList = new List<uint>();
        float halfLength = length / 2;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 2.0f * MathF.PI / segments;
            float x = MathF.Cos(angle) * radius;
            float z = MathF.Sin(angle) * radius;
            var normal = Vector3.Normalize(new Vector3(x, 0, z));

            float u = (float)i / segments;

            float vTop = 0.25f;
            float vBottom = 0.75f;

            vertexList.Add(new Vertex(new Vector3(x, halfLength, z), normal, new Vector2(u, vTop)));
            vertexList.Add(new Vertex(new Vector3(x, -halfLength, z), normal, new Vector2(u, vBottom)));
        }

        for (uint i = 0; i < segments; i++)
        {
            uint i0 = i * 2;
            uint i1 = (i + 1) * 2;
            indexList.Add(i0); indexList.Add(i1); indexList.Add(i0 + 1);
            indexList.Add(i1); indexList.Add(i1 + 1); indexList.Add(i0 + 1);
        }

        uint baseVertex = (uint)vertexList.Count;
        for (int j = 0; j <= rings; j++)
        {
            float pitch = j * MathF.PI / (2 * rings);
            float y_offset = MathF.Sin(pitch);
            float r_scale = MathF.Cos(pitch);

            for (int i = 0; i <= segments; i++)
            {
                float yaw = i * 2.0f * MathF.PI / segments;
                float x = MathF.Cos(yaw) * r_scale;
                float z = MathF.Sin(yaw) * r_scale;
                var normal = new Vector3(x, y_offset, z);

                float u = (float)i / segments;

                float vTop = 0.25f - (y_offset * 0.25f);
                float vBottom = 0.75f + (y_offset * 0.25f);

                vertexList.Add(new Vertex(new Vector3(x * radius, halfLength + y_offset * radius, z * radius), normal, new Vector2(u, vTop)));
                vertexList.Add(new Vertex(new Vector3(x * radius, -halfLength - y_offset * radius, z * radius), new Vector3(x, -y_offset, z), new Vector2(u, vBottom)));
            }
        }

        for (uint j = 0; j < rings; j++)
        {
            for (uint i = 0; i < segments; i++)
            {
                uint row1 = j * (uint)(segments + 1) * 2;
                uint row2 = (j + 1) * (uint)(segments + 1) * 2;
                uint i0 = i * 2;
                uint i1 = (i + 1) * 2;

                indexList.Add(baseVertex + row1 + i0);
                indexList.Add(baseVertex + row2 + i0);
                indexList.Add(baseVertex + row2 + i1);
                indexList.Add(baseVertex + row1 + i0);
                indexList.Add(baseVertex + row2 + i1);
                indexList.Add(baseVertex + row1 + i1);

                indexList.Add(baseVertex + row1 + i0 + 1);
                indexList.Add(baseVertex + row2 + i1 + 1);
                indexList.Add(baseVertex + row2 + i0 + 1);
                indexList.Add(baseVertex + row1 + i0 + 1);
                indexList.Add(baseVertex + row1 + i1 + 1);
                indexList.Add(baseVertex + row2 + i1 + 1);
            }
        }

        vertices = [.. vertexList];
        indices = [.. indexList];
    }

    public static void CreateIcosphere(float radius, int subdivisions, out Vertex[] vertices, out uint[] indices)
    {
        var vertexList = new List<Vector3>();
        var indexList = new List<uint>();
        var midPointCache = new Dictionary<long, uint>();

        float t = (1.0f + MathF.Sqrt(5.0f)) / 2.0f;

        vertexList.Add(Vector3.Normalize(new Vector3(-1, t, 0)));
        vertexList.Add(Vector3.Normalize(new Vector3(1, t, 0)));
        vertexList.Add(Vector3.Normalize(new Vector3(-1, -t, 0)));
        vertexList.Add(Vector3.Normalize(new Vector3(1, -t, 0)));

        vertexList.Add(Vector3.Normalize(new Vector3(0, -1, t)));
        vertexList.Add(Vector3.Normalize(new Vector3(0, 1, t)));
        vertexList.Add(Vector3.Normalize(new Vector3(0, -1, -t)));
        vertexList.Add(Vector3.Normalize(new Vector3(0, 1, -t)));

        vertexList.Add(Vector3.Normalize(new Vector3(t, 0, -1)));
        vertexList.Add(Vector3.Normalize(new Vector3(t, 0, 1)));
        vertexList.Add(Vector3.Normalize(new Vector3(-t, 0, -1)));
        vertexList.Add(Vector3.Normalize(new Vector3(-t, 0, 1)));

        indexList.AddRange(
        [
            0, 11, 5, 0, 5, 1, 0, 1, 7, 0, 7, 10, 0, 10, 11,
            1, 5, 9, 5, 11, 4, 11, 10, 2, 10, 7, 6, 7, 1, 8,
            3, 9, 4, 3, 4, 2, 3, 2, 6, 3, 6, 8, 3, 8, 9,
            4, 9, 5, 2, 4, 11, 6, 2, 10, 8, 6, 7, 9, 8, 1
        ]);

        for (int i = 0; i < subdivisions; i++)
        {
            var newIndices = new List<uint>();
            for (int j = 0; j < indexList.Count; j += 3)
            {
                uint v1 = indexList[j];
                uint v2 = indexList[j + 1];
                uint v3 = indexList[j + 2];

                uint a = GetMidPoint(v1, v2, ref vertexList, midPointCache);
                uint b = GetMidPoint(v2, v3, ref vertexList, midPointCache);
                uint c = GetMidPoint(v3, v1, ref vertexList, midPointCache);

                newIndices.AddRange([v1, a, c]);
                newIndices.AddRange([v2, b, a]);
                newIndices.AddRange([v3, c, b]);
                newIndices.AddRange([a, b, c]);
            }
            indexList = newIndices;
        }

        var finalVertices = new List<Vertex>();
        foreach (var pos in vertexList)
        {
            var normal = pos;
            var position = pos * radius;

            float u = 0.5f + MathF.Atan2(pos.Z, pos.X) / (2 * MathF.PI);
            float v = 0.5f - MathF.Asin(pos.Y) / MathF.PI;

            finalVertices.Add(new Vertex(position, normal, new Vector2(u, v)));
        }

        vertices = [.. finalVertices];
        indices = [.. indexList];
    }

    private static uint GetMidPoint(uint p1, uint p2, ref List<Vector3> vertices, Dictionary<long, uint> cache)
    {
        long smallerIndex = Math.Min(p1, p2);
        long greaterIndex = Math.Max(p1, p2);
        long key = (smallerIndex << 32) + greaterIndex;

        if (cache.TryGetValue(key, out uint ret))
            return ret;

        Vector3 v1 = vertices[(int)p1];
        Vector3 v2 = vertices[(int)p2];
        Vector3 middle = Vector3.Normalize(v1 + v2);

        vertices.Add(middle);
        uint i = (uint)vertices.Count - 1;

        cache.Add(key, i);
        return i;
    }
}