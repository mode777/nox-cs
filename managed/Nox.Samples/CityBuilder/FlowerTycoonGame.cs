namespace Nox.Samples.CityBuilder;

using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

public class FlowerTycoonGame : Game
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    public override void Init()
    {
        _spriteBatch = new SpriteBatch();
        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf", 24);
        base.Init();
    }

    public override void Render()
    {
        _spriteBatch.Begin();
        _spriteBatch.DrawLine(new Vector2(0, 0), new Vector2(1280, 720), 5, ColorRGBA.Red);
        //_spriteBatch.DrawLine(new Vector2(0, 720), new Vector2(1280, 0), 5, ColorRGBA.Red);
        _spriteBatch.DrawLine(new Vector2(1280, 0), new Vector2(0, 720), 5, ColorRGBA.Red);
        DrawGrid(16, 16, 32);
        _spriteBatch.DrawText(_font, "Flower Tycoon Pre-Alpha", new Vector2(10, 24), ColorRGBA.White);
        _spriteBatch.End();
        base.Render();
    }

    private void DrawGrid(int width, int height, int size)
    {
        for (int x = 0; x < width; x++)
        {
            _spriteBatch.DrawLine(new Vector2(x * size, 0), new Vector2(x * size, height * size), 1, ColorRGBA.Gray);
        }
        for (int y = 0; y < height; y++)
        {
            _spriteBatch.DrawLine(new Vector2(0, y * size), new Vector2(width * size, y * size), 1, ColorRGBA.Gray);
        }
        
    }
    
}