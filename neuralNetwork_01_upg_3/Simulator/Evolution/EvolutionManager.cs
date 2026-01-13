using neuralNetwork_01_upg_3.RNG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.Evolution
{
    public class EvolutionManager
    {
        public EvolutionSpecimin[] population;
        private int rngNext = 100;
        private int genomeSize;



        private EvolutionSpecimin[] children_buffer;
        private EvolutionChild_Parrents[] children_parents;
 


        public ISelection   _selectionManager;
        public ICrossover   _crossoverManager;
        public IMutation    _mutationManager;


        public EvolutionManager(int populationSize, int seed)
        {
            population = new EvolutionSpecimin[populationSize];
            children_buffer = new EvolutionSpecimin[populationSize];
            children_parents = new EvolutionChild_Parrents[populationSize];
            rngNext = seed;

        }

        public void InitializePopulation(int genomeSize)
        {
            this.genomeSize = genomeSize;

            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new EvolutionSpecimin(genomeSize);

                for(int j = 0; j < genomeSize; j++)
                {
                    rngNext = CustomRandom.ShiftRandomXOr(rngNext);

                    population[i].genome[j] = (Math.Abs(rngNext) % 2000) / 1000f - 1;  
                }

                children_buffer[i] = new EvolutionSpecimin(genomeSize);
                children_parents[i] = new();
            }
        }

        public void SetEvolutionManagers(ISelection selectionManager, ICrossover crossoverManager, IMutation mutationManager)
        {
            if(selectionManager != null)
                _selectionManager = selectionManager;

            if(crossoverManager != null)
                _crossoverManager = crossoverManager;
            
            if(mutationManager != null)
                _mutationManager = mutationManager;
        }

        public void RunEvolution()
        {
            

            Selection();
            Crossover();
            Mutation();
            Replacement();
        }

        protected void Selection()
        {
            if (_selectionManager == null) return;

            _selectionManager.RunSelection(population, children_parents);
            
        }

        protected void Crossover()
        {
            if (_crossoverManager == null) return;

            _crossoverManager.RunCrossover(children_buffer, children_parents);

        }

        protected void Mutation()
        {
            if (_mutationManager == null) return;

            _mutationManager.RunMutation(children_buffer);
        }

        protected void Replacement()
        {
            var temp = population;

            population = children_buffer;

            children_buffer = temp;
        }

    }

    public class EvolutionChild_Parrents
    {
        public EvolutionSpecimin ParentA, ParentB;
    }

    public interface ISelection
    {
        public void RunSelection(EvolutionSpecimin[] last_population, EvolutionChild_Parrents[] selected_parents);
    }

    public interface ICrossover
    {
        public void RunCrossover(EvolutionSpecimin[] child_buffer, EvolutionChild_Parrents[] parrents);
    }

    public interface IMutation
    {
        public void RunMutation(EvolutionSpecimin[] target_population);
    }

    
}
