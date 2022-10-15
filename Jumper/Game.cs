using System;
using System.Collections.Generic;

namespace Seeker
{
    class Game
    {
        private string word;
        private List<char> guessed;
        private const int lives = 5;
        private int lost_lives;
        private static string[] parachute = {
            "  __",
            " /  \\",
            "/____\\",
            "\\    /",
            " \\  /",
            "  ||"
        };
        private static string[] person = {
            "  O",
            " /|\\",
            "  /\\"
        };

        public Game(string word)
        {
            this.word = word;
            this.guessed = new List<char>();
            this.lost_lives = 0;
        }

        public bool turn()
        {
            // Print the parachute.
            for (int i = this.lost_lives; i < parachute.Length; i++)
            {
                Console.WriteLine(parachute[i]);
            }
            // Print the person.
            for (int i = 0; i < person.Length; i++)
            {
                Console.WriteLine(person[i]);
            }
            // Print the word.
            bool game_won = true;
            for (int i = 0; i < this.word.Length; i++)
            {
                if (this.guessed.Contains(this.word[i]))
                {
                    Console.Write("{0} ", this.word[i]);
                }
                else
                {
                    game_won = false;
                    Console.Write("_ ");
                }
            }
            Console.Write("\n");
            // We can stop early as we've won the game.
            if (game_won)
            {
                Console.WriteLine("Congratulations on winning the game!");
                return game_won;
            }

            // Get valid input.
            while (true)
            {
                Console.WriteLine("Please enter your guess! [a-z]");
                string input = Console.ReadLine();
                char guess = input.ToUpper()[0];
                if (this.guessed.Contains(guess))
                {
                    Console.WriteLine("Guess a character you haven't guessed yet!");
                }
                else
                {
                    this.guessed.Add(guess);
                    // Check to see if the guess was right or not.
                    bool right = false;
                    for (int i = 0; i < this.word.Length; i++)
                    {
                        if (guess == this.word[i])
                        {
                            right = true;
                            break;
                        }
                    }
                    if (!right)
                    {
                        lost_lives++;
                        if (lost_lives > lives)
                        {
                            Console.WriteLine("Sorry, you lost!");
                            Console.WriteLine("The word was: {0}", this.word);
                            return true;
                        }
                    }
                    break;
                }
            }
            Console.Write("\n");
            return false;
        }
    }

    class Words
    {
        private int max_words;
        private static string[] words;
        private Random rnd;
        private static string[] default_words = {
            "ZOOPLANKTONS",
            "WRESTERS",
            "COOKIES",
            "DOGS",
            "CATS",
            "FISH",
            "WAFFLE",
            "ZEBRA",
            "CUDDLE",
            "DICTIONARY",
            "PARACHUTE",
            "BOAT",
            "A",
            "THIS",
            "IS",
            "PROBABLY",
            "NOT",
            "THE",
            "BEST",
            "WAY",
            "OF",
            "MAKING",
            "SOMETHING",
            "LIKE",
            "WHAT",
            "I",
            "CURRENTLY",
            "AM"
        };

        public Words()
        {
            this.rnd = new Random();
            if (System.IO.File.Exists("./dictionary.txt"))
            {
                words = System.IO.File.ReadAllLines("./dictionary.txt");
            }
            else
            {
                // The dictionary file cannot be found, rely off the few words I've hard coded in.
                words = default_words;
            }
            this.max_words = words.Length;
        }

        public string getWord()
        {
            return words[this.rnd.NextInt64(0, max_words)];
        }
    }
}