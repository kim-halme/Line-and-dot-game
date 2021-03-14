using System;
using System.Collections.Generic;

namespace LineAndDotGame
{
    public class Game
    {
        public TileInfo[][] BoardData { get; set; }
        private bool newDotIncludedInLine;
        public Game()
        {
            BoardData = new TileInfo[20][];
            for (int i = 0; i < BoardData.Length; i++)
            {
                BoardData[i] = new TileInfo[20];
                for(int j = 0; j < BoardData[i].Length; j++)
                {
                    BoardData[i][j] = new TileInfo();
                }
            }
        }

        public bool AddDotIfLegal(int i, int j)
        {
            if (BoardData[i][j].Dot)
                return false;
            BoardData[i][j].Dot = true;
            return true;
        }

        public bool IsDotLegal(int i, int j)
        {
            if (BoardData[i][j].Dot)
                return false;
            return true;
        }

        public bool IsLineLegal(int i1, int j1, int i2, int j2, int dotI, int dotJ)
        {
            newDotIncludedInLine = false;

            if (BoardData[dotI][dotJ].Dot == true)
                return false; 

            // checks length and direction
            if (Math.Abs(i1 - i2) == 4 || Math.Abs(j1 - j2) == 4) 
            { 
                if (i1 != i2 && j1 != j2)
                {
                    if (!(Math.Abs(i1 - i2) == 4 && Math.Abs(j1 - j2) == 4))
                        return false;
                }
            }
            else
                return false;


            
            if (i1 == i2)
            {
                if (j1 < j2)
                {

                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1][j1 + i].Dot == false && !(i1 == dotI && (j1 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1][j1 + i].VerticalLine == true)
                            return false;
                        if (i1 == dotI && (j1 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true; // end for vertical line checks
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1][j2 + i].Dot == false && !(i1 == dotI && (j2 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1][j2 + i].VerticalLine == true)
                            return false;
                        if (i1 == dotI && (j2 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true; // end for vertical line checks
                }
            }
            else if (j1 == j2)
            {
                if (i1 < i2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1 + i][j1].Dot == false && !((i1 + i) == dotI && j1 == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1 + i][j1].HorizontalLine == true)
                            return false;
                        if ((i1 + i) == dotI && j1 == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true; // end for horizontal line checks
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i2 + i][j1].Dot == false && !((i2 + i) == dotI && j1 == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i2 + i][j1].HorizontalLine == true)
                            return false;
                        if ((i2 + i) == dotI && j1 == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true; // end for horizontal line checks
                }
            }
            else if ((i1 < i2 && j1 < j2) || (i1 > i2 && j1 > j2))
            {
                if (i1 < i2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1 + i][j1 + i].Dot == false && !((i1 + i) == dotI && (j1 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1 + i][j1 + i].LeftToRightDiagonalLine == true)
                            return false;
                        if ((i1 + i) == dotI && (j1 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i2 + i][j2 + i].Dot == false && !((i2 + i) == dotI && (j2 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i2 + i][j2 + i].LeftToRightDiagonalLine == true)
                            return false;
                        if ((i2 + i) == dotI && (j2 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true;
                }
            }
            else
            {
                if (i1 < i2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1 + i][j1 - i].Dot == false && !((i1 + i) == dotI && (j1 - i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1 + i][j1 - i].RightToLeftDiagonalLine == true)
                            return false;
                        if ((i1 + i) == dotI && (j1 - i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i2 + i][j2 - i].Dot == false && !((i2 + i) == dotI && (j2 - i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i2 + i][j2 - i].RightToLeftDiagonalLine == true)
                            return false;
                        if ((i2 + i) == dotI && (j2 - i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    return true;
                }
            }

        }

        public List<int[]> CalculateAllMoves()
        {
            
            List<int[]> moveList = new List<int[]>();
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (0 < i + 4  && i + 4 < 20)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if(IsLineLegal(i, j, i + 4, j, i + k, j))
                            {
                                int[] array = { i, j, i + 4, j, i + k, j };
                                moveList.Add(array);
                            }
                        }
                    }

                    if (0 < j + 4 && j + 4 < 20)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (IsLineLegal(i, j, i, j + 4, i, j + k))
                            {
                                int[] array = { i, j, i, j + 4, i, j + k };
                                moveList.Add(array);
                            }
                        }
                    }

                    if (0 < i + 4 && i + 4 < 20 && 0 < j + 4 && j + 4 < 20)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (IsLineLegal(i, j, i + 4, j + 4, i + k, j + k))
                            {
                                int[] array = { i, j, i + 4, j + 4, i + k, j + k };
                                moveList.Add(array);
                            }
                        }
                    }

                    if (0 < i - 4 && i - 4 < 20 && 0 < j + 4 && j + 4 < 20)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (IsLineLegal(i, j, i - 4, j + 4, i - k, j + k))
                            {
                                int[] array = { i, j, i - 4, j + 4, i - k, j + k };
                                moveList.Add(array);
                            }
                        }
                    }
                }
            }

            return moveList;
        }

        public bool AddLineIfLegal(int i1, int j1, int i2, int j2, int dotI, int dotJ)
        {
            newDotIncludedInLine = false;

            // checks length and direction
            if (Math.Abs(i1 - i2) == 4 || Math.Abs(j1 - j2) == 4)
            {
                if (i1 != i2 && j1 != j2)
                {
                    if (!(Math.Abs(i1 - i2) == 4 && Math.Abs(j1 - j2) == 4))
                        return false;
                }
            }
            else
                return false;



            if (i1 == i2)
            {
                if (j1 < j2)
                {

                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1][j1 + i].Dot == false && !(i1 == dotI && (j1 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1][j1 + i].VerticalLine == true)
                            return false;
                        if (i1 == dotI && (j1 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i1][j1 + i].VerticalLine = true;
                    }
                    return true; // end for vertical line checks
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1][j2 + i].Dot == false && !(i1 == dotI && (j2 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1][j2 + i].VerticalLine == true)
                            return false;
                        if (i1 == dotI && (j2 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i1][j2 + i].VerticalLine = true;
                    }
                    return true; // end for vertical line checks
                }
            }
            else if (j1 == j2)
            {
                if (i1 < i2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1 + i][j1].Dot == false && !((i1 + i) == dotI && j1 == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1 + i][j1].HorizontalLine == true)
                            return false;
                        if ((i1 + i) == dotI && j1 == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i1 + i][j1].HorizontalLine = true;
                    }
                    return true; // end for horizontal line checks
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i2 + i][j1].Dot == false && !((i2 + i) == dotI && j1 == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i2 + i][j1].HorizontalLine == true)
                            return false;
                        if ((i2 + i) == dotI && j1 == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i2 + i][j1].HorizontalLine = true;
                    }
                    return true; // end for horizontal line checks
                }
            }
            else if ((i1 < i2 && j1 < j2) || (i1 > i2 && j1 > j2))
            {
                if (i1 < i2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1 + i][j1 + i].Dot == false && !((i1 + i) == dotI && (j1 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1 + i][j1 + i].LeftToRightDiagonalLine == true)
                            return false;
                        if ((i1 + i) == dotI && (j1 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i1 + i][j1 + i].LeftToRightDiagonalLine = true;
                    }
                    return true;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i2 + i][j2 + i].Dot == false && !((i2 + i) == dotI && (j2 + i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i2 + i][j2 + i].LeftToRightDiagonalLine == true)
                            return false;
                        if ((i2 + i) == dotI && (j2 + i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i2 + i][j2 + i].LeftToRightDiagonalLine = true;
                    }
                    return true;
                }
            }
            else
            {
                if (i1 < i2)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i1 + i][j1 - i].Dot == false && !((i1 + i) == dotI && (j1 - i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i1 + i][j1 - i].RightToLeftDiagonalLine == true)
                            return false;
                        if ((i1 + i) == dotI && (j1 - i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i1 + i][j1 - i].RightToLeftDiagonalLine = true;
                    }
                    return true;
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (BoardData[i2 + i][j2 - i].Dot == false && !((i2 + i) == dotI && (j2 - i) == dotJ))
                            return false;
                        if (i > 0 && i < 4 && BoardData[i2 + i][j2 - i].RightToLeftDiagonalLine == true)
                            return false;
                        if ((i2 + i) == dotI && (j2 - i) == dotJ)
                            newDotIncludedInLine = true;
                    }
                    if (!newDotIncludedInLine)
                        return false;

                    for (int i = 0; i < 5; i++)
                    {
                        BoardData[i2 + i][j2 - i].RightToLeftDiagonalLine = true;
                    }
                    return true;
                }
            }

        }

        public Game Duplicate()
        {
            Game newGame = new Game();
            for (int i = 0; i < BoardData.Length; i++)
            {
                for (int j = 0; j < BoardData[i].Length; j++)
                {
                    newGame.BoardData[i][j].HorizontalLine = BoardData[i][j].HorizontalLine;
                    newGame.BoardData[i][j].VerticalLine = BoardData[i][j].VerticalLine;
                    newGame.BoardData[i][j].LeftToRightDiagonalLine = BoardData[i][j].LeftToRightDiagonalLine;
                    newGame.BoardData[i][j].RightToLeftDiagonalLine = BoardData[i][j].RightToLeftDiagonalLine;
                    newGame.BoardData[i][j].Dot = BoardData[i][j].Dot;
                }
            }
            return newGame;
        }

        public void ClearBoardData()
        {
            BoardData = new TileInfo[20][];
            for (int i = 0; i < BoardData.Length; i++)
            {
                BoardData[i] = new TileInfo[20];
                for (int j = 0; j < BoardData[i].Length; j++)
                {
                    BoardData[i][j] = new TileInfo();
                }
            }
        }
    }
    public class TileInfo
    {
        public bool HorizontalLine { get; set; }
        public bool VerticalLine { get; set; }
        public bool LeftToRightDiagonalLine { get; set; }
        public bool RightToLeftDiagonalLine { get; set; }
        public bool Dot { get; set; }
        public TileInfo()
        {
            HorizontalLine = false;
            VerticalLine = false;
            LeftToRightDiagonalLine = false;
            RightToLeftDiagonalLine = false;
            Dot = false;
        }
    }
}
