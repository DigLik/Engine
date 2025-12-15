using Engine.ECS.Components.Rendering;
using Engine.Rendering.Abstractions;
using Engine.Rendering.Data;
using StbImageSharp;
using System.Globalization;
using System.Text.Json;

namespace Engine.Rendering;

public sealed class AssetService(IRenderDevice renderDevice, string assetRoot) : IAssetService
{
    private readonly Dictionary<string, MeshHandle> _meshCache = [];
    private readonly Dictionary<string, TextureHandle> _textureCache = [];
    private readonly Dictionary<string, MaterialHandle> _materialCache = [];
    private readonly Dictionary<(string, string), ShaderHandle> _shaderCache = [];

    public MeshHandle LoadMesh(string path)
    {
        if (_meshCache.TryGetValue(path, out var handle))
            return handle;

        var fullPath = Path.Combine(assetRoot, path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Mesh file not found.", fullPath);

        ParseObj(File.ReadAllText(fullPath), out var vertices, out var indices);
        var newHandle = renderDevice.CreateMesh(vertices, indices, isDynamic: false);

        _meshCache[path] = newHandle;
        return newHandle;
    }

    public TextureHandle LoadTexture(string path)
    {
        if (_textureCache.TryGetValue(path, out var handle))
            return handle;

        var fullPath = Path.Combine(assetRoot, path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Texture file not found.", fullPath);

        using var stream = File.OpenRead(fullPath);
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        var newHandle = renderDevice.CreateTexture(image.Width, image.Height, image.Data);
        _textureCache[path] = newHandle;
        return newHandle;
    }

    public ShaderHandle LoadShader(string vertexPath, string fragmentPath)
    {
        if (_shaderCache.TryGetValue((vertexPath, fragmentPath), out var handle))
            return handle;

        var fullVertexPath = Path.Combine(assetRoot, vertexPath);
        var fullFragmentPath = Path.Combine(assetRoot, fragmentPath);

        if (!File.Exists(fullVertexPath)) throw new FileNotFoundException("Vertex shader not found.", fullVertexPath);
        if (!File.Exists(fullFragmentPath)) throw new FileNotFoundException("Fragment shader not found.", fullFragmentPath);

        var vertexSource = File.ReadAllText(fullVertexPath);
        var fragmentSource = File.ReadAllText(fullFragmentPath);

        var newHandle = renderDevice.CreateShader(vertexSource, fragmentSource);
        _shaderCache[(vertexPath, fragmentPath)] = newHandle;
        return newHandle;
    }

    public MaterialHandle LoadMaterial(string path)
    {
        if (_materialCache.TryGetValue(path, out var handle))
            return handle;

        var fullPath = Path.Combine(assetRoot, path);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Material file not found.", fullPath);

        var json = File.ReadAllText(fullPath);

        var definition = JsonSerializer.Deserialize(json, AssetSerializationContext.Default.MaterialDefinition)
            ?? throw new InvalidDataException($"Failed to parse material file: {path}");
        var shaderHandle = LoadShader(definition.Shader.Vertex, definition.Shader.Fragment);
        var parameters = new Dictionary<string, object>();

        foreach (var (uniformName, texturePath) in definition.Textures)
            parameters[uniformName] = LoadTexture(texturePath);

        var newHandle = renderDevice.CreateMaterial(shaderHandle, parameters);
        _materialCache[path] = newHandle;
        return newHandle;
    }

    private static void ParseObj(string objContent, out Vertex[] vertices, out uint[] indices)
    {
        var positions = new List<Vector3>();
        var normals = new List<Vector3>();
        var texCoords = new List<Vector2>();

        var outVertices = new List<Vertex>();
        var outIndices = new List<uint>();
        var vertexMap = new Dictionary<string, uint>();

        var lines = objContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "v":
                    positions.Add(new Vector3(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture)));
                    break;
                case "vn":
                    normals.Add(new Vector3(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture),
                        float.Parse(parts[3], CultureInfo.InvariantCulture)));
                    break;
                case "vt":
                    texCoords.Add(new Vector2(
                        float.Parse(parts[1], CultureInfo.InvariantCulture),
                        float.Parse(parts[2], CultureInfo.InvariantCulture)));
                    break;
                case "f":
                    for (int i = 1; i <= 3; i++)
                    {
                        if (!vertexMap.TryGetValue(parts[i], out var index))
                        {
                            var faceParts = parts[i].Split('/');
                            var vIdx = int.Parse(faceParts[0]) - 1;
                            var vtIdx = faceParts.Length > 1 && !string.IsNullOrEmpty(faceParts[1]) ? int.Parse(faceParts[1]) - 1 : -1;
                            var vnIdx = faceParts.Length > 2 ? int.Parse(faceParts[2]) - 1 : -1;

                            var vertex = new Vertex(
                                positions[vIdx],
                                vnIdx != -1 ? normals[vnIdx] : Vector3.Zero,
                                vtIdx != -1 ? texCoords[vtIdx] : Vector2.Zero
                            );
                            outVertices.Add(vertex);
                            index = (uint)(outVertices.Count - 1);
                            vertexMap[parts[i]] = index;
                        }
                        outIndices.Add(index);
                    }
                    if (parts.Length == 5) // Triangulate quad
                    {
                        outIndices.Add(vertexMap[parts[1]]);
                        outIndices.Add(vertexMap[parts[3]]);
                        outIndices.Add(vertexMap[parts[4]]);
                    }
                    break;
            }
        }

        vertices = [.. outVertices];
        indices = [.. outIndices];
    }
}