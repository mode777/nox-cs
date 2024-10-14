using System;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public enum VertexFormat
{
    Float2 = sg_vertex_format.SG_VERTEXFORMAT_FLOAT2,
    Float3 = sg_vertex_format.SG_VERTEXFORMAT_FLOAT3,
    Float4 = sg_vertex_format.SG_VERTEXFORMAT_FLOAT4,
    Byte4 = sg_vertex_format.SG_VERTEXFORMAT_UBYTE4,
    Byte4N = sg_vertex_format.SG_VERTEXFORMAT_UBYTE4N,
    Short2 = sg_vertex_format.SG_VERTEXFORMAT_SHORT2,
    Short2N = sg_vertex_format.SG_VERTEXFORMAT_SHORT2N,
    Short4 = sg_vertex_format.SG_VERTEXFORMAT_SHORT4,
    Short4N = sg_vertex_format.SG_VERTEXFORMAT_SHORT4N
}

public enum IndexType
{
    Uint16 = sg_index_type.SG_INDEXTYPE_UINT16,
    Uint32 = sg_index_type.SG_INDEXTYPE_UINT32
}

public enum BlendFactor
{
    Zero = sg_blend_factor.SG_BLENDFACTOR_ZERO,
    One = sg_blend_factor.SG_BLENDFACTOR_ONE,
    SrcColor = sg_blend_factor.SG_BLENDFACTOR_SRC_COLOR,
    OneMinusSrcColor = sg_blend_factor.SG_BLENDFACTOR_ONE_MINUS_SRC_COLOR,
    DstColor = sg_blend_factor.SG_BLENDFACTOR_DST_COLOR,
    OneMinusDstColor = sg_blend_factor.SG_BLENDFACTOR_ONE_MINUS_DST_COLOR,
    SrcAlpha = sg_blend_factor.SG_BLENDFACTOR_SRC_ALPHA,
    OneMinusSrcAlpha = sg_blend_factor.SG_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
    DstAlpha = sg_blend_factor.SG_BLENDFACTOR_DST_ALPHA,
    OneMinusDstAlpha = sg_blend_factor.SG_BLENDFACTOR_ONE_MINUS_DST_ALPHA
}

public class AttributeBuilder
{
    private readonly int _index;
    private readonly RenderPipelineBuilder _builder;

    internal AttributeBuilder(int index, RenderPipelineBuilder builder)
    {
        _index = index;
        _builder = builder;
    }

    public AttributeBuilder HasFormat(VertexFormat format)
    {
        _builder._pipeline.layout.attrs[_index].format = (sg_vertex_format)format;
        return this;
    }
}

public class ColorAttachmentBuilder
{
    private readonly int _index;
    private readonly RenderPipelineBuilder _builder;

    internal ColorAttachmentBuilder(int index, RenderPipelineBuilder builder)
    {
        _index = index;
        _builder = builder;
    }

    public BlendingBuilder HasBlending()
    {
        _builder._pipeline.colors[_index].blend.enabled = true;
        return new BlendingBuilder(_index, _builder);
    }
}

public class BlendingBuilder
{
    private readonly int _index;
    private readonly RenderPipelineBuilder _builder;

    internal BlendingBuilder(int index, RenderPipelineBuilder builder)
    {
        _index = index;
        _builder = builder;
    }

    public BlendingBuilder WithSourceFactor(BlendFactor factor)
    {
        _builder._pipeline.colors[_index].blend.src_factor_rgb = (sg_blend_factor)factor;
        return this;
    }

    public BlendingBuilder WithDestinationFactor(BlendFactor factor)
    {
        _builder._pipeline.colors[_index].blend.dst_factor_rgb = (sg_blend_factor)factor;
        return this;
    }
}

public class RenderPipelineBuilder
{
    private readonly Shader _shader;
    internal sg_pipeline_desc _pipeline;

    internal RenderPipelineBuilder(Shader shader)
    {
        _shader = shader;
        _pipeline = new sg_pipeline_desc();
    }


    public RenderPipelineBuilder WithIndexType(IndexType indexType)
    {
        _pipeline.index_type = (sg_index_type)indexType;
        return this;
    }


    public AttributeBuilder Attribute(int index)
    {
        return new AttributeBuilder(index, this);
    }

    public ColorAttachmentBuilder Color(int i)
    {
        return new ColorAttachmentBuilder(i, this);
    }

    public void WithLabel(string label)
    {
        _pipeline.label = label;
    }

    public RenderPipeline Build()
    {
        return new RenderPipeline(_shader, ref _pipeline);
    }
}


public class RenderPipeline : IDisposable {
    private readonly uint _handle;
    private readonly Shader _shader;
    private bool _disposedValue;

    public static RenderPipelineBuilder CreateBuilder(Shader shader)
    {
        return new RenderPipelineBuilder(shader);
    }

    public Shader Shader => _shader;
    internal uint Handle => _handle;

    internal RenderPipeline(Shader shader, ref sg_pipeline_desc desc){
        _shader = shader;
        desc.shader = shader.Handle;
        AssertCall(nox_pipeline_create(ref desc, out _handle));
    }

    public void Apply(){
        AssertCall(nox_pipeline_apply(_handle));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            AssertCall(nox_pipeline_free(_handle));
            _disposedValue = true;
        }
    }

    ~RenderPipeline()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    
}