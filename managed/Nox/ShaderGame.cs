using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Nox.Framework;
using Nox.Shaders;
using static Nox.Native.LibNox;

namespace Nox;

public class ShaderGame : Game {

    [StructLayout(LayoutKind.Sequential)]
    private struct Uniforms {
        public Vector2 viewport;
        public Vector2 texture_size;
    }

    private Texture2D _texture;
    //private SpriteBatch _batch;

    private Bindings _bindings;
    private QuadBuffer _quadBuffer;
    private Uniforms _uniforms;
    private ProgramDescription _desc;
    private Shader _shader;
    private RenderPipeline _pipeline;
    private Sampler _sampler;

    public override void Init()
    {
        // Load a texture
        _texture = Texture2D.Load("../../assets/wabbit_alpha.png");

        // Create a shader
        var path = "../../assets/shaders/shader_reflection.yaml";
        var metadata = ShaderMetadata.Load(path);
        _desc = metadata.FindForBackend(GraphicsDevice.Backend).programs[0];
        _shader = Shader.FromMetadata(_desc);

        // Create a pipeline
        var builder = RenderPipeline.CreateBuilder(_shader);
        builder.WithIndexType(IndexType.Uint16);
        builder.Attribute(_desc.vs.GetAttributeSlot("aPosition")).HasFormat(VertexFormat.Float2);
        builder.Attribute(_desc.vs.GetAttributeSlot("aTexCoord")).HasFormat(VertexFormat.Float2);
        builder.Attribute(_desc.vs.GetAttributeSlot("aColor")).HasFormat(VertexFormat.Byte4N);
        builder.Color(0).HasBlending()
            .WithSourceFactor(BlendFactor.SrcAlpha)
            .WithDestinationFactor(BlendFactor.OneMinusSrcAlpha);
        builder.WithLabel("2d-pipeline2");
        _pipeline = builder.Build();
        
        // Create a sampler
        _sampler = Sampler.CreateBuilder()
            .WithMinFilter(TextureFilter.Linear)
            .WithMagFilter(TextureFilter.Nearest)
            .WithWrap(TextureWrap.Repeat)
            .Build();

        // create a buffer
        _quadBuffer = new QuadBuffer();

        // Create a bindings
        _bindings = new Bindings()
            .WithFragmentSampler(_desc.fs.GetSamplerSlot("uTextureSampler"), _sampler)
            .WithFragmentTexture(_desc.fs.GetTextureSlot("_uTexture"), _texture);

        // Set uniforms
        _uniforms = new Uniforms();
        _uniforms.viewport = GraphicsDevice.Size;
        _uniforms.texture_size = new Vector2(_texture.Width, _texture.Height);

        base.Init();
    }

    public override void Render()
    {
        _quadBuffer.Clear();
        _quadBuffer.TryAddQuad(Mouse.GetPosition(), new Rectangle(0, 0, 32, 32), ColorRGBA.AliceBlue);
        _quadBuffer.Update();

        _pipeline.Apply();
        _bindings.WithVertexBuffer(0, _quadBuffer.GetBuffer())
            .WithIndexBuffer(_quadBuffer.GetIndexBuffer());
        GraphicsDevice.ApplyBindings(_bindings);
        GraphicsDevice.ApplyUniforms(ShaderStage.Vertex, _desc.vs.GetUniformSlot("uParams"), _uniforms);
        GraphicsDevice.Draw(0,6,1);
        base.Render();
    }
}