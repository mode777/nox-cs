using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class GlyphImage
{
    public Image Image { get; internal set; }
    public Vector2 Offset { get; internal set; }
}

public struct GlyphInfo
{
    public int Index;
    public int Advance;
    public int Bearing;
    public bool Exists => Index != -1;
}

public class Font : IDisposable
{
    public static Font Load(string path)
    {
        var bytes = File.ReadAllBytes(path);
        return new Font(Path.GetFileNameWithoutExtension(path), bytes);
    }

    private int _ascender, _descender, _lineGap;

    public int Ascender => _ascender;
    public int Descender => _descender;
    public int LineGap => _lineGap;
    private readonly Dictionary<(int, int), int> _kernings;
    private readonly Dictionary<char, GlyphInfo> _glyphs = new();
    private readonly string _name;
    private nint _handle;
    private bool disposedValue;

    public Font(string name, byte[] ttfFile)
    {
        _name = name;
        unsafe
        {
            int nKern;
            fixed (byte* ptr = ttfFile)
            {
                AssertCall(nox_font_load(ptr, ttfFile.Length, out _handle, out _ascender, out _descender, out _lineGap, out nKern));
            }
            var kernings = new NoxKernInfo[nKern];
            if (kernings.Length > 0)
            {
                fixed (NoxKernInfo* ptr = kernings)
                {
                    AssertCall(nox_font_load_kernings(_handle, ptr, (IntPtr)kernings.Length));
                }
            }
            _kernings = kernings.ToDictionary(x => (x.glyph_a, x.glyph_b), x => x.advance);
        }
    }

    public int GetKerning(GlyphInfo a, GlyphInfo b) => _kernings.TryGetValue((a.Index, b.Index), out var k) ? k : 0;

    public float GetScale(float size) => size / (_ascender - _descender);

    public GlyphImage? LoadGlyphImage(GlyphInfo gi, float size)
    {
        if (gi.Index == -1) return null;
        var scale = GetScale(size);
        var result = nox_font_load_glyph_bitmap(_handle, gi.Index, scale, out var imgData, out var w, out var h, out var ox, out var oy);
        if (result != NoxResult.SUCCESS) return null;
        var glyphImg = new Image(imgData, w, h, 4);
        var info = new GlyphImage
        {
            Image = glyphImg,
            Offset = new Vector2(ox, oy)
        };
        return info;
    }
    
    public GlyphInfo GetGlyph(char c)
    {
        if(_glyphs.TryGetValue(c, out var i)){
            return i;
        }
        var gi = new GlyphInfo();
        gi.Index = -1;
        var codePoint = char.ConvertToUtf32(c.ToString(), 0);
        nox_font_load_glyph(_handle, codePoint, out gi.Index, out gi.Advance, out gi.Bearing);
        _glyphs[c] = gi;
        return gi;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            System.Console.WriteLine("Disposing font: " + _handle);
            AssertCall(nox_font_free(_handle));
            disposedValue = true;
        }
    }

    ~Font()
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