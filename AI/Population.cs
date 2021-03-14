using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CSharpNeuralNetworkLib
{
    public class Population
    {
        private Net[] agents;
        private uint currentAgentId = 0;
        private double[] agentScores;
        private readonly uint size;
        private readonly uint[] structure;
        public Population(uint[] structure, uint size)
        {
            this.size = size;
            this.structure = structure;

            agents = new Net[size];
            for (int i = 0; i < size; i++)
            {
                agents[i] = new Net(structure);
            }

            agentScores = new double[size];
        }

        public Population(string path)
        {
            size = 0; 
            List<uint> xmlStructure = new List<uint>();
            List<double> weights = new List<double>();

            using(XmlReader reader = XmlReader.Create(path))
            {
                while(reader.Read())
                {
                    if(reader.IsStartElement())
                    {
                        switch(reader.Name)
                        {
                            case "size":
                                reader.Read();
                                size = UInt32.Parse(reader.Value);
                                break;

                            case "layer":
                                reader.Read();
                                xmlStructure.Add(UInt32.Parse(reader.Value));
                                break;

                            case "weight":
                                reader.Read();
                                weights.Add(Double.Parse(reader.Value));
                                break;
                        }
                    }
                }
            }

            structure = new uint[xmlStructure.Count];
            for (int i = 0; i < structure.Length; i++)
            {
                structure[i] = xmlStructure[i];
            }

            agents = new Net[size];
            for (int i = 0; i < size; i++)
            {
                agents[i] = new Net(structure);
            }

            int m = 0;
            for (int i = 0; i < agents.Length; i++)
            {
                for (int j = 0; j < agents[i].Layers.Length - 1; j++)
                {
                    for (int k = 0; k < agents[i].Layers[j].Neurons.Length; k++)
                    {
                        for (int l = 0; l < agents[i].Layers[j].Neurons[k].Weights.Length; l++)
                        {
                            agents[i].Layers[j].Neurons[k].Weights[l] = weights[m];
                            m++;
                        }
                    }
                }
            }
            agentScores = new double[size];
        }

        public void SaveXml(string path)
        {
            using(XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("data");

                writer.WriteStartElement("specifications");
                writer.WriteElementString("size", size.ToString());
                writer.WriteStartElement("structure");

                for (int i = 0; i < structure.Length; i++)
                {
                    writer.WriteElementString("layer", structure[i].ToString());
                }

                

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("weights");

                for (int i = 0; i < agents.Length; i++)
                {
                    for (int j = 0; j < agents[i].Layers.Length - 1; j++)
                    {
                        for (int k = 0; k < agents[i].Layers[j].Neurons.Length; k++)
                        {
                            for (int l = 0; l < agents[i].Layers[j].Neurons[k].Weights.Length; l++)
                            {
                                writer.WriteElementString("weight", agents[i].Layers[j].Neurons[k].Weights[l].ToString());
                            }
                        }
                    }
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void SaveCsv(string path)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine(size.ToString());
            string structureStr = "";
            for (int i = 0; i < structure.Length; i++)
            {
                structureStr += (i == structure.Length - 1) ? structure[i].ToString() : structure[i].ToString() + ",";
            }
            content.AppendLine(structureStr);

            string weightStr = "";
            for (int i = 0; i < agents.Length; i++)
            {
                for (int j = 0; j < agents[i].Layers.Length - 1; j++)
                {
                    for (int k = 0; k < agents[i].Layers[j].Neurons.Length; k++)
                    {
                        for (int l = 0; l < agents[i].Layers[j].Neurons[k].Weights.Length; l++)
                        {
                            weightStr += agents[i].Layers[j].Neurons[k].Weights[l].ToString() + ",";
                        }
                    }
                }
            }

            weightStr = weightStr.Remove(weightStr.Length - 1);
            content.AppendLine(weightStr);
            File.WriteAllText(path, content.ToString());
        }

        public double[] EvaluateWithCurrentAgent(double[] inputs)
        {
            return agents[currentAgentId].Evaluate(inputs);
        }

        public bool ScoreAgentAndSelectNext(double score)
        {
            agentScores[currentAgentId] = score;
            if (currentAgentId + 1 < size)
            {
                currentAgentId++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EvolvePopulation(uint topAgentsToEvolve, uint copiesPerAgent, uint randomAgents, double precentageOfWeightsToChange, double changeRate)
        {
            if (currentAgentId != size - 1)
            {
                throw new Exception("Cannot evolve population, because not all agents have been used");
            }
            
            if(topAgentsToEvolve * copiesPerAgent + randomAgents != size)
            {
                throw new Exception("Parameters 0, 1, 2 dont match the population size");
            }

            int index;
            Net[] newAgents = new Net[size];
            int l = 0;
            for (int i = 0; i < topAgentsToEvolve; i++)
            {
                index = Array.IndexOf(agentScores, agentScores.Max());
                for (int j = 0; j < copiesPerAgent; j++)
                {
                    (newAgents[l] = agents[index].Duplicate()).Evolve(precentageOfWeightsToChange, changeRate);
                    l++;
                }
                agentScores[index] = 0;               
            }
            for (int i = 0; i < randomAgents; i++)
            {
                newAgents[l] = new Net(structure);
                l++;
            }
            agents = newAgents;
        }
    }
}
