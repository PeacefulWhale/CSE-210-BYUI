using System;

namespace Seeker
{
    class Jumper
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Words word_list = new Words();
                Console.WriteLine("Would you like to play a game (y/n)?");
                string input = Console.ReadLine();
                if (input[0] == 'Y' || input[0] == 'y')
                {
                    Game game = new Game(word_list.getWord());
                    while (!game.turn());
                }
                else
                {
                    break;
                }
            }
        }
    }
}
