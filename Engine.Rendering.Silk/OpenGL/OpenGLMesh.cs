using Engine.Rendering.Data;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Engine.Rendering.Silk.OpenGL;

internal sealed class OpenGLMesh : IDisposable
{
    public readonly uint Vao;
    public readonly uint Vbo;
    public readonly uint Ebo;

    public uint IndexCount { get; private set; }

    private readonly GL _gl;
    private readonly BufferUsageARB _usage;

    public unsafe OpenGLMesh(GL gl, ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices, bool isDynamic)
    {
        _gl = gl;
        IndexCount = (uint)indices.Length;
        _usage = isDynamic ? BufferUsageARB.DynamicDraw : BufferUsageARB.StaticDraw;

        Vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, vertices, _usage);

        Ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo);
        _gl.BufferData(BufferTargetARB.ElementArrayBuffer, indices, _usage);

        Vao = _gl.GenVertexArray();
        _gl.BindVertexArray(Vao);

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo);

        _gl.EnableVertexAttribArray(0);
        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)0);

        _gl.EnableVertexAttribArray(1);
        _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)sizeof(Vector3));

        _gl.EnableVertexAttribArray(2);
        _gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, (uint)sizeof(Vertex), (void*)(sizeof(Vector3) * 2));

        _gl.BindVertexArray(0);
    }

    public unsafe void Update(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices)
    {
        IndexCount = (uint)indices.Length;

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, vertices, _usage);

        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo);
        _gl.BufferData(BufferTargetARB.ElementArrayBuffer, indices, _usage);
    }

    public void Dispose()
    {
        _gl.DeleteVertexArray(Vao);
        _gl.DeleteBuffer(Vbo);
        _gl.DeleteBuffer(Ebo);
    }
}