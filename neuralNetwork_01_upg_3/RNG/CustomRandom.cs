using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.RNG
{
    public static class CustomRandom
    {
        public static int ShiftRandomXOr(int seed)
        {
            unchecked
            {
                seed ^= seed << 13;
                seed ^= seed >> 17;
                seed ^= seed << 5;
                return seed;
            }

        }
    }
}
