#version 300 es
precision mediump float;
precision highp int;

uniform highp sampler2D _uTexture_uTextureSampler;

layout(location = 0) out highp vec4 FragColor;
in highp vec2 vTexCoord;
in highp vec4 vColor;

void main()
{
    FragColor = texture(_uTexture_uTextureSampler, vTexCoord) * vColor;
}

