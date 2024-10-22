using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class SpriteBatch {
    private struct DrawCall {
        public Texture2D Texture;
        public int Start;
        public int Count;
        public BlendMode BlendMode;
    }

    private readonly QuadBuffer _quadBuffer = new();
    private readonly List<DrawCall> _drawCalls = new();
    private DrawCall _call;
    private BlendMode _blendMode = BlendMode.Alpha;

    public void Begin() 
    {
        _drawCalls.Clear();
        _call = new();
        _quadBuffer.Clear();
    }

    public void DrawRect(RectangleF rect, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha) => Draw(GraphicsDevice.WhiteTexture, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, (int)rect.Width, (int)rect.Height), color, blendMode);

    public void Draw(Texture2D texture, Vector2 position, Rectangle srcRect, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha)
    {
        if(_call.Count == 0){
            _call.Texture = texture;
            _call.BlendMode = blendMode;
        }
        if(_call.Texture != texture || _call.BlendMode != blendMode){
            _drawCalls.Add(_call);
            var call = new DrawCall();
            call.Texture = texture;
            call.BlendMode = blendMode;
            call.Start = _call.Start + _call.Count;
            _call = call;
        }
        _quadBuffer.TryAddQuad(position, srcRect, color);
        _call.Count++;
    }

    public void Draw(Texture2D texture, Transform2d transform, Rectangle srcRect, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha)
    {
        if(_call.Count == 0){
            _call.Texture = texture;
            _call.BlendMode = blendMode;
        }
        if(_call.Texture != texture || _call.BlendMode != blendMode){
            _drawCalls.Add(_call);
            var call = new DrawCall();
            call.Texture = texture;
            call.BlendMode = blendMode;
            call.Start = _call.Start + _call.Count;
            _call = call;
        }
        _quadBuffer.TryAddQuadTransformed(transform, srcRect, color);
        _call.Count++;
    }

    public void Draw(Texture2D texture, Vector2 position, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha) => Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color, blendMode);

    public void DrawText(SpriteFont font, string text, Vector2 position, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha)
    {
        var setter = new TypeSetter(font, position, color);
        setter.Write(text);
        foreach(var drawInfo in setter.Flush()){
            Draw(font.GetTexture(), drawInfo.position, drawInfo.sourceRectangle, color, blendMode);
        }
    }

    public void End() 
    {
        _quadBuffer.Update();
        if(_call.Count > 0){
            _drawCalls.Add(_call);
        }
        Renderer2D.Begin();
        foreach(var call in _drawCalls){
            Renderer2D.Draw(call.Texture, _quadBuffer.GetOrCreateBuffer(), _quadBuffer.GetOrCreateIndexBuffer(), call.Start*6, call.Count*6, call.BlendMode);
        }
        Renderer2D.End();
    }
}