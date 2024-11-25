using System.Numerics;
using System.Runtime.InteropServices;
using Nox.Shaders;

namespace Nox.Framework;

public enum BlendMode {
    Alpha,
    Additive,
    Subtractive,
}

public static class Renderer2D {
    [StructLayout(LayoutKind.Sequential)]
    private struct Uniforms2D {
        public Vector2 viewport;
        public Vector2 texture_size;
        public Matrix4x4 matrix;
    }

    private static Shader _shader;
    private static RenderPipeline _alphaPipeline;
    private static RenderPipeline _additivePipeline;
    private static RenderPipeline _multiplyPipeline;
    private static RenderPipeline _activePipeline;
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
        builder.WithLabel("2d-pipeline-alpha-blend");
        _alphaPipeline = builder.Build();

        // additive blending
        builder.Color(0).HasBlending()
            .WithSourceFactor(BlendFactor.SrcAlpha)
            .WithDestinationFactor(BlendFactor.One);
        builder.WithLabel("2d-pipeline-additive-blend");
        _additivePipeline = builder.Build();

        // Multiplicative blending
        //builder.Color(0).HasBlending()
        //    .WithSourceFactor(BlendFactor.DstColor)
        //    .WithDestinationFactor(BlendFactor.One)
        //    .WithOperation(BlendOp.ReverseSubtract);
        //builder.WithLabel("2d-pipeline-multiplicative-blend");
        //_multiplyPipeline = builder.Build();
        
        // Create a sampler
        _sampler = Sampler.CreateBuilder()
            .WithMinFilter(TextureFilter.Linear)
            .WithMagFilter(TextureFilter.Nearest)
            .WithWrap(TextureWrap.Repeat)
            .Build();

        _bindings.WithFragmentSampler(_uTextureSamplerSlot, _sampler);
        _activePipeline = null;
    }

    public static void Begin(){
        
    }

    public static void End(){
        _activePipeline = null;
    }

    public static void Draw(Texture2D texture, Buffer vertexBuffer, Buffer indexBuffer, int start, int count, Matrix4x4 globalTransform, BlendMode blendMode = BlendMode.Alpha){ 
        ApplyPipelineForBlendMode(blendMode);
        _uniforms.viewport = _uniforms.viewport = GraphicsDevice.Size;
        _uniforms.texture_size = new Vector2(texture.Width, texture.Height);
        _uniforms.matrix = globalTransform;
        _bindings.WithFragmentTexture(_uTextureSlot, texture)
            .WithVertexBuffer(0, vertexBuffer)
            .WithIndexBuffer(indexBuffer);
        GraphicsDevice.ApplyBindings(_bindings);
        GraphicsDevice.ApplyUniforms(ShaderStage.Vertex, _uParamsSlot, _uniforms);
        GraphicsDevice.Draw(start, count, 1);
    }

    private static void ApplyPipelineForBlendMode(BlendMode blendMode){
        RenderPipeline pipeline = null;
        switch(blendMode){
            case BlendMode.Alpha:
                pipeline = _alphaPipeline;
                break;
            case BlendMode.Additive:
                pipeline = _additivePipeline;
                break;
            case BlendMode.Subtractive:
                pipeline = _multiplyPipeline;
                break;
        }
        if(_activePipeline != pipeline){
            _activePipeline = pipeline;
            _activePipeline.Apply();
        }
    }

}
