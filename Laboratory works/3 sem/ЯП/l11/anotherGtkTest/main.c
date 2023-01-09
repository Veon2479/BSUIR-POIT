#include <stdio.h>
#include <stdlib.h>
#include <gtk/gtk.h>

GtkWidget *window = NULL;
GtkWidget * image = NULL;
GtkWidget * scenes[28] = {0};  
int curScene =  0;


gboolean setNextImage(gpointer data)
{
    
        (curScene)++;
        if (curScene > 27)
            curScene = 1;
        char name[40] = {0};
        if (image)
            gtk_container_remove(GTK_CONTAINER(window), image);
        
        strcpy(name, "");
        sprintf(name, "src/%d.gif", curScene);
        image = gtk_image_new_from_file(name);
        
        
        gtk_widget_show(image);
        gtk_container_add(GTK_CONTAINER(window), image);
     
  
   return TRUE;
}

int main(int argc, char *argv[])
{
   
    gtk_init(&argc, &argv);
    
    window = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_window_set_title(GTK_WINDOW(window), "Hello, my fellow friend!");
   
    g_timeout_add(100, setNextImage, NULL );
    g_signal_connect(G_OBJECT(window), "destroy", G_CALLBACK(gtk_main_quit), NULL);
    gtk_widget_show  (window);
    
    gtk_main ();
 
    return 0;
}
