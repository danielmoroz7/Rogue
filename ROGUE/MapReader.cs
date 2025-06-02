using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboMapReader;

namespace ROGUE
{
    
    internal class MapReader
    {


        public void ReadMapFromFileTest(string fileName)
        {
            using (StreamReader reader = File.OpenText(fileName))
            {
                Console.WriteLine("File contents:");
                Console.WriteLine();

                string line;
                while (true)
                {
                    line = reader.ReadLine();
                    if (line == null)
                    {
                        break; // End of file
                    }
                    Console.WriteLine(line);
                }

            }

        }
        public Map LoadTiledMap(string fileName)
        {
            TiledMap LoadTiledMap = TurboMapReader.MapReader.LoadMapFromFile(fileName);
            // Tarkista onnistuiko lataaminen
            if (LoadTiledMap != null)
            {
                // Muuta Map olioksi ja palauta
                Map loadedMap = ConvertTiledMapToMap(LoadTiledMap);
                loadedMap.LoadEnemies();
                loadedMap.LoadItems();
                return loadedMap;

            }
            else
            {
                // OH NO!
                return null;
            }
        }
        public Map LoadMapFromFile(string fileName)
        {
            bool exists = File.Exists(fileName);
            if (exists == false)
            {
                Console.WriteLine($"File {fileName} not found");
                return null; // Return the test map as fallback
            }

            string fileContents;

            using (StreamReader reader = File.OpenText(fileName))
            {
                // TODO: Read all lines into fileContens
                fileContents = reader.ReadToEnd();
            }

            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);
            loadedMap.LoadEnemies();
            loadedMap.LoadItems();
            return loadedMap;
        }
        public Map ConvertTiledMapToMap(TiledMap turboMap)
        {
            // Luo tyhjä kenttä
            Map rogueMap = new Map();

            // Muunna tason "ground" tiedot
            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");
            TurboMapReader.MapLayer enemyLayer = turboMap.GetLayerByName("enemies");
            TurboMapReader.MapLayer itemLayer = turboMap.GetLayerByName("items");

            // TODO: Lue kentän leveys. Kaikilla TurboMapReader.MapLayer olioilla on sama leveys
            rogueMap.mapWidth = turboMap.width;

            // Kuinka monta kenttäpalaa tässä tasossa on?
            int howManyTiles = groundLayer.data.Length;

            howManyTiles = enemyLayer.data.Length;
            // Taulukko jossa palat ovat
            int[] groundTiles = groundLayer.data;

            



            // Luo uusi taso tietojen perusteella
            MapLayer myGroundLayer = new MapLayer(howManyTiles);
            myGroundLayer.name = "ground";
            myGroundLayer.mapTiles = groundLayer.data;

            MapLayer myEnemyLayer = new MapLayer(howManyTiles);
            myEnemyLayer.name = "enemies";
            myEnemyLayer.mapTiles = enemyLayer.data;

            MapLayer myItemLayer = new MapLayer(howManyTiles);
            myItemLayer.name = "items";
            myItemLayer.mapTiles = itemLayer.data;
            // TODO: lue tason palat



            // Tallenna taso kenttään
            rogueMap.layers[0] = myGroundLayer;
            rogueMap.layers[1] = myEnemyLayer;
            rogueMap.layers[2] = myItemLayer;

            // TODO: Muunna tason "enemies" tiedot...
            // TODO: Muunna tason "items" tiedot...

            // Lopulta palauta kenttä
            return rogueMap;
        }

    }
}

