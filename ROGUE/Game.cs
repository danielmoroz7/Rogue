using RayGuiCreator;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using ZeroElectric.Vinculum;

namespace ROGUE
{
    internal class Game
    {
        public static List<int> WallTileNumbers;
        public static List<int> StairsTileNumbers;
        Pelaaja player = new Pelaaja();
        Map level1;
        Map level3;
        Map currentlevel;
        Texture Playerimage;
        Texture Tilemap;
        string statusMessage;
        string ItemstatusMessage;
        Stack<GameState> currentGameState = new Stack<GameState>();
        TextBoxEntry CharacterLoading = new TextBoxEntry(15);
        MultipleChoiceEntry classChoices = new MultipleChoiceEntry(
            new string[] { "Warrior", "Thief", "Magic User" });
        OptionsMenu myOptionsMenu;
        PauseMenu myPauseMenu;

        public void DrawMainMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int menuStartX = 10;
            int menuStartY = 0;
            int rowHeight = Raylib.GetScreenHeight() / 20;
            int menuWidth = Raylib.GetScreenWidth() / 4;

            // HUOM MenuCreator luodaan aina uudestaan ennen kuin valikko piirrettään.
            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);

            creator.Label("Main Menu");
            creator.Label("Options");
            if (creator.Button("Options"))
            {
                currentGameState.Push(GameState.OptionsMenu);
            }
            creator.Label("Game");
            if (creator.Button("Start Game"))
            {
                currentGameState.Push(GameState.CharacterMenu);
            }
            // TODO: new MenuCreator
            // TODO: use MenuCreator to draw the labels and button


            Raylib.EndDrawing();
        }
        enum GameState
        {
            MainMenu,
            GameLoop,
            CharacterMenu,
            PauseMenu,
            OptionsMenu
        }


        public static readonly int tileSize = 16;

        public static readonly int imagesPerRow = 12;

        public static readonly int PlayerStartTile = 99;

        public void Run()
        {
            
            myOptionsMenu = new OptionsMenu();
            // Kytke asetusvalikon tapahtumaan funktio
            myOptionsMenu.BackButtonPressedEvent += this.OnOptionsBackButtonPressed;
            myPauseMenu = new PauseMenu();
           
            // Kytke asetusvalikon tapahtumaan funktio
            myPauseMenu.BackButtonPressedEvent += this.OnPauseBackButtonPressed;

            myPauseMenu.OptionsButton += this.OptionsButton;

            currentGameState.Push (GameState.MainMenu);
            WallTileNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 12, 13, 14, 15, 16, 17, 18, 19, 20, 24, 25, 26, 27, 28, 29, 40, 57, 58, 59 };
            StairsTileNumbers = new List<int> { 52 };
            Raylib.InitWindow(800, 800, "rogue");
            Playerimage = Raylib.LoadTexture("tile_0097.png");
            Tilemap = Raylib.LoadTexture("tilemap_packed.png");
            MapReader reader = new MapReader();
            level3 = reader.LoadTiledMap("tiledmap (1).tmj");
            Map level1 = reader.LoadMapFromFile("mapfile.json");
            Map level2 = reader.LoadMapFromFile("mapfile2.json");
            currentlevel = level1;
   


            


            player.paikka = new Vector2(1, 1);


            while (Raylib.WindowShouldClose() == false)
            {
                switch (currentGameState.Peek())
                {
                    case GameState.OptionsMenu:
                        myOptionsMenu.DrawMenu();
                        break;

                    case GameState.PauseMenu:
                        myPauseMenu.DrawMenu();
                        break;

                    case GameState.MainMenu:
                        // Tämä koodi on uutta
                        DrawMainMenu();
                        break;

                    case GameState.GameLoop:
                        // Tämä koodi on se mitä GameLoop() funktiossa oli ennen muutoksia
                        UpdateGame();

                        break;

                    case GameState.CharacterMenu:

                        DrawCharacterMenu();

                        break;

                }
            }
        }

        void OptionsButton(object sender, EventArgs args)
        {
            currentGameState.Push(GameState.OptionsMenu);
        }
        void OnOptionsBackButtonPressed(object sender, EventArgs args)
        {
            currentGameState.Pop();
        }
        void OnPauseBackButtonPressed(object sender, EventArgs args)
        {
            currentGameState.Pop();
        }

        void DrawCharacterMenu()
        {
            
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int menuStartX = 10;
            int menuStartY = 0;
            int rowHeight = Raylib.GetScreenHeight() / 20;
            int menuWidth = Raylib.GetScreenWidth() / 4;

            // HUOM MenuCreator luodaan aina uudestaan ennen kuin valikko piirrettään.
            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);

            creator.Label("CharacterMenu");
            creator.TextBox(CharacterLoading);
            creator.DropDown(classChoices);
            creator.Label("Game");
            if (creator.Button("Start Game"))
            {
                string nimi = CharacterLoading.ToString();
                bool nameOk = true;
                if (string.IsNullOrEmpty(nimi))
                {
                    Console.WriteLine("Ei Kelpaa");
                    nameOk = false;
                }
                
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
                    currentGameState.Push(GameState.GameLoop);
                }
                    

                

                
            }
            // TODO: new MenuCreator
            // TODO: use MenuCreator to draw the labels and button
            creator.EndMenu();
            Raylib.EndDrawing();

        }
        public void UpdateGame()
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

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_P))
            {
                currentGameState.Push(GameState.PauseMenu);
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

