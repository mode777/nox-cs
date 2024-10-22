using System.Drawing;
using Microsoft.Xna.Framework;
using Nox.Native;

namespace Nox.Framework;

public class Game
{
    private double _lastTime = Application.Time;

    internal void OnInit()
    {   
        Init();
    }

    internal void OnFrame()
    {
        var currentTime = Application.Time;
        Update(currentTime-_lastTime);
        GraphicsDevice.BeginFrame();
        Render();
        GraphicsDevice.EndFrame();
        _lastTime = currentTime;
    }

    public virtual void Init()
    {
    }

    public virtual void Update(double time)
    {
    }

    public virtual void Render()
    {

    }
}