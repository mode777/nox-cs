#include "stdio.h"

#include "sokol_app.h"
#include "sokol_gfx.h"
#include "sokol_glue.h"
#include "sokol_log.h"
#include "sokol_audio.h"

#include "lib.h"
#include "common.h"
#include "renderer.h"
#include "event.h"

static NoxInitFunc init_func;
static NoxFrameFunc frame_func;
static NoxAudioFunc audio_func;

static void init(void) {
    app_renderer_init();
    if(audio_func != NULL){
        saudio_setup(&(saudio_desc){ .logger.func = slog_func, .stream_cb = audio_func, .num_channels = 2 });
    }
    if(init_func != NULL) init_func();
    //uint64_t image_id;
    //int sprite_id;
    //app_renderer_load_image_id("../../assets/0.png", &image_id);
    //app_renderer_alloc_sprites(1, &sprite_id);
    //app_renderer_set_sprite(sprite_id, 0, 0, true, 0xFFFFFFFF, image_id);
}

static void frame(void) {
    //app_renderer_frame();
    if(frame_func != NULL) frame_func();
}

static void cleanup(void) {
    app_renderer_cleanup();
}

static sapp_desc describe(NoxLogFunc loggo, NoxEventFunc evento) {

    sapp_desc desc = {
        .logger.func = slog_func,
        .init_cb = init,
        .frame_cb = frame,
        .cleanup_cb = cleanup,
        .event_cb = evento,
        .width = 1280,
        .height = 720
    };
    
    return desc;
}

LIB_API void nox_run(NoxLogFunc logFunc, NoxEventFunc eventFunc, NoxInitFunc initFunc, NoxFrameFunc frameFunc, NoxAudioFunc audioFunc) {
    init_func = initFunc;
    frame_func = frameFunc;
    audio_func = audioFunc;
    sapp_desc desc = describe(logFunc, eventFunc);
    sapp_run(&desc);
}

