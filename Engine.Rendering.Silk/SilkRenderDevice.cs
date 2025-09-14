using Engine.ECS.Components.Rendering;
using Engine.Rendering.Silk.OpenGL;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Engine.Rendering.Silk;

public sealed unsafe class SilkRenderDevice : IRenderDevice
{
    private readonly GL _gl;

    private readonly List<OpenGLMesh?> _meshes = [];
    private readonly List<OpenGLShader?> _shaders = [];
    private readonly List<OpenGLMaterial?> _materials = [];
    private readonly List<OpenGLTexture?> _textures = [];

    private int _nextMeshId = 0;
    private int _nextShaderId = 0;
    private int _nextMaterialId = 0;
    private int _nextTextureId = 0;

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
        _shaders.Add(shader);
        return new ShaderHandle(_nextShaderId++);
    }

    public TextureHandle CreateTexture(int width, int height, ReadOnlySpan<byte> data)
    {
        var texture = new OpenGLTexture(_gl, width, height, data);
        _textures.Add(texture);
        return new TextureHandle(_nextTextureId++);
    }

    public MeshHandle CreateMesh(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices)
    {
        var mesh = new OpenGLMesh(_gl, vertices, indices);
        _meshes.Add(mesh);

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

        return new MeshHandle(_nextMeshId++);
    }

    public MaterialHandle CreateMaterial(ShaderHandle shader, Dictionary<string, object> parameters)
    {
        var material = new OpenGLMaterial(shader, parameters);
        _materials.Add(material);
        return new MaterialHandle(_nextMaterialId++);
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

        var mesh = _meshes[meshHandle.Id];
        var material = _materials[materialHandle.Id];
        if (mesh is null || material is null) return;

        var shader = _shaders[material.Shader.Id];
        if (shader is null) return;

        shader.Use();

        _gl.UniformMatrix4(shader.GetUniformLocation("view"), 1, false, in _viewMatrix.M11);
        _gl.UniformMatrix4(shader.GetUniformLocation("projection"), 1, false, in _projectionMatrix.M11);

        if (material.Parameters.TryGetValue("u_Texture", out var textureObj) && textureObj is TextureHandle textureHandle)
        {
            var texture = _textures[textureHandle.Id];
            if (texture != null)
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
        if (handle.Id < _meshes.Count && _meshes[handle.Id] is not null)
        {
            _meshes[handle.Id]!.Dispose();
            _meshes[handle.Id] = null;
        }
    }

    public void DestroyShader(ShaderHandle handle)
    {
        if (handle.Id < _shaders.Count && _shaders[handle.Id] is not null)
        {
            _shaders[handle.Id]!.Dispose();
            _shaders[handle.Id] = null;
        }
    }

    public void DestroyMaterial(MaterialHandle handle)
    {
        if (handle.Id < _materials.Count)
            _materials[handle.Id] = null;
    }

    public void DestroyTexture(TextureHandle handle)
    {
        if (handle.Id < _textures.Count && _textures[handle.Id] is not null)
        {
            _textures[handle.Id]!.Dispose();
            _textures[handle.Id] = null;
        }
    }

    public void Dispose()
    {
        foreach (var mesh in _meshes) mesh?.Dispose();
        foreach (var shader in _shaders) shader?.Dispose();
        _gl.DeleteBuffer(_instanceMatrixBuffer);
    }
}