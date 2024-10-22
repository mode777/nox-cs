using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Nox.Framework;
using static Nox.Native.LibNox;

public enum GraphicsBackend
{
    Gles3 = sg_backend.SG_BACKEND_GLES3,
    D3d11 = sg_backend.SG_BACKEND_D3D11,
    Metal = sg_backend.SG_BACKEND_METAL_MACOS,
    OpenGl = sg_backend.SG_BACKEND_GLCORE
}

public enum ShaderStage {
    Vertex = sg_shader_stage.SG_SHADERSTAGE_VS,
    Fragment = sg_shader_stage.SG_SHADERSTAGE_FS
}

public static class GraphicsDevice
{
    private static Lazy<Texture2D> _whiteTexture = new Lazy<Texture2D>(() => Texture2D.CreateWhiteTexture());
    public static Texture2D WhiteTexture => _whiteTexture.Value;
    public static int Frame { get; private set; }
    public static event Action OnFrameStart;
    public static event Action OnFrame;
    public static event Action OnFrameEnd;

    public static GraphicsBackend Backend
    {
        get
        {
            nox_get_backend(out var backend);
            return (GraphicsBackend)backend;
        }
    }

    public static ColorRGBA ClearColor {
        get {
            nox_get_clear_color(out var color);
            return color;
        }
        set 
        {
            nox_set_clear_color(value);
        }
    }

    public static Vector2 Size
    {
        get
        {
            nox_surface_size(out var x, out var y);
            return new Vector2(x,y);
        }
    }

    public static float DpiScale
    {
        get
        {
            nox_dpi_scale(out var s);
            return s;
        }
    }

    public static double FrameTime
    {
        get
        {
            nox_frame_time(out var time);
            return time;
        }
    }

    public static Double FPS => 1.0 / FrameTime;

    public static void BeginFrame()
    {
        nox_begin_frame();
    }

    public static void ApplyBindings(Bindings bindings){
        AssertCall(nox_pipeline_bindings(ref bindings._bindings));
    }

    public static void ApplyUniforms<T>(ShaderStage stage, int slot, T data) where T : struct {
        unsafe {
            AssertCall(nox_pipeline_uniforms((sg_shader_stage)stage, slot, &data, (IntPtr)Marshal.SizeOf<T>()));
        }
    }

    public static void Draw(int startIndex, int count, int instanceCount){
        AssertCall(nox_pipeline_draw(startIndex, count, instanceCount));
    }

    public static void EndFrame()
    {
        nox_end_frame();
    }

    internal static void FrameCallback() {
        OnFrameStart?.Invoke();
        OnFrame?.Invoke();
        OnFrameEnd?.Invoke();
        Frame++;
    }
}