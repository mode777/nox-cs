using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;

namespace Nox.Framework;

public struct DrawInfo
{
    public Vector2 position;
    public Rectangle sourceRectangle;
}

public class TypeSetter
{
    private readonly SpriteFont _font;
    private float _size;
    private readonly Vector2 _startingPosition;
    private Vector2 _position;
    private ColorRGBA _color;
    private readonly List<DrawInfo> _line = new();
    private GlyphInfo _lastChar = new() { index = -1 };


    public TypeSetter(SpriteFont font, float size, Vector2 startingPosition, ColorRGBA color)
    {
        _font = font;
        _size = size;
        _startingPosition = startingPosition;
        _position = startingPosition;
        _color = color;
    }

    public void Write(char c)
    {
        var info = _font.GetGlyphSpriteInfo(c, _size);
        if(!info.Glyph.Exists) return;
        var offset = _font.Font.GetKerning(_lastChar, info.Glyph, _size);
        _position.X += offset;
        if (info is {SpriteSheetRectangle: not null, Offset: not null})
        {
            var drawInfo = new DrawInfo();
            drawInfo.position = _position + info.Offset.Value;
            drawInfo.sourceRectangle = info.SpriteSheetRectangle.Value;
            _line.Add(drawInfo);
        }
        _lastChar = info.Glyph;
    }

    public void Write(string text)
    {
        foreach (var c in text)
        {
            Write(c);
        }
    }

    public IEnumerable<DrawInfo> Flush()
    {
        return _line; 
    }
}


public class SpriteFont
{
    public static SpriteFont Load(string path)
    {
        return new SpriteFont(Font.Load(path));
    }

    public class GlyphSpriteInfo
    {
        public GlyphInfo Glyph { get; internal set; }
        public Rectangle? SpriteSheetRectangle { get; internal set; }
        public Vector2? Offset { get; internal set; }

    }

    private readonly Font _font;
    private readonly SpriteAtlas<(char, float)> _atlas;
    private readonly Dictionary<(char, float), GlyphSpriteInfo> _glyphs = new();
    public Font Font => _font;


    public SpriteFont(Font font) : this(font, new SpriteAtlas<(char,float)>(512, 512))
    {
    }

    public SpriteFont(Font font, SpriteAtlas<(char,float)> atlas)
    {
        _font = font;
        _atlas = atlas;
    }

    public void LoadGlyphs(float size, string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMOPQRSTUVWXYZ1234567890()?!.,_:'%[]{}")
    {
        foreach (var c in text)
        {
            GetGlyphSpriteInfo(c, size);
        }
    }

    public GlyphSpriteInfo GetGlyphSpriteInfo(char c, float size)
    {
        if (_glyphs.TryGetValue((c, size), out var val))
        {
            return val;
        }
        var glyph = new GlyphSpriteInfo();
        glyph.Glyph = _font.GetGlyph(c);
        if (glyph.Glyph.index > -1)
        {
            var gimg = _font.LoadGlyphImage(glyph.Glyph, size);
            if (gimg != null)
            {
                var rect = _atlas.Add((c, size), gimg.Image);
                glyph.SpriteSheetRectangle = rect;
                glyph.Offset = gimg.Offset;
            }
        }
        _glyphs[(c, size)] = glyph;
        return glyph;
    }

    public void Update(){
        _atlas.Update();
    }

    public Texture2D GetTexture() => _atlas.GetTexture();
}