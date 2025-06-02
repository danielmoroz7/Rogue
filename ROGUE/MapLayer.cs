using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROGUE
{
    internal class MapLayer
    {
        public string name;
        public int[] mapTiles;
        public MapLayer(int mapSize)
        {
            name = "";
            mapTiles = new int[mapSize];
        }
    }
}
