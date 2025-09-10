using Engine.ECS.Components.Rendering;

namespace Engine.Rendering;

public interface IAssetManager
{
    MeshHandle LoadMesh(string path);
    MaterialHandle LoadMaterial(string path);
}