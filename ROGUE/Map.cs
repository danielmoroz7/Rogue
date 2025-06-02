using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ZeroElectric.Vinculum;

namespace ROGUE
{
    enum MapTile {Wall, Floor, Stairs }
    internal class Map
    {
        public int mapWidth;
        public MapLayer[] layers;

      
        public List<Item> items;
        public List<Enemy> enemies;
        

        public int getTile(int x, int y)
        {
            int index = x + y * mapWidth;
            int tileId = GetLayer("ground").mapTiles[index];
            return tileId;
        }
        public MapTile GetTileAt(int x, int y)
        {
            // Calculate index: index = x + y * mapWidth
            int indexInMap = x + y * mapWidth;

            // Use the index to get a map tile from map's array
            MapLayer groundLayer = GetLayer("ground");
            int[] mapTiles = groundLayer.mapTiles;
            int tileId = mapTiles[indexInMap];

            if (Game.WallTileNumbers.Contains(tileId))
            {
                // Is a wall
                return MapTile.Wall;
            }
            else if (Game.StairsTileNumbers.Contains(tileId))
            {
                return MapTile.Stairs;
            } 
            else { return MapTile.Floor; }
            

            
     
        }
/// <summary>
/// Etsii vihollista jos ei löydy niin se palauttaa null 
/// </summary>
/// <param name="playerx">pelaajan positio x</param>
/// <param name="playery">pelaajan positio y</param>
/// <returns>vihollinen joka on samassa kohdassa kuin pelaaja</returns>
      


        public Enemy? EnemyFind(int playerx, int playery)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy Find = enemies[i];



                if (playerx == Find.position.X && playery == Find.position.Y)
                {
                    return Find;
                }

            }
            return null;
        }

        public Item? itemThere(int playerx, int playery)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Item There = items[i];



                if (playerx == There.position.X && playery == There.position.Y)
                {
                    return There;
                }

            }
            return null;
        }

        public Vector2 GetPlayerStart(int PlayerSpawn)

        {
                      MapLayer enemiesLayer = GetLayer("enemies");
            int[] enemyTiles = enemiesLayer.mapTiles;
            int layerHeight = enemyTiles.Length / mapWidth;
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = enemyTiles[index];

                    if (tileId == PlayerSpawn)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        return new Vector2(x, y);
                    }
                }
            }
            return new Vector2(mapWidth / 2, layerHeight / 2);

        }
        public string GetEnemyName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost"; break;
                case 109: return "Cyclops"; break;
                default: return "Unknown"; break;
            }
        }
        public Map()
        {
            layers = new MapLayer[3];
            enemies = new List<Enemy>();
            items = new List<Item>();
        }
        public void LoadEnemies()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer enemyLayer = GetLayer("enemies");
            int[] enemyTiles = enemyLayer.mapTiles;
            int layerHeight = enemyTiles.Length / mapWidth;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = enemyTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetEnemyName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        enemies.Add(new Enemy(name, position, spriteId));
                    }
                }
            }
        }

        public string GetItemName(int spriteIndex)
        {
            switch (spriteIndex)
            {
                case 108: return "Ghost"; break;
                case 109: return "Cyclops"; break;
                default: return "Unknown"; break;
            }
        }
        public void LoadItems()
        {
            // Hae viholliset sisältävä taso kentästä
            MapLayer itemLayer = GetLayer("items");
            int[] itemTiles = itemLayer.mapTiles;
            int layerHeight = itemTiles.Length / mapWidth;

            // Käy taso läpi ja luo viholliset
            for (int y = 0; y < layerHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    // Laske paikka valmiiksi
                    Vector2 position = new Vector2(x, y);

                    int index = x + y * mapWidth;

                    int tileId = itemTiles[index];

                    if (tileId == 0)
                    {
                        // Tässä kohdassa kenttää ei ole vihollista
                        continue;
                    }
                    else
                    {
                        // Tässä kohdassa kenttää on jokin vihollinen

                        // Tässä pitää vähentää 1,
                        // koska Tiled editori tallentaa
                        // palojen numerot alkaen 1:sestä.
                        int spriteId = tileId - 1;

                        // Hae vihollisen nimi
                        string name = GetItemName(spriteId);

                        // Luo uusi vihollinen ja lisää se listaan
                        items.Add(new Item(name, position, spriteId));
                    }
                }
            }
        }
        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
             return null; // Wanted layer was not found!
        }

        public void draw(Texture Tilemap)
        {


                    Console.ForegroundColor = ConsoleColor.Gray; // Change to map color
            int mapHeight = GetLayer("ground").mapTiles.Length / mapWidth; // Calculate the height: the amount of rows
            for (int y = 0; y < mapHeight; y++) // for each row
            {
                for (int x = 0; x < mapWidth; x++) // for each column in the row
                {
                    int index = x + y * mapWidth; // Calculate index of tile at (x, y)
                    int tileId = GetLayer("ground").mapTiles[index]; // Read the tile value at index
                     tileId = tileId - 1;
                    int imageX = tileId % Game.imagesPerRow;
                    int imageY = (int)(tileId / Game.imagesPerRow);
                    int imagePixelX = imageX * Game.tileSize;
                    int imagePixelY = imageY * Game.tileSize;
                    Rectangle imageRect = new Rectangle(imagePixelX, imagePixelY, Game.tileSize, Game.tileSize);
                    int draw_x = (int)(x * Game.tileSize);
                    int draw_y = (int)(y * Game.tileSize);
                    Raylib.DrawTextureRec(Tilemap,imageRect, new Vector2 (draw_x, draw_y), Raylib.WHITE);
                    // Draw the tile graphics
                   
                }

            }
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy currentEnemy = enemies[i];
                Vector2 enemyPosition = currentEnemy.position;
                int enemySpriteIndex = currentEnemy.spriteIndex;
                int pixelX = (int)(enemyPosition.X * Game.tileSize);
                int pixelY = (int)(enemyPosition.Y * Game.tileSize);

                Rectangle enemyect = new Rectangle(
                               (enemySpriteIndex % Game.imagesPerRow) * Game.tileSize,
                               (enemySpriteIndex / Game.imagesPerRow) * Game.tileSize,
                               Game.tileSize,
                               Game.tileSize
                );

                Raylib.DrawTextureRec(Tilemap, enemyect, new Vector2(pixelX, pixelY), Raylib.WHITE);
            }
            for (int i = 0; i < items.Count; i++)
            {
                Item currentItem = items[i];
                Vector2 itemPosition = currentItem.position;
                int itemSpriteIndex = currentItem.spriteIndex;
                int pixelX = (int)(itemPosition.X * Game.tileSize);
                int pixelY = (int)(itemPosition.Y * Game.tileSize);

                Rectangle itemRect = new Rectangle(
                    (itemSpriteIndex % Game.imagesPerRow) * Game.tileSize,
                    (itemSpriteIndex / Game.imagesPerRow) * Game.tileSize,
                    Game.tileSize,
                    Game.tileSize
                );

                Raylib.DrawTextureRec(Tilemap, itemRect, new Vector2(pixelX, pixelY), Raylib.WHITE);
            }
        }
    }
}

