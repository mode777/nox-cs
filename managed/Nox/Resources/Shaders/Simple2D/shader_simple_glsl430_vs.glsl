#version 430

uniform vec4 uParams[5];
layout(location = 0) in vec2 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 0) out vec4 vColor;
layout(location = 2) in vec4 aColor;
layout(location = 1) out vec2 vTexCoord;

void main()
{
    gl_Position = mat4(uParams[1], uParams[2], uParams[3], uParams[4]) * vec4((((aPosition / uParams[0].xy) * 2.0) - vec2(1.0)) * vec2(1.0, -1.0), 0.0, 1.0);
    vColor = aColor;
    vTexCoord = aTexCoord / uParams[0].zw;
}

