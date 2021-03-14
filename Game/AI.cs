using CSharpNeuralNetworkLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LineAndDotGame
{
    class AI
    {
        private Game game;
        private MainWindow window;
        // structure is {2000, 15, 15, 1} size is 50
        private Population population;
        private List<int[]> availableMoves;

        public AI(Game game, MainWindow window)
        {
            this.game = game;
            this.window = window;
            population = new Population("AIData.xml");
        }

        public void Play()
        {
            game.ClearBoardData();
            window.Dispatcher.Invoke(() => {
                window.ResetBoard();
            });
            while (window.AIEnabled)
            {
                while (GetMoves())
                {
                    double[] moveScores = new double[availableMoves.Count];
                    for (int i = 0; i < availableMoves.Count; i++)
                    {
                        moveScores[i] = population.EvaluateWithCurrentAgent(GetInputArray(i))[0];
                    }
                    int bestMove = Array.IndexOf(moveScores, moveScores.Max());
                    game.AddLineIfLegal(availableMoves[bestMove][0], availableMoves[bestMove][1], availableMoves[bestMove][2], availableMoves[bestMove][3], availableMoves[bestMove][4], availableMoves[bestMove][5]); ;
                    window.Dispatcher.Invoke(() =>
                    {
                        window.DrawLine(availableMoves[bestMove][0], availableMoves[bestMove][1], availableMoves[bestMove][2], availableMoves[bestMove][3]);
                        window.DrawEllipse(availableMoves[bestMove][4] * 50, availableMoves[bestMove][5] * 50);
                    });
                }
                if(!population.ScoreAgentAndSelectNext(window.Score))
                {
                    population.EvolvePopulation(5, 9, 5, 1, 1);
                }
                

                game.ClearBoardData();
                window.Dispatcher.Invoke(() => {
                    window.ResetBoard();
                });
            }
            population.Save("AIData.xml");
        }


        private double[] GetInputArray(int index)
        {
            if(availableMoves.Count > 0)
            {
                Game newGame = game.Duplicate();
                newGame.AddLineIfLegal(availableMoves[index][0], availableMoves[index][1], availableMoves[index][2], availableMoves[index][3], availableMoves[index][4], availableMoves[index][5]);
                List<double> inputList = new List<double>();
                for (int i = 0; i < newGame.BoardData.Length; i++)
                {
                    for (int j = 0; j < newGame.BoardData[i].Length; j++)
                    {
                        inputList.Add(Convert.ToDouble(newGame.BoardData[i][j].HorizontalLine));
                        inputList.Add(Convert.ToDouble(newGame.BoardData[i][j].VerticalLine));
                        inputList.Add(Convert.ToDouble(newGame.BoardData[i][j].LeftToRightDiagonalLine));
                        inputList.Add(Convert.ToDouble(newGame.BoardData[i][j].RightToLeftDiagonalLine));
                        inputList.Add(Convert.ToDouble(newGame.BoardData[i][j].Dot));
                    }
                }
                return inputList.ToArray();
            }
            return null;
        }

        private bool GetMoves()
        {
            availableMoves = game.CalculateAllMoves();
            if(availableMoves.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
