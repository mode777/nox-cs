#ifndef LIB_H
#define LIB_H

#ifdef _WIN32
  #ifdef BUILD_LIB
    #define LIB_API __declspec(dllexport)
  #else
    #define LIB_API __declspec(dllimport)
  #endif
#else
  #define LIB_API
#endif

#include <stdbool.h>
#include "sokol_app.h"
#include "sokol_gfx.h"
#include "stb_truetype.h"

typedef struct pixel_t {
    uint8_t r;
    uint8_t g;
    uint8_t b;
    uint8_t a;
} pixel_t;

typedef struct {
  int glyph_a;
  int glyph_b;
  int advance;
} NoxKernInfo;

typedef struct NoxAppDesc {
    void (*init_cb)(void);                  // these are the user-provided callbacks without user data
    void (*frame_cb)(void);
    void (*event_cb)(const sapp_event*);
    void (*stream_cb)(float* buffer, int num_frames, int num_channels);
    void (*logger)(
        const char* tag,                // always "sapp"
        uint32_t log_level,             // 0=panic, 1=error, 2=warning, 3=info
        uint32_t log_item_id,           // SAPP_LOGITEM_*
        const char* message_or_null,    // a message string, may be nullptr in release mode
        uint32_t line_nr,               // line number in sokol_app.h
        const char* filename_or_null,   // source filename, may be nullptr in release mode
        void* user_data);
    int width;                          // the preferred width of the window / canvas
    int height;                         // the preferred height of the window / canvas
    int sample_count;                   // MSAA sample count
    int swap_interval;                  // the preferred swap interval (ignored on some platforms)
    bool high_dpi;                      // whether the rendering canvas is full-resolution on HighDPI displays
    bool fullscreen;                    // whether the window should be created in fullscreen mode
    bool alpha;                         // whether the framebuffer should have an alpha channel (ignored on some platforms)
    const char* window_title;           // the window title as UTF-8 encoded string
    sapp_icon_desc icon;                // the initial window icon to set
} NoxAppDesc;

LIB_API void nox_run(NoxAppDesc* desc);
LIB_API int nox_get_backend(sg_backend* out_backend);
LIB_API int nox_surface_size(int* w, int* h);
LIB_API int nox_dpi_scale(float* s);
LIB_API int nox_set_clear_color(pixel_t color);
LIB_API int nox_get_clear_color(pixel_t* out_color);
LIB_API int nox_set_window_title(const char* title);
LIB_API int nox_frame_time(double* out_time);
LIB_API int nox_time(double* out_duration);

// audio
LIB_API int nox_sample_rate(int* out_rate);


// images
LIB_API int nox_image_load(void* data, size_t len, void** out_data, int* out_w, int* out_h, int* out_c);
LIB_API int nox_image_alloc(int w, int h, int c, void** out_data);
LIB_API int nox_image_blit(char* dest, int dest_width, int dest_height, int dest_c, const char* src, int src_width, int src_height, int x, int y);
LIB_API int nox_image_free(void* data);

// textures
LIB_API int nox_texture_create(int w, int h, sg_pixel_format format, uint32_t* out_image);
LIB_API int nox_texture_update(uint32_t image, pixel_t* data, int w, int h, int c);
LIB_API int nox_texture_free(uint32_t image);

// buffers
LIB_API int nox_buffer_create(size_t size, sg_buffer_type type, uint32_t* out_buffer);
LIB_API int nox_buffer_update(uint32_t buffer, void* data, size_t length);
LIB_API int nox_buffer_free(uint32_t buffer);

// fonts
LIB_API int nox_font_load(void* data, int length, void** out_handle, int* out_ascent, int* out_descent, int* out_line_gap, int* out_num_kernings);
LIB_API int nox_font_load_kernings(void* handle, NoxKernInfo* out_kernings, size_t len);
LIB_API int nox_font_free(void* handle);
LIB_API int nox_font_load_glyph(void* handle, int codepoint, int* out_index, int* out_advance, int* out_bearing);
LIB_API int nox_font_load_glyph_bitmap(void* handle, int index, float scale, void** out_data, int* out_w, int* out_h, int* out_offset_x, int* out_offset_y);
LIB_API int nox_font_free_glyph_bitmap(void* data);

// rendering
LIB_API int nox_begin_frame();
LIB_API int nox_end_frame();
LIB_API int nox_shader_create(sg_shader_desc* desc, unsigned int* handle);
LIB_API int nox_shader_free(unsigned int handle);
LIB_API int nox_pipeline_create(sg_pipeline_desc* desc, unsigned int* handle);
LIB_API int nox_pipeline_apply(unsigned int handle);
LIB_API int nox_pipeline_free(unsigned int handle);
LIB_API int nox_pipeline_bindings(sg_bindings* bindings);
LIB_API int nox_pipeline_uniforms(sg_shader_stage stage, int slot, void* uniforms, size_t uniforms_size);
LIB_API int nox_pipeline_draw(int start, int count, int instances);
LIB_API int nox_sampler_create(sg_sampler_desc* desc, unsigned int* handle);
LIB_API int nox_sampler_free(unsigned int handle);

#endif // LIB_H