using System.Runtime.InteropServices;
using static Nox.Native.LibNox;

namespace Nox.Native;

internal static partial class LibNox {
    const string LIB_PATH = "libnox";

    public static unsafe void MyLogFunc(
        byte* tag,
        uint log_level,
        uint log_item_id,
        byte* message_or_null,
        uint line_nr,
        byte* filename_or_null,
        void* user_data)
    {
        string tagStr = tag != null ? Marshal.PtrToStringAnsi((IntPtr)tag) : "null";
        string message = message_or_null != null ? Marshal.PtrToStringAnsi((IntPtr)message_or_null) : "null";
        string filename = filename_or_null != null ? Marshal.PtrToStringAnsi((IntPtr)filename_or_null) : "null";

        Console.WriteLine($"Tag: {tagStr}, Level: {log_level}, Item ID: {log_item_id}, " +
                          $"Message: {message}, Line: {line_nr}, Filename: {filename}");
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void NoxLogFunc(
        byte* tag,
        uint log_level,
        uint log_item_id,
        byte* message_or_null,
        uint line_nr,
        byte* filename_or_null,
        void* user_data
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void NoxEventFunc(sapp_event* ptr);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void NoxAudioFunc(float* buffer, int num_frames, int num_channels);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void NoxInitFunc();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void NoxFrameFunc();

    public static void AssertCall(NoxResult result)
    {
        if (result != NoxResult.SUCCESS)
        {
            throw new Exception($"Nox call failed with result {result}");
        }
    }

    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern void nox_run(NoxLogFunc logFunc, NoxEventFunc eventFunc, NoxInitFunc initFunc, NoxFrameFunc frameFunc, NoxAudioFunc audioFunc);
    
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_image_blit(IntPtr dest, int dest_width, int dest_height, int dest_c, IntPtr src, int src_width, int src_height, int x, int y);
    
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_image_alloc(int x, int y, int c, out IntPtr out_data);

    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe NoxResult nox_image_load(byte* data, nuint len, out IntPtr out_data, out int out_w, out int out_h, out int out_c);
    
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_image_free(nint ptr);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_texture_create(int w, int h, sg_pixel_format format, out uint out_image);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_texture_update(uint image, IntPtr data, int w, int h, int c);
    
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_texture_free(uint image);
    
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_buffer_create(IntPtr size, sg_buffer_type type, out uint out_buffer);
    
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_buffer_update(uint buffer, IntPtr data, IntPtr length);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_buffer_free(uint buffer);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_begin_frame();
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_end_frame();
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)] 
    public static unsafe extern NoxResult nox_font_load(byte* data, out IntPtr out_handle, out int out_ascent, out int out_descent, out int out_line_gap, out int out_num_kernings);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern NoxResult nox_font_load_kernings(IntPtr handle, NoxKernInfo* out_kernings, IntPtr len);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_font_free(IntPtr handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_font_load_glyph(IntPtr handle, int codepoint, out int out_index, out int out_advance, out int out_bearing);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_font_load_glyph_bitmap(IntPtr handle, int index, float scale, out IntPtr out_data, out int out_w, out int out_h, out int out_offset_x, out int out_offset_y);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_font_free_glyph_bitmap(IntPtr data);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_get_backend(out sg_backend out_backend);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_shader_create(ref sg_shader_desc desc, out uint handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_shader_free(uint handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_pipeline_create(ref sg_pipeline_desc desc, out uint handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_pipeline_apply(uint handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_pipeline_free(uint handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_pipeline_bindings(ref sg_bindings bindings);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static unsafe extern NoxResult nox_pipeline_uniforms(sg_shader_stage stage, int slot, void* uniforms, IntPtr uniforms_size);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_pipeline_draw(int start, int count, int instances);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_sampler_create(ref sg_sampler_desc desc, out uint handle);
    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_sampler_free(uint handle);

    [DllImport(LIB_PATH, CallingConvention = CallingConvention.Cdecl)]
    public static extern NoxResult nox_surface_size(out int w, out int h);
}