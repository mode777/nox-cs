using System;
using static Nox.Native.LibNox;

namespace Nox.Native;

internal static partial class LibNox
{
    internal const int SG_MAX_UB_MEMBERS = 16;
    internal const int SG_MAX_SHADERSTAGE_UBS = 4;
    internal const int SG_MAX_SHADERSTAGE_STORAGEBUFFERS = 8;
    internal const int SG_MAX_SHADERSTAGE_IMAGES = 12;
    internal const int SG_MAX_SHADERSTAGE_SAMPLERS = 8;
    internal const int SG_MAX_SHADERSTAGE_IMAGESAMPLERPAIRS = 12;
    internal const int SG_MAX_VERTEX_ATTRIBUTES = 16;
    internal const int SG_MAX_COLOR_ATTACHMENTS = 4;
    internal const int SG_MAX_VERTEX_BUFFERS = 8;

    public enum sg_primitive_type
    {
        _SG_PRIMITIVETYPE_DEFAULT,  // value 0 reserved for default-init
        SG_PRIMITIVETYPE_POINTS,
        SG_PRIMITIVETYPE_LINES,
        SG_PRIMITIVETYPE_LINE_STRIP,
        SG_PRIMITIVETYPE_TRIANGLES,
        SG_PRIMITIVETYPE_TRIANGLE_STRIP,
        _SG_PRIMITIVETYPE_NUM,
        _SG_PRIMITIVETYPE_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_uniform_type
    {
        SG_UNIFORMTYPE_INVALID,
        SG_UNIFORMTYPE_FLOAT,
        SG_UNIFORMTYPE_FLOAT2,
        SG_UNIFORMTYPE_FLOAT3,
        SG_UNIFORMTYPE_FLOAT4,
        SG_UNIFORMTYPE_INT,
        SG_UNIFORMTYPE_INT2,
        SG_UNIFORMTYPE_INT3,
        SG_UNIFORMTYPE_INT4,
        SG_UNIFORMTYPE_MAT4,
        _SG_UNIFORMTYPE_NUM,
        _SG_UNIFORMTYPE_FORCE_U32 = 0x7FFFFFFF
    }
    public enum sg_uniform_layout
    {
        _SG_UNIFORMLAYOUT_DEFAULT,     // value 0 reserved for default-init
        SG_UNIFORMLAYOUT_NATIVE,       // default: layout depends on currently active backend
        SG_UNIFORMLAYOUT_STD140,       // std140: memory layout according to std140
        _SG_UNIFORMLAYOUT_NUM,
        _SG_UNIFORMLAYOUT_FORCE_U32 = 0x7FFFFFFF
    }
    public enum sg_image_type
    {
        _SG_IMAGETYPE_DEFAULT,  // value 0 reserved for default-init
        SG_IMAGETYPE_2D,
        SG_IMAGETYPE_CUBE,
        SG_IMAGETYPE_3D,
        SG_IMAGETYPE_ARRAY,
        _SG_IMAGETYPE_NUM,
        _SG_IMAGETYPE_FORCE_U32 = 0x7FFFFFFF
    }
    public enum sg_image_sample_type
    {
        _SG_IMAGESAMPLETYPE_DEFAULT,  // value 0 reserved for default-init
        SG_IMAGESAMPLETYPE_FLOAT,
        SG_IMAGESAMPLETYPE_DEPTH,
        SG_IMAGESAMPLETYPE_SINT,
        SG_IMAGESAMPLETYPE_UINT,
        SG_IMAGESAMPLETYPE_UNFILTERABLE_FLOAT,
        _SG_IMAGESAMPLETYPE_NUM,
        _SG_IMAGESAMPLETYPE_FORCE_U32 = 0x7FFFFFFF
    }
    public enum sg_sampler_type
    {
        _SG_SAMPLERTYPE_DEFAULT,
        SG_SAMPLERTYPE_FILTERING,
        SG_SAMPLERTYPE_NONFILTERING,
        SG_SAMPLERTYPE_COMPARISON,
        _SG_SAMPLERTYPE_NUM,
        _SG_SAMPLERTYPE_FORCE_U32,
    }

    public enum sg_index_type
    {
        _SG_INDEXTYPE_DEFAULT,   // value 0 reserved for default-init
        SG_INDEXTYPE_NONE,
        SG_INDEXTYPE_UINT16,
        SG_INDEXTYPE_UINT32,
        _SG_INDEXTYPE_NUM,
        _SG_INDEXTYPE_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_vertex_format
    {
        SG_VERTEXFORMAT_INVALID,
        SG_VERTEXFORMAT_FLOAT,
        SG_VERTEXFORMAT_FLOAT2,
        SG_VERTEXFORMAT_FLOAT3,
        SG_VERTEXFORMAT_FLOAT4,
        SG_VERTEXFORMAT_BYTE4,
        SG_VERTEXFORMAT_BYTE4N,
        SG_VERTEXFORMAT_UBYTE4,
        SG_VERTEXFORMAT_UBYTE4N,
        SG_VERTEXFORMAT_SHORT2,
        SG_VERTEXFORMAT_SHORT2N,
        SG_VERTEXFORMAT_USHORT2N,
        SG_VERTEXFORMAT_SHORT4,
        SG_VERTEXFORMAT_SHORT4N,
        SG_VERTEXFORMAT_USHORT4N,
        SG_VERTEXFORMAT_UINT10_N2,
        SG_VERTEXFORMAT_HALF2,
        SG_VERTEXFORMAT_HALF4,
        _SG_VERTEXFORMAT_NUM,
        _SG_VERTEXFORMAT_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_vertex_step
    {
        _SG_VERTEXSTEP_DEFAULT,     // value 0 reserved for default-init
        SG_VERTEXSTEP_PER_VERTEX,
        SG_VERTEXSTEP_PER_INSTANCE,
        _SG_VERTEXSTEP_NUM,
        _SG_VERTEXSTEP_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_compare_func
    {
        _SG_COMPAREFUNC_DEFAULT,    // value 0 reserved for default-init
        SG_COMPAREFUNC_NEVER,
        SG_COMPAREFUNC_LESS,
        SG_COMPAREFUNC_EQUAL,
        SG_COMPAREFUNC_LESS_EQUAL,
        SG_COMPAREFUNC_GREATER,
        SG_COMPAREFUNC_NOT_EQUAL,
        SG_COMPAREFUNC_GREATER_EQUAL,
        SG_COMPAREFUNC_ALWAYS,
        _SG_COMPAREFUNC_NUM,
        _SG_COMPAREFUNC_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_stencil_op
    {
        _SG_STENCILOP_DEFAULT,      // value 0 reserved for default-init
        SG_STENCILOP_KEEP,
        SG_STENCILOP_ZERO,
        SG_STENCILOP_REPLACE,
        SG_STENCILOP_INCR_CLAMP,
        SG_STENCILOP_DECR_CLAMP,
        SG_STENCILOP_INVERT,
        SG_STENCILOP_INCR_WRAP,
        SG_STENCILOP_DECR_WRAP,
        _SG_STENCILOP_NUM,
        _SG_STENCILOP_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_blend_factor
    {
        _SG_BLENDFACTOR_DEFAULT,    // value 0 reserved for default-init
        SG_BLENDFACTOR_ZERO,
        SG_BLENDFACTOR_ONE,
        SG_BLENDFACTOR_SRC_COLOR,
        SG_BLENDFACTOR_ONE_MINUS_SRC_COLOR,
        SG_BLENDFACTOR_SRC_ALPHA,
        SG_BLENDFACTOR_ONE_MINUS_SRC_ALPHA,
        SG_BLENDFACTOR_DST_COLOR,
        SG_BLENDFACTOR_ONE_MINUS_DST_COLOR,
        SG_BLENDFACTOR_DST_ALPHA,
        SG_BLENDFACTOR_ONE_MINUS_DST_ALPHA,
        SG_BLENDFACTOR_SRC_ALPHA_SATURATED,
        SG_BLENDFACTOR_BLEND_COLOR,
        SG_BLENDFACTOR_ONE_MINUS_BLEND_COLOR,
        SG_BLENDFACTOR_BLEND_ALPHA,
        SG_BLENDFACTOR_ONE_MINUS_BLEND_ALPHA,
        _SG_BLENDFACTOR_NUM,
        _SG_BLENDFACTOR_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_color_mask
    {
        _SG_COLORMASK_DEFAULT = 0,    // value 0 reserved for default-init
        SG_COLORMASK_NONE = 0x10,   // special value for 'all channels disabled
        SG_COLORMASK_R = 0x1,
        SG_COLORMASK_G = 0x2,
        SG_COLORMASK_RG = 0x3,
        SG_COLORMASK_B = 0x4,
        SG_COLORMASK_RB = 0x5,
        SG_COLORMASK_GB = 0x6,
        SG_COLORMASK_RGB = 0x7,
        SG_COLORMASK_A = 0x8,
        SG_COLORMASK_RA = 0x9,
        SG_COLORMASK_GA = 0xA,
        SG_COLORMASK_RGA = 0xB,
        SG_COLORMASK_BA = 0xC,
        SG_COLORMASK_RBA = 0xD,
        SG_COLORMASK_GBA = 0xE,
        SG_COLORMASK_RGBA = 0xF,
        _SG_COLORMASK_FORCE_U32 = 0x7FFFFFFF
    }
    public enum sg_blend_op
    {
        _SG_BLENDOP_DEFAULT,    // value 0 reserved for default-init
        SG_BLENDOP_ADD,
        SG_BLENDOP_SUBTRACT,
        SG_BLENDOP_REVERSE_SUBTRACT,
        _SG_BLENDOP_NUM,
        _SG_BLENDOP_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_cull_mode
    {
        _SG_CULLMODE_DEFAULT,   // value 0 reserved for default-init
        SG_CULLMODE_NONE,
        SG_CULLMODE_FRONT,
        SG_CULLMODE_BACK,
        _SG_CULLMODE_NUM,
        _SG_CULLMODE_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_face_winding
    {
        _SG_FACEWINDING_DEFAULT,    // value 0 reserved for default-init
        SG_FACEWINDING_CCW,
        SG_FACEWINDING_CW,
        _SG_FACEWINDING_NUM,
        _SG_FACEWINDING_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_wrap
    {
        _SG_WRAP_DEFAULT,   // value 0 reserved for default-init
        SG_WRAP_REPEAT,
        SG_WRAP_CLAMP_TO_EDGE,
        SG_WRAP_CLAMP_TO_BORDER,
        SG_WRAP_MIRRORED_REPEAT,
        _SG_WRAP_NUM,
        _SG_WRAP_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_filter
    {
        _SG_FILTER_DEFAULT, // value 0 reserved for default-init
        SG_FILTER_NEAREST,
        SG_FILTER_LINEAR,
        _SG_FILTER_NUM,
        _SG_FILTER_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_border_color
    {
        _SG_BORDERCOLOR_DEFAULT,    // value 0 reserved for default-init
        SG_BORDERCOLOR_TRANSPARENT_BLACK,
        SG_BORDERCOLOR_OPAQUE_BLACK,
        SG_BORDERCOLOR_OPAQUE_WHITE,
        _SG_BORDERCOLOR_NUM,
        _SG_BORDERCOLOR_FORCE_U32 = 0x7FFFFFFF
    }

    public enum NoxResult
    {
        SUCCESS = 0,
        FAILURE = 1
    }

    public enum sg_pixel_format
    {
        _SG_PIXELFORMAT_DEFAULT,    // value 0 reserved for default-init
        SG_PIXELFORMAT_NONE,

        SG_PIXELFORMAT_R8,
        SG_PIXELFORMAT_R8SN,
        SG_PIXELFORMAT_R8UI,
        SG_PIXELFORMAT_R8SI,

        SG_PIXELFORMAT_R16,
        SG_PIXELFORMAT_R16SN,
        SG_PIXELFORMAT_R16UI,
        SG_PIXELFORMAT_R16SI,
        SG_PIXELFORMAT_R16F,
        SG_PIXELFORMAT_RG8,
        SG_PIXELFORMAT_RG8SN,
        SG_PIXELFORMAT_RG8UI,
        SG_PIXELFORMAT_RG8SI,

        SG_PIXELFORMAT_R32UI,
        SG_PIXELFORMAT_R32SI,
        SG_PIXELFORMAT_R32F,
        SG_PIXELFORMAT_RG16,
        SG_PIXELFORMAT_RG16SN,
        SG_PIXELFORMAT_RG16UI,
        SG_PIXELFORMAT_RG16SI,
        SG_PIXELFORMAT_RG16F,
        SG_PIXELFORMAT_RGBA8,
        SG_PIXELFORMAT_SRGB8A8,
        SG_PIXELFORMAT_RGBA8SN,
        SG_PIXELFORMAT_RGBA8UI,
        SG_PIXELFORMAT_RGBA8SI,
        SG_PIXELFORMAT_BGRA8,
        SG_PIXELFORMAT_RGB10A2,
        SG_PIXELFORMAT_RG11B10F,
        SG_PIXELFORMAT_RGB9E5,

        SG_PIXELFORMAT_RG32UI,
        SG_PIXELFORMAT_RG32SI,
        SG_PIXELFORMAT_RG32F,
        SG_PIXELFORMAT_RGBA16,
        SG_PIXELFORMAT_RGBA16SN,
        SG_PIXELFORMAT_RGBA16UI,
        SG_PIXELFORMAT_RGBA16SI,
        SG_PIXELFORMAT_RGBA16F,

        SG_PIXELFORMAT_RGBA32UI,
        SG_PIXELFORMAT_RGBA32SI,
        SG_PIXELFORMAT_RGBA32F,

        // NOTE: when adding/removing pixel formats before DEPTH, also update sokol_app.h/_SAPP_PIXELFORMAT_*
        SG_PIXELFORMAT_DEPTH,
        SG_PIXELFORMAT_DEPTH_STENCIL,

        // NOTE: don't put any new compressed format in front of here
        SG_PIXELFORMAT_BC1_RGBA,
        SG_PIXELFORMAT_BC2_RGBA,
        SG_PIXELFORMAT_BC3_RGBA,
        SG_PIXELFORMAT_BC3_SRGBA,
        SG_PIXELFORMAT_BC4_R,
        SG_PIXELFORMAT_BC4_RSN,
        SG_PIXELFORMAT_BC5_RG,
        SG_PIXELFORMAT_BC5_RGSN,
        SG_PIXELFORMAT_BC6H_RGBF,
        SG_PIXELFORMAT_BC6H_RGBUF,
        SG_PIXELFORMAT_BC7_RGBA,
        SG_PIXELFORMAT_BC7_SRGBA,
        SG_PIXELFORMAT_PVRTC_RGB_2BPP,      // FIXME: deprecated
        SG_PIXELFORMAT_PVRTC_RGB_4BPP,      // FIXME: deprecated
        SG_PIXELFORMAT_PVRTC_RGBA_2BPP,     // FIXME: deprecated
        SG_PIXELFORMAT_PVRTC_RGBA_4BPP,     // FIXME: deprecated
        SG_PIXELFORMAT_ETC2_RGB8,
        SG_PIXELFORMAT_ETC2_SRGB8,
        SG_PIXELFORMAT_ETC2_RGB8A1,
        SG_PIXELFORMAT_ETC2_RGBA8,
        SG_PIXELFORMAT_ETC2_SRGB8A8,
        SG_PIXELFORMAT_EAC_R11,
        SG_PIXELFORMAT_EAC_R11SN,
        SG_PIXELFORMAT_EAC_RG11,
        SG_PIXELFORMAT_EAC_RG11SN,

        SG_PIXELFORMAT_ASTC_4x4_RGBA,
        SG_PIXELFORMAT_ASTC_4x4_SRGBA,

        _SG_PIXELFORMAT_NUM,
        _SG_PIXELFORMAT_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_buffer_type
    {
        _SG_BUFFERTYPE_DEFAULT,         // value 0 reserved for default-init
        SG_BUFFERTYPE_VERTEXBUFFER,
        SG_BUFFERTYPE_INDEXBUFFER,
        SG_BUFFERTYPE_STORAGEBUFFER,
        _SG_BUFFERTYPE_NUM,
        _SG_BUFFERTYPE_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sapp_event_type : uint
    {
        SAPP_EVENTTYPE_INVALID = 0,
        SAPP_EVENTTYPE_KEY_DOWN = 1,
        SAPP_EVENTTYPE_KEY_UP = 2,
        SAPP_EVENTTYPE_CHAR = 3,
        SAPP_EVENTTYPE_MOUSE_DOWN = 4,
        SAPP_EVENTTYPE_MOUSE_UP = 5,
        SAPP_EVENTTYPE_MOUSE_SCROLL = 6,
        SAPP_EVENTTYPE_MOUSE_MOVE = 7,
        SAPP_EVENTTYPE_MOUSE_ENTER = 8,
        SAPP_EVENTTYPE_MOUSE_LEAVE = 9,
        SAPP_EVENTTYPE_TOUCHES_BEGAN = 10,
        SAPP_EVENTTYPE_TOUCHES_MOVED = 11,
        SAPP_EVENTTYPE_TOUCHES_ENDED = 12,
        SAPP_EVENTTYPE_TOUCHES_CANCELLED = 13,
        SAPP_EVENTTYPE_RESIZED = 14,
        SAPP_EVENTTYPE_ICONIFIED = 15,
        SAPP_EVENTTYPE_RESTORED = 16,
        SAPP_EVENTTYPE_FOCUSED = 17,
        SAPP_EVENTTYPE_UNFOCUSED = 18,
        SAPP_EVENTTYPE_SUSPENDED = 19,
        SAPP_EVENTTYPE_RESUMED = 20,
        SAPP_EVENTTYPE_QUIT_REQUESTED = 21,
        SAPP_EVENTTYPE_CLIPBOARD_PASTED = 22,
        SAPP_EVENTTYPE_FILES_DROPPED = 23,
        _SAPP_EVENTTYPE_NUM = 24,
        _SAPP_EVENTTYPE_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sg_backend
    {
        SG_BACKEND_GLCORE,
        SG_BACKEND_GLES3,
        SG_BACKEND_D3D11,
        SG_BACKEND_METAL_IOS,
        SG_BACKEND_METAL_MACOS,
        SG_BACKEND_METAL_SIMULATOR,
        SG_BACKEND_WGPU,
        SG_BACKEND_DUMMY,
    }

    public enum sg_shader_stage
    {
        SG_SHADERSTAGE_VS,
        SG_SHADERSTAGE_FS,
        _SG_SHADERSTAGE_FORCE_U32 = 0x7FFFFFFF
    }

    public enum sapp_mousebutton
    {
        SAPP_MOUSEBUTTON_LEFT = 0x0,
        SAPP_MOUSEBUTTON_RIGHT = 0x1,
        SAPP_MOUSEBUTTON_MIDDLE = 0x2,
        SAPP_MOUSEBUTTON_INVALID = 0x100,
    }
    public enum sapp_keycode
    {
        SAPP_KEYCODE_INVALID = 0,
        SAPP_KEYCODE_SPACE = 32,
        SAPP_KEYCODE_APOSTROPHE = 39,  /* ' */
        SAPP_KEYCODE_COMMA = 44,  /* , */
        SAPP_KEYCODE_MINUS = 45,  /* - */
        SAPP_KEYCODE_PERIOD = 46,  /* . */
        SAPP_KEYCODE_SLASH = 47,  /* / */
        SAPP_KEYCODE_0 = 48,
        SAPP_KEYCODE_1 = 49,
        SAPP_KEYCODE_2 = 50,
        SAPP_KEYCODE_3 = 51,
        SAPP_KEYCODE_4 = 52,
        SAPP_KEYCODE_5 = 53,
        SAPP_KEYCODE_6 = 54,
        SAPP_KEYCODE_7 = 55,
        SAPP_KEYCODE_8 = 56,
        SAPP_KEYCODE_9 = 57,
        SAPP_KEYCODE_SEMICOLON = 59,  /* ; */
        SAPP_KEYCODE_EQUAL = 61,  /* = */
        SAPP_KEYCODE_A = 65,
        SAPP_KEYCODE_B = 66,
        SAPP_KEYCODE_C = 67,
        SAPP_KEYCODE_D = 68,
        SAPP_KEYCODE_E = 69,
        SAPP_KEYCODE_F = 70,
        SAPP_KEYCODE_G = 71,
        SAPP_KEYCODE_H = 72,
        SAPP_KEYCODE_I = 73,
        SAPP_KEYCODE_J = 74,
        SAPP_KEYCODE_K = 75,
        SAPP_KEYCODE_L = 76,
        SAPP_KEYCODE_M = 77,
        SAPP_KEYCODE_N = 78,
        SAPP_KEYCODE_O = 79,
        SAPP_KEYCODE_P = 80,
        SAPP_KEYCODE_Q = 81,
        SAPP_KEYCODE_R = 82,
        SAPP_KEYCODE_S = 83,
        SAPP_KEYCODE_T = 84,
        SAPP_KEYCODE_U = 85,
        SAPP_KEYCODE_V = 86,
        SAPP_KEYCODE_W = 87,
        SAPP_KEYCODE_X = 88,
        SAPP_KEYCODE_Y = 89,
        SAPP_KEYCODE_Z = 90,
        SAPP_KEYCODE_LEFT_BRACKET = 91,  /* [ */
        SAPP_KEYCODE_BACKSLASH = 92,  /* \ */
        SAPP_KEYCODE_RIGHT_BRACKET = 93,  /* ] */
        SAPP_KEYCODE_GRAVE_ACCENT = 96,  /* ` */
        SAPP_KEYCODE_WORLD_1 = 161, /* non-US #1 */
        SAPP_KEYCODE_WORLD_2 = 162, /* non-US #2 */
        SAPP_KEYCODE_ESCAPE = 256,
        SAPP_KEYCODE_ENTER = 257,
        SAPP_KEYCODE_TAB = 258,
        SAPP_KEYCODE_BACKSPACE = 259,
        SAPP_KEYCODE_INSERT = 260,
        SAPP_KEYCODE_DELETE = 261,
        SAPP_KEYCODE_RIGHT = 262,
        SAPP_KEYCODE_LEFT = 263,
        SAPP_KEYCODE_DOWN = 264,
        SAPP_KEYCODE_UP = 265,
        SAPP_KEYCODE_PAGE_UP = 266,
        SAPP_KEYCODE_PAGE_DOWN = 267,
        SAPP_KEYCODE_HOME = 268,
        SAPP_KEYCODE_END = 269,
        SAPP_KEYCODE_CAPS_LOCK = 280,
        SAPP_KEYCODE_SCROLL_LOCK = 281,
        SAPP_KEYCODE_NUM_LOCK = 282,
        SAPP_KEYCODE_PRINT_SCREEN = 283,
        SAPP_KEYCODE_PAUSE = 284,
        SAPP_KEYCODE_F1 = 290,
        SAPP_KEYCODE_F2 = 291,
        SAPP_KEYCODE_F3 = 292,
        SAPP_KEYCODE_F4 = 293,
        SAPP_KEYCODE_F5 = 294,
        SAPP_KEYCODE_F6 = 295,
        SAPP_KEYCODE_F7 = 296,
        SAPP_KEYCODE_F8 = 297,
        SAPP_KEYCODE_F9 = 298,
        SAPP_KEYCODE_F10 = 299,
        SAPP_KEYCODE_F11 = 300,
        SAPP_KEYCODE_F12 = 301,
        SAPP_KEYCODE_F13 = 302,
        SAPP_KEYCODE_F14 = 303,
        SAPP_KEYCODE_F15 = 304,
        SAPP_KEYCODE_F16 = 305,
        SAPP_KEYCODE_F17 = 306,
        SAPP_KEYCODE_F18 = 307,
        SAPP_KEYCODE_F19 = 308,
        SAPP_KEYCODE_F20 = 309,
        SAPP_KEYCODE_F21 = 310,
        SAPP_KEYCODE_F22 = 311,
        SAPP_KEYCODE_F23 = 312,
        SAPP_KEYCODE_F24 = 313,
        SAPP_KEYCODE_F25 = 314,
        SAPP_KEYCODE_KP_0 = 320,
        SAPP_KEYCODE_KP_1 = 321,
        SAPP_KEYCODE_KP_2 = 322,
        SAPP_KEYCODE_KP_3 = 323,
        SAPP_KEYCODE_KP_4 = 324,
        SAPP_KEYCODE_KP_5 = 325,
        SAPP_KEYCODE_KP_6 = 326,
        SAPP_KEYCODE_KP_7 = 327,
        SAPP_KEYCODE_KP_8 = 328,
        SAPP_KEYCODE_KP_9 = 329,
        SAPP_KEYCODE_KP_DECIMAL = 330,
        SAPP_KEYCODE_KP_DIVIDE = 331,
        SAPP_KEYCODE_KP_MULTIPLY = 332,
        SAPP_KEYCODE_KP_SUBTRACT = 333,
        SAPP_KEYCODE_KP_ADD = 334,
        SAPP_KEYCODE_KP_ENTER = 335,
        SAPP_KEYCODE_KP_EQUAL = 336,
        SAPP_KEYCODE_LEFT_SHIFT = 340,
        SAPP_KEYCODE_LEFT_CONTROL = 341,
        SAPP_KEYCODE_LEFT_ALT = 342,
        SAPP_KEYCODE_LEFT_SUPER = 343,
        SAPP_KEYCODE_RIGHT_SHIFT = 344,
        SAPP_KEYCODE_RIGHT_CONTROL = 345,
        SAPP_KEYCODE_RIGHT_ALT = 346,
        SAPP_KEYCODE_RIGHT_SUPER = 347,
        SAPP_KEYCODE_MENU = 348,
    }
    [Flags]
    public enum sapp_keymod {
        SAPP_MODIFIER_SHIFT = 0x1,      // left or right shift key
        SAPP_MODIFIER_CTRL  = 0x2,      // left or right control key
        SAPP_MODIFIER_ALT   = 0x4,      // left or right alt key
        SAPP_MODIFIER_SUPER = 0x8,      // left or right 'super' key
        SAPP_MODIFIER_LMB   = 0x100,    // left mouse button
        SAPP_MODIFIER_RMB   = 0x200,    // right mouse button
        SAPP_MODIFIER_MMB   = 0x400,    // middle mouse button
    }
}