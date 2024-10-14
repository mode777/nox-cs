#define SOKOL_IMPL
#if defined(_WIN32)
#define SOKOL_D3D11
#elif defined(__EMSCRIPTEN__)
#define SOKOL_GLES3
#elif defined(__APPLE__)
// NOTE: on macOS, sokol.c is compiled explicitly as ObjC
//#define SOKOL_GLCORE
#define SOKOL_METAL
#else
#define SOKOL_GLCORE
#endif
#define SOKOL_NO_ENTRY
#include "sokol/sokol_app.h"
#include "sokol/sokol_gfx.h"
#include "sokol/sokol_audio.h"
#include "sokol/sokol_glue.h"
#include "sokol/sokol_log.h"
#undef SOKOL_IMPL
