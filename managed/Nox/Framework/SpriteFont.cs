﻿using System.Collections.Generic;
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
    private float _scale;
    private readonly List<DrawInfo> _line = new();
    private GlyphInfo _lastChar = new() { Index = -1 };


    public TypeSetter(SpriteFont font, Vector2 startingPosition, ColorRGBA color)
    {
        _font = font;
        _size = font.Size;
        _startingPosition = startingPosition;
        _position = startingPosition;
        _color = color;
        _scale = font.Font.GetScale(_size);
    }

    public void Write(char c)
    {
        var info = _font.GetGlyphSpriteInfo(c, _size);
        if(!info.Glyph.Exists) return;

        var kerning = _font.Font.GetKerning(_lastChar, info.Glyph) * _scale;
        if (info is {SpriteSheetRectangle: not null, Offset: not null})
        {
            var drawInfo = new DrawInfo();
            drawInfo.position = _position + info.Offset.Value + new Vector2(kerning, 0);
            drawInfo.sourceRectangle = info.SpriteSheetRectangle.Value;
            _line.Add(drawInfo);
        }
        _lastChar = info.Glyph;
        _position.X += info.Glyph.Advance * _scale;
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
        var line = _line;
        return _line; 
    }

    public void Reset(){
        _position = _startingPosition;
        _line.Clear();
    }
}


public class SpriteFont
{
    public static SpriteFont Load(string path, float size)
    {
        return new SpriteFont(Font.Load(path), size);
    }

    public class GlyphSpriteInfo
    {
        public GlyphInfo Glyph { get; internal set; }
        public Rectangle? SpriteSheetRectangle { get; internal set; }
        public Vector2? Offset { get; internal set; }

    }

    private readonly Font _font;
    private readonly SpriteAtlas<char> _atlas = new(512, 512);
    private readonly Dictionary<char, GlyphSpriteInfo> _glyphs = new();
    public Font Font => _font;
    public float Size { get; private set; }


    public SpriteFont(Font font, float size, string glyphs = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ ")
    {
        _font = font;
        Size = size;
        LoadGlyphs(size, glyphs);
        Update();
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
        if (_glyphs.TryGetValue(c, out var val))
        {
            return val;
        }
        var glyph = new GlyphSpriteInfo();
        glyph.Glyph = _font.GetGlyph(c);
        if (glyph.Glyph.Index > -1)
        {
            var gimg = _font.LoadGlyphImage(glyph.Glyph, size);
            if (gimg != null)
            {
                var rect = _atlas.Add(c, gimg.Image);
                glyph.SpriteSheetRectangle = rect;
                glyph.Offset = gimg.Offset;
            }
        }
        _glyphs[c] = glyph;
        return glyph;
    }

    public void Update(){
        _atlas.Update();
    }

    public Texture2D GetTexture() => _atlas.GetTexture();
}