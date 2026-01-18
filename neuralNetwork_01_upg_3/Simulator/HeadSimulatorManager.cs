using Microsoft.Xna.Framework;
using neuralNetwork_01_upg_3.RNG;
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
        public float BestScore { get; private set; }
        public float HighestScoreLastRound { get; private set; }
        public float HighestScoreEver { get; private set; }
        public int BestPhenotype { get; private set; }
        

        public int GameFrame {  get; private set; }
        
        protected EvolutionSpecimin[] population => _evolutionManager.population;



        protected A_NeuralNet[] neuralNetworks;
        
        protected IActivationFunction activationFunction;


        public SimulationManager _simulationManager { get; private set; }

        public EvolutionManager _evolutionManager { get; private set; }

        protected bool ActiveGeneration;

        private EvoluionSimData simData;


        public List<float> Scores { get; private set; }

        public HeadSimulatorManager(EvoluionSimData simData, int rngSeed)
        {
            activationFunction = new SigAF(2);

            this.simData = simData;

            _evolutionManager = new EvolutionManager(simData.population, rngSeed);


            //population = new EvolutionSpecimin[simData.population];
            neuralNetworks = new A_NeuralNet[simData.population];

            InitializeANNs();

            _simulationManager = new SimulationManager(neuralNetworks, new SnakeMapData()
            {
                size = simData.mapSize,
                seed = CustomRandom.ShiftRandomXOr(rngSeed - 3),
            });



            Scores = new List<float>();

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

            _evolutionManager.InitializePopulation(weights_count + 3);
        }

        public void StartNewGeneration()
        {
            Generation++;

            

            for (int i = 0; i < population.Length; i++)
            {
                neuralNetworks[i].UpdateWeights((int weight) => { return population[i].genome[weight]; });
            }

            _simulationManager.ResetSimulators();
        }

        public void EndGeneration()
        {
            HighestScoreLastRound = BestScore;


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

            _simulationManager.FindBestScore(out float BestScore, out int BestPhenotype);

            if(BestScore > HighestScoreEver) HighestScoreEver = BestScore;

            this.BestScore = BestScore;
            this.BestPhenotype = BestPhenotype;

            if (_simulationManager.AlivePopulation <= 0)
            {
                Scores.Add(BestScore);
                GameFrame = 0;
                //Scores[Generation] = _simulationManager.FindBestScore();

                EndGeneration();
                StartNewGeneration();
            }

            GameFrame++;
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
