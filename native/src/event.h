#ifndef EVENT_H
#define EVENT_H

#include "sokol_app.h"

int app_event_dispatch(const sapp_event* e);
float app_event_get_mousex();
float app_event_get_mousey();
bool app_event_is_key_down(uint32_t key_code);
bool app_event_is_key_pressed(uint32_t key_code);

#endif