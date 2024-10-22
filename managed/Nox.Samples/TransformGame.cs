using System;
using System.Drawing;
using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox.Samples;

public class TransformGame : Game
{
    private Texture2D _texture = Texture2D.Load("../../assets/wabbit_alpha.png");
    private SpriteBatch _spriteBatch = new();
    private Transform2d _transform = new();

    public override void Init(){
        _transform.Position = GraphicsDevice.Size / 2;
        _transform.Origin = new Vector2(16, 16);
        base.Init();
    }

    public override void Update(double delta)
    {
        var sin = (float)Math.Sin(Application.Time) * 8;
        _transform.Scale = new Vector2((12+sin)*GraphicsDevice.DpiScale, (12+sin)*GraphicsDevice.DpiScale);
        _transform.Rotation += 0.01f;
        base.Update(delta);
    }

    public override void Render()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_texture, _transform, new Rectangle(0, 0, 32, 32), ColorRGBA.Cyan);
        _spriteBatch.End();
        base.Render();
    }

}
