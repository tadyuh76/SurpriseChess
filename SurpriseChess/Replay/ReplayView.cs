using System;
using System.Collections.Generic;

namespace SurpriseChess
{
    public class ReplayView
    {
        static string[] columnLabels = { "a", "b", "c", "d", "e", "f", "g", "h" };
        static string[] rowLabels = { "8", "7", "6", "5", "4", "3", "2", "1" };
        private string actualMove;
        private string bestMove;

        public void RenderBoard(ReplayBoard board, string actualMove, string bestMove)
        {
            Console.Clear();
            this.actualMove = actualMove;
            this.bestMove = bestMove;

            //vẽ bàn cờ
            DrawBoardReplay(board);

            //// Hiển thị tên cột dưới bàn cờ
            DisplayColumnLabels();

            // In ra miêu tả từng quân cở
            PrintDescription();
            
            //In ra bảng điều khiển
            DisplayNavigationOptions();

        }


        private void DrawBoardReplay(ReplayBoard board)
        {
            // Render bảng
            for (int row = 0; row < 8; row++)
            {
                Console.Write($"{rowLabels[row]}");
                for (int col = 0; col < 8; col++)
                {
                    Position currentPosition = new Position(row, col);
                    DrawSquareReplay(board, currentPosition);
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private void DrawSquareReplay(ReplayBoard board, Position position)
        {
            string squareNotation = $"{(char)('a' + position.Col)}{8 - position.Row}";
            ConsoleColor backgroundColor = GetSquareBackgroundColor(position, squareNotation);
            ConsoleColor foregroundColor = ConsoleColor.Black;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;

            Piece? piece = board.GetPieceAt(position);
            string pieceSymbol = piece?.DisplaySymbol ?? "  ";

            // Adjust foreground color for better visibility if needed
            if (backgroundColor == ConsoleColor.DarkGreen || backgroundColor == ConsoleColor.DarkYellow)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write($" {pieceSymbol} ");
            Console.ResetColor();
        }

        private bool IsPartOfMove(string squareNotation, string move)
        {
            return move.Contains(squareNotation);
        }

        private ConsoleColor GetSquareBackgroundColor(Position position, string squareNotation)
        {
            if (IsPartOfMove(squareNotation, actualMove))
            {
                return ConsoleColor.DarkGreen;
            }
            else if (IsPartOfMove(squareNotation, bestMove))
            {
                return ConsoleColor.DarkYellow;
            }
            else
            {
                return (position.Row + position.Col) % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray;
            }
        }

        // Hiển thị tên cột dưới bàn cờ
        static void DisplayColumnLabels()
        {
            Console.Write("   ");
            foreach (string label in columnLabels)
            {
                Console.Write($"{label}   ");
            }
            Console.WriteLine();
        }

        private void PrintDescription()
        {
            int currentLine = 0;
            PrintPieceDescription("Trắng", ChessUtils.WhitePieceEmojis, ref currentLine);
            PrintPieceDescription("Đen", ChessUtils.BlackPieceEmojis, ref currentLine);
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
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("\nĐiều khiển:");
            Console.WriteLine("Mũi tên phải (→) - Nước đi tiếp theo");
            Console.WriteLine("Mũi tên trái (←) - Nước đi trước");
            Console.WriteLine("Backspace - Thoát replay");
        }

        public ConsoleKeyInfo GetUserInput()
        {
            return Console.ReadKey(true);
        }

        public void DisplayMoveInfo(string actualNextMove, string bestNextMove)
        {
            Console.SetCursorPosition(0, 16);
            Console.Write("Bước đi thực tế tiếp theo: ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(actualNextMove);
            Console.ResetColor();

            Console.Write("Bước đi tiếp theo tối ưu nhất (Stockfish): ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(bestNextMove);
            Console.ResetColor();
        }


        public void DisplayError(string message)
        {
            Console.WriteLine($"Lỗi: {message}");
        }
    }
}