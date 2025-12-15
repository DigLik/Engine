namespace Engine.ECS.Components.Rendering;

public record struct RenderMesh
{
    public MeshHandle Mesh;
    public MaterialHandle Material;
}