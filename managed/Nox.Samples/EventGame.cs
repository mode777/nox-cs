using System.Numerics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nox.Framework;
using static System.Net.Mime.MediaTypeNames;
using Window = Nox.Framework.Window;

namespace Nox;

public class EventGame : Game
{
    private SpriteBatch _batch;
    private SpriteFont _font;
    private Queue<string> _messages = new Queue<string>();
    private float FontSize;

    public override void Init()
    {
        FontSize = 20 * GraphicsDevice.DpiScale;
        _batch = new SpriteBatch();

        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf", FontSize);
        Window.OnBlur += ev => OnEvent("Window blur", ev);
        Window.OnFocus += ev => OnEvent("Window focus", ev);
        Window.OnResize += ev => OnEvent("Window resize", ev);
        Window.OnMouseDown += @event => OnEvent("Mouse down", @event);
        Window.OnMouseUp += @event => OnEvent("Mouse up", @event);
        Window.OnMouseMove += @event => OnEvent("Mouse move", @event);
        Window.OnMouseScroll += @event => OnEvent("Mouse wheel", @event);
        Window.OnMouseEnter += @event => OnEvent("Mouse enter", @event);
        Window.OnMouseLeave += @event => OnEvent("Mouse leave", @event);
        Window.OnKeyDown += @event => OnEvent("Key down", @event);
        Window.OnKeyUp += @event => OnEvent("Key up", @event);
        Window.OnChar += @event => OnEvent("Char", @event);


        base.Init();
    }

    private void OnEvent(string type, object obj)
    {
        QueueMessage($"{type} event: {JsonConvert.SerializeObject(obj)}");
    }

    private void QueueMessage(string message)
    {
        _messages.Enqueue(message);
        while (_messages.Count > (GraphicsDevice.Size.Y / FontSize) -2)
        {
            _messages.Dequeue();
        }
    }

    public override void Render()
    {
        _batch.Begin();
        for (int i = 0; i < _messages.Count; i++)
        {
            _batch.DrawText(_font, _messages.ElementAt(i), new Vector2(30, 30 + i * FontSize), ColorRGBA.White);
        }
        _batch.End();

    }
}

