using Engine.ECS.Components.Rendering;

namespace Engine.Rendering.Silk.OpenGL;

internal sealed class OpenGLMaterial
{
    public ShaderHandle Shader { get; }
    public Dictionary<string, object> Parameters { get; }

    public OpenGLMaterial(ShaderHandle shader, Dictionary<string, object> parameters)
    {
        Shader = shader;
        Parameters = parameters;
    }
}