using Explorer700Library;
using System;
using System.Drawing;
using System.Threading;

namespace Explorer700Demo
{
    public class Program
    {
        static Explorer700 exp = new Explorer700();
        const char PLAYER_1 = 'X';
        const char PLAYER_2 = 'O';

        static void Main(string[] args)
        {
            // Set up the screen
            var screen = exp.Display.Graphics;

            // Define the Tic Tac Toe board
            char[,] board = new char[,] {
                { '-', '-', '-' },
                { '-', '-', '-' },
                { '-', '-', '-' }
            };

            // Define the players
            
            char currentPlayer = PLAYER_1;

            int col = 0;
            int row = 0;
            // Draw the initial Tic Tac Toe board on the screen
            DrawBoard(screen, board, (col, row));

            // Play the game
            bool gameOver = false;
            while (!gameOver)
            {
                // Wait for player 1 to move
                bool moveMade = false;
                while (!moveMade)
                {
                    bool changed = false;
                    Keys keys = exp.Joystick.Keys;
                        

                    // Check for joystick movements
                    if ((keys & Keys.Left) != 0 && col > 0)
                    {
                        col--;
                        changed = true;
                    }
                    else if ((keys & Keys.Right) != 0 && col < 2)
                    {
                        col++;
                        changed = true;
                    }
                    else if ((keys & Keys.Up) != 0 && row > 0)
                    {
                        row--;
                        changed = true;
                    }
                    else if ((keys & Keys.Down) != 0 && row < 2)
                    {
                        row++;
                        changed = true;
                    }
                    else if ((keys & Keys.Center) != 0 && board[row, col] == '-')
                    {
                        // Valid move, place X on board and switch to player 2
                        board[row, col] = currentPlayer;
                        moveMade = changed = true;
                    }

                    // Update the display
                    if(changed)
                    {
                        DrawBoard(screen, board, (row, col));
                        Thread.Sleep(100);
                    }
                }

                // Check for a win or tie
                if (CheckWin(board, currentPlayer))
                {
                    DrawWinOrTie(screen, currentPlayer, true);
                    gameOver = true;
                }
                else if (CheckTie(board))
                {
                    DrawWinOrTie(screen, currentPlayer, false);
                    gameOver = true;
                }
                else
                {
                    currentPlayer = getNextPlayer(currentPlayer);
                }
            }

            // Wait for user input
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void DrawWinOrTie(Graphics screen, char currentPlayer, bool win)
        {
            String txt = win ? currentPlayer + " wins!" : "Game tied";

            screen.DrawString(txt, new Font(FontFamily.GenericSansSerif, 10), Brushes.White, 60, 20);

            exp.Display.Update();
        }

        static void DrawBoard(Graphics screen, char[,] board, (int row, int col) currentCoords)
        {
            //TODO: color cufrent position differently
            screen.Clear(Color.Black);

            for (int i = 0;i < 3;i++)
            {
                screen.DrawString(board[i, 0].ToString() + GetPosSymbol(currentCoords, (i, 0)), new Font(FontFamily.GenericSansSerif, 10), Brushes.White, 0, i * 20);
                screen.DrawString("|", new Font(FontFamily.GenericSansSerif, 10), Brushes.Blue, 10, i * 20);
                screen.DrawString(board[i, 1].ToString() + GetPosSymbol(currentCoords, (i, 1)), new Font(FontFamily.GenericSansSerif, 10), Brushes.White, 20, i * 20);
                screen.DrawString("|", new Font(FontFamily.GenericSansSerif, 10), Brushes.White, 30, i * 20);
                screen.DrawString(board[i, 2].ToString() + GetPosSymbol(currentCoords, (i, 2)), new Font(FontFamily.GenericSansSerif, 10), Brushes.White, 40, i * 20);
            }
            
            exp.Display.Update();
        }

        static String GetPosSymbol((int row, int col) currentPosition, (int row, int col) currentCoords)
        {
            if (currentPosition.col == currentCoords.col && currentPosition.row == currentCoords.row)
                return "I";
            else
                return "";
        }

        static char getNextPlayer(char currentPlayer)
        {
            return currentPlayer == PLAYER_1 ? PLAYER_2 : PLAYER_1;
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
