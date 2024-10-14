using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox;

public class SpriteAtlasGame : Game {
    private SpriteBatch _spriteBatch;
    private SpriteAtlas<char> _atlas;
    private Font _font;

    public override void Init()
    {
        base.Init();
        _spriteBatch = new SpriteBatch();
        _atlas = new SpriteAtlas<char>(512,512);
        _font = Font.Load("../../assets/open-sans.italic.ttf");
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMOPQRSTUVWXYZ1234567890".ToList().ForEach(x => _atlas.Add(x,_font.LoadGlyphImage(_font.GetGlyph(x), 96f).Image));
    }

    public override void Render()
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_atlas.GetTexture(), Mouse.GetPosition(), ColorRGBA.White);
        _spriteBatch.End();
        base.Render();
    }
}