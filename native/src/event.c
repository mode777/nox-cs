#include "stdio.h"
#include "event.h"

static struct {
    float mouse[2];
    uint32_t keys[SAPP_KEYCODE_MENU+1];
} state = {}; 

int app_event_dispatch(const sapp_event* e){
    switch(e->type){
        case SAPP_EVENTTYPE_KEY_DOWN:
            if (e->key_code == SAPP_KEYCODE_ESCAPE) {
                sapp_request_quit();
            }
            state.keys[e->key_code] = e->frame_count;
            break;
        case SAPP_EVENTTYPE_KEY_UP:
            state.keys[e->key_code] = 0;
            break;
        case SAPP_EVENTTYPE_MOUSE_MOVE:
            state.mouse[0] = e->mouse_x;
            state.mouse[1] = e->mouse_y;
            break;
    }
    return 0;
}

float app_event_get_mousex(){
    return state.mouse[0];
}

float app_event_get_mousey(){
    return state.mouse[1];
}

bool app_event_is_key_down(uint32_t key_code){
    if(key_code > SAPP_KEYCODE_MENU) return false;
    return state.keys[key_code] > 0;
}

bool app_event_is_key_pressed(uint32_t key_code){
    if(key_code > SAPP_KEYCODE_MENU || state.keys[key_code] == 0) return false;
    return state.keys[key_code] == sapp_frame_count();
}