cmake_minimum_required(VERSION 3.0)

project(l8 LANGUAGES C)

add_executable(l8 main.c)

install(TARGETS l8 RUNTIME DESTINATION bin)
