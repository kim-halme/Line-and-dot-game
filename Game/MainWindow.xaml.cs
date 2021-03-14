using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Shapes;

namespace LineAndDotGame
{
    public partial class MainWindow : Window
    {
        private Game game;
        private AI ai;

        private const int ellipseOffSet = 15;
        private const int lineOffSet = 25;
        private Line dragLine = new Line
        {
            Stroke = Brushes.Gray,
            StrokeThickness = 3
        };
        private Ellipse previewEllipse = new Ellipse
        {
            Width = 20,
            Height = 20,
            Fill = Brushes.Gray,
            Stroke = Brushes.Gray,
            StrokeThickness = 1
        };

        private List<int[]> allPossibleMoves;
        private List<Shape> previewMoveShapes = new List<Shape>();

        private bool draggingEnabled = false;
        private bool showingAllMoves = false;
        public bool AIEnabled { get; set; } = false;
        public int Score { get; set; } = 0;
        private int peakScore = 0;


        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            DrawStartingBoard();
        }

        public void IncreaseScore()
        {
            Score++;
            score.Text = Score.ToString();
            if(Score > peakScore)
            {
                IncreasePeakScore();
            }
        }

        private void ResetScore()
        {
            score.Text = "0";
            Score = 0;
        }

        private void IncreasePeakScore()
        {
            peakScore++;
            peak_score.Text = $"Peak: {peakScore}";
        }

        private void DrawStartingBoard()
        {
            for (int i = 1; i < 20; i++)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.X1 = 0;
                line.Y1 = i * 50;
                line.X2 = 1000;
                line.Y2 = i * 50;
                line.StrokeThickness = 1;
                line.SnapsToDevicePixels = true;
                canvas.Children.Add(line);
            }

            for (int i = 1; i < 20; i++)
            {
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.X1 = i * 50;
                line.Y1 = 0;
                line.X2 = i * 50;
                line.Y2 = 1000;
                line.StrokeThickness = 1;
                line.SnapsToDevicePixels = true;
                canvas.Children.Add(line);
            }

            for (int i = 0; i < 4; i++)
            {
                DrawEllipse(i * 50 + 5 * 50, 8 * 50);
            }
            for (int i = 0; i < 4; i++)
            {
                DrawEllipse(i * 50 + 8 * 50, 5 * 50);
            }
            for (int i = 0; i < 4; i++)
            {
                DrawEllipse(i * 50 + 11 * 50, 8 * 50);
            }
            for (int i = 0; i < 4; i++)
            {
                DrawEllipse(i * 50 + 5 * 50, 11 * 50);
            }
            for (int i = 0; i < 4; i++)
            {
                DrawEllipse(i * 50 + 8 * 50, 14 * 50);
            }
            for (int i = 0; i < 4; i++)
            {
                DrawEllipse(i * 50 + 11 * 50, 11 * 50);
            }

            for (int i = 0; i < 2; i++)
            {
                DrawEllipse(8 * 50, i * 50 + 6 * 50);
            }
            for (int i = 0; i < 2; i++)
            {
                DrawEllipse(5 * 50, i * 50 + 9 * 50);
            }
            for (int i = 0; i < 2; i++)
            {
                DrawEllipse(8 * 50, i * 50 + 12 * 50);
            }
            for (int i = 0; i < 2; i++)
            {
                DrawEllipse(11 * 50, i * 50 + 6 * 50);
            }
            for (int i = 0; i < 2; i++)
            {
                DrawEllipse(14 * 50, i * 50 + 9 * 50);
            }
            for (int i = 0; i < 2; i++)
            {
                DrawEllipse(11 * 50, i * 50 + 12 * 50);
            }
        }

        public bool DrawEllipse(int x, int y)
        {
            if (game.AddDotIfLegal(x / 50, y / 50))
            {
                Ellipse newEllipse = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Black,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(newEllipse);
                Canvas.SetTop(newEllipse, y + ellipseOffSet);
                Canvas.SetLeft(newEllipse, x + ellipseOffSet);
                IncreaseScore();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PreviewEllipse(int x, int y)
        {
            canvas.Children.Add(previewEllipse);
            Canvas.SetLeft(previewEllipse, x + ellipseOffSet);
            Canvas.SetTop(previewEllipse, y + ellipseOffSet);
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            canvas.Children.Add(new Line
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3,
                Y2 = y2 * 50 + lineOffSet,
                X1 = x1 * 50 + lineOffSet,
                Y1 = y1 * 50 + lineOffSet,
                X2 = x2 * 50 + lineOffSet
            });
        }

        public void ResetBoard()
        {
            ResetScore();
            canvas.Children.RemoveRange(0, canvas.Children.Count);
            DrawStartingBoard();
        }

        private void canvas_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(showingAllMoves)
            {
                RemoveDrawnMoves();
                showingAllMoves = false;
            }

            Point point = e.GetPosition(canvas);
            int i = ((int)point.X) / 50;
            int j = ((int)point.Y) / 50;
            if(game.IsDotLegal(i, j) && !(canvas.Children.Contains(previewEllipse) && (Canvas.GetLeft(previewEllipse) - ellipseOffSet) / 50 == i && (Canvas.GetTop(previewEllipse) - ellipseOffSet) / 50 == j))
            {
                canvas.Children.Remove(previewEllipse);
                PreviewEllipse(i * 50, j * 50);
                draggingEnabled = false;
            }
            else if (canvas.Children.Contains(previewEllipse) )
            {
                draggingEnabled = true;
                dragLine.X1 = (int)(point.X - (point.X % 50)) + lineOffSet;
                dragLine.Y1 = (int)(point.Y - (point.Y % 50)) + lineOffSet;
                dragLine.Y2 = (int)(point.Y - (point.Y % 50)) + lineOffSet;
                dragLine.X2 = (int)(point.X - (point.X % 50)) + lineOffSet;
                canvas.Children.Add(dragLine);
            }
            else
            {
                draggingEnabled = false;
            }
        }

        private void canvas_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && draggingEnabled)
            {
                Point point = e.GetPosition(canvas);
                dragLine.X2 = point.X;
                dragLine.Y2 = point.Y;
                
            }
        }

        private void canvas_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!draggingEnabled)
                return;
            Point point = e.GetPosition(canvas);
            int i = ((int)point.X) / 50;
            int j = ((int)point.Y) / 50;
            if (game.AddLineIfLegal((int)(dragLine.X1 - lineOffSet) / 50, (int)(dragLine.Y1 - lineOffSet) / 50, i, j, (int)(Canvas.GetLeft(previewEllipse) - ellipseOffSet) / 50, (int)(Canvas.GetTop(previewEllipse) - ellipseOffSet) / 50))
            {
                int x = (int)Canvas.GetLeft(previewEllipse);
                int y = (int)Canvas.GetTop(previewEllipse);
                DrawEllipse(x - x % 50, y - y % 50);
                canvas.Children.Add(new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 3,
                    X1 = dragLine.X1,
                    Y1 = dragLine.Y1,
                    X2 = point.X - point.X % 50 + lineOffSet,
                    Y2 = point.Y - point.Y % 50 + lineOffSet
                });
                canvas.Children.Remove(previewEllipse);
                canvas.Children.Remove(dragLine);
            }
            else
            {
                canvas.Children.Remove(previewEllipse);
                canvas.Children.Remove(dragLine);
            }
        }

        private void DrawAllMoves()
        {
            for(int i = 0; i < previewMoveShapes.Count; i++)
            {
                canvas.Children.Remove(previewMoveShapes[i]);
            }
            previewMoveShapes = new List<Shape>();

            foreach(int[] array in allPossibleMoves)
            {
                Ellipse newEllipse = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1
                };
                canvas.Children.Add(newEllipse);
                Canvas.SetLeft(newEllipse, array[4] * 50 + ellipseOffSet);
                Canvas.SetTop(newEllipse, array[5] * 50 + ellipseOffSet);
                previewMoveShapes.Add(newEllipse);

                Line newLine = new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 3,
                    X1 = array[0] * 50 + lineOffSet,
                    Y1 = array[1] * 50 + lineOffSet,
                    X2 = array[2] * 50 + lineOffSet,
                    Y2 = array[3] * 50 + lineOffSet
                };
                canvas.Children.Add(newLine);
                previewMoveShapes.Add(newLine);
            }
        }

        private void RemoveDrawnMoves()
        {
            for (int i = 0; i < previewMoveShapes.Count; i++)
            {
                canvas.Children.Remove(previewMoveShapes[i]);
            }
            previewMoveShapes = new List<Shape>();
        }

        private void button_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!showingAllMoves)
            {
                allPossibleMoves = game.CalculateAllMoves();
                DrawAllMoves();
                showingAllMoves = true;
            }
            else
            {
                RemoveDrawnMoves();
                showingAllMoves = false;
            }
        }

        private void btn_ai_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!AIEnabled)
            {
                if(ai == null)
                {
                    ai = new AI(game, this);
                }
                AIEnabled = true;
                button.IsEnabled = false;
                canvas.IsEnabled = false;
                ThreadStart aiThreadStart = new ThreadStart(ai.Play);
                Thread aiThread = new Thread(aiThreadStart);
                aiThread.Start();
            }
            else
            {
                AIEnabled = false;
                button.IsEnabled = true;
                canvas.IsEnabled = true;
            }
        }
    }
}
