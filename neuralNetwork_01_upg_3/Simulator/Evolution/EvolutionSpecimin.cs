using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Evolution
{
    public class EvolutionSpecimin
    {
        public float[] genome;
        public float fitness;

        public EvolutionSpecimin(int genomeSize)
        {
            genome = new float[genomeSize];
        }
    }
}
