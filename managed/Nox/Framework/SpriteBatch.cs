using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class SpriteBatch {
    private struct DrawCall {
        public Texture2D texture;
        public int start;
        public int count;
    }

    private readonly QuadBuffer _quadBuffer = new();
    private readonly List<DrawCall> _drawCalls = new();
    private DrawCall _call;

    public void Begin() 
    {
        _quadBuffer.Clear();
        _drawCalls.Clear();
        _call = new();
    }

    public void Draw(Texture2D texture, Vector2 position, Rectangle srcRect, ColorRGBA color)
    {
        if(_call.count == 0){
            _call.texture = texture;
        }
        if(_call.texture != texture){
            _drawCalls.Add(_call);
            _call = new();
            _call.texture = texture;
        }
        _quadBuffer.TryAddQuad(position, srcRect, color);
        _call.count++;
    }

    public void Draw(Texture2D texture, Vector2 position, ColorRGBA color) => Draw(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), color);

    public void DrawText(SpriteFont font, string text, float size, Vector2 position, ColorRGBA color)
    {
        var setter = new TypeSetter(font, size, position, color);
        setter.Write(text);
        foreach(var drawInfo in setter.Flush()){
            Draw(font.GetTexture(), drawInfo.position, drawInfo.sourceRectangle, color);
        }
    }

    public void End() 
    {
        _quadBuffer.Update();
        if(_call.count > 0){
            _drawCalls.Add(_call);
        }
        Renderer2D.Begin();
        foreach(var call in _drawCalls){
            Renderer2D.DrawQuads(call.texture, _quadBuffer.GetBuffer(), _quadBuffer.GetIndexBuffer(), 0, _quadBuffer.Count);
        }

    }
}