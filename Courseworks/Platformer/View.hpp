#ifndef VIEW_HPP_INCLUDED
#define VIEW_HPP_INCLUDED


#include <SFML/Graphics.hpp>
using namespace sf;
 
View view;
float leftXBorder = 345;
float rightXBorder = 4106;  //сдвинуть на один тайл, чтобы скрыть границу
float upperYBorder = 192;
float bottomYBorder = 2368;

void setView (float tmpX, float tmpY)
{
//    printf("%f, %f\n", tmpX, tmpY);
    if (tmpX<leftXBorder)
            tmpX = leftXBorder;
        else
            if (tmpX>rightXBorder)
                tmpX = rightXBorder;
    if (tmpY<upperYBorder)
            tmpY = upperYBorder;
        else
            if (tmpY>bottomYBorder)
                tmpY = bottomYBorder;
    view.setCenter(tmpX, tmpY);
}

#endif // VIEW_HPP_INCLUDED
