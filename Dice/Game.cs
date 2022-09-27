using System;

namespace Dice
{
    class Game
    {
        private UInt64 dice_count;
        private readonly char[] dice_faces = {
            '\u2680', '\u2681', '\u2682', '\u2683', '\u2684', '\u2685'
        };
        private Random rnd;
        private const int total_colors = 15;

        public Game(UInt64 _dc)
        {
            dice_count = _dc;
            rnd = new Random();
        }

        public UInt64 roll()
        {
            // Randomize the total number of each dice instead of rolling randomly for every single dice.
            // But I want to do this fairly.
            // I will do it by choosing 6 random numbers in the range of 0 to dice_count.
            // And then treating the distance between the 6 random numbers as the total number of dice in each face of dice.
            UInt64[] numbers = new UInt64[7];
            numbers[0] = 0;
            for (int i = 1; i < 6; i++)
            {
                // I will merge two 32 bit numbers to get our 64 bit random number.
                // Or I could just create random bytes and convert them to 64 bit numbers... Which is probably safer.
                byte[] buf = new byte[8];
                rnd.NextBytes(buf);
                numbers[i] = (BitConverter.ToUInt64(buf, 0) % (dice_count + 1));
            }
            numbers[6] = dice_count;
            // Super quick insertion sort to order the numbers in their proper places.
            for (int x = 0; x < 6; x++)
            {
                for (int i = x; i >= 0 && numbers[i + 1] < numbers[i]; i--)
                {
                    UInt64 temp = numbers[i];
                    numbers[i] = numbers[i + 1];
                    numbers[i + 1] = temp;
                }
            }
            // Convert the numbers to dice counts.
            UInt64[] dice = new UInt64[6];
            for (int i = 0; i < 6; i++)
            {
                dice[i] = numbers[i + 1] - numbers[i];
            }
            // Print the dice, and the total score.
            UInt64 score = (50 * dice[0]) + (100 * dice[5]);
            Console.Write("You Rolled... ");
            for (int i = 0; i < 6; i++)
            {
                set_color(i + 1);
                Console.Write("{0} {1}", dice[i], dice_faces[i]);
                if (i != 5)
                {
                    Console.Write(", ");
                }
                Console.ResetColor();
            }
            // 0 - 1 - 2 - 3 - 4 - 5 - 6
            Console.Write("\nYou Scored: ");
            set_color(10);
            Console.WriteLine("{0}", score);
            Console.ResetColor();
            // I was testing to make sure that my method for random dice totals would actually worked.
            // Console.WriteLine("Total Dice: {0}", dice[0] + dice[1] + dice[2] + dice[3] + dice[4] + dice[5]);
            return score;
        }

        private void set_color(int color)
        {
            Console.ForegroundColor = (ConsoleColor)(color % total_colors);
        }
    }
}