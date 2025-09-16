using Engine.DataStructures;
using Engine.ECS.Components.Rendering;
using Engine.Rendering.Abstractions;
using Engine.Rendering.Data;
using Engine.Rendering.Silk.OpenGL;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Engine.Rendering.Silk;

public sealed unsafe class SilkRenderDevice : IRenderDevice
{
    private readonly GL _gl;

    private readonly SparseSet<OpenGLMesh> _meshes = [];
    private readonly SparseSet<OpenGLShader> _shaders = [];
    private readonly SparseSet<OpenGLMaterial> _materials = [];
    private readonly SparseSet<OpenGLTexture> _textures = [];
    private uint _nextResourceId = 1;

    private readonly uint _instanceMatrixBuffer;
    private const int MaxInstances = 10000;

    private Matrix4x4 _viewMatrix;
    private Matrix4x4 _projectionMatrix;

    public SilkRenderDevice(GL gl)
    {
        _gl = gl;
        _gl.Enable(EnableCap.DepthTest);
        _gl.Enable(EnableCap.CullFace);
        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _instanceMatrixBuffer = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _instanceMatrixBuffer);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(MaxInstances * sizeof(Matrix4x4)), null, BufferUsageARB.DynamicDraw);
    }

    public ShaderHandle CreateShader(string vertexSource, string fragmentSource)
    {
        var shader = new OpenGLShader(_gl, vertexSource, fragmentSource);
        var id = _nextResourceId++;
        _shaders.Add(id, shader);
        return new ShaderHandle((int)id);
    }

    public TextureHandle CreateTexture(int width, int height, ReadOnlySpan<byte> data)
    {
        var texture = new OpenGLTexture(_gl, width, height, data);
        var id = _nextResourceId++;
        _textures.Add(id, texture);
        return new TextureHandle((int)id);
    }

    public MeshHandle CreateMesh(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices)
    {
        var mesh = new OpenGLMesh(_gl, vertices, indices);
        var id = _nextResourceId++;
        _meshes.Add(id, mesh);

        _gl.BindVertexArray(mesh.Vao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _instanceMatrixBuffer);

        for (int i = 0; i < 4; i++)
        {
            uint location = 3u + (uint)i;
            _gl.EnableVertexAttribArray(location);
            _gl.VertexAttribPointer(location, 4, VertexAttribPointerType.Float, false, (uint)sizeof(Matrix4x4), (void*)(sizeof(Vector4) * i));
            _gl.VertexAttribDivisor(location, 1);
        }
        _gl.BindVertexArray(0);

        return new MeshHandle((int)id);
    }

    public MaterialHandle CreateMaterial(ShaderHandle shader, Dictionary<string, object> parameters)
    {
        var material = new OpenGLMaterial(shader, parameters);
        var id = _nextResourceId++;
        _materials.Add(id, material);
        return new MaterialHandle((int)id);
    }

    public void BeginFrame()
    {
        _gl.ClearColor(System.Drawing.Color.Black);
        _gl.Clear((uint)(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
    }

    public void SetCameraUniforms(Matrix4x4 view, Matrix4x4 projection)
    {
        _viewMatrix = view;
        _projectionMatrix = projection;
    }

    public void Draw(MeshHandle meshHandle, MaterialHandle materialHandle, ReadOnlySpan<Matrix4x4> worldMatrices)
    {
        if (worldMatrices.IsEmpty) return;

        if (!_meshes.TryGetValue((uint)meshHandle.Id, out var mesh) ||
            !_materials.TryGetValue((uint)materialHandle.Id, out var material) ||
            !_shaders.TryGetValue((uint)material.Shader.Id, out var shader))
        {
            return;
        }

        shader.Use();

        _gl.UniformMatrix4(shader.GetUniformLocation("view"), 1, false, in _viewMatrix.M11);
        _gl.UniformMatrix4(shader.GetUniformLocation("projection"), 1, false, in _projectionMatrix.M11);

        if (material.Parameters.TryGetValue("u_Texture", out var textureObj) && textureObj is TextureHandle textureHandle)
        {
            if (_textures.TryGetValue((uint)textureHandle.Id, out var texture))
            {
                int textureLocation = shader.GetUniformLocation("u_Texture");
                _gl.Uniform1(textureLocation, 0);
                texture.Bind(TextureUnit.Texture0);
            }
        }

        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _instanceMatrixBuffer);

        fixed (void* pData = worldMatrices)
            _gl.BufferSubData(BufferTargetARB.ArrayBuffer, 0, (nuint)(worldMatrices.Length * sizeof(Matrix4x4)), pData);

        _gl.BindVertexArray(mesh.Vao);
        _gl.DrawElementsInstanced(PrimitiveType.Triangles, mesh.IndexCount, DrawElementsType.UnsignedInt, null, (uint)worldMatrices.Length);
        _gl.BindVertexArray(0);
    }

    public void EndFrame()
    {
    }

    public void DestroyMesh(MeshHandle handle)
    {
        if (_meshes.TryGetValue((uint)handle.Id, out var mesh))
        {
            mesh.Dispose();
            _meshes.Remove((uint)handle.Id);
        }
    }

    public void DestroyShader(ShaderHandle handle)
    {
        if (_shaders.TryGetValue((uint)handle.Id, out var shader))
        {
            shader.Dispose();
            _shaders.Remove((uint)handle.Id);
        }
    }

    public void DestroyMaterial(MaterialHandle handle)
    {
        _materials.Remove((uint)handle.Id);
    }

    public void DestroyTexture(TextureHandle handle)
    {
        if (_textures.TryGetValue((uint)handle.Id, out var texture))
        {
            texture.Dispose();
            _textures.Remove((uint)handle.Id);
        }
    }

    public void Dispose()
    {
        foreach (var kvp in _meshes) kvp.Value.Dispose();
        _meshes.Clear();

        foreach (var kvp in _shaders) kvp.Value.Dispose();
        _shaders.Clear();

        foreach (var kvp in _textures) kvp.Value.Dispose();
        _textures.Clear();

        _materials.Clear();

        _gl.DeleteBuffer(_instanceMatrixBuffer);
    }
}