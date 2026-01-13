using neuralNetwork_01_upg_3.RNG;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Simulator.NeuralNet;
using neuralNetwork_01_upg_3.Simulator.SnakeEvolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator
{
    public class SimulationManager
    {
        public NeuralNet_SnakeControler[] snakeControlers;
        public SnakeSimulator[] simulators;
        public float[] scores { get; private set; }

        protected SnakeMapData snakeMapData;

        public int AlivePopulation { get; private set; }    
        
        public SimulationManager(A_NeuralNet[] neuralNets, SnakeMapData snakeMapData)
        {
            this.snakeMapData = snakeMapData;

            snakeControlers = new NeuralNet_SnakeControler[neuralNets.Length];

            for(int i = 0; i < neuralNets.Length; i++)
            {
                snakeControlers[i] = new NeuralNet_SnakeControler(neuralNets[i]);
            }

            simulators = new SnakeSimulator[neuralNets.Length];

            InitializeSimulators();

            scores = new float[neuralNets.Length];
        }

        protected void InitializeSimulators()
        {
            int rngNext = snakeMapData.seed;

            for (int i = 0; i < simulators.Length; i++)
            {
                snakeMapData.seed = rngNext = CustomRandom.ShiftRandomXOr(rngNext + 16);
                //snakeMapData.size.X = Math.Abs(CustomRandom.ShiftRandomXOr(snakeMapData.seed)) % 10 + 1;
                simulators[i] = new SnakeSimulator(snakeControlers[i],snakeMapData);
                simulators[i].InitializeSnakeMap();
            }
        }

        public void ResetSimulators()
        {
            

            AlivePopulation = 0;
            for (int i = 0; i < simulators.Length; i++)
            {
                simulators[i].InitializeSnakeMap();
                AlivePopulation++;
            }
        }

        public void ScoreSimulation()
        {
            float maxScore = 0;

            for (int i = 0; i < simulators.Length; i++)
            {
                scores[i] = simulators[i].score;
                if (scores[i] > maxScore) maxScore = scores[i];
            }

            for (int i = 0; i < simulators.Length; i++)
            {
                scores[i] /= maxScore;
            }
        }

        public void FindBestScore(out float bestScore, out int bestPhenotype)
        {
            bestScore = simulators[0].score;
            bestPhenotype = 0;
            for (int i = 1; i < simulators.Length; i++)
            {
                if (bestScore < simulators[i].score)
                {
                    bestScore = simulators[i].score;
                    bestPhenotype = i;
                }
            }

            
        }
        
        public void UpdateSimulators()
        {
            AlivePopulation = 0;
            for ( int i = 0;i < simulators.Length; i++)
            {
                simulators[i].Update();
                if (!simulators[i].gameOver)
                {
                    AlivePopulation++;
                }
            }
        }

    }
}
