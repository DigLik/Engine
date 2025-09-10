using Engine.Rendering;
using System.Numerics;

namespace TestGame;

internal class CubeGenerator
{
    public static void Create(float size, out Vertex[] vertices, out uint[] indices)
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
}