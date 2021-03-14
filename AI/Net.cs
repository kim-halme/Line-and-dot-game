using System;

namespace CSharpNeuralNetworkLib
{
    class Net
    {
        public Layer[] Layers { get; set; }
        private readonly Random random = new Random();
        private readonly uint[] structure;
        private double[] allWeights;

        public Net(uint[] structure)
        {
            this.structure = structure;
            Layers = new Layer[structure.Length];
            
            for (int i = 0; i < structure.Length; i++)
            {
                if(i != structure.Length - 1)
                {
                    Layers[i] = new Layer(structure[i], structure[i + 1]);

                }
                else
                {
                    Layers[i] = new Layer(structure[i], 0);

                }
            }
            int m = 0;
            for (int j = 0; j < Layers.Length - 1; j++)
            {
                for (int k = 0; k < Layers[j].Neurons.Length; k++)
                {
                    for (int l = 0; l < Layers[j].Neurons[k].Weights.Length; l++)
                    {
                        allWeights[m] = Layers[j].Neurons[k].Weights[l];
                        m++;
                    }
                }
            }
        }

        public double[] Evaluate(double[] inputs)
        {
            if(inputs.Length != Layers[0].Neurons.Length - 1)
            {
                throw new Exception("Input size doesn't match with the network");
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                Layers[0].Neurons[i].Value = Sigmoid(inputs[i]);
            }

            for (int i = 1; i < Layers.Length; i++)
            {
                for (int j = 0; j < Layers[i].Neurons.Length - 1; j++)
                {
                    double value = 0;
                    for (int k = 0; k < Layers[i - 1].Neurons.Length; k++)
                    {
                        value += Layers[i - 1].Neurons[k].Weights[j] * Layers[i - 1].Neurons[k].Value;
                    }
                    Layers[i].Neurons[j].Value = Sigmoid(value);
                }
            }

            double[] array = new double[Layers[^1].Neurons.Length - 1];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = Layers[^1].Neurons[i].Value;
            }
            return array;
        }

        private double Sigmoid(double value)
        {
            return 1 / (1 + Math.Pow(Math.E, -value));
        }

        public void Evolve(double precentageOfWeightsToChange, double changeRate)
        {

            for (int j = 0; j < Layers.Length - 1; j++)
            {
                for (int k = 0; k < Layers[j].Neurons.Length; k++)
                {
                    for (int l = 0; l < Layers[j].Neurons[k].Weights.Length; l++)
                    {
                        int i = random.Next(0, 10001);
                        if (i < precentageOfWeightsToChange * 100)
                        {
                            if(i % 2 == 0)
                            {
                                Layers[j].Neurons[k].Weights[l] -= 1 * (changeRate / 100);
                            }
                            else
                            {
                                Layers[j].Neurons[k].Weights[l] += 1 * (changeRate / 100);
                            }
                        }
                    }
                }
            }
            
        }

        public Net Duplicate()
        {
            Net newNet = new Net(structure);
            for (int j = 0; j < Layers.Length - 1; j++)
            {
                for (int k = 0; k < Layers[j].Neurons.Length; k++)
                {
                    for (int l = 0; l < Layers[j].Neurons[k].Weights.Length; l++)
                    {
                        newNet.Layers[j].Neurons[k].Weights[l] = Layers[j].Neurons[k].Weights[l];
                    }
                }
            }
            return newNet;
        }
    }
}
