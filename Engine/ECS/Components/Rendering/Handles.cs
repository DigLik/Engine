using System.Runtime.InteropServices;

namespace Engine.ECS.Components.Rendering;

public readonly record struct MeshHandle(int Id, BoundingBox Bounds);
public readonly record struct MaterialHandle(int Id);
public readonly record struct ShaderHandle(int Id);
public readonly record struct TextureHandle(int Id);
[StructLayout(LayoutKind.Sequential)]
public readonly record struct BoundingBox(Vector3 Min, Vector3 Max);