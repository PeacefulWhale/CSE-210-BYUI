using System;

namespace Dice
{
    class Program
    {
        private const UInt64 max = 100000000000000;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello. Please enter the # of dice you would like to roll (1-100,000,000,000,000).");
            string input = Console.ReadLine();
            UInt64 dice_count = Math.Clamp(Convert.ToUInt64(input), 1, max);
            Game game = new Game(dice_count);
            UInt64 game_score = 0;
            while (true)
            {
                Console.WriteLine("Roll dice?");
                input = Console.ReadLine();
                if (input == "y" || input == "Y")
                {
                    game_score += game.roll();
                }
                else
                {
                    Console.WriteLine("You scored a total of {0} points!\nGoodbye!", game_score);
                    break;
                }
            }
        }
    }
}