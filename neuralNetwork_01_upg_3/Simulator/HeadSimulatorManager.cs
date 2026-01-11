using Microsoft.Xna.Framework;
using neuralNetwork_01_upg_3.Simulator.Evolution;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Simulator.NeuralNet;
using neuralNetwork_01_upg_3.Simulator.NeuralNet.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator
{
    public class HeadSimulatorManager
    {
        public int Generation {  get; private set; }

        protected EvolutionSpecimin[] population => _evolutionManager.population;



        protected A_NeuralNet[] neuralNetworks;
        
        protected IActivationFunction activationFunction;


        public SimulationManager _simulationManager { get; private set; }

        public EvolutionManager _evolutionManager { get; private set; }

        protected bool ActiveGeneration;

        private EvoluionSimData simData;
        public HeadSimulatorManager(EvoluionSimData simData)
        {
            activationFunction = new SigAF(10);

            this.simData = simData;

            _evolutionManager = new EvolutionManager(simData.population);


            //population = new EvolutionSpecimin[simData.population];
            neuralNetworks = new A_NeuralNet[simData.population];

            InitializeANNs();

            _simulationManager = new SimulationManager(neuralNetworks, new SnakeMapData()
            {
                size = simData.mapSize,
            });

            
            

            
            StartNewGeneration();
        }

        protected void InitializeANNs()
        {
            int weights_count = 0;

            for (int i = 0; i < neuralNetworks.Length; i++)
            {
                
                neuralNetworks[i] = new A_NeuralNet(6,simData.neuralNetwork_Height, 4, simData.neuralNetwork_Width,activationFunction);
                weights_count = neuralNetworks[i].WeightsCount;
                //population[i] = new EvolutionSpecimin(neuralNetworks[i].WeightsCount);
            }

            _evolutionManager.InitializePopulation(weights_count);
        }

        public void StartNewGeneration()
        {
            Generation++;

            for(int i = 0; i < population.Length; i++)
            {
                neuralNetworks[i].UpdateWeights((int weight) => { return population[i].genome[weight]; });
            }

            _simulationManager.ResetSimulators();
        }

        public void EndGeneration()
        {
            _simulationManager.ScoreSimulation();
            for(int i = 0; i < population.Length; i++)
            {
                population[i].fitness = _simulationManager.scores[i];   
            }

            _evolutionManager.RunEvolution();

        }

        public void Update()
        {
            _simulationManager.UpdateSimulators();

            if(_simulationManager.AlivePopulation == 0)
            {
                EndGeneration();
                StartNewGeneration();
            }

        }

    }

    public struct EvoluionSimData
    {
        public int population; // amount of indeviduals per population

        public int neuralNetwork_Height;    // nodes per hidden layer
        public int neuralNetwork_Width;     // amount of hidden layers

        public Point mapSize;
     

    }
}
