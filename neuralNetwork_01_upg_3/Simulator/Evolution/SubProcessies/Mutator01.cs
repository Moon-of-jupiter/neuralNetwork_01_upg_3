using neuralNetwork_01_upg_3.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Evolution.SubProcessies
{
    public class Mutator01: IMutation
    {
        public float mutation_probability;
        public float mutation_strength;

        public int rngNext;
        public Mutator01(float mutation_probability, float mutation_strength, int seed)
        {
            this.mutation_probability = mutation_probability;
            this.mutation_strength = mutation_strength;

            this.rngNext = seed;
        }

        public void RunMutation(EvolutionSpecimin[] target_population)
        {
            

            for (int i = 0; i < target_population.Length; i++)
            {
                for (int j = 0; j < target_population[i].genome.Length; j++)
                {
                    rngNext = CustomRandom.ShiftRandomXOr(rngNext);

                    float probRng = CustomRandom.ShiftRandomXOr(rngNext);
                    if (((Math.Abs(probRng) % 1000) / 1000f) > mutation_probability)
                    {
                        continue;
                    }
                    

                    target_population[i].genome[j] += ((Math.Abs(rngNext) % 2000) / 1000f - 1) * mutation_strength;

                }
            }
        }



    }
}
