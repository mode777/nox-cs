using System.Drawing;
using Microsoft.Xna.Framework;
using Nox.Native;

namespace Nox.Framework;

public class Game
{
    private void OnInit()
    {   
        Init();
    }

    private void OnFrame()
    {
        Update();
        GraphicsDevice.BeginFrame();
        Render();
        GraphicsDevice.EndFrame();
        Update();
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

    public void Run()
    {
        GraphicsDevice.OnFrame += OnFrame;
        Application.OnInit += OnInit;
        Application.Run();
    }
}