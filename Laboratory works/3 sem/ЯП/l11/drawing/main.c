#include <stdio.h>
#include <stdlib.h>
#include <gtk/gtk.h>
#include <cairo.h>
#include <math.h>

#define vert struct vert
#define pol struct po

vert
{
    float x, y, z;
    vert* next;
};

pol
{
    int n1, n2, n3, n4;
    pol* next;
};

vert* initVert(float x, float y, float z)
{
    vert* res = (vert*) malloc(sizeof(vert));
    res->next = NULL;
    res->x = x;
    res->y = y;
    res->z = z;
    return res;
}

pol* initPol(int n1, int n2, int n3, int n4)
{
    pol* res = (pol*) malloc(sizeof(pol));
    res->n1 = n1;
    res->n2 = n2;
    res->n3 = n3;
    res->n4 = n4;
    return res;
}

GtkWidget *window = NULL, *area = NULL;
vert* firstVert = NULL;
pol* firstPol = NULL;

cairo_surface_t *surface = NULL;
cairo_t *cr = NULL;

void resizeVert(vert* Vert, float a, float b, float c, float x, float y, float z)
{
    while (Vert != NULL)
    {
        Vert->x = Vert->x * a + x;
        Vert->y = Vert->y * b + y;
        Vert->z = Vert->z * c + z;
        Vert = Vert->next;
    }
}

vert* getVert(int num)
{
    vert* res = firstVert;
    int counter = 1;
    while (counter != num && res != NULL)
    {
        counter++;
        if (res!=NULL)
        res = res->next;
    }
    return res;
}

void drawLines(cairo_t *cr)
{
    {
        float len = 1;
        pol* cur = firstPol;
    vert* tmp = NULL;
    gint x1 = 0, x2 = 0, x3 = 0, x4 = 0, y1 = 0, y2 = 0, y3 = 0, y4 = 0, z1 = 0, z2 = 0, z3 = 0, z4 = 0;
  
    int i = 1;
    float angle = 0, da = 0.1/3.14;
    
    while (i<=4)
    {
        tmp = getVert(cur->n1);
        if (tmp!=NULL)
        {
        x1 = tmp->x;
        y1 = tmp->y;
        z1 = tmp->z;
        }
       // puts("1");
        tmp = getVert(cur->n2);
        if (tmp!=NULL)
        {
        x2 = tmp->x;
        y2 = tmp->y;
        z2 = tmp->z;
        }
       // puts("2");
        tmp = getVert(cur->n3);
        if (tmp!=NULL)
        {
        x3 = tmp->x;
        y3 = tmp->y;
        z3 = tmp->z;
        }
     //   puts("3");
        tmp = getVert(cur->n4);
        if (tmp!=NULL)
        {
        x4 = tmp->x;
        y4 = tmp->y;
        z4 = tmp->z;
        }
     //   puts("4");
       cairo_move_to(cr, len*((double)x1*cos(angle))/z1, len*((double)y1*cos(angle))/z1);
       cairo_line_to(cr, len*((double)x2*cos(angle))/z2, len*((double)y2*cos(angle))/z2);
       
       cairo_line_to(cr, len*((double)x4*cos(angle))/z4, len*((double)y4*cos(angle))/z4);
       cairo_line_to(cr, len*((double)x3*cos(angle))/z3, len*((double)y3*cos(angle))/z3);
       cairo_line_to(cr, len*((double)x1*cos(angle))/z1, len*((double)y1)*cos(angle)/z1);
        i++;
        angle += da;
        cur = cur->next;
    }
     // printf("%f", ((double)x1)/z1);
    }
  
    
    cairo_set_line_width (cr, 1);
  
    
    cairo_set_source_rgb (cr, 0.6, 0.4, 0.6);
   // cairo_fill (cr); 
    cairo_stroke(cr);
}

void rotX(vert* cur)
{
    cur->x += 0.5;
    cur->y += 0.5;
   
}

void changeVert()
{
    vert* cur = firstVert;
    while (cur != NULL)
    {
        rotX(cur);
        
        cur = cur->next;
    }
    
}


gboolean on_draw_event (GtkWidget *widget, cairo_t *cr, gpointer user_data)
{
    puts("Draw");
    drawLines(cr);
    (void)user_data, (void)widget; 
   
    return TRUE;
  
}

gboolean changeImage(gpointer data)
{
    puts("Time");
   
    changeVert();
   
    gtk_widget_queue_draw(window);
    return TRUE;
}

int main(int argc, char *argv[])
{
    puts("Hello, World!");
    gtk_init(&argc, &argv);
    window = gtk_window_new (GTK_WINDOW_TOPLEVEL);
    gtk_window_set_title(GTK_WINDOW(window), "Hello, my fellow friend!");
    g_signal_connect(G_OBJECT(window), "destroy", G_CALLBACK(gtk_main_quit), NULL);
    gtk_widget_show  (window);
    
    
{   ///////init////////////
    firstVert = initVert(0, 0, 0);
    
    vert* tmpVert = initVert(0, 0, 1);
    firstVert->next = tmpVert;
    
    vert* newVert = initVert(0, 1, 0);
    tmpVert->next = newVert;
    
    tmpVert = initVert(0, 1, 1);
    newVert->next = tmpVert;
    
    newVert = initVert(1, 0, 0);
    tmpVert->next = newVert;
    
    tmpVert = initVert(1, 0, 1);
    newVert->next = tmpVert;
    
    newVert = initVert(1, 1, 0);
    tmpVert->next = newVert;
    
    tmpVert = initVert(1, 1, 1);
    newVert->next = tmpVert;
    
    
    firstPol = initPol(1, 2, 3, 4);
    
    pol* newPol = initPol(2, 4, 6, 8);
    firstPol->next = newPol;
    
    pol* tmpPol = initPol(5, 6, 7, 8);
    newPol->next = tmpPol;
    
    newPol = initPol(1, 3, 5, 7);
    tmpPol ->next = newPol;     
    
}    
    resizeVert(firstVert, 100, 100, 1, 100, 100, 1);
    
   // resizeVert(firstVert, 150, 150, 8, 400, 800, 1);
    
    area = gtk_drawing_area_new();
     gtk_widget_set_size_request (area, 800, 500);
   
    //gtk_drawing_area_size(GTK_DRAWING_AREA(area), 300, 300);
    gtk_container_add(GTK_CONTAINER(window), area);
   
    
        surface = cairo_image_surface_create (CAIRO_FORMAT_ARGB32, 120, 120);
     cr = cairo_create (surface);
     cairo_set_line_width (cr, 0.1);
     cairo_set_source_rgb (cr, 255, 255, 255);
    
   g_signal_connect (G_OBJECT(area), "draw",  G_CALLBACK(on_draw_event), NULL);
    gtk_widget_show_all (window);
     
    g_timeout_add(100, changeImage, area);
    
    
    gtk_main ();
     
     
    return 0;
}
