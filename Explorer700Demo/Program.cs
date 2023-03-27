using Explorer700Library;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Explorer700Demo
{
    public class Program
    {
        static Explorer700 exp = new Explorer700();
        static void Main(string[] args)
        {
            // Set up the screen
            var screen = exp.Display.Graphics;

            // Define the Tic Tac Toe board
            char[,] board = new char[,] {
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' }
            };

            // Define the players
            char player1 = 'X';
            char player2 = 'O';
            char currentPlayer = player1;

            // Draw the initial Tic Tac Toe board on the screen
            DrawBoard(screen, board);

            // Play the game
            bool gameOver = false;
            while (!gameOver)
            {
                // Player 1's turn
                if (currentPlayer == 'X')
                {
                    // Wait for player 1 to move
                    bool moveMade = false;
                    while (!moveMade)
                    {
                        Keys keys = exp.Joystick.Keys;

                        // Check for joystick movements
                        if ((keys & Keys.Left) != 0 && col > 0)
                        {
                            col--;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Right) != 0 && col < 2)
                        {
                            col++;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Up) != 0 && row > 0)
                        {
                            row--;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Down) != 0 && row < 2)
                        {
                            row++;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Center) != 0 && board[row, col] == '-')
                        {
                            // Valid move, place X on board and switch to player 2
                            board[row, col] = 'X';
                            currentPlayer = 'O';
                            moveMade = true;
                        }

                        // Update the display
                        DrawBoard(board);
                    }
                }

                // Player 2's turn
                else if (currentPlayer == 'O')
                {
                    // Wait for player 2 to move
                    bool moveMade = false;
                    while (!moveMade)
                    {
                        Keys keys = joystick.Keys;

                        // Check for joystick movements
                        if ((keys & Keys.Left) != 0 && col > 0)
                        {
                            col--;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Right) != 0 && col < 2)
                        {
                            col++;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Up) != 0 && row > 0)
                        {
                            row--;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Down) != 0 && row < 2)
                        {
                            row++;
                            moveMade = true;
                        }
                        else if ((keys & Keys.Center) != 0 && board[row, col] == '-')
                        {
                            // Valid move, place O on board and switch to player 1
                            board[row, col] = 'O';
                            currentPlayer = 'X';
                            moveMade = true;
                        }

                        // Update the display
                        DrawBoard(board);
                    }
                }

                // Check for a win or tie
                if (CheckWin(board, currentPlayer))
                {
                    Console.WriteLine($"Player {currentPlayer} wins!");
                    gameOver = true;
                }
                else if (CheckTie(board))
                {
                    Console.WriteLine("Tie game!");
                    gameOver = true;
                }
            }

            // Wait for user input
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DrawBoard(System.Drawing.Graphics screen, char[,] board)
        {
            screen.Clear(exp.Display.Color.Black);
            screen.DrawString(" " + board[0, 0] + " | " + board[0, 1] + " | " + board[0, 2] + " ", new Font(FontFamily.GenericSansSerif, 10), exp.Display.Brushes.White, 0, 0);
            screen.DrawString("---+---+---", new Font(FontFamily.GenericSansSerif, 10), Exp.Display.Brushes.White, 0, 10);
            screen.DrawString(" " + board[1, 0] + " | " + board[1, 1] + " | " + board[1, 2] + " ", new Font(FontFamily.GenericSansSerif, 10), exp.Display.Brushes.White, 0, 20);
            screen.DrawString("---+---+---", new Font(FontFamily.GenericSansSerif, 10), Exp.Display.Brushes.White, 0, 30);
            screen.DrawString(" " + board[2, 0] + " | " + board[2, 1] + " | " + board[2, 2] + " ", new Font(FontFamily.GenericSansSerif, 10), exp.Display.Brushes.White, 0, 40);
            exp.Display.Update();
        }

        static bool CheckWin(char[,] board, char player)
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] == player && board[row, 1] == player && board[row, 2] == player)
                {
                    return true;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (board[0, col] == player && board[1, col] == player && board[2, col] == player)
                {
                    return true;
                }
            }

            // Check diagonals
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            {
                return true;
            }
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            {
                return true;
            }

            return false;
        }

        static bool CheckTie(char[,] board)
        {
            // Check if all cells are filled
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == '-')
                    {
                        // Found an empty cell, game is not tied
                        return false;
                    }
                }
            }

            // All cells are filled and no player has won, game is tied
            return true;
        }
    }
}
