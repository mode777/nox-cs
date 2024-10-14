using static Nox.Native.LibNox;

namespace Nox.Framework;

public class Bindings
{
    internal sg_bindings _bindings = new();
    // Need to keep them around because of GC
    private Buffer[] _buffers = new Buffer[SG_MAX_VERTEX_BUFFERS];
    private Texture2D[] _vsImages = new Texture2D[SG_MAX_SHADERSTAGE_IMAGES];
    private Texture2D[] _fsImages = new Texture2D[SG_MAX_SHADERSTAGE_IMAGES];
    private Sampler[] _fsSamplers = new Sampler[SG_MAX_SHADERSTAGE_SAMPLERS];
    private Sampler[] _vsSamplers = new Sampler[SG_MAX_SHADERSTAGE_SAMPLERS];
    private Buffer _indexBuffer;

    public Bindings WithFragmentSampler(int index, Sampler sampler)
    {
        _fsSamplers[index] = sampler;
        _bindings.fs.samplers[index] = sampler.Handle;
        return this;
    }

    public Bindings WithFragmentTexture(int index, Texture2D image)
    {
        _fsImages[index] = image;
        _bindings.fs.images[index] = image.Handle;
        return this;
    }

    public Bindings WithVertexSampler(int index, Sampler sampler)
    {
        _vsSamplers[index] = sampler;
        _bindings.vs.samplers[index] = sampler.Handle;
        return this;
    }

    public Bindings WithVertexTexture(int index, Texture2D image)
    {
        _vsImages[index] = image;
        _bindings.vs.images[index] = image.Handle;
        return this;
    }

    public Bindings WithVertexBuffer(int index, Buffer buffer)
    {
        _buffers[index] = buffer;
        _bindings.vertex_buffers[index] = buffer.Handle;
        return this;
    }

    public Bindings WithIndexBuffer(Buffer buffer)
    {
        _indexBuffer = buffer;
        _bindings.index_buffer = buffer.Handle;
        return this;
    }
}