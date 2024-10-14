#define STB_IMAGE_IMPLEMENTATION
#if defined(__clang__)
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wunused-function"
#pragma clang diagnostic ignored "-Wsign-compare"
#pragma clang diagnostic ignored "-Wunused-but-set-variable"
#endif
#include "stb_image.h"
#if defined(__clang__)
#pragma clang diagnostic pop
#endif
#undef STB_IMAGE_IMPLEMENTATION

#include "renderer.h"
#include "lib.h"

LIB_API int nox_image_blit(char* dest, int dest_width, int dest_height, int dest_c,
                          const char* src, int src_width, int src_height,
                          int x, int y) {
    //printf("%d, %d, %d\n", dest_width, dest_height, dest_c);
    // Calculate the range of rows and columns to be copied
    int start_row = (y < 0) ? -y : 0;
    int start_col = (x < 0) ? -x : 0;
    int end_row = (y + src_height > dest_height) ? dest_height - y : src_height;
    int end_col = (x + src_width > dest_width) ? dest_width - x : src_width;

    // Adjust destination starting point
    char* dest_start = dest + ((y > 0 ? y : 0) * dest_width + (x > 0 ? x : 0)) * dest_c;

    // Adjust source starting point
    const char* src_start = src + (start_row * src_width + start_col) * dest_c;

    // Copy pixels row by row
    for (int row = start_row; row < end_row; ++row) {
        memcpy(dest_start + row * dest_width * dest_c, src_start + row * src_width * dest_c, (end_col - start_col) * dest_c);
    }
    return 0;
}

LIB_API int nox_image_load(void* data, size_t len, void** out_data, int* out_w, int* out_h, int* out_c){
    if(data == NULL) return -1;
    *out_data = (void*)stbi_load_from_memory(data, len, out_w, out_h, out_c, 4);
    *out_c = 4;
    if(*out_data == NULL) return -1;
    return 0;
}

LIB_API int nox_image_free(void* data){
    if(data == NULL) return -1;
    stbi_image_free(data);
    return 0;
}

LIB_API int nox_image_alloc(int w, int h, int c, void** out_data){
    *out_data = (void*)calloc(c * w * h, sizeof(char));
    if(*out_data == NULL) return -1;
    return 0;
}