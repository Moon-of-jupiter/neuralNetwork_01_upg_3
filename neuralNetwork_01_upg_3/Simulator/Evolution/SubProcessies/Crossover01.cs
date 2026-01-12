using neuralNetwork_01_upg_3.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Evolution.SubProcessies
{
    public class Crossover01: ICrossover
    {
        public int rngNext;

        public Crossover01(int rngNext)
        {
            this.rngNext = rngNext;
        }

        public void RunCrossover(EvolutionSpecimin[] child_buffer, EvolutionChild_Parrents[] parents)
        {
            for(int i = 0; i < child_buffer.Length; i++)
            {
                for(int j = 0; j < child_buffer[i].genome.Length; j++)
                {
                    rngNext = CustomRandom.ShiftRandomXOr(rngNext);

                    
                    if (Math.Abs(rngNext) % 2 == 0)
                    {
                        child_buffer[i].genome[j] = parents[i].ParentA.genome[j];
                    }
                    else
                    {
                        child_buffer[i].genome[j] = parents[i].ParentB.genome[j];
                    }
                }
            }
        }
    }
}
