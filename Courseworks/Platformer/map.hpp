#ifndef MAP_HPP_INCLUDED
#define MAP_HPP_INCLUDED


#include <SFML/Graphics.hpp>
#include <string>
#include <fstream>
#include <vector>

#include "View.hpp"



int MAP_HEIGHT = 0;
int MAP_WIDTH = 0;

std::vector<sf::String> Tile_Map;

sf::Sprite mapSprite;
sf::Texture mapTexture;

void loadMap()
{
 
    std::fstream map;
    std::string line;
    map.open("src/Map.txt", std::ios::in);
    if (map)
    {
        while ( std::getline(map, line))
        {
            Tile_Map.push_back(line);
            MAP_HEIGHT++;
            MAP_WIDTH = (line.length()>MAP_WIDTH) ? line.length() : MAP_WIDTH;
            
        }
        map.close();
        std::cout << "LOG: map created" << std::endl;

    }
        else
            std::cout << "ERR: map hasn't read!" << std::endl;
    
}

void drawMap(sf::RenderWindow &window, float x, float y)
{
                    mapSprite.setScale(32/18.0f+0.1e-2, 32/18.0f+0.1e-2);

    for (int i = 0; i < MAP_HEIGHT; i++)
        for ( int j = 0; j < MAP_WIDTH; j++)
        {
            if (i<(y+200)/32 && i>(y - 250)/32 && j<(x + 683)/32 && j>(x - 683)/32 )
            {
//             if (Tile_Map[i][j] == ' ')
//                 mapSprite.setTextureRect(sf::IntRect(0, 0, 32, 32));
//             else if (Tile_Map[i][j] == 's')
//                 mapSprite.setTextureRect(sf::IntRect(32, 0, 32, 32));
//         else if (Tile_Map[i][i] == '0'|| Tile_Map[i][j] == 'b' || Tile_Map[i][j] == 'g' || Tile_Map[i][j] == 'd' )
//                 mapSprite.setTextureRect(sf::IntRect(64, 0, 32, 32));
//             else 
//                 mapSprite.setTextureRect(sf::IntRect(96, 0, 32, 32));
//         
                if ((Tile_Map[i][j] == ' ' || Tile_Map[i][j] == '0' )&& i<=39)
                    mapSprite.setTextureRect(sf::IntRect(13*18, 3*18, 18, 18));
                else if ((Tile_Map[i][j] == ' ' || Tile_Map[i][j] == '0' )&& i>39)
                    mapSprite.setTextureRect(sf::IntRect(4*18, 0*18, 18, 18));
                else if (Tile_Map[i][j] == 'b')
                    mapSprite.setTextureRect(sf::IntRect(9*18, 0, 18, 18));
                else if (Tile_Map[i][j] == 'd')
                    mapSprite.setTextureRect(sf::IntRect(2*18, 1*18, 18, 18));
                else if (Tile_Map[i][j] == 'e')
                    mapSprite.setTextureRect(sf::IntRect(10*18, 7*18, 18, 18));
                else if (Tile_Map[i][j] == 'z')
                    mapSprite.setTextureRect(sf::IntRect(4*18, 0*18, 18, 18));
                else if (Tile_Map[i][j] == 's' || Tile_Map[i][j] == '0' || Tile_Map[i][j] == 'z' )
                    mapSprite.setTextureRect(sf::IntRect(9*18, 1*18, 18, 18));
                else 
                    mapSprite.setTextureRect(sf::IntRect(5*18, 7*18, 18, 18));
                mapSprite.setPosition(j*32, i*32);
                window.draw(mapSprite);
            }
        }
}

bool isAbsSolid(int i, int j)
{
    switch (Tile_Map[i][j])
    {
        case '0':
        case 's':
        case 'b':
        case 'd':
            return true;
    }
    return false;
}

#endif // MAP_HPP_INCLUDED
