using System.Numerics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Nox.Framework;
using static System.Net.Mime.MediaTypeNames;
using Application = Nox.Framework.Application;

namespace Nox;

public class EventGame : Game
{
    private SpriteBatch _batch;
    private SpriteFont _font;
    private Queue<string> _messages = new Queue<string>();

    public override void Init()
    {
        _batch = new SpriteBatch();

        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf");
        _font.LoadGlyphs(20);
        _font.Update();

        Application.OnBlur += ev => OnEvent("Window blur", ev);
        Application.OnFocus += ev => OnEvent("Window focus", ev);
        Application.OnResize += ev => OnEvent("Window resize", ev);
        Application.OnMouseDown += @event => OnEvent("Mouse down", @event);
        Application.OnMouseUp += @event => OnEvent("Mouse up", @event);
        Application.OnMouseMove += @event => OnEvent("Mouse move", @event);
        Application.OnMouseScroll += @event => OnEvent("Mouse wheel", @event);
        Application.OnMouseEnter += @event => OnEvent("Mouse enter", @event);
        Application.OnMouseLeave += @event => OnEvent("Mouse leave", @event);
        Application.OnKeyDown += @event => OnEvent("Key down", @event);
        Application.OnKeyUp += @event => OnEvent("Key up", @event);
        Application.OnChar += @event => OnEvent("Char", @event);


        base.Init();
    }

    private void OnEvent(string type, object obj)
    {
        QueueMessage($"{type} event: {JsonConvert.SerializeObject(obj)}");
    }

    private void QueueMessage(string message)
    {
        _messages.Enqueue(message);
        while (_messages.Count > (GraphicsDevice.Size.Y / 20) -2)
        {
            _messages.Dequeue();
        }
    }

    public override void Render()
    {
        _batch.Begin();
        for (int i = 0; i < _messages.Count; i++)
        {
            _batch.DrawText(_font, _messages.ElementAt(i), 20, new Vector2(30, 30 + i * 20), ColorRGBA.White);
        }
        _batch.End();

    }
}

