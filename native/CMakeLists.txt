cmake_minimum_required(VERSION 3.5)
project(libnox VERSION 1.0 LANGUAGES C)
set(CMAKE_C_STANDARD 11)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)
set(CMAKE_OSX_ARCHITECTURES arm64 x86_64)

# Linux -pthread shenanigans
if (CMAKE_SYSTEM_NAME STREQUAL Linux)
    set(THREADS_PREFER_PTHREAD_FLAG ON)
    find_package(Threads REQUIRED)
endif()

#=== LIBRARY: sokol
# add headers to the the file list because they are useful to have in IDEs
set(SOKOL_HEADERS
    thirdparty/sokol/sokol_gfx.h
    thirdparty/sokol/sokol_app.h
    thirdparty/sokol/sokol_audio.h
    thirdparty/sokol/sokol_glue.h)
add_library(sokol STATIC thirdparty/sokol.c ${SOKOL_HEADERS})
if(CMAKE_SYSTEM_NAME STREQUAL Darwin)
    # compile sokol.c as Objective-C
    target_compile_options(sokol PRIVATE -x objective-c)
    target_link_libraries(sokol
        "-framework QuartzCore"
        "-framework Cocoa"
        "-framework MetalKit"
        "-framework Metal"
        "-framework OpenGL"
        "-framework AudioToolbox")
elseif(CMAKE_SYSTEM_NAME STREQUAL Windows)
    target_link_libraries(sokol PRIVATE kernel32 user32 shell32 dxgi d3d11 ole32 gdi32)
else()
    if (CMAKE_SYSTEM_NAME STREQUAL Linux)
        target_link_libraries(sokol INTERFACE X11 Xi Xcursor GL asound dl m)
        target_link_libraries(sokol PUBLIC Threads::Threads)
    endif()
endif()
target_include_directories(sokol INTERFACE sokol)

#=== STB
add_library(stb_image INTERFACE)
add_library(stb_ds INTERFACE)
add_library(stb_rect_pack INTERFACE)
add_library(stb_tt INTERFACE)
target_include_directories(stb_image INTERFACE thirdparty/stb)
target_include_directories(stb_ds INTERFACE thirdparty/stb)
target_include_directories(stb_rect_pack INTERFACE thirdparty/stb)
target_include_directories(stb_tt INTERFACE thirdparty/stb)

#=== SHARED LIB: libnox
if (CMAKE_BUILD_TYPE STREQUAL Debug)
    if (CMAKE_SYSTEM_NAME STREQUAL Linux)
        set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../managed/Nox/runtimes/linux-x64/native/)
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../managed/Nox/runtimes/linux-x64/native/)
    endif()
    if (CMAKE_SYSTEM_NAME STREQUAL Windows)
        set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../managed/Nox/runtimes/win-x64/native/)
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../managed/Nox/runtimes/win-x64/native/)
    endif()
    if (CMAKE_SYSTEM_NAME STREQUAL Darwin)
        set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../managed/Nox/runtimes/osx/native/)
        set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/../managed/Nox/runtimes/osx/native/)
    endif()
endif()
set(MY_SOURCES src/main.c src/renderer.c src/event.c src/image.c src/font.c)
add_library(libnox SHARED ${MY_SOURCES})
target_link_libraries(libnox sokol stb_image)

if (MSVC)
    target_compile_options(libnox PUBLIC /W3)
else()
    target_compile_options(libnox PUBLIC -Wall -Wextra -Wsign-compare)
endif()
target_include_directories(libnox PUBLIC thirdparty/sokol)
target_include_directories(libnox PUBLIC include)
target_compile_options(libnox PRIVATE -Wall -Wno-unused-parameter  -Wno-unused-variable)
set_target_properties(libnox PROPERTIES PREFIX "")

# explicitly strip dead code
if (CMAKE_C_COMPILER_ID MATCHES "Clang" AND NOT CMAKE_SYSTEM_NAME STREQUAL Emscripten)
    target_link_options(libnox PRIVATE LINKER:-dead_strip)
endif()