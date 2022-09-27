using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello. Please enter the board size (2-64).");
            string input = Console.ReadLine();
            int board_size = Math.Clamp(Convert.ToInt32(input), 2, 64);
            // Max players is based off the fact that if you have too many players literally no one can win.
            // I am lazy, and I will just set the max player size as the board size, as that allows each player to create their own winning score.
            Console.WriteLine("Thank you, please enter the number of players (2-{0}).", board_size);
            input = Console.ReadLine();
            int player_count = Math.Clamp(Convert.ToInt32(input), 2, board_size);
            Console.WriteLine("Thank you, please enter the minium line length to win (2-{0})", board_size);
            input = Console.ReadLine();
            int win_length;
            if (Convert.ToInt32(input) == 0)
            {
                win_length = board_size;
            }
            else
            {
                win_length = Math.Clamp(Convert.ToInt32(input), 2, board_size);
            }
            // Play loop.
            while (true)
            {
                // Create the board.
                Board main_board = new Board(board_size, player_count, win_length);
                int turn = 0;
                // Game loop.
                while (true)
                {
                    int square_target = 0;
                    // Users must enter valid input!
                    while (true)
                    {
                        Console.Clear();
                        main_board.print_board();
                        main_board.set_color(turn % player_count);
                        Console.WriteLine("Player {0} enter a valid square number.", turn % player_count);
                        Console.ResetColor();
                        input = Console.ReadLine();
                        try
                        {
                            square_target = Math.Clamp(Convert.ToInt32(input), 0, (board_size * board_size) - 1);
                        }
                        catch
                        {
                            continue;
                        }
                        if (main_board.is_empty(square_target))
                        {
                            break;
                        }
                    }
                    if (main_board.play_turn(square_target, turn % player_count))
                    {
                        Console.Clear();
                        main_board.print_board();
                        main_board.set_color(turn % player_count);
                        Console.WriteLine("Congratulations to Player {0} for winning!", turn % player_count);
                        Console.ResetColor();
                        break;
                    }
                    else if (turn == board_size * board_size)
                    {
                        Console.Clear();
                        main_board.print_board();
                        Console.WriteLine("It's a tie!");
                        break;
                    }
                    // Increment player turn and loop again.
                    turn++;
                }
                Console.WriteLine("Would you like to play again (y/n)?");
                input = Console.ReadLine();
                if (input[0] != 'y' && input[0] != 'Y')
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
            }
        }
    }
}
