using System;
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox.Samples;

public class GlobalTransformGame : Game
{
    private Texture2D _texture = Texture2D.Load("../../assets/wabbit_alpha.png");
    private SpriteBatch _spriteBatch = new();
    private Transform2d _transform = new();

    public override void Init(){
        base.Init();
    }

    public override void Update(double delta)
    {
        _transform.Position += new Vector2(1, 1);
        // var sin = (float)Math.Sin(Application.Time) * 8;
        // _transform.Scale = new Vector2((12+sin)*GraphicsDevice.DpiScale, (12+sin)*GraphicsDevice.DpiScale);
        // _transform.Rotation += 0.01f;
        // base.Update(delta);
    }

    public override void Render()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, new Vector2(100,100), _texture.Bounds, ColorRGBA.Cyan);
        _spriteBatch.PushTransform(_transform);
        _spriteBatch.Draw(_texture, new Vector2(100,100), _texture.Bounds, ColorRGBA.Cyan);
        _spriteBatch.End();
        base.Render();
    }

}
