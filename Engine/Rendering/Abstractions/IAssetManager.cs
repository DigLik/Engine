using Engine.ECS.Components.Rendering;

namespace Engine.Rendering.Abstractions;

public interface IAssetManager
{
    MeshHandle LoadMesh(string path);
    MaterialHandle LoadMaterial(string path);
}