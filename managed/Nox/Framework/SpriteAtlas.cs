using System.Drawing;

namespace Nox.Framework;

public class SpriteAtlas<TKey>
{
    private Image _image;
    private Texture2D _texture;
    private RectanglePacker _packer;

    private readonly Dictionary<TKey, Rectangle> sprites = new Dictionary<TKey, Rectangle>();
    private readonly int _width;
    private readonly int _height;
    private bool _isDirty;

    public SpriteAtlas(int width, int height)
    {
        _width = width;
        _height = height;
        _image = new Image(width, height);
        _texture = new Texture2D(width,height);
        _packer = new RectanglePacker(width,height);
    }

    public Rectangle Add(TKey key, Image image)
    {
        if(_packer.AddRect(image.Width, image.Height, out var x, out var y)){
            _image.BlitImage(image, x, y);
            _isDirty = true;
            sprites[key] = new Rectangle(x,y,image.Width,image.Height);
            return sprites[key];
        }
        else {
            throw new InvalidOperationException("SpriteAtlas too small");
            // TODO: Grow
        }
    }

    public Rectangle this[TKey name] => sprites[name];

    public void Update(){
        if(_isDirty){
            _texture.Update(_image);
            _isDirty = false;
        }
    }

    public Texture2D GetTexture(){
        return _texture;
    }

    public bool Contains(TKey c) => sprites.ContainsKey(c);
}