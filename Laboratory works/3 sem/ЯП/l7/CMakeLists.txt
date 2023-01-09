cmake_minimum_required(VERSION 3.0)

project(l7 LANGUAGES C)

add_executable(l7 main.c)

install(TARGETS l7 RUNTIME DESTINATION bin)
