using System;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public enum PixelFormat {
    R8 = sg_pixel_format.SG_PIXELFORMAT_R8,
    RGBA8 = sg_pixel_format.SG_PIXELFORMAT_RGBA8,
}

public class Texture2D : IDisposable
{
    public static Texture2D FromImage(Image image, PixelFormat format = PixelFormat.RGBA8){
        var texture = new Texture2D(image.Width, image.Height, image.Components, format);
        texture.Update(image);
        return texture;
    }

    public static Texture2D Load(string path, PixelFormat format = PixelFormat.RGBA8){
        var image = Image.Load(path);
        var texture = new Texture2D(image.Width, image.Height, image.Components, format);
        texture.Update(image);
        return texture;
    }

    private readonly int _w;
    private readonly int _h;
    private readonly int _c;
    private readonly uint _handle;
    private readonly PixelFormat _format;
    private bool disposedValue;
    private int _lastUpdateFrame = -1;

    public int Width => _w;
    public int Height => _h;
    public int Components => _c;
    internal uint Handle => _handle;
    public PixelFormat Format => _format;
    public Texture2D(int w, int h, int c = 4, PixelFormat format = PixelFormat.RGBA8){
        _w = w;
        _h = h;
        _c = c;
        AssertCall(nox_texture_create(w,h, (sg_pixel_format)format, out _handle));
    }

    public void Update(Image image){
        if(_lastUpdateFrame == GraphicsDevice.Frame) throw new InvalidOperationException("Only one update is allowed per frame");
        if(Width != image.Width || Height != image.Height || Components != image.Components) throw new InvalidOperationException("Imagem ust be same size as texture");
        AssertCall(nox_texture_update(_handle, image.Pointer, _w, _h, _c));
        _lastUpdateFrame = GraphicsDevice.Frame;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            AssertCall(nox_texture_free(_handle));
            disposedValue = true;
        }
    }

    ~Texture2D()
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
