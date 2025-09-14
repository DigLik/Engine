using Silk.NET.OpenGL;

namespace Engine.Rendering.Silk.OpenGL;

internal sealed class OpenGLShader : IDisposable
{
    public readonly uint Program;
    private readonly GL _gl;
    private readonly Dictionary<string, int> _uniformLocations = [];

    public OpenGLShader(GL gl, string vertexSource, string fragmentSource)
    {
        _gl = gl;

        var vertexShader = CompileShader(ShaderType.VertexShader, vertexSource);
        var fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentSource);

        Program = _gl.CreateProgram();
        _gl.AttachShader(Program, vertexShader);
        _gl.AttachShader(Program, fragmentShader);
        _gl.LinkProgram(Program);

        _gl.GetProgram(Program, ProgramPropertyARB.LinkStatus, out var status);
        if (status == 0)
        {
            throw new Exception($"Program failed to link: {_gl.GetProgramInfoLog(Program)}");
        }

        _gl.DetachShader(Program, vertexShader);
        _gl.DetachShader(Program, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
    }

    public void Use() => _gl.UseProgram(Program);

    public int GetUniformLocation(string name)
    {
        if (_uniformLocations.TryGetValue(name, out int location))
        {
            return location;
        }

        location = _gl.GetUniformLocation(Program, name);
        _uniformLocations[name] = location;
        return location;
    }

    private uint CompileShader(ShaderType type, string source)
    {
        var shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, source);
        _gl.CompileShader(shader);

        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
        return status == 0
            ? throw new Exception($"Failed to compile {type} shader: {_gl.GetShaderInfoLog(shader)}")
            : shader;
    }

    public void Dispose() => _gl.DeleteProgram(Program);
}