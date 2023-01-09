#ifndef MENU_HPP_INCLUDED
#define MENU_HPP_INCLUDED

#include <SFML/Graphics.hpp>

sf::Image menuImage, playImage, aboutImage, exitImage;


void Menu(sf::RenderWindow &window)
{

    sf::Texture txtrMenu, txtrPlay, txtrAbout, txtrExit;
    playImage.createMaskFromColor(sf::Color::Black);
    aboutImage.createMaskFromColor(sf::Color::Black);
    exitImage.createMaskFromColor(sf::Color::Black);

    
    txtrMenu.loadFromImage(menuImage);
    txtrPlay.loadFromImage(playImage);
    txtrAbout.loadFromImage(aboutImage);
    txtrExit.loadFromImage(exitImage);
    
    sf::Sprite sprMenu, sprPlay, sprAbout, sprExit;
    bool isMenu = true;
    int menuCode = 0;
    
    sprMenu.setTexture(txtrMenu);
    sprPlay.setTexture(txtrPlay);
    sprAbout.setTexture(txtrAbout);
    sprExit.setTexture(txtrExit);
    
    sprMenu.setPosition(0, 0);
    sprPlay.setPosition(100, 30);
        sprPlay.setScale(2, 2);
    sprAbout.setPosition(80, 90);
        sprAbout.setScale(2, 2);
    sprExit.setPosition(70, 150);
        sprExit.setScale(2, 2);
    float curX = 0, curY = 0;
   
    
    while (isMenu)
    {
        window.clear(sf::Color::Black);
        sprPlay.setColor(sf::Color::White);
        sprAbout.setColor(sf::Color::White);
        sprExit.setColor(sf::Color::White);
        
        curX = (float) sf::Mouse::getPosition(window).x;
        curY = (float) sf::Mouse::getPosition(window).y;

        menuCode = 0;
        if (sprPlay.getGlobalBounds().contains(curX, curY))
        { 
            sprPlay.setColor(sf::Color::Blue); 
            menuCode = 1; 
        }
        if (sprAbout.getGlobalBounds().contains(curX, curY))
        { 
            sprAbout.setColor(sf::Color::Blue); 
            menuCode = 2; 
        }
        if (sprExit.getGlobalBounds().contains(curX, curY))
        { 
            sprExit.setColor(sf::Color::Blue); 
            menuCode = 3; 
        }
        
        if (sf::Mouse::isButtonPressed(sf::Mouse::Left))
        {
            if (menuCode == 1) 
                isMenu = false;//если нажали первую кнопку, то выходим из меню 
            if (menuCode == 2) 
            { 
//                 window.draw(about); 
//                 window.display(); 
//                 while (!Keyboard::isKeyPressed(Keyboard::Escape))
//                     ; 
//                 
            }
            if (menuCode == 3)  
            { 
                window.close(); 
                isMenu = false; 
                
            }
 
        }
 
        
        window.draw(sprMenu);
        window.draw(sprPlay);
        window.draw(sprAbout);
        window.draw(sprExit);
        
        window.display();
    }
    
    
    
    
}

#endif // MENU_HPP_INCLUDED
