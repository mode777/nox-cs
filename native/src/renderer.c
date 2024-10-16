#include "sokol_gfx.h"
#include "sokol_log.h"
#include "renderer.h"
#include "sokol_app.h"
#include "sokol_glue.h"
#include "shd.h"
#include "lib.h"
#include <stdio.h>

static struct {
    sg_pass_action pass_action;
} state;

LIB_API int nox_texture_create(int w, int h, sg_pixel_format format, uint32_t* out_img){
    sg_image image = sg_alloc_image();
    sg_init_image(image, &(sg_image_desc){
        .usage = SG_USAGE_DYNAMIC,
        .type = SG_IMAGETYPE_2D,
        .width = w,
        .height = h,
        .num_slices = 1,
        .pixel_format = format
    });
    *out_img = image.id;
    return 0;
}

LIB_API int nox_texture_update(uint32_t image, pixel_t* data, int w, int h, int c){
    sg_update_image((sg_image){ .id = image }, &(sg_image_data){
        .subimage[0][0] = {
            .ptr = data,
            .size = c * w * h
        }
    });
    return 0;
}

LIB_API int nox_texture_free(uint32_t image){
    sg_dealloc_image((sg_image){ .id = image });
    return 0;
}

int app_renderer_init(sg_logger logger){
    sg_setup(&(sg_desc){
        .environment = sglue_environment(),
        .logger = logger
    });

    /* a pass action to clear framebuffer */
    state.pass_action = (sg_pass_action) {
        .colors[0] = { .load_action=SG_LOADACTION_CLEAR, .clear_value={0.176f, 0.0f, 0.306f, 1.0f} }
    };

    return 0;
}

LIB_API int nox_begin_frame() {
    sg_begin_pass(&(sg_pass){ .action = state.pass_action, .swapchain = sglue_swapchain() });
    return 0;
}

LIB_API int nox_end_frame() {
    sg_end_pass();
    sg_commit();
    return 0;
}

int app_renderer_cleanup(){
    sg_shutdown();
    return 0;
}

LIB_API int nox_buffer_create(size_t size, sg_buffer_type type, uint32_t* out_buffer){
    sg_buffer buffer = sg_make_buffer(&(sg_buffer_desc){
        .type = type,
        .size = size,
        .usage = SG_USAGE_DYNAMIC,
        .label = "vertices"
    });
    *out_buffer = buffer.id;
    return 0;
}

LIB_API int nox_buffer_update(uint32_t buffer, void* data, size_t length){
    sg_update_buffer((sg_buffer){ .id = buffer }, &(sg_range){ .ptr = data, .size = length });
    return 0;
}

LIB_API int nox_buffer_free(uint32_t buffer) {
    sg_destroy_buffer((sg_buffer){ .id = buffer });
    return 0;
}

LIB_API int nox_get_backend(sg_backend* out_backend) {
    *out_backend = sg_query_backend();
    return 0;
}

LIB_API int nox_shader_create(sg_shader_desc* desc, unsigned int* handle) {
    sg_shader shd = sg_make_shader(desc);
    *handle = shd.id;
    sg_resource_state state = sg_query_shader_state(shd);
    if(state != SG_RESOURCESTATE_VALID){
        return -1;
    }
    return 0;
}

LIB_API int nox_shader_free(unsigned int handle) {
    sg_destroy_shader((sg_shader){ .id = handle });
    return 0;
}

LIB_API int nox_pipeline_create(sg_pipeline_desc* desc, unsigned int* handle) {
    sg_pipeline pip = sg_make_pipeline(desc);
    *handle = pip.id;
    sg_resource_state state = sg_query_pipeline_state(pip);
    if(state != SG_RESOURCESTATE_VALID){
        return -1;
    }
    return 0;
}

LIB_API int nox_pipeline_apply(unsigned int handle) {
    sg_apply_pipeline((sg_pipeline){ .id = handle });
    return 0;
}

LIB_API int nox_pipeline_free(unsigned int handle) {
    sg_destroy_pipeline((sg_pipeline){ .id = handle });
    return 0;
}

LIB_API int nox_pipeline_bindings(sg_bindings* bindings) {
    sg_apply_bindings(bindings);
    return 0;
}

LIB_API int nox_pipeline_uniforms(sg_shader_stage stage, int slot, void* uniforms, size_t uniforms_size) {
    sg_apply_uniforms(stage, slot, &(sg_range){ .ptr = uniforms, .size = uniforms_size });
    return 0;
}

LIB_API int nox_pipeline_draw(int start, int count, int instances) {
    sg_draw(start, count, instances);
    return 0;
}

LIB_API int nox_sampler_create(sg_sampler_desc* desc, unsigned int* handle){
    sg_sampler sampler = sg_make_sampler(desc);
    *handle = sampler.id;
    sg_resource_state state = sg_query_sampler_state(sampler);
    if(state != SG_RESOURCESTATE_VALID){
        return -1;
    }
    return 0;
}

LIB_API int nox_sampler_free(unsigned int handle){
    sg_destroy_sampler((sg_sampler){ .id = handle });
    return 0;
}

LIB_API int nox_surface_size(int* w, int* h) {
    *w = sapp_width();
    *h = sapp_height();
    return 0;
}

LIB_API int nox_dpi_scale(float* s) {
    *s = sapp_dpi_scale();
    return 0;
}

LIB_API int nox_set_clear_color(pixel_t color){
    state.pass_action.colors[0].clear_value = (sg_color){ color.r / 255.0f, color.g / 255.0f, color.b / 255.0f, color.a / 255.0f};
    return 0;
}
LIB_API int nox_get_clear_color(pixel_t* out_color){
    out_color->r = state.pass_action.colors[0].clear_value.r * 255;
    out_color->g = state.pass_action.colors[0].clear_value.g * 255;
    out_color->b = state.pass_action.colors[0].clear_value.b * 255;
    out_color->a = state.pass_action.colors[0].clear_value.a * 255;
    return 0;
}
LIB_API int nox_set_window_title(const char* title){
    sapp_set_window_title(title);
    return 0;
}

