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

static void (*init_cb)(void);                  // these are the user-provided callbacks without user data
//static void (*frame_cb)(void);
//static void (*event_cb)(const sapp_event*);
static void (*stream_cb)(float* buffer, int num_frames, int num_channels);
static void (*logger)(
        const char* tag,                // always "sapp"
        uint32_t log_level,             // 0=panic, 1=error, 2=warning, 3=info
        uint32_t log_item_id,           // SAPP_LOGITEM_*
        const char* message_or_null,    // a message string, may be nullptr in release mode
        uint32_t line_nr,               // line number in sokol_app.h
        const char* filename_or_null,   // source filename, may be nullptr in release mode
        void* user_data);

static void init(void) {
    app_renderer_init((sg_logger){.func = logger});
    if(stream_cb != NULL){
        saudio_setup(&(saudio_desc){ .logger.func = logger, .stream_cb = stream_cb, .num_channels = 2 });
    }
    if(init_cb != NULL) init_cb();
}

static void cleanup(void) {
    app_renderer_cleanup();
}

LIB_API void nox_run(NoxAppDesc* desc) {
    printf("%d x %d x %d\n", desc->width, desc->height, desc->high_dpi);
    fflush(stdout);
    init_cb = desc->init_cb;
    //frame_cb = desc->frame_cb;
    //event_cb = desc->event_cb;
    stream_cb = desc->stream_cb;
    logger = desc->logger;
    sapp_desc sokol_desc = {
        .logger.func = desc->logger,
        .init_cb = init,
        .frame_cb = desc->frame_cb,
        .cleanup_cb = cleanup,
        .event_cb = desc->event_cb,
        .width = desc->width,
        .height = desc->height,
        .sample_count = desc->sample_count,
        .swap_interval = desc->swap_interval,
        .high_dpi = desc->high_dpi,
        .fullscreen = desc->fullscreen,
        .alpha = desc->alpha,
        .window_title = desc->window_title,
        .icon = desc->icon
    };
    sapp_run(&sokol_desc);
}

