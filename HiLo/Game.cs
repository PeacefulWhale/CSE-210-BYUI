using System;

namespace HiLo
{
    // Main Game Class.
    class Game
    {
        private int score = 0;
        private Deck deck;
        private int past_card;
        // -1 : Lower
        // 1 : Higher
        private int guess;
        public Game()
        {
            this.deck = new Deck();
            this.past_card = -1;
            this.guess = 0;
        }
        // Main game loop.
        public void runGame()
        {
            while(true)
            {
                int card = this.deck.drawCard();
                // Logic for running out of cards.
                if (card != -1)
                {
                    Console.WriteLine("The card is {0} ({1}), and there are {2} cards remaining.", this.deck.getCard(card), card + 1, deck.totalCards() - deck.draw);
                    // Logic for first turn.
                    if (past_card != -1 && this.guess != 0)
                    {
                        Console.WriteLine("The last card was {0} ({1})", this.deck.getCard(this.past_card), this.past_card + 1);
                        if ((this.past_card > card && this.guess == -1) || (this.past_card < card && this.guess == 1))
                        {
                            // They guessed correctly.
                            this.score += 100;
                            Console.WriteLine("You guessed correctly!");
                        }
                        else
                        {
                            // They guessed wrong.
                            this.score -= 75;
                            Console.WriteLine("You guessed incorrectly...");
                        }
                        // Break early if they've already lost.
                        if (this.score <= 0)
                        {
                            break;
                        }
                    }
                    this.past_card = card;
                    // Logic to skip "next card" dialogue if there is no next card.
                    if (deck.draw < deck.totalCards())
                    {
                        Console.WriteLine("Would you like to guess the next card?");
                        string input = Console.ReadLine();
                        if (input[0] == 'y' || input[0] == 'Y')
                        {
                            this.guess = 0;
                            // User is forced to enter valid input. There is no escaping!
                            while(this.guess == 0)
                            {
                                Console.WriteLine("Will the next card be higher (H) or lower (L) than {0} ({1})?", deck.getCard(card), card + 1);
                                input = Console.ReadLine();
                                if (input[0] == 'H' || input[0] == 'h')
                                {
                                    this.guess = 1;
                                }
                                else if (input[0] == 'L' || input[0] == 'l')
                                {
                                    this.guess = -1;
                                }
                                else
                                {
                                    Console.WriteLine("Please enter a valid input!");
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("There are no more cards!");
                    break;
                }
            }
            Console.WriteLine("The game is over! You scored: {0} points!", this.score);
            if (this.score == deck.totalCards() * 100)
            {
                Console.WriteLine("A perfect game!");
            }
            Console.WriteLine("");
        }
    }
    class Deck
    {
        // Unicode card faces.
        private static readonly string[] card_faces = {
            "🂡", "🂢", "🂣", "🂤", "🂥", "🂦", "🂨", "🂩", "🂪", "🂫", "🂬", "🂭", "🂮"
        };
        private const int total_cards = 13;
        private int[] deck;
        // The current card we're on.
        public int draw;
        private Random rnd;
        public Deck()
        {
            this.draw = 0;
            this.rnd = new Random();
            // Initialize deck.
            deck = new int[total_cards];
            for (int i = 0; i < total_cards; i++)
            {
                this.deck[i] = i;
            }
            // Shuffle deck.
            for (int i = 0; i < total_cards; i++)
            {
                int j = (int) rnd.NextInt64(0, total_cards);
                int temp = this.deck[j];
                this.deck[j] = this.deck[i];
                this.deck[i] = temp;
            }
        }
        // Simply returns the drawn card count.
        public int drawCard()
        {
            if (this.draw < total_cards)
            {
                return this.deck[this.draw++];
            }
            else
            {
                // There are no more cards to draw.
                return -1;
            }
        }
        public string getCard(int card)
        {
            card = Math.Clamp(card, 0, total_cards);
            return card_faces[card];
        }
        public int totalCards()
        {
            return total_cards;
        }
    }
}