using System;

namespace CSharpNeuralNetworkLib
{
    class Neuron
    {
        private static readonly Random random = new Random();

        public double[] Weights { get; set; }
        public double Value { get; set; }

        public Neuron(uint neuronsInNextLayer)
        {
            Value = 1;

            if(neuronsInNextLayer != 0)
            {
                Weights = new double[neuronsInNextLayer];
                for (int i = 0; i < neuronsInNextLayer; i++)
                {
                    Weights[i] = random.Next(-3, 4);
                }
            }
        }

    }
}
