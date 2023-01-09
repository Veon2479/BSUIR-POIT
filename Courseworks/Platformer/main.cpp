#include <SFML/Graphics.hpp>
#include <iostream>
#include <list>
#include <unistd.h>

#include "Resources.hpp"

using namespace sf;

bool isDead;

float rslX = 683, rslY = 384;

bool startGame(RenderWindow &window)
{
    window.setMouseCursorVisible(false);
    
    Clock clock;
    clock.restart();

    float time = 0;
   
    view.reset(FloatRect(0, 0, rslX, rslY));


	Player player(heroImage, "Player", 41*32, 34*32, 0, 0);

    loadEntities();

    mapTexture.loadFromImage(mapImage);
    mapSprite.setTexture(mapTexture);


    while (window.isOpen())
    {

        window.clear(Color::Black);

        time = clock.getElapsedTime().asSeconds();

        clock.restart();
        
        time = time / 2;

        Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
                window.close();
        }

        if (Keyboard::isKeyPressed(Keyboard::Tab)) 
        { 
            return true; 
        }
        if (Keyboard::isKeyPressed(Keyboard::Escape)) 
        { 
            return false; 
        }
        
            drawMap(window, player.x, player.y);
            player.update(time);
            clearEntities(time);
            
            computeEntities(player, window, time);
            window.setView(view);
            window.draw(player.sprite);
          
        
        if (!player.life)
        {
            //TODO: you died!
            isDead = true;
            window.clear(sf::Color::Black);
            txtDead.setPosition(view.getCenter());
            window.draw(txtDead);
            window.display();   
            sleep(4);
            return false;
        }
        
        if (cntEnemiesAll == cntEnemiesElapsed)
        {
            txtWin.setPosition(view.getCenter().x-128, view.getCenter().y-64);
            window.draw(txtWin);
        }
        
        showInfo(window, player);

        window.display();

    }
    return false;
    
}

void GameRunning(RenderWindow &window)
{

    if (startGame(window))
            GameRunning(window);
       // else Menu(window);
}

// void menuLoop(RenderWindow &window)
// {
//     std::cout << "LOG: game is starting" << std::endl;
// 
//     isDead = false;
//     Menu(window);
//     GameRunning(window);
//     if (isDead)
//         menuLoop(window);
// }

int main(int argc, char **argv) {
    
  
    std::cout << "LOG: app is starting" << std::endl;
    loadResources();
    RenderWindow window(VideoMode( 1366, 768), "The Game", Style::Fullscreen);
    window.setVerticalSyncEnabled(true);
    Menu(window);
    GameRunning(window);     
    //menuLoop(window);
    std::cout << "LOG: app is closed" << std::endl;

    return 0;
}
