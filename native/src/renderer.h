#ifndef RENDERER_H
#define RENDERER_H
#include "sokol_gfx.h"

int app_renderer_init(sg_logger logger);
int app_renderer_frame();
int app_renderer_cleanup();

#endif