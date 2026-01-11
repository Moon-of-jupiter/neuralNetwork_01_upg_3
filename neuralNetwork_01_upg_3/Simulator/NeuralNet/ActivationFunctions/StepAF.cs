using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet.ActivationFunctions
{
    public class StepAF: IActivationFunction
    {
        public float Activation(float input)
        {
            
            float threshold = 1;

            return input >= threshold ? 0: threshold;
        }
    }

    public class SigAF : IActivationFunction
    {
        protected float power;
        public SigAF(float power = 1)
        {
            this.power = power;
        }
        public float Activation(float input)
        {
            return 1f / (1f + MathF.Pow(MathF.E, -input*power));
        }
    }
}
