using neuralNetwork_01_upg_3.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Evolution.SubProcessies
{
    public class Selector01: ISelection
    {
        public float fitnessThreshold;

        public int randomNext;

        public float weighted_interpolation;

        public Selector01(float fitnessThreshold,float weighted_interpolation, int seed)
        {
            this.fitnessThreshold = fitnessThreshold;
            this.weighted_interpolation = weighted_interpolation;
            randomNext = seed;
        }

        public void RunSelection(EvolutionSpecimin[] last_population, EvolutionChild_Parrents[] selected_parents)
        {
            List<(EvolutionSpecimin, float)> wheel = new List<(EvolutionSpecimin, float)> ();

            float fitnessSum = 0;
            
            for (int i = 0; i < last_population.Length; i++)
            {
                if (last_population[i].fitness >= fitnessThreshold)
                {
                    wheel.Add((last_population[i], fitnessSum += last_population[i].fitness * (weighted_interpolation) + (1 - weighted_interpolation)));
                }
            }



            for(int i = 0; i < selected_parents.Length; i++)
            {
                selected_parents[i].ParentA = SelectRoulete(wheel, RandomUpTo(randomNext, fitnessSum, 10000));
                selected_parents[i].ParentB = SelectRoulete(wheel, RandomUpTo(randomNext, fitnessSum, 10000));
            }


        }
        protected float RandomUpTo(int randomNext, float max, int resolution)
        {
            randomNext = CustomRandom.ShiftRandomXOr(randomNext);

            
            return (Math.Abs(randomNext) % (max * resolution)) / (float)resolution;
        }

        protected EvolutionSpecimin SelectRoulete(List<(EvolutionSpecimin, float)> wheel, float randomValue)
        {
            // could be binary search
            for(int i = 0;i < wheel.Count; i++)
            {
                if (wheel[i].Item2 < randomValue)
                    continue;

                return wheel[i].Item1;

                if (i + 1 >= wheel.Count || wheel[i+1].Item2 < randomValue)
                {
                    return wheel[i].Item1;
                }
            }

            return wheel[wheel.Count-1].Item1;
        }




    }
}
