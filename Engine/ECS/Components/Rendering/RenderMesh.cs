namespace Engine.ECS.Components.Rendering;

public record struct RenderMesh
{
    public MeshHandle Mesh { get; set; }
    public MaterialHandle Material { get; set; }
}