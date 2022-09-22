using System;

namespace TicTacToe
{
    class Board
    {
        private static int size;
        private static int players;
        private static char[,] board;
        private static int padding;
        private static string middle_line;
        // The player colors and symbols.
        private readonly char[] player_symbols = {
            'X', 'O',
            '#', '@', '&', '%',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'Y', 'Z',
            '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '!', '[', '}', ':', '<', '?', '/', '\u0008', '\u0376', '\u03D8', '\u03C6', '\u0394', '\u03FF', '\u0424', '\u0489', '\u058D',
        };
        private const int player_colors = 15;

        public Board(int _size, int _players)
        {
            size = _size;
            players = _players;
            board = new char[size, size];
            // Initialize our board.
            for (int i = 0; i < size * size; i++)
            {
                board[i % size, i / size] = '\0';
            }

            // Calculate how much padding we need.
            padding = 1;
            for (int i = size * size; i >= 10; i /= 10)
            {
                padding += 1;
            }

            // This just generates the middle line, which are just a bit wacky.
            string pad = new string('-', padding);
            string temp = string.Format("{0}+", pad);
            middle_line = "";
            for (int i = 0; i < size - 1; i++)
            {
                middle_line += temp;
            }
            middle_line += new string('-', padding);
        }

        public bool play_turn(int square, int player)
        {
            board[square % size, square / size] = player_symbols[player];
            int x = square % size;
            int y = square / size;
            // Check horizontal.
            bool game_won = true;
            for (int i = 0; i < size; i++)
            {
                if (board[x, i] != player_symbols[player])
                {
                    game_won = false;
                    break;
                }
            }
            if (game_won)
            {
                return game_won;
            }
            // Check sideways.
            game_won = true;
            for (int i = 0; i < size; i++)
            {
                if (board[i, y] != player_symbols[player])
                {
                    game_won = false;
                    break;
                }
            }
            if (game_won)
            {
                return game_won;
            }
            // Check diagonals.
            if (square % (size + 1) == 0 || (square % (size - 1)) == 0)
            {
                // If the above is true, then we are inside a diagonal square, isn't that neat?
                game_won = true;
                for (int i = 0; i < size; i++)
                {
                    if (board[i, i] != player_symbols[player])
                    {
                        game_won = false;
                        break;
                    }
                }
                if (game_won)
                {
                    return game_won;
                }
                game_won = true;
                for (int i = 0; i < size; i++)
                {
                    if (board[i, (size - 1) - i] != player_symbols[player])
                    {
                        game_won = false;
                        break;
                    }
                }
                if (game_won)
                {
                    return game_won;
                }
            }
            return false;
        }

        public bool is_empty(int square)
        {
            if (board[square % size, square / size] == '\0')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void set_color(int player)
        {
            // I offset the color so the default isn't just a lot of grey.
            player += 4;
            Console.ForegroundColor = (ConsoleColor)(player % player_colors);
            // This ensures the background color is different.
            Console.BackgroundColor = (ConsoleColor)((player + (player_colors / 2)) % player_colors);
        }

        public void print_board()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Generate our string to print.
                    string cell_string;
                    if (board[x, y] != '\0')
                    {
                        // Print Player Markers.
                        int player = Array.IndexOf(player_symbols, board[x, y]);
                        set_color(player);
                        cell_string = string.Format("{0}", new string(board[x, y], padding + 1));
                    }
                    else
                    {
                        // Print the cell number.
                        // I used to have this in one very long line of code, but I broke it up so it's more human readable.
                        string num = (x + (y * size)).ToString();
                        float pad = ((float)padding - (float)num.Length) / 2;
                        string left_pad = "";
                        string right_pad = "";
                        if (pad > 0)
                        {
                            left_pad += new string(' ', (int)Math.Floor(pad));
                            right_pad += new string(' ', (int)Math.Ceiling(pad));
                        }
                        cell_string = string.Format("{1}{0}{2}", num, left_pad, right_pad);
                    }
                    Console.Write(cell_string);
                    Console.ResetColor();
                    if (x < size - 1)
                    {
                        Console.Write("|");
                    }
                }
                Console.Write("\n");
                if (y < size - 1)
                {
                    Console.WriteLine(middle_line);
                }
            }
        }
    }
}
