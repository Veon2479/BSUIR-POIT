cmake_minimum_required(VERSION 3.0)

project(l10 LANGUAGES C)

add_executable(l10 main.c)

install(TARGETS l10 RUNTIME DESTINATION bin)
