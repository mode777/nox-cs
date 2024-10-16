using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox;

public class MyGame : Game
{
    private struct Item {
        public const int MAX_X = (1280-32);
        public const int MAX_Y = (720-32);
        public Vector2 pos = new(Random.Shared.NextSingle() * MAX_X, Random.Shared.NextSingle() * MAX_Y);
        public Vector2 vel = new((Random.Shared.NextSingle() * 5)-2.5f,(Random.Shared.NextSingle() * 5)-2.5f);
        public ColorRGBA color = new(new Vector3(Random.Shared.NextSingle(), Random.Shared.NextSingle(), Random.Shared.NextSingle()));
        public Item(){}
    }

    private Texture2D _texture;
    private Image _img = new Image(32,32,4);
    private Font _font = Font.Load("../../assets/open-sans.italic.ttf");
    private SpriteBatch _batch = new();
    private Item[] _items = new Item[16000];

    public override void Init()
    {
        var Q = _font.LoadGlyphImage(_font.GetGlyph('@'), 32f);
        _img.BlitImage(Q.Image, 0, 0);
        _texture = Texture2D.FromImage(_img);
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = new Item();
        }

        base.Init();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void Render()
    {
        var mouse = Mouse.GetPosition();
        var size = GraphicsDevice.Size;
        _batch.Begin();
        //_batch.Draw(_texture, mouse, ColorRGBA.White);
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].pos += _items[i].vel;
            var v2 = _items[i].pos;
            if(v2.X > size.X-32 || v2.X < 0) _items[i].vel *= new Vector2(-1,1);
            if(v2.Y > size.Y-32 || v2.Y < 0) _items[i].vel *= new Vector2(1,-1);
            _batch.Draw(_texture, _items[i].pos, _items[i].color);
        }
        _batch.End();
        base.Render();
    }
}
