#include "stdio.h"

#include "sokol_app.h"
#include "sokol_gfx.h"
#include "sokol_glue.h"
#include "sokol_log.h"
#include "sokol_audio.h"
#include "sokol_time.h"

#include "lib.h"
#include "common.h"
#include "renderer.h"
#include "event.h"

#include <SDL3/SDL_joystick.h>
#include <SDL3/SDL_gamepad.h>
#include <SDL3/SDL_init.h>

static void (*init_cb)(void);                  // these are the user-provided callbacks without user data
static void (*frame_cb)(void);
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
    SDL_Init(SDL_INIT_GAMEPAD | SDL_INIT_JOYSTICK | SDL_INIT_HAPTIC);
    
    app_renderer_init((sg_logger){.func = logger});
    if(stream_cb != NULL){
        saudio_setup(&(saudio_desc){ .logger.func = logger, .stream_cb = stream_cb, .num_channels = 2 });
    }
    stm_setup();
    if(init_cb != NULL) init_cb();
}

static void frame(void) {
    SDL_UpdateGamepads();
    if(frame_cb != NULL) frame_cb();
    // if(SDL_HasJoystick() || SDL_HasGamepad()){
    //     int count;
    //     SDL_JoystickID* joysticks = SDL_GetJoysticks(&count);
    //     for(int i = 0; i < count; i++){
    //         const char* name = SDL_GetJoystickNameForID(joysticks[i]);
    //         const char* path = SDL_GetJoystickPathForID(joysticks[i]);
    //         printf("Joystick %d: %s\n", i, name);
    //         printf("Path: %s\n", path);
    //     }
    // } else {
    //     printf("No joysticks connected\n");
    // }
}

static void cleanup(void) {
    app_renderer_cleanup();
}

LIB_API int nox_sample_rate(int* out_rate){
    *out_rate = saudio_sample_rate();
    return 0;
}

LIB_API int nox_time(double* out_duration){
    *out_duration = stm_sec(stm_now());
    return 0;
}

LIB_API int nox_get_gamepads(uint32_t** out_gamepads, int* out_count){
    *out_gamepads = SDL_GetGamepads(out_count);
    return 0;
}

LIB_API int nox_get_gamepad_state(uint32_t gamepad_id, NoxGamepadState* out_state){
    SDL_Gamepad* gamepad = SDL_OpenGamepad(gamepad_id); 
    out_state->connected = SDL_GamepadConnected(gamepad);
    //printf("%d\n", SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_LEFTX)); 
    out_state->left_x = SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_LEFTX) / 32767.0f;
    out_state->left_y = SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_LEFTY) / 32767.0f;
    out_state->right_x = SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_RIGHTX) / 32767.0f;
    out_state->right_y = SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_RIGHTY) / 32767.0f;
    out_state->trigger_l = SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_LEFT_TRIGGER) / 32767.0f;
    out_state->trigger_r = SDL_GetGamepadAxis(gamepad, SDL_GAMEPAD_AXIS_RIGHT_TRIGGER) / 32767.0f;
    return 0;
}


LIB_API void nox_run(NoxAppDesc* desc) {
    init_cb = desc->init_cb;
    frame_cb = desc->frame_cb;
    //event_cb = desc->event_cb;
    stream_cb = desc->stream_cb;
    logger = desc->logger;
    sapp_desc sokol_desc = {
        .logger.func = desc->logger,
        .init_cb = init,
        .frame_cb = frame,
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

