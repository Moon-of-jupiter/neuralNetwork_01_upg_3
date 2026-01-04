using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet
{
    public class NeuralLayer
    {
        public List<Neuron> neurons;

        public NeuralLayer(int layerCount)
        {
            neurons = new List<Neuron>();
        }

        


        public Signal GetSignal(int index)
        {
            if(neurons[index] == null)  return null;

            return neurons[index].output;
        
        }

        

        public void FireLayer()
        {
            foreach (Neuron neuron in neurons)
            {
                neuron.Fire();
            }
        }

    }
}
