cmake_minimum_required(VERSION 3.0)

project(l9 LANGUAGES C)

add_executable(l9 main.c)

install(TARGETS l9 RUNTIME DESTINATION bin)
