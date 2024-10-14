using System;
using System.Runtime.InteropServices;
using static Nox.Native.LibNox;

namespace Nox.Native;

internal static partial class LibNox
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct sg_shader_attr_desc
    {
        public string name;           // GLSL vertex attribute name (optional)
        public string sem_name;       // HLSL semantic name
        public int sem_index;         // HLSL semantic index
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct sg_shader_uniform_desc
    {
        public string name;
        public sg_uniform_type type;
        public int array_count;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_shader_uniform_block_desc
    {
        public IntPtr size;  // size_t mapped to ulong
        public sg_uniform_layout layout;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_UB_MEMBERS)]
        public sg_shader_uniform_desc[] uniforms;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_shader_storage_buffer_desc
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool used;
        [MarshalAs(UnmanagedType.I1)]
        public bool @readonly;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_shader_image_desc
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool used;

        [MarshalAs(UnmanagedType.I1)]
        public bool multisampled;
        public sg_image_type image_type;
        public sg_image_sample_type sample_type;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_shader_sampler_desc
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool used;
        public sg_sampler_type sampler_type;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct sg_shader_image_sampler_pair_desc
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool used;
        public int image_slot;
        public int sampler_slot;
        public string glsl_name;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_range
    {
        IntPtr ptr;
        IntPtr size;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct sg_shader_stage_desc
    {
        public string source;
        public sg_range bytecode;
        public string entry;
        public string d3d11_target;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_UBS)]
        public sg_shader_uniform_block_desc[] uniform_blocks;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_STORAGEBUFFERS)]
        public sg_shader_storage_buffer_desc[] storage_buffers;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_IMAGES)]
        public sg_shader_image_desc[] images;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_SAMPLERS)]
        public sg_shader_sampler_desc[] samplers;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_IMAGESAMPLERPAIRS)]
        public sg_shader_image_sampler_pair_desc[] image_sampler_pairs;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct sg_shader_desc
    {
        public uint _start_canary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_VERTEX_ATTRIBUTES)]
        public sg_shader_attr_desc[] attrs;
        public sg_shader_stage_desc vs;
        public sg_shader_stage_desc fs;
        public string label;
        public uint _end_canary;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_vertex_buffer_layout_state
    {
        public int stride;
        public sg_vertex_step step_func;
        public int step_rate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_vertex_attr_state
    {
        public int buffer_index;
        public int offset;
        public sg_vertex_format format;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_vertex_layout_state
    {

        public sg_vertex_layout_state()
        {
            buffers = new sg_vertex_buffer_layout_state[SG_MAX_VERTEX_BUFFERS];
            attrs = new sg_vertex_attr_state[SG_MAX_VERTEX_ATTRIBUTES];
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_VERTEX_BUFFERS)]
        public sg_vertex_buffer_layout_state[] buffers;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_VERTEX_ATTRIBUTES)]
        public sg_vertex_attr_state[] attrs;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_stencil_face_state
    {
        public sg_compare_func compare;
        public sg_stencil_op fail_op;
        public sg_stencil_op depth_fail_op;
        public sg_stencil_op pass_op;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_stencil_state
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool enabled;
        public sg_stencil_face_state front;
        public sg_stencil_face_state back;
        public byte read_mask;
        public byte write_mask;
        public byte @ref;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_depth_state
    {
        public sg_pixel_format pixel_format;
        public sg_compare_func compare;
        [MarshalAs(UnmanagedType.I1)]
        public bool write_enabled;
        public float bias;
        public float bias_slope_scale;
        public float bias_clamp;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_blend_state
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool enabled;
        public sg_blend_factor src_factor_rgb;
        public sg_blend_factor dst_factor_rgb;
        public sg_blend_op op_rgb;
        public sg_blend_factor src_factor_alpha;
        public sg_blend_factor dst_factor_alpha;
        public sg_blend_op op_alpha;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_color_target_state
    {
        public sg_pixel_format pixel_format;
        public sg_color_mask write_mask;
        public sg_blend_state blend;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_color { float r, g, b, a; }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_pipeline_desc
    {
        public sg_pipeline_desc()
        {
            _start_canary = 0;
            _end_canary = 0;
            colors = new sg_color_target_state[SG_MAX_COLOR_ATTACHMENTS];
            layout = new sg_vertex_layout_state();
            depth = new sg_depth_state();
            stencil = new sg_stencil_state();
        }
        public uint _start_canary;
        public uint shader;
        public sg_vertex_layout_state layout;
        public sg_depth_state depth;
        public sg_stencil_state stencil;
        public int color_count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_COLOR_ATTACHMENTS)]
        public sg_color_target_state[] colors;
        public sg_primitive_type primitive_type;
        public sg_index_type index_type;
        public sg_cull_mode cull_mode;
        public sg_face_winding face_winding;
        public int sample_count;
        public sg_color blend_color;
        [MarshalAs(UnmanagedType.I1)]
        public bool alpha_to_coverage_enabled;
        public string label;
        public uint _end_canary;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_stage_bindings
    {
        public sg_stage_bindings()
        {
            images = new uint[SG_MAX_SHADERSTAGE_IMAGES];
            samplers = new uint[SG_MAX_SHADERSTAGE_SAMPLERS];
            storage_buffers = new uint[SG_MAX_SHADERSTAGE_STORAGEBUFFERS];
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_IMAGES)]
        public uint[] images;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_SAMPLERS)]
        public uint[] samplers;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_SHADERSTAGE_STORAGEBUFFERS)]
        public uint[] storage_buffers;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_bindings
    {
        public sg_bindings()
        {
            _start_canary = 0;
            _end_canary = 0;
            vertex_buffers = new uint[SG_MAX_VERTEX_BUFFERS];
            vertex_buffer_offsets = new int[SG_MAX_VERTEX_BUFFERS];
            vs = new sg_stage_bindings();
            fs = new sg_stage_bindings();
        }

        public uint _start_canary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_VERTEX_BUFFERS)]
        public uint[] vertex_buffers;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = SG_MAX_VERTEX_BUFFERS)]
        public int[] vertex_buffer_offsets;
        public uint index_buffer;
        public int index_buffer_offset;
        public sg_stage_bindings vs;
        public sg_stage_bindings fs;
        public uint _end_canary;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct sg_sampler_desc
    {
        public uint _start_canary;
        public sg_filter min_filter;
        public sg_filter mag_filter;
        public sg_filter mipmap_filter;
        public sg_wrap wrap_u;
        public sg_wrap wrap_v;
        public sg_wrap wrap_w;
        public float min_lod;
        public float max_lod;
        public sg_border_color border_color;
        public sg_compare_func compare;
        public uint max_anisotropy;
        public string label;
        public uint gl_sampler;
        public IntPtr mtl_sampler;
        public IntPtr d3d11_sampler;
        public IntPtr wgpu_sampler;
        public uint _end_canary;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct sapp_event
    {
        public ulong frame_count;               // uint64_t in C
        public sapp_event_type type;            // custom enum for event type
        public sapp_keycode key_code;           // custom enum for key code
        public uint char_code;                  // UTF-32 character code
        [MarshalAs(UnmanagedType.I1)] 
        public bool key_repeat;                 // bool in C
        public uint modifiers;                  // uint32_t in C
        public uint mouse_button;   // custom enum for mouse button
        public float mouse_x;                   // float in C
        public float mouse_y;                   // float in C
        public float mouse_dx;                  // float in C
        public float mouse_dy;                  // float in C
        public float scroll_x;                  // float in C
        public float scroll_y;                  // float in C
        public int num_touches;                 // int in C

        // Fixed-size array for the touch points
        public fixed uint touches[8];

        public int window_width;                // int in C
        public int window_height;               // int in C
        public int framebuffer_width;           // int in C
        public int framebuffer_height;          // int in C
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NoxKernInfo 
    {
        public int glyph_a;
        public int glyph_b;
        public int advance;
    }

    
}