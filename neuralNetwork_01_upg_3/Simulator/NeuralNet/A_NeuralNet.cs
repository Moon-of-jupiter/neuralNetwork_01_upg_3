using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuralNetwork_01_upg_3.Simulator.NeuralNet
{
    public class A_NeuralNet
    {
        public int InputLayersCount => input.Count;
        public int OutputLayersCount => output.Count;
        public int WeightsCount => weights.Count;

        public List<NeuralLayer> neuralLayers;

        protected List<Weight> weights;

        protected IActivationFunction activationFunction;

        public List<Signal> input { get; protected set; }

        public List<Signal> output { get; protected set; }

        protected Signal bias;

        public A_NeuralNet(int input_n, int hidden_n, int output_n, int hidden_layers, IActivationFunction activationFunction)
        {
            neuralLayers = new List<NeuralLayer>();
            weights = new List<Weight>();

            this.activationFunction = activationFunction;

            bias = new Signal();

            bias.value = 1;

            CreateLayers(input_n, hidden_n, output_n, hidden_layers);

        }

        #region Create Empty Net

        protected void CreateLayers(int input_n, int hidden_n, int output_n, int hidden_layers)
        {
            CreateInputLayer(input_n);

            CreateHiddenLayers(hidden_n, hidden_layers);

            CreateOutputLayer(output_n);

        }
        protected void CreateInputLayer(int input_n)
        {
            input = new();
            var inputLayer = new NeuralLayer(input_n);
            
            List<Signal> oneInput = new List<Signal>();
            oneInput.Add(null);
            for (int i = 0; i < input_n; i++)
            {

                input.Add(new Signal());
                oneInput[0] = input[i];
                inputLayer.neurons.Add(CreateNeuron(oneInput));
            }

            neuralLayers.Add(inputLayer);
        }

        protected void CreateHiddenLayers(int hidden_n, int hidden_layers) 
        {
            List<Signal> parrentSignals = new List<Signal>();

            for (int i = 0; i < hidden_layers; i++)
            {
                LayerToSignals(neuralLayers.Last(), ref parrentSignals);

                var layer = CreateLayer(parrentSignals, hidden_n);
            }
        }

        protected void CreateOutputLayer(int output_n)
        {
            output = new();
            List<Signal> parrentSignals = new List<Signal>();

            LayerToSignals(neuralLayers.Last(), ref parrentSignals);

            var outputLayer = CreateLayer(parrentSignals, output_n);

            foreach (var outputNeuron in outputLayer.neurons)
            {
                output.Add(outputNeuron.output);
            }

            neuralLayers.Add(outputLayer);
        }


        protected Weight CreateWeight()
        {
            var weight = new Weight();

            weights.Add(weight);

            return weight;
        }

        protected Neuron CreateNeuron(List<Signal> inputs)
        {
            var neuron = new Neuron(activationFunction);

            foreach (var input in inputs)
            {
                neuron.inputs.Add(new Receptor(CreateWeight(), input, true));
            }

            return neuron;
        }

        protected NeuralLayer CreateLayer(List<Signal> inputs, int neuronCount)
        {
            NeuralLayer layer = new NeuralLayer(neuronCount);

            for(int i = 0; i < neuronCount; i++)
            {
                layer.neurons.Add(CreateNeuron(inputs));
            }

            return layer;
        }

        
        protected void LayerToSignals(NeuralLayer layer, ref List<Signal> signals, bool bias = true)
        {
            foreach(var neuron in layer.neurons)
            {
                signals.Add(neuron.output);
            }

            if (!bias) return;

            signals.Add(this.bias);
        }

        #endregion

        #region Use Net

        public void SetInput(int inputIndex,float value)
        {
            input[inputIndex].value = value;
        }

        public float ReadOutput(int outputIndex)
        {
            return output[outputIndex].value;
        }

        public void UpdateWeights(Func<int,float> GetNewValues)
        {
            for(int i = 0; i < weights.Count; i++)
            {
                weights[i].weight = GetNewValues(i);
            }
        }

        public void FireNeurons()
        {
            foreach(var layer in neuralLayers)
            {
                layer.FireLayer();
            }
        }

        #endregion

    }
}
