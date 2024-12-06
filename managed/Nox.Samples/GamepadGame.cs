using System.Numerics;
using Microsoft.Xna.Framework;
using Nox.Framework;

namespace Nox;

public class GamepadGame : Game {
    private SpriteBatch _batch;
    private SpriteFont _font;
    private int _count;
    private GamepadInstance _gamepad;

    public override void Init()
    {
        _batch = new SpriteBatch();

        _font = SpriteFont.Load("../../assets/open-sans.italic.ttf", 30);
        base.Init();
    }

    public override void Update(double deltaTime)
    {
        _count = Gamepad.All.Count();
        _gamepad = Gamepad.All.FirstOrDefault();
        base.Update(deltaTime);
    }

    public override void Render()
    {
        _batch.Begin();
        _batch.DrawText(_font, "Gamepads connected: " + _count, new Vector2(30,60), ColorRGBA.White);
        if(_count > 0){
            _batch.DrawText(_font, "Left Stick X: " + _gamepad.LeftX, new Vector2(30,90), ColorRGBA.White);
            _batch.DrawText(_font, "Left Stick Y: " + _gamepad.LeftY, new Vector2(30,120), ColorRGBA.White);
            _batch.DrawText(_font, "Right Stick X: " + _gamepad.RightX, new Vector2(30,150), ColorRGBA.White);
            _batch.DrawText(_font, "Right Stick Y: " + _gamepad.RightY, new Vector2(30,180), ColorRGBA.White);
            _batch.DrawText(_font, "Trigger L: " + _gamepad.LeftTrigger, new Vector2(30,210), ColorRGBA.White);
            _batch.DrawText(_font, "Trigger R: " + _gamepad.RightTrigger, new Vector2(30,240), ColorRGBA.White);
        }
        _batch.End();
        base.Render();
    }
}
