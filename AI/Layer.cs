namespace CSharpNeuralNetworkLib
{
    class Layer
    {
        public Neuron[] Neurons { get; }
        public Layer(uint numberOfNeurons, uint neuronsInNextLayer)
        {
            Neurons = new Neuron[numberOfNeurons + 1];

            for(int i = 0; i <= numberOfNeurons; i++)
            {
                Neurons[i] = new Neuron(neuronsInNextLayer);
                
            }
        }

    }
}
