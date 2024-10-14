#version 430

layout(binding = 0) uniform sampler2D _uTexture_uTextureSampler;

layout(location = 0) out vec4 FragColor;
layout(location = 1) in vec2 vTexCoord;
layout(location = 0) in vec4 vColor;

void main()
{
    FragColor = texture(_uTexture_uTextureSampler, vTexCoord) * vColor;
}

