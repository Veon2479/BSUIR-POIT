cmake_minimum_required(VERSION 3.0)

project(coursework)

find_package(PkgConfig REQUIRED)

LINK_LIBRARIES(${MyProject} -lsfml-graphics -lsfml-window -lsfml-system )   

add_executable(coursework main.cpp)

install(TARGETS coursework RUNTIME DESTINATION bin)
