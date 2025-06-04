using RayGuiCreator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace ROGUE
{
    class OptionsMenu
    {
        public event EventHandler BackButtonPressedEvent;
        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int menuStartX = 10;
            int menuStartY = 0;
            int rowHeight = Raylib.GetScreenHeight() / 20;
            int menuWidth = Raylib.GetScreenWidth() / 4;

            // HUOM MenuCreator luodaan aina uudestaan ennen kuin valikko piirrettään.
            MenuCreator creator = new MenuCreator(menuStartX, menuStartY, rowHeight, menuWidth);

            creator.Label("OPTIONS");
            creator.Label("Game");
                if (creator.Button("Back"))
                {
                    BackButtonPressedEvent.Invoke(this, EventArgs.Empty);
                }
            // TODO: new MenuCreator
            // TODO: use MenuCreator to draw the labels and button


            Raylib.EndDrawing();
        }
    
    }
}
