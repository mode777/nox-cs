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

typedef void (*NoxLogFunc)(
    const char* tag, 
    uint32_t log_level, 
    uint32_t log_item_id, 
    const char* message_or_null, 
    uint32_t line_nr, 
    const char* filename_or_null, 
    void* user_data
);

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

typedef void (*NoxEventFunc)(const sapp_event*);
typedef void (*NoxInitFunc)(void);
typedef void (*NoxFrameFunc)(void);
typedef void (*NoxAudioFunc)(float* buffer, int num_frames, int num_channels);

LIB_API void nox_run(NoxLogFunc logFunc, NoxEventFunc eventFunc, NoxInitFunc initFunc, NoxFrameFunc frameFunc, NoxAudioFunc audioFunc);
LIB_API int nox_get_backend(sg_backend* out_backend);
LIB_API int nox_surface_size(int* w, int* h);

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
LIB_API int nox_font_load(void* data, void** out_handle, int* out_ascent, int* out_descent, int* out_line_gap, int* out_num_kernings);
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