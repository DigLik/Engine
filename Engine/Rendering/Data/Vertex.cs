namespace Engine.Rendering.Data;

public struct Vertex(Vector3 position, Vector3 normal, Vector2 texCoords)
{
    public Vector3 Position = position;
    public Vector3 Normal = normal;
    public Vector2 TexCoords = texCoords;
}