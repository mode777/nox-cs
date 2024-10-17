using System;
using static Nox.Native.LibNox;

namespace Nox.Framework;

public class ApplicationConfiguration {
    internal NoxAppDesc _desc = new NoxAppDesc();

    public ApplicationConfiguration(){
        unsafe {
            _desc.width = 800;
            _desc.height = 600;
            _desc.window_title = "Nox";
            _desc.high_dpi = true;
            _desc.init_cb = Application.InitCallback;
            _desc.event_cb = Window.EventCallback;
            _desc.frame_cb = GraphicsDevice.FrameCallback;
            _desc.stream_cb = AudioDevice.AudioCallback;
            _desc.logger = MyLogFunc;
        }
    }

    public int Width {
        get => _desc.width;
        set => _desc.width = value;
    }

    public int Height {
        get => _desc.height;
        set => _desc.height = value;
    }

    public string WindowTitle {
        get => _desc.window_title;
        set => _desc.window_title = value;
    }

    public bool HighDpi {
        get => _desc.high_dpi;
        set => _desc.high_dpi = value;
    }

    public bool Fullscreen {
        get => _desc.fullscreen;
        set => _desc.fullscreen = value;
    }
}

public static class Application {

    public static event Action OnInit;

    internal static void InitCallback()
    {
        Renderer2D.Init();
        OnInit?.Invoke();
    }

    public static void Run<T>(ApplicationConfiguration config) where T : Game, new()
    {
        T game = null;
        OnInit += () => {
            System.Console.WriteLine($"Nox initialized with backend '{GraphicsDevice.Backend}'. Framebuffer: {GraphicsDevice.Size} DpiScale {GraphicsDevice.DpiScale}");
            game = Activator.CreateInstance<T>();
            game.OnInit();
        };
        GraphicsDevice.OnFrame += () => game.OnFrame();
        nox_run(ref config._desc);
    }

    public static void Run<T>() where T : Game, new() => Run<T>(new ApplicationConfiguration());

}


