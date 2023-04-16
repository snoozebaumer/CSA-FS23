using Explorer700Library;
using System;
using System.Drawing;
using System.Threading;

namespace TicTacToeExplorer700
{
    public class Program
    {
        static Explorer700 exp = new Explorer700();
        const char PLAYER_1 = 'X';
        const char PLAYER_2 = 'O';

        static void Main(string[] args)
        {
            while (true)
            {
                playGame();
            }
        }

        private static void playGame()
        {
            var screen = exp.Display.Graphics;

            char[,] board = new char[,] {
                { '-', '-', '-' },
                { '-', '-', '-' },
                { '-', '-', '-' }
            };

            char currentPlayer = PLAYER_1;

            // start point definition, in this case top left
            int col = 0;
            int row = 0;

            DrawBoard(screen, board, (col, row));


            bool gameOver = false;
            while (!gameOver)
            {
                // Wait for player to move
                bool moveMade = false;
                while (!moveMade)
                {
                    moveMade = MakeMove(ref row, ref col, board, currentPlayer, screen, exp);
                }

                // Check for a win or tie
                if (CheckWin(board, currentPlayer))
                {
                    DrawWinOrTie(screen, currentPlayer, true);
                    gameOver = true;

                    //try to prevent starting new game through previous input
                    Thread.Sleep(200);

                }
                else if (CheckTie(board))
                {
                    DrawWinOrTie(screen, currentPlayer, false);

                    //try to prevent starting new game through previous input
                    gameOver = true;
                }
                else
                {
                    currentPlayer = getNextPlayer(currentPlayer);
                }
            }

            // wait for Joystick Keys input
            Keys keys = exp.Joystick.Keys;
            while (keys == Keys.NoKey)
            {
                keys = exp.Joystick.Keys;
            }
            
        }

        private static bool MakeMove(ref int row,ref int col, char[,] board, char currentPlayer, Graphics screen, Explorer700 exp)
        {
            bool moveMade = false;
            bool changed = false;
            Keys keys = exp.Joystick.Keys;

            switch (keys)
            {
                case Keys.Left when col > 0:
                    col--;
                    changed = true;
                    break;
                case Keys.Right when col < 2:
                    col++;
                    changed = true;
                    break;
                case Keys.Up when row > 0:
                    row--;
                    changed = true;
                    break;
                case Keys.Down when row < 2:
                    row++;
                    changed = true;
                    break;
                case Keys.Center when CheckForValidMove(row, col, board, currentPlayer):
                    // Valid move, place sign on board and switch to player
                    board[row, col] = currentPlayer;
                    changed = moveMade = true;
                    break;
            }

            // Update the display
            if (changed)
            {
                DrawBoard(screen, board, (row, col));

                //try to prevent double input
                Thread.Sleep(200);
            }

            return moveMade;
        }

        private static bool CheckForValidMove(int row, int col, char[,] board, char currentPlayer)
        {
            return board[row, col] == '-';
        }


        private static void DrawWinOrTie(Graphics screen, char currentPlayer, bool win)
        {
            String txt = win ? currentPlayer + " wins!" : "Game tied";

            screen.DrawString(txt, new Font(FontFamily.GenericSansSerif, 9), Brushes.White, 60, 20);

            exp.Display.Update();
        }

        private static void DrawBoard(Graphics screen, char[,] board, (int row, int col) currentCoords)
        {
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

        private static String GetPosSymbol((int row, int col) currentPosition, (int row, int col) currentCoords)
        {
            if (currentPosition.col == currentCoords.col && currentPosition.row == currentCoords.row)
                return "I";
            else
                return "";
        }

        private static char getNextPlayer(char currentPlayer)
        {
            return currentPlayer == PLAYER_1 ? PLAYER_2 : PLAYER_1;
        }

        private static bool CheckWin(char[,] board, char player)
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

        private static bool CheckTie(char[,] board)
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

            return true;
        }
    }
}
