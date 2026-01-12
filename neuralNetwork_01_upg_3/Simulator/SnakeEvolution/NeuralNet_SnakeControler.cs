using Microsoft.Xna.Framework;
using neuralNetwork_01_upg_3.Simulator.Game;
using neuralNetwork_01_upg_3.Simulator.Game.Snake;
using neuralNetwork_01_upg_3.Simulator.NeuralNet;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.SnakeEvolution
{
    public class NeuralNet_SnakeControler: ISnakeControler
    {
        

        protected A_NeuralNet neuralNet;
        
        public NeuralNet_SnakeControler(A_NeuralNet neuralNet)
        {
            this.neuralNet = neuralNet;
        }


        // 0 = apple dist x 
        // 1 = apple dist y
        // 2 = danger ray upp
        // 3 = danger ray down
        // 4 = danger ray left
        // 5 = danger ray right

        public static bool CheckANN_Compat(A_NeuralNet neuralNet)
        {
            return neuralNet.InputLayersCount == 6 && neuralNet.OutputLayersCount == 4;
        }

        protected void UpdateInputs(SnakeSimulator snakeSim)
        {
            int appleDist_x =   snakeSim.LatestApple.X - snakeSim.SnakeHead.X;
            int appleDist_y =   snakeSim.LatestApple.Y - snakeSim.SnakeHead.Y;

            neuralNet.SetInput(0, appleDist_x);
            neuralNet.SetInput(1, appleDist_y);
            int distanceToDanger = 0;
            Point direction = Point.Zero;
            Point snakeOrigin = snakeSim.SnakeHead;
            for (int i = 0; i < 4; i++)
            {
                GetDirection(i, ref direction);
                snakeSim.Raycast(ref snakeOrigin, ref direction, out distanceToDanger, SnakeSimulator.MapElementType.snake);
                neuralNet.SetInput(i + 2, distanceToDanger - 1);
            }
        }

        protected Point GetOutputDirection()
        {
            int index_best = 0;
            float value_best = neuralNet.ReadOutput(0);
            float currentValue;
            for (int i = 1; i < 4; i++)
            {
                currentValue = neuralNet.ReadOutput(i);
                if (value_best < currentValue)
                {
                    index_best = i;
                    value_best = currentValue;
                }
            }

            Point p = Point.Zero;
            GetDirection(index_best, ref p);

            return p;
        }

        protected static void GetDirection(int index, ref Point direction) 
        {
            switch (index)
            {
                case 0:
                    direction.X = 0;
                    direction.Y = 1;
                    break;

                case 1:
                    direction.X = 1;
                    direction.Y = 0;
                    break;

                case 2:
                    direction.X =  0;
                    direction.Y = -1;
                    break;

                case 3:
                    direction.X = -1;
                    direction.Y =  0;
                    break;

                case 4:
                    direction.X =  1;
                    direction.Y =  1;
                    break;

                case 5:
                    direction.X =  1;
                    direction.Y = -1;
                    break;

                case 6:
                    direction.X = -1;
                    direction.Y = -1;
                    break;

                case 7:
                    direction.X = -1;
                    direction.Y =  1;
                    break;

                default:
                    direction.X = 0;
                    direction.Y = 0;
                    break;
            
            };
        }
        


        public Point Control(SnakeSimulator snakeSim)
        {
            UpdateInputs(snakeSim);

            neuralNet.FireNeurons();

            return GetOutputDirection();

        }

    }
}
