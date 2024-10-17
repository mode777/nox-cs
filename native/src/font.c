#include <stdio.h>

#include "lib.h"
#define STB_TRUETYPE_IMPLEMENTATION  // force following include to generate implementation
#include "stb_truetype.h"



LIB_API int nox_font_load(void* data, int length, void** out_handle, int* out_ascent, int* out_descent, int* out_line_gap, int* out_num_kernings) {
    int num_fonts = stbtt_GetNumberOfFonts(data);
    if(num_fonts == -1){
        printf("Invalid font file\n");
        return -1;
    }
    *out_handle = malloc(sizeof(stbtt_fontinfo));
    // Create a copy of data
    unsigned char* copy = malloc(length);
    memcpy(copy, data, length);
    stbtt_InitFont(*out_handle, copy, 0);
    stbtt_GetFontVMetrics(*out_handle, out_ascent, out_descent, out_line_gap);
    *out_num_kernings = stbtt_GetKerningTableLength(*out_handle);
    return 0;
}

LIB_API int nox_font_load_kernings(void* handle, NoxKernInfo* out_kernings, size_t len){
    if(len == 0) return 0;
    stbtt_GetKerningTable((stbtt_fontinfo*)handle, (stbtt_kerningentry*)out_kernings, len);
    return 0;
}

LIB_API int nox_font_free(void* handle) {
    if(handle == NULL) return -1;
    free(((stbtt_fontinfo*)handle)->data);
    free(handle);
    return 0;
}

LIB_API int nox_font_load_glyph(void* handle, int codepoint, int* out_index, int* out_advance, int* out_bearing){

    int index = stbtt_FindGlyphIndex((stbtt_fontinfo*)handle, codepoint);
    if(index == 0){
        return -1;
    }
    *out_index = index;
    stbtt_GetGlyphHMetrics((stbtt_fontinfo*)handle, index, out_advance, out_bearing);
    return 0;
}

LIB_API int nox_font_load_glyph_bitmap(void* handle, int index, float scale, void** out_data, int* out_w, int* out_h, int* out_offset_x, int* out_offset_y) {
    unsigned char* bitmap = stbtt_GetGlyphBitmap((stbtt_fontinfo*)handle, scale, scale, index, out_w, out_h, out_offset_x, out_offset_y);
    if(bitmap == NULL) return -1;
    pixel_t pixel = { .r = 255, .g = 255, .b = 255 };
    int w = *out_w;
    int h = *out_h;
    pixel_t* pixels = malloc(w*h*sizeof(pixel_t));
    for (size_t i = 0; i < (size_t)(w*h); i++)
    {
        pixel.a = bitmap[i];
        pixels[i] = pixel;
    }
    stbtt_FreeBitmap(bitmap, NULL);
    *out_data = pixels;
    return 0;
}

LIB_API int nox_font_free_glyph_bitmap(void* data) {
    if(data == NULL) return -1;
    free(data);
    return 0;
}

#undef STB_TRUETYPE_IMPLEMENTATION