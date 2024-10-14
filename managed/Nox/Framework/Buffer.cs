using System.Runtime.InteropServices;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public enum BufferType {
    VertexBuffer = sg_buffer_type.SG_BUFFERTYPE_VERTEXBUFFER,
    IndexBuffer = sg_buffer_type.SG_BUFFERTYPE_INDEXBUFFER
}

public class Buffer
{
    public static Buffer<T> FromData<T>(T[] data, BufferType type) where T : struct
    {
        var buffer = new Buffer<T>(data.Length, type);
        buffer.Update(data, data.Length);
        return buffer;
    }

    internal readonly uint _handle;
    internal uint Handle => _handle;

    public Buffer(int size, BufferType type)
    {
        unsafe
        {
            AssertCall(nox_buffer_create(size, (sg_buffer_type)type, out _handle));
        }
    }

}

public class Buffer<T> : Buffer, IDisposable where T : struct 
{
    private readonly int _size;
    private readonly BufferType _type;
    private bool disposedValue;

    public BufferType Type => _type;
    public int Size => _size;

    public int ByteSize
    {
        get
        {
            unsafe {
                return Marshal.SizeOf<T>() * _size;
            }
        }
    }

    public Buffer(int size, BufferType type) : base(size * Marshal.SizeOf<T>(), type)
    {
        _size = size;
    }



    public void Update(T[] data, int? size = null){
        var len = size ?? data.Length;
        if(data.Length < len || _size < len) throw new InvalidOperationException("Buffer is to small");
        unsafe {
            fixed (void* p = data)
            {
                AssertCall(nox_buffer_update(_handle, (IntPtr)p, Marshal.SizeOf<T>() * len));
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            AssertCall(nox_buffer_free(_handle));
            disposedValue = true;
        }
    }

    ~Buffer()
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