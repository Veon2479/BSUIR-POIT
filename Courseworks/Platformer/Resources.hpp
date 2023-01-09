#ifndef RESOURCES_HPP_INCLUDED
#define RESOURCES_HPP_INCLUDED

#include <SFML/Graphics.hpp>
#include "Menu.hpp"
#include <vector>
#include <sstream>
#include <list>

#include "Classes.hpp"

sf::Font font;

sf::Image mapImage;
sf::Image heroImage;

std::vector<sf::Image> imgEnemies;
sf::Image imgPlatform;

std::list<Entity*> lstEntities;
std::list<Entity*>::iterator itr1, itr2;

int cntEnemiesAll = 0, cntEnemiesElapsed = 0;
sf::Text txtHealth, txtEnemies, txtWin, txtDead;

void showInfo(sf::RenderWindow &window, Player &player)
{
    txtHealth.setString("Health: "+std::to_string(player.health).erase(5));
    txtEnemies.setString("Enemies defeated: "+std::to_string(cntEnemiesElapsed)+"/"+std::to_string(cntEnemiesAll));

    txtEnemies.setPosition(view.getCenter().x+40, view.getCenter().y-185);
    txtHealth.setPosition(view.getCenter().x+150, view.getCenter().y+150);
        
    window.draw(txtEnemies);
    window.draw(txtHealth);
}

void loadResources()
{
    std::cout << "LOG: loading resources" << std::endl;
    if (!font.loadFromFile("src/uglyFont.otf"))
            std::cout << "ERR: problem while loading font" << std::endl;
        else
        {
            txtEnemies.setFont(font);
            //txtEnemies.setCharacterSize(24);
            txtEnemies.setFillColor(sf::Color::Black);
            
            txtHealth.setFont(font);
            txtHealth.setFillColor(sf::Color::Red);
            
            txtWin.setFont(font);
            txtWin.setFillColor(sf::Color::Red);
            txtWin.setCharacterSize(30);
            txtWin.setString("You Won!");
            
            txtDead.setFont(font);
            txtDead.setFillColor(sf::Color::Red);
            txtDead.setString("YOU DIED");

            
        }
   	heroImage.loadFromFile("src/copia.png");
    //mapImage.loadFromFile("src/map.png");
    mapImage.loadFromFile("src/tilemap.png");
    menuImage.loadFromFile("src/baphomet.jpg");
    playImage.loadFromFile("src/startGame.png");
    aboutImage.loadFromFile("src/about.png");
    exitImage.loadFromFile("src/exit.png");

    imgEnemies.resize(1);
    imgEnemies[0].loadFromFile("src/enemy_0.png");
    
    imgPlatform.loadFromFile("src/platform.png");
  
    
    loadMap();
}

void destroyEntities()
{
        std::cout<<std::endl;

    if (lstEntities.begin() != lstEntities.end())
    for ( itr1 = lstEntities.begin(); itr1 != lstEntities.end(); )
    {
        delete (*itr1);
        itr1 = lstEntities.erase(itr1);
    }
    std::cout<<std::endl;
}

void loadEntities()
{
    destroyEntities();

    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 20*32, 36*32, 193, 261, 10*32, 21*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 30*32, 36*32, 193, 261, 20*32, 39*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 35*32, 36*32, 193, 261, 30*32, 38*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 25*32, 36*32, 193, 261, 25*32, 35*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 35*32, 36*32, 193, 261, 20*32, 37*32));
    
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 70*32, 36*32, 193, 261, 50*32, 130*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 80*32, 36*32, 193, 261, 60*32, 130*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 71*32, 36*32, 193, 261, 70*32, 130*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 87*32, 36*32, 193, 261, 80*32, 130*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 100*32, 36*32, 193, 261, 90*32, 130*32));

    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 49*32, 30*32, 193, 261, 44*32, 53*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 50*32, 30*32, 193, 261, 44*32, 53*32));
    
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 48*32, 24*32, 193, 261, 44*32, 53*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 51*32, 24*32, 193, 261, 44*32, 53*32));
    
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 50*32, 18*32, 193, 261, 44*32, 53*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 52*32, 18*32, 193, 261, 44*32, 53*32));
    
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 49*32, 12*32, 193, 261, 44*32, 53*32));
    lstEntities.push_back(new Enemy(imgEnemies[0], "Enemy_0", 50*32, 12*32, 193, 261, 44*32, 53*32));




    cntEnemiesAll = 18;
    cntEnemiesElapsed = 0;

    lstEntities.push_back(new Platform(imgPlatform, "Platform_0", 15*32, 39*32, 32*2, 10, 0, -250, 0.5));
    lstEntities.push_back(new Platform(imgPlatform, "Platform_0", 41*32, 36*32+16, 32*2, 10, 0, -150, 5));


}

void clearEntities(float time)
{
    
    for ( itr1 = lstEntities.begin(); itr1 != lstEntities.end(); )
    {
        
        (*itr1)->update(time);
        if ((*itr1)->life == false)
        {
            delete (*itr1);
            itr1 = lstEntities.erase(itr1);
        }
        else
        {   
            

            itr1++;
        }
            
            
    }
}

float lastTime = 0;


void computeEntities(Player &player, sf::RenderWindow &window, float time)
{
    std::string tmp = {0};
    //std::cout << std::endl;
    for (itr1 = lstEntities.begin(); itr1 != lstEntities.end(); itr1++)
            {
                tmp = (*itr1)->name;

                window.draw( (*itr1)->sprite );
            //    std::cout << "LOG: computing entity: "<<tmp<<std::endl;
                if ( (*itr1)->getRect().intersects( player.getRect() ) )
                {
                    std::cout << "LOG: collision with: " << tmp << " at " << player.x/32 <<", "<<player.y/32 <<std::endl;
                    if ( (*itr1)->name == "Platform_0" )
                    {   
                        if ( player.dy > 0 || player.onGround == false )
                            if ( player.y + player.h < (*itr1)->y + (*itr1)->h )
                            {
                                player.y = (*itr1)->y - player.h ;//- 2.0;
                                player.x += (*itr1)->dx * time;
                                player.dy = 0;
                                player.onGround = true;
                            }
                    }
                    
                    if ((*itr1)->name == "Enemy_0")
                    {
                        if ((player.dy > 0) && (player.onGround == false))
                        {
                            (*itr1)->dx = 0;
                            player.dy = -550;
                            (*itr1)->health = 0;
                            cntEnemiesElapsed++;
                        }
                        else
                        {
                            if (std::abs(time - lastTime) > 0.0001f)
                            {
                               // (*itr1)->sprite.scale(-1, 1);
                              //  (*itr1)->dx *= -1;
                                lastTime = time;
                                player.health -= 1;
                                player.isInjured = true;

                               
                                
                            }
                          /*  if ((*itr1)->dx > 0)
                                    (*itr1)->x = player.x-player.w;
                            if ((*itr1)->dx < 0)
                                    (*itr1)->x = player.x+(*itr1)->w;
                            if (player.dx > 0)
                                    player.x = (*itr1)->x - player.w;
                            if (player.dx < 0)
                                    player.x = (*itr1)->x+(*itr1)->w;*/                            
                        
                           
                            
                        }
                      //  std::cout << player.health << std::endl;
                    }
                }
                
                for (itr2 = lstEntities.begin(); itr2 != lstEntities.end(); itr2++)
                {
                    if ((*itr1)->getRect() != (*itr2)->getRect())
                        if ((*itr1)->name == "Enemy_0" && (*itr2)->name == "Enemy_0")
                            if ((*itr1)->getRect().intersects((*itr2)->getRect()))
                            {
                                (*itr1)->dx *= -1;
                                (*itr1)->sprite.scale(-1, 1);
                                (*itr2)->dx *= -1;
                                (*itr2)->sprite.scale(-1, 1);
                            }
                            
                }
            }
}





#endif
