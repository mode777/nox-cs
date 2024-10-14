#include <metal_stdlib>
#include <simd/simd.h>

using namespace metal;

struct uParams
{
    float2 uViewport;
    float2 uTextureSize;
};

struct main0_out
{
    float4 vColor [[user(locn0)]];
    float2 vTexCoord [[user(locn1)]];
    float4 gl_Position [[position]];
};

struct main0_in
{
    float2 aPosition [[attribute(0)]];
    float2 aTexCoord [[attribute(1)]];
    float4 aColor [[attribute(2)]];
};

vertex main0_out main0(main0_in in [[stage_in]], constant uParams& _15 [[buffer(0)]])
{
    main0_out out = {};
    out.gl_Position = float4((((in.aPosition / _15.uViewport) * 2.0) - float2(1.0)) * float2(1.0, -1.0), 0.0, 1.0);
    out.vColor = in.aColor;
    out.vTexCoord = in.aTexCoord / _15.uTextureSize;
    return out;
}
