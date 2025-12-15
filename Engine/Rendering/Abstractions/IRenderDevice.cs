using Engine.ECS.Components.Rendering;
using Engine.Rendering.Data;

namespace Engine.Rendering.Abstractions;

public interface IRenderDevice : IDisposable
{
    MeshHandle CreateMesh(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices, bool isDynamic = false);

    void UpdateMesh(MeshHandle handle, ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices);

    MaterialHandle CreateMaterial(ShaderHandle shader, Dictionary<string, object> parameters);
    ShaderHandle CreateShader(string vertexSource, string fragmentSource);
    TextureHandle CreateTexture(int width, int height, ReadOnlySpan<byte> data);

    void DestroyMesh(MeshHandle handle);
    void DestroyMaterial(MaterialHandle handle);
    void DestroyShader(ShaderHandle handle);
    void DestroyTexture(TextureHandle handle);

    void BeginFrame();
    void SetCameraUniforms(Matrix4x4 view, Matrix4x4 projection);
    void Draw(MeshHandle mesh, MaterialHandle material, ReadOnlySpan<Matrix4x4> worldMatrices);
    void EndFrame();
}