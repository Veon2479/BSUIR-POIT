cmake_minimum_required(VERSION 3.0)

project(anothergtktest LANGUAGES C)


find_package(PkgConfig REQUIRED)    
pkg_check_modules(GTK REQUIRED gtk+-3.0)
include_directories(${GTK_INCLUDE_DIRS})
link_directories(${GTK_LIBRARY_DIRS})
add_definitions(${GTK_CFLAGS_OTHER})

add_executable(anothergtktest main.c)

LINK_LIBRARIES(${MyProject} ${GTK_LIBRARIES} )


install(TARGETS anothergtktest RUNTIME DESTINATION bin)
