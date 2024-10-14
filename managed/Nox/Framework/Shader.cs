using System;
using Nox.Shaders;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class Shader : IDisposable
{
    public static Shader FromMetadata(ProgramDescription shader)
    {
        var desc = shader.CreateShaderDesc();
        return new Shader(ref desc);
    }

    private bool _disposedValue;
    private readonly uint _handle;
    internal uint Handle => _handle;

    internal Shader(ref sg_shader_desc desc){
        AssertCall(nox_shader_create(ref desc, out _handle));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            AssertCall(nox_shader_free(_handle));
            _disposedValue = true;
        }
    }

    ~Shader()
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