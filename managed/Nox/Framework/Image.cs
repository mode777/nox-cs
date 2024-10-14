using Nox.Native;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class Image : IDisposable
{
    private readonly IntPtr _pointer;
    private readonly int _w;
    private readonly int _h;
    private readonly int _c;
    private bool disposedValue;

    public static Image Load(string path)
    {
        unsafe
        {
            var bytes = System.IO.File.ReadAllBytes(path);
            fixed (byte* ptr = bytes)
            {
                AssertCall(nox_image_load(ptr, (UIntPtr) bytes.Length, out var data, out var w, out var h,out var c));
                return new Image(data, w, h, c);
            }
        }
    }

    public Image(int w, int h, int c = 4){
        _w = w;
        _h = h;
        _c = c;
        AssertCall(nox_image_alloc(w,h,c,out var ptr));
        _pointer = ptr;
    }

    internal Image(IntPtr pointer, int w, int h, int c)
    {
        _pointer = pointer;
        _w = w;
        _h = h;
        _c = c;
    }

    public int Width => _w;
    public int Height => _h;
    public int Components => _c;
    internal IntPtr Pointer => _pointer;

    public void BlitImage(Image other, int x, int y){
        if(other._c != _c) throw new InvalidOperationException("Images do not have the same format");
        AssertCall(nox_image_blit(_pointer, _w, _h, _c, other._pointer, other._w, other._h, x, y));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            AssertCall(nox_image_free(_pointer));
            disposedValue = true;
        }
    }

    ~Image()
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