using System.Drawing;
using Microsoft.Xna.Framework;
using Nox.Native;

namespace Nox.Framework;

public class Game
{

    internal void OnInit()
    {   
        Init();
    }

    internal void OnFrame()
    {
        Update();
        GraphicsDevice.BeginFrame();
        Render();
        GraphicsDevice.EndFrame();
    }

    public virtual void Init()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Render()
    {

    }
}