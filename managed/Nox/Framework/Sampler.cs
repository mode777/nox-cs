using System;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public enum TextureFilter {
    Nearest = sg_filter.SG_FILTER_NEAREST,
    Linear = sg_filter.SG_FILTER_LINEAR
}

public enum TextureWrap {
    Repeat = sg_wrap.SG_WRAP_REPEAT,
    ClampToEdge = sg_wrap.SG_WRAP_CLAMP_TO_EDGE,
    MirroredRepeat = sg_wrap.SG_WRAP_MIRRORED_REPEAT
}

public class SamplerBuilder
{
    internal sg_sampler_desc _desc = new();
    
    public SamplerBuilder WithMinFilter(TextureFilter filter)
    {
        _desc.min_filter = (sg_filter)filter;
        return this;
    }

    public SamplerBuilder WithMagFilter(TextureFilter filter)
    {
        _desc.mag_filter = (sg_filter)filter;
        return this;
    }

    public SamplerBuilder WithWrapU(TextureWrap wrap)
    {
        _desc.wrap_u = (sg_wrap)wrap;
        return this;
    }

    public SamplerBuilder WithWrapV(TextureWrap wrap)
    {
        _desc.wrap_v = (sg_wrap)wrap;
        return this;
    }

    public SamplerBuilder WithWrap(TextureWrap wrap)
    {
        _desc.wrap_u = (sg_wrap)wrap;
        _desc.wrap_v = (sg_wrap)wrap;
        return this;
    }

    public SamplerBuilder WithFilter(TextureFilter filter)
    {
        _desc.min_filter = (sg_filter)filter;
        _desc.mag_filter = (sg_filter)filter;
        return this;
    }

    public Sampler Build()
    {
        return new Sampler(ref _desc);
    }
}

public class Sampler : IDisposable
{
    public static SamplerBuilder CreateBuilder()
    {
        return new SamplerBuilder();
    }

    private bool _disposedValue;
    private readonly uint _handle;
    internal uint Handle => _handle;

    internal Sampler(ref sg_sampler_desc desc)
    {
        AssertCall(nox_sampler_create(ref desc, out _handle));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            AssertCall(nox_sampler_free(_handle));
            _disposedValue = true;
        }
    }

    ~Sampler()
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