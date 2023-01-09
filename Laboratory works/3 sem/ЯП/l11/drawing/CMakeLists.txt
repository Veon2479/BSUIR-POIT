cmake_minimum_required(VERSION 3.0)

project(drawing LANGUAGES C)


find_package(PkgConfig REQUIRED)    
pkg_check_modules(GTK REQUIRED gtk+-3.0)
include_directories(${GTK_INCLUDE_DIRS})
link_directories(${GTK_LIBRARY_DIRS})
add_definitions(${GTK_CFLAGS_OTHER})



LINK_LIBRARIES(${MyProject} ${GTK_LIBRARIES} -lm)


add_executable(drawing main.c)


install(TARGETS drawing RUNTIME DESTINATION bin)
