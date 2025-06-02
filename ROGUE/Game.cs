using System.Numerics;
using ZeroElectric.Vinculum;

namespace ROGUE
{
    internal class Game
    {
        public static List<int> WallTileNumbers;
        public static List<int> StairsTileNumbers;
        Pelaaja player = new Pelaaja();
        Map level1;
        Texture Playerimage;
        Texture Tilemap;
        string statusMessage;
        string ItemstatusMessage;
        public string askName()
        {
            while (true)
            {
                Console.WriteLine("What is your characters name?");
                string nimi = Console.ReadLine();
                if (string.IsNullOrEmpty(nimi))
                {
                    Console.WriteLine("Ei Kelpaa");
                    continue;
                }
                bool nameOk = true;
                for (int i = 0; i < nimi.Length; i++)
                {
                    char kirjain = nimi[i];
                    if (Char.IsLetter(kirjain) == false)
                    {
                        nameOk = false;
                        Console.WriteLine("Nimessä täytyy olla vain kirjaimia");
                        break;
                    }

                }

                if (nameOk)
                {
                    return nimi;
                }
            }

        }

        public static readonly int tileSize = 16;

        public static readonly int imagesPerRow = 12;

        public static readonly int PlayerStartTile = 99;

        public void Run()
        {
            WallTileNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20, 24, 25, 26, 27, 28, 29, 40, 57, 58, 59 };
            StairsTileNumbers = new List<int> { 52 };
            Raylib.InitWindow(800, 800, "rogue");
            Playerimage = Raylib.LoadTexture("tile_0097.png");
            Tilemap = Raylib.LoadTexture("tilemap_packed.png");
            MapReader reader = new MapReader();
            Map level3 = reader.LoadTiledMap("tiledmap (1).tmj");
            Map level1 = reader.LoadMapFromFile("mapfile.json");
            Map level2 = reader.LoadMapFromFile("mapfile2.json");
            Map currentlevel = level1;
            while (true)
            {
                Console.WriteLine("S0    elect Race");
                Console.WriteLine("1 tai " + Race.Human.ToString());
                Console.WriteLine("2 tai " + Race.Elf.ToString());
                Console.WriteLine("3 tai " + Race.Orc.ToString());
                string rotuVastaus = Console.ReadLine();

                if (rotuVastaus == Race.Human.ToString() || rotuVastaus == "1")
                {
                    player.rotu = Race.Human;
                    break;
                }
                else if (rotuVastaus == Race.Elf.ToString() || rotuVastaus == "2")
                {
                    player.rotu = Race.Elf;
                    break;
                }
                else if (rotuVastaus == Race.Orc.ToString() || rotuVastaus == "3")
                {
                    player.rotu = Race.Orc;
                    break;
                }
                else
                {
                    Console.WriteLine("Ei kelpaa");
                }
            }

            while (true)
            {
                Console.WriteLine("Select Class");
                Console.WriteLine("1 tai " + Class.Rogue.ToString());
                Console.WriteLine("2 tai " + Class.Warrior.ToString());
                Console.WriteLine("3 tai " + Class.Magician.ToString());
                string luokkaVastaus = Console.ReadLine();

                if (luokkaVastaus == Class.Rogue.ToString() || luokkaVastaus == "1")
                {
                    player.luokka = Class.Rogue;
                    break;
                }
                else if (luokkaVastaus == Class.Warrior.ToString() || luokkaVastaus == "2")
                {
                    player.luokka = Class.Warrior;
                    break;
                }
                else if (luokkaVastaus == Class.Magician.ToString() || luokkaVastaus == "3")
                {
                    player.luokka = Class.Magician;
                    break;
                }
                else
                {
                    Console.WriteLine("Ei kelpaa");
                }

            }

            player.name = askName();

            
            player.paikka = new Vector2(1, 1);
            

            while (Raylib.WindowShouldClose() == false)
            {
                // ------------Update:
                // Prepare to read movement input
                int moveX = 0;
                int moveY = 0;
                // Wait for keypress and compare value to ConsoleKey enum

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    moveY = -1;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
                {
                    moveY = 1;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    moveX = -1;
                }
                else if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    moveX = 1;
                }

                //
                // TODO: CHECK COLLISION WITH WALLS
                int newX = (int)player.paikka.X + moveX;
                int newY = (int)player.paikka.Y + moveY;
                Enemy enemytarget = currentlevel.EnemyFind(newX, newY);
                if (enemytarget == null)
                {
                    MapTile tile = currentlevel.GetTileAt(newX, newY);
                    if (tile == MapTile.Floor)
                    {
                        player.paikka.X += moveX;
                        player.paikka.Y += moveY;
                    }
                    if (tile == MapTile.Stairs)
                    {
                        Console.Clear();
                        player.paikka = new Vector2(1, 1);
                        currentlevel = level3;
                        player.paikka = currentlevel.GetPlayerStart(PlayerStartTile);

                    }
                    // ei vihua
                }
                else
                {
                    statusMessage = "HitEnemy";
                }

                Item itemtarget = currentlevel.itemThere(newX, newY);
                if (itemtarget != null)
                {
                    ItemstatusMessage = "Found a item" + itemtarget.name;
                }



                // Prevent player from going outside screen
                if (player.paikka.X < 0)
                {
                    player.paikka.X = 0;
                }
                else if (player.paikka.X > Console.WindowWidth - 1)
                {
                    player.paikka.X = Console.WindowWidth - 1;
                }
                if (player.paikka.Y < 0)
                {
                    player.paikka.Y = 0;
                }
                else if (player.paikka.Y > Console.WindowHeight - 1)
                {
                    player.paikka.Y = Console.WindowHeight - 1;
                }

                // -----------Draw:
                Raylib.BeginDrawing();
                // Clear the screen so that player appears only in one place
                Raylib.ClearBackground(Raylib.BLACK);
                // Draw the player
                currentlevel.draw(Tilemap);
                int drawPixelX = (int)(player.paikka.X * Game.tileSize);
                int drawPixelY = (int)(player.paikka.Y * Game.tileSize);
                Raylib.DrawTexture(Playerimage, drawPixelX, drawPixelY, Raylib.WHITE);
                Raylib.DrawText(statusMessage, 0, 0, 20, Raylib.RED);
                Raylib.DrawText(ItemstatusMessage, 2, 0, 20, Raylib.YELLOW);
                Raylib.EndDrawing();
            } // game loop ends


        }
    }
}
