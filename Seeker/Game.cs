using System;

namespace Seeker
{
    class Game
    {
        private int random_number;
        private int last_guess;
        private const int max_number = 1000;

        public Game()
        {
            Random rand = new Random();
            this.random_number = (int) (rand.NextInt64() % max_number) + 1;
            last_guess = 0;
        }
        
        public bool guess()
        {
            Console.WriteLine("Guess from 1 to {0}", max_number);
            int input = Convert.ToInt32(Console.ReadLine());
            if (input == this.random_number)
            {
                Console.WriteLine("You did it! You found the number!");
                return true;
            }
            else if (this.distance(last_guess) == this.distance(input))
            {
                Console.WriteLine("Huh... You're staying at the same temperature...");
            }
            else if (this.distance(last_guess) > this.distance(input))
            {
                Console.WriteLine("You're getting warmer!");
            }
            else
            {
                Console.WriteLine("You're getting colder!");
            }
            this.last_guess = input;
            return false;
        }
        
        private int distance(int x)
        {
            return Math.Abs(this.random_number - x);
        }
    }
}