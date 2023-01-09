cmake_minimum_required(VERSION 3.23)
project(osasp_1)

set(CMAKE_CXX_STANDARD 14)

LINK_LIBRARIES(${MyProject} -lgdiplus )

add_executable(var1 WIN32 var1.cpp Drawer.cpp)
add_executable(var2 WIN32 var2.cpp Drawer.cpp)


