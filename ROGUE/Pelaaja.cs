using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ROGUE
{
    public enum Race
    {
        Human,
        Elf,
        Orc
    }

    public enum Class
    {
        Rogue,
        Warrior,
        Magician
    }
    internal class Pelaaja
    {
        public string name;
        public Race rotu;
        public Class luokka;

        public Vector2 paikka;
    }
}
