using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet
{
    public class Neuron
    {
        public List<Receptor> inputs { get; set; }
        public Signal output { get; protected set; }

        public IActivationFunction activationFunction;

        public Neuron(IActivationFunction activationFunction) {
            
            inputs = new List<Receptor>(); // possible proformance problem
            output = new Signal(); 

            this.activationFunction = activationFunction;
        }


        public void Fire()
        {
            output.value = Activation(InputSum());
        }

        protected float InputSum()
        {
            float sum = 0;

            foreach (Receptor incoming in inputs)
            {
                sum += incoming.Value;
            }

            return sum;
        }

        protected float Activation(float input) => activationFunction.Activation(input);




    }
}
