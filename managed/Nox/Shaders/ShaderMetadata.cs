using System.Reflection;
using Nox.Native;
using YamlDotNet.Serialization;
using static Nox.Native.LibNox;

namespace Nox.Shaders;

public class ShaderMetadata
{
    public static ShaderMetadata Load(string path){
        var yaml = File.ReadAllText(path);

        var deserializer = new DeserializerBuilder()
            .Build();

        var metadata = deserializer.Deserialize<ShaderMetadata>(yaml);
        var dir = Path.GetDirectoryName(path);
        foreach(var desc in metadata.shaders){
            foreach(var prog in desc.programs){
                prog.fs.source = File.ReadAllText(Path.Combine(dir, prog.fs.path));
                prog.vs.source = File.ReadAllText(Path.Combine(dir, prog.vs.path));
            }
        }
        return metadata;
    }

    public static ShaderMetadata LoadFromResource(Assembly asm, string ns, string name){
        var yaml = ReadEmbeddedResourceString(asm, $"{ns}.{name}") ;

        var deserializer = new DeserializerBuilder()
            .Build();

        var metadata = deserializer.Deserialize<ShaderMetadata>(yaml);
        foreach(var desc in metadata.shaders){
            foreach(var prog in desc.programs){
                prog.fs.source = ReadEmbeddedResourceString(asm, $"{ns}.{prog.fs.path}");
                prog.vs.source = ReadEmbeddedResourceString(asm, $"{ns}.{prog.vs.path}");
            }
        }
        return metadata;
    }

    private static string ReadEmbeddedResourceString(Assembly assembly, string id){
        using Stream stream = assembly.GetManifestResourceStream(id);
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public List<ShaderDescription> shaders { get; set; }
    internal ShaderDescription? FindForBackend(GraphicsBackend backend)
    {
        switch ((sg_backend)backend)
        {
            case sg_backend.SG_BACKEND_D3D11:
                return shaders.FirstOrDefault(x => x.slang == "hlsl5");
            case sg_backend.SG_BACKEND_METAL_MACOS:
                return shaders.FirstOrDefault(x => x.slang == "metal_macos");
            case sg_backend.SG_BACKEND_GLCORE:
                return shaders.FirstOrDefault(x => x.slang == "glsl430");
            case sg_backend.SG_BACKEND_GLES3:
                return shaders.FirstOrDefault(x => x.slang == "glsl300es");
            default:
                throw new InvalidOperationException($"Backend {backend} not available");
        }
    }
}