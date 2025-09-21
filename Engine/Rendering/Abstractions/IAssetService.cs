using Engine.ECS.Components.Rendering;

namespace Engine.Rendering.Abstractions;

public interface IAssetService
{
    MeshHandle LoadMesh(string path);
    TextureHandle LoadTexture(string path);
    ShaderHandle LoadShader(string vertexPath, string fragmentPath);
    MaterialHandle LoadMaterial(string path);
}