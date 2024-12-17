using System;

namespace JogoDaCobrinha
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            SnakeGame snakeGame = new SnakeGame();
            snakeGame.Start();
            Console.Clear();
        }
    }
}
