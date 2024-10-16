using System.Numerics;
using System.Runtime.InteropServices;
using Nox.Shaders;

namespace Nox.Framework;

public static class Renderer2D {
    [StructLayout(LayoutKind.Sequential)]
    private struct Uniforms2D {
        public Vector2 viewport;
        public Vector2 texture_size;
    }

    private static Shader _shader;
    private static RenderPipeline _pipeline;
    private static Sampler _sampler;
    private static Bindings _bindings;
    private static Uniforms2D _uniforms;
    private static int _aPositionSlot;
    private static int _aTexCoordSlot;
    private static int _aColorSlot;
    private static int _uTextureSamplerSlot;
    private static int _uTextureSlot;
    private static int _uParamsSlot;

    
    internal static void Init(){
        _bindings = new Bindings();
        _uniforms = new Uniforms2D();
        // Create a shader
        
        var metadata = ShaderMetadata.LoadFromResource(typeof(Renderer2D).Assembly, "Nox.Resources.Shaders.Simple2D", "shader_reflection.yaml");
        var shader = metadata.FindForBackend(GraphicsDevice.Backend).programs[0];
        
        _aPositionSlot = shader.vs.GetAttributeSlot("aPosition");
        _aTexCoordSlot = shader.vs.GetAttributeSlot("aTexCoord");
        _aColorSlot = shader.vs.GetAttributeSlot("aColor");
        _uTextureSamplerSlot = shader.fs.GetSamplerSlot("uTextureSampler");
        _uTextureSlot = shader.fs.GetTextureSlot("_uTexture");
        _uParamsSlot = shader.vs.GetUniformSlot("uParams");
        _shader = Shader.FromMetadata(shader);

        // Create a pipeline
        var builder = RenderPipeline.CreateBuilder(_shader);
        builder.WithIndexType(IndexType.Uint16);
        builder.Attribute(_aPositionSlot).HasFormat(VertexFormat.Float2);
        builder.Attribute(_aTexCoordSlot).HasFormat(VertexFormat.Float2);
        builder.Attribute(_aColorSlot).HasFormat(VertexFormat.Byte4N);
        builder.Color(0).HasBlending()
            .WithSourceFactor(BlendFactor.SrcAlpha)
            .WithDestinationFactor(BlendFactor.OneMinusSrcAlpha);
        builder.WithLabel("2d-pipeline");
        _pipeline = builder.Build();
        
        // Create a sampler
        _sampler = Sampler.CreateBuilder()
            .WithMinFilter(TextureFilter.Linear)
            .WithMagFilter(TextureFilter.Nearest)
            .WithWrap(TextureWrap.Repeat)
            .Build();

        _bindings.WithFragmentSampler(_uTextureSamplerSlot, _sampler);
    }

    public static void Begin(){
        _pipeline.Apply();
    }

    public static void DrawQuads(Texture2D texture, Buffer vertexBuffer, Buffer indexBuffer, int start, int count){
        _uniforms.viewport = _uniforms.viewport = GraphicsDevice.Size;
        _uniforms.texture_size = new Vector2(texture.Width, texture.Height);
        _bindings.WithFragmentTexture(_uTextureSlot, texture)
            .WithVertexBuffer(0, vertexBuffer)
            .WithIndexBuffer(indexBuffer);
        GraphicsDevice.ApplyBindings(_bindings);
        GraphicsDevice.ApplyUniforms(ShaderStage.Vertex, _uParamsSlot, _uniforms);
        GraphicsDevice.Draw(start * 6, count * 6, 1);
    }
}
