using System;
using System.Collections.Generic;

namespace SurpriseChess
{
    public class ReplayView
    {
        public void RenderBoard(ReplayBoard board)
        {
            Console.Clear();

            // Render bảng
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Position currentPosition = new Position(row, col);
                    DrawSquare(board, currentPosition);
                }
                Console.ResetColor();
                Console.WriteLine();
            }

            // In ra miêu tả từng quân cở
            PrintDescription();
        }

        private void DrawSquare(ReplayBoard board, Position position)
        {
            SetSquareBackgroundColor(position);

            Piece? piece = board.GetPieceAt(position);
            Console.Write($" {piece?.DisplaySymbol ?? "  "} ");
        }

        private void SetSquareBackgroundColor(Position position)
        {
            if ((position.Row + position.Col) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
        }

        private void PrintDescription()
        {
            int currentLine = 0;
            PrintPieceDescription("White", ChessUtils.WhitePieceEmojis, ref currentLine);
            PrintPieceDescription("Black", ChessUtils.BlackPieceEmojis, ref currentLine);
        }

        private void PrintPieceDescription(string pieceName, Dictionary<string, string> emojisDict, ref int currentLine)
        {
            Console.SetCursorPosition(40, currentLine++);
            Console.WriteLine($"{pieceName}: ");
            foreach (var pieceDescription in emojisDict)
            {
                Console.SetCursorPosition(40, currentLine++);
                Console.WriteLine($"{pieceDescription.Value}: {pieceDescription.Key}");
            }
        }

        public void DisplayNavigationOptions()
        {
            Console.WriteLine("\nĐiều khiển:");
            Console.WriteLine("Mũi tên phải (→) - Nước đi tiếp theo");
            Console.WriteLine("Mũi tên trái (←) - Nước đi trước");
            Console.WriteLine("Backspace - Thoát replay");
        }

        public ConsoleKeyInfo GetUserInput()
        {
            return Console.ReadKey(true);
        }
    }
}