using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet
{
    public class Receptor
    {
        public Signal input;
        public Weight weight;
        public bool learnable;

        public float Value => input.value * weight.weight;

        public Receptor(Weight weight, Signal input, bool learnable)
        {
            this.input = input;
            this.learnable = learnable;
            this.weight = weight;
        }

        
    }
}
