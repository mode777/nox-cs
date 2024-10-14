cbuffer uParams : register(b0)
{
    float2 _15_uViewport : packoffset(c0);
    float2 _15_uTextureSize : packoffset(c0.z);
};


static float4 gl_Position;
static float2 aPosition;
static float2 aTexCoord;
static float4 vColor;
static float4 aColor;
static float2 vTexCoord;

struct SPIRV_Cross_Input
{
    float2 aPosition : TEXCOORD0;
    float2 aTexCoord : TEXCOORD1;
    float4 aColor : TEXCOORD2;
};

struct SPIRV_Cross_Output
{
    float4 vColor : TEXCOORD0;
    float2 vTexCoord : TEXCOORD1;
    float4 gl_Position : SV_Position;
};

void vert_main()
{
    gl_Position = float4((((aPosition / _15_uViewport) * 2.0f) - 1.0f.xx) * float2(1.0f, -1.0f), 0.0f, 1.0f);
    vColor = aColor;
    vTexCoord = aTexCoord / _15_uTextureSize;
}

SPIRV_Cross_Output main(SPIRV_Cross_Input stage_input)
{
    aPosition = stage_input.aPosition;
    aTexCoord = stage_input.aTexCoord;
    aColor = stage_input.aColor;
    vert_main();
    SPIRV_Cross_Output stage_output;
    stage_output.gl_Position = gl_Position;
    stage_output.vColor = vColor;
    stage_output.vTexCoord = vTexCoord;
    return stage_output;
}
