using System;

namespace HiLo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Hello, would you like to play a game?");
                string input = Console.ReadLine();
                if (input[0] == 'y' || input[0] == 'Y')
                {
                    Game game = new Game();
                    game.runGame();
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Goodbye!");
        }
    }
}