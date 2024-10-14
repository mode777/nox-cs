#./tools/sokol-tools-bin/bin/$1/sokol-shdc --input shaders/shd.glsl --output include/shd.h --slang glsl430:hlsl5:metal_macos:glsl300es
./tools/sokol-tools-bin/bin/$1/sokol-shdc --input shaders/shd.glsl --format bare_yaml --output shader --slang glsl430:hlsl5:metal_macos:glsl300es
mv shader_* ./../assets/shaders

