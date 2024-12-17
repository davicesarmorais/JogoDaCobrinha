using System;
using System.Diagnostics;
using System.Threading;


namespace JogoDaCobrinha
{
    public class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class SnakeGame
    {
        private readonly Snake snake = new Snake();
        private Pos fruit;
        private readonly Stopwatch time = new Stopwatch();
        private readonly int widthWindow = Math.Min(Console.WindowWidth, 60);
        private readonly int heightWindow = Math.Min(Console.WindowHeight, 20);
        private readonly char[,] window;
        public int HeightWindow
        { get { return heightWindow; } }
        public int WidthWindow
        { get { return widthWindow; } }

        public SnakeGame()
        {
            window = new char[heightWindow, widthWindow];
            CreateWindow();
            Console.CursorVisible = false;
        }


        public void Start()
        {
            SummonSnake();
            SummonFruit();
            Thread inputThread = new Thread(ReadInput);
            inputThread.Start();
            time.Reset();
            time.Start();
            while (true)
            {
                if ((snake.Head.X <= 1 || snake.Head.X >= WidthWindow - 2) ||
                    (snake.Head.Y <= 1 || snake.Head.Y >= HeightWindow - 2) ||
                    (!snake.Alive) || (snake.Collision()))
                {
                    snake.Kill();
                    time.Stop();
                    Console.WriteLine("\nYou died.");
                    ShowStats();
                    Console.WriteLine("Aperte ESC para sair.");
                    Console.WriteLine("Aperte ENTER para jogar novamente.");
                    inputThread.Join();
                    break;
                }
                if (HasEatenFruit())
                {
                    SummonFruit();
                    snake.IncreaseSize();
                }

                snake.Move();
                DisplayWindow();
                Thread.Sleep(30);
            }
        }

        public void ShowStats()
        {
            Console.WriteLine("Tamanho da sua cobra: {0}", snake.Size);
            if (time.Elapsed.Minutes == 0)
            {
                Console.WriteLine("O tempo percorrido foi: {0} segundos", time.Elapsed.Seconds);
            } 
            else
            {
                Console.WriteLine("O tempo percorrido foi: {0}:{1}", time.Elapsed.Minutes, time.Elapsed.Seconds);
            }

        }

        private void SummonSnake()
        {
            snake.Generate();
            snake.Head.Y = (heightWindow / 2);
            snake.Head.X = (widthWindow / 2);
        }

        private bool HasEatenFruit()
        {
            return (snake.Head.X == fruit.X && snake.Head.Y == fruit.Y);
        }

        private void SummonFruit()
        {
            Random rnd = new Random();
            int x, y;
            while (true)
            {
                x = rnd.Next(5, WidthWindow - 5);
                y = rnd.Next(5, HeightWindow - 5);
                if (x != snake.Head.X || y != snake.Head.Y) break;
            }
            fruit = new Pos(x, y);
        }

        private void ReadInput()
        {
            while (true)
            {
                ConsoleKey Key = Console.ReadKey(true).Key;
                if ((Key == ConsoleKey.LeftArrow) && !(snake.Direction == SnakeDirection.Right))
                { snake.Direction = SnakeDirection.Left; }

                if ((Key == ConsoleKey.RightArrow) && !(snake.Direction == SnakeDirection.Left))
                { snake.Direction = SnakeDirection.Right; }

                if ((Key == ConsoleKey.UpArrow) && !(snake.Direction == SnakeDirection.Down))
                { snake.Direction = SnakeDirection.Up; }

                if ((Key == ConsoleKey.DownArrow) && !(snake.Direction == SnakeDirection.Up))
                { snake.Direction = SnakeDirection.Down; }

                if (Key == ConsoleKey.Escape)
                {
                    if (snake.Alive) snake.Kill();
                    else break;
                }

                if (Key == ConsoleKey.Enter && !snake.Alive)
                {
                    Console.Clear();
                    Start();
                    break;
                }
            }
        }
      
        private void CreateWindow()
        {
            for (int i = 0; i < heightWindow; i++)
            {
                for (int j = 0; j < widthWindow; j++)
                {
                    if (i == 0 || i == heightWindow - 1) window[i, j] = '-';
                    else if (j == 0 || j == widthWindow - 1) window[i, j] = '|';
                    else window[i, j] = ' ';
                }
            }
        }

        private void UpdateWindow()
        {
            ClearWindow();
            window[snake.Head.Y, snake.Head.X] = 'O';
            for (int i = 1; i < snake.Size; i++)
            {
                window[snake.Body[i].Y, snake.Body[i].X] = 'o';
            }
            window[fruit.Y, fruit.X] = '*';
        }

        private void ClearWindow()
        {
            for (int i = 1; i < heightWindow - 1; i++)
            {
                for (int j = 1; j < widthWindow - 1; j++)
                {
                    window[i, j] = ' ';
                }
            }
        }
        private void DisplayWindow()
        {
            UpdateWindow();
            for (int i = 0; i < heightWindow; i++)
            {
                for (int j = 0; j < widthWindow; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(window[i, j]);
                }
            }
        }
    }
}

