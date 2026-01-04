using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet
{
    public interface IActivationFunction
    {
        public float Activation(float input);

    }
}
