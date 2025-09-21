using System.Text.Json.Serialization;

namespace Engine.Rendering;

internal record MaterialDefinition(MaterialShaderDefinition Shader, Dictionary<string, string> Textures);
internal record MaterialShaderDefinition(string Vertex, string Fragment);

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(MaterialDefinition))]
internal partial class AssetSerializationContext : JsonSerializerContext
{
}