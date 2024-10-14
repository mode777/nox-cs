using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox;

internal class SpriteFontGame : Game
{
    private SpriteBatch _batch;
    private SpriteFont _font;

    public override void Init()
    {
        _batch = new SpriteBatch();
        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf");
        _font.LoadGlyphs(32f);
        base.Init();
    }

    public override void Render()
    {
        _batch.Begin();
        _batch.DrawText(_font, "Hello World!", 32f, Mouse.Position, ColorRGBA.White);
        _batch.End();
        base.Render();
    }
}