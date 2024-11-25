using System;
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
        public Matrix4x4 Transform;
    }

    private readonly QuadBuffer _quadBuffer = new();
    private readonly List<DrawCall> _drawCalls = new();
    private DrawCall _call;
    private BlendMode _blendMode = BlendMode.Alpha;
    private Stack<Matrix4x4> _transformStack = new();

    public void Begin() 
    {
        _drawCalls.Clear();
        _call = new();
        _quadBuffer.Clear();
        _transformStack.Clear();
        _transformStack.Push(Matrix4x4.Identity);
    }

    public void DrawRect(RectangleF rect, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha) => Draw(GraphicsDevice.WhiteTexture, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, (int)rect.Width, (int)rect.Height), color, blendMode);

    public void Draw(Texture2D texture, Vector2 position, Rectangle srcRect, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha)
    {
        SetupDrawCall(texture, blendMode);
        _quadBuffer.TryAddQuad(position, srcRect, color);
        _call.Count++;
    }

    public void Draw(Texture2D texture, Transform2d transform, Rectangle srcRect, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha)
    {
        SetupDrawCall(texture, blendMode);
        _quadBuffer.TryAddQuadTransformed(transform, srcRect, color);
        _call.Count++;
    }

    public void DrawLine(Vector2 a, Vector2 b, float width, ColorRGBA color, BlendMode blendMode = BlendMode.Alpha)
    {
        SetupDrawCall(GraphicsDevice.WhiteTexture, blendMode);
        // Construct four points of a line from a to b with width
        var n = Vector2.Normalize(b - a);
        var t = new Vector2(-n.Y, n.X);
        var w = width / 2;
        var p1 = a + t * w;
        var p2 = a - t * w;
        var p3 = b - t * w;
        var p4 = b + t * w;
        _quadBuffer.TryAddQuadPoints(p1, p2, p3, p4, Rectangle.Empty, color);
        _call.Count++;
    }

    public void PushTransform(Transform2d transform)
    {
        _transformStack.Push(_transformStack.Peek() * transform.GetMatrix4x4());
        if(_call.Count > 0){
            _drawCalls.Add(_call);
            _call = new();
        }
    }

    public void PopTransform()
    {
        _transformStack.Pop();
        if(_call.Count > 0){
            _drawCalls.Add(_call);
            _call = new();
        }
    }

    private void SetupDrawCall(Texture2D texture, BlendMode blendMode)
    {
        if (_call.Count == 0)
        {
            _call.Texture = texture;
            _call.BlendMode = blendMode;
            _call.Transform = _transformStack.Peek();
        }
        if (_call.Texture != texture || _call.BlendMode != blendMode)
        {
            _drawCalls.Add(_call);
            var call = new DrawCall();
            call.Texture = texture;
            call.BlendMode = blendMode;
            call.Transform = _transformStack.Peek();
            call.Start = _call.Start + _call.Count;
            _call = call;
        }
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
            Renderer2D.Draw(call.Texture, _quadBuffer.GetOrCreateBuffer(), _quadBuffer.GetOrCreateIndexBuffer(), call.Start*6, call.Count*6, call.Transform, call.BlendMode);
        }
        Renderer2D.End();
    }


}