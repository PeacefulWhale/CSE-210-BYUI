using System;

namespace Seeker
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Would you like to play a game (y/n)?");
                string input = Console.ReadLine();
                if (input[0] == 'Y' || input[0] == 'y')
                {
                    Game game = new Game();
                    while (!game.guess());
                }
                else
                {
                    break;
                }
            }
        }
    }
}
