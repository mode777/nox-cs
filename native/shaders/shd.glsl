@vs vs
in vec2 aPosition;
in vec2 aTexCoord;
in vec4 aColor;
  
out vec4 vColor;
out vec2 vTexCoord;

uniform uParams {
    vec2 uViewport;
    vec2 uTextureSize;
};

void main() {
    vec2 normalized = ((aPosition / uViewport) * 2.0 - 1.0) * vec2(1,-1);
    vec2 uv_normalized = aTexCoord / uTextureSize;
    gl_Position = vec4(normalized, 0.0, 1.0);
    vColor = aColor;
    vTexCoord = uv_normalized;
}
@end

@fs fs
out vec4 FragColor;

in vec4 vColor;
in vec2 vTexCoord;

uniform texture2D _uTexture;
uniform sampler uTextureSampler;

#define uTexture sampler2D(_uTexture, uTextureSampler)

void main() {
    FragColor = texture(uTexture, vTexCoord) * vColor;
}
@end

@program simple vs fs