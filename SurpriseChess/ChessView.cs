using System;

namespace SurpriseChess
{
    internal class ChessView
    {
        public void DrawBoard(Board board, Position? selectedPosition, HashSet<Position> highlightedMoves, int cursorX, int cursorY)
        {
            Console.Clear();
            bool isWhite = true;
            //foreach(var item in high)

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var currentPosition = new Position(row, col);
                    Piece? piece = board.GetPieceAt(currentPosition);

                    // Determine background color for square

                    if (col == cursorX && row == cursorY)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }
                    else if (highlightedMoves.Contains(currentPosition))
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow; // Highlight legal moves
                    }
                    else if (selectedPosition == currentPosition)
                    {
                        Console.BackgroundColor = ConsoleColor.Red; // Highlight selected piece
                    }
                    else if (isWhite)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray; // White square
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray; // Black square
                    }

                    // Display the piece or an empty space
                    Console.Write($" {piece?.DisplaySymbol ?? "  "} ");

                    // Alternate square color
                    isWhite = !isWhite;
                }

                // Change color pattern on the next row
                isWhite = !isWhite;

                // Reset the background and move to the next line
                Console.ResetColor();
                Console.WriteLine();
            }

            PrintDescription();
        }

        private readonly Dictionary<string, string> blackPieceEmojis = new()
        {
            { "King", "🦁" },
            { "Queen", "🐯" },
            { "Rook", "🐻" },
            { "Bishop", "🦉" },
            { "Knight", "🐴" },
            { "Pawn", "🐹" },

        };

        private readonly Dictionary<string, string> whitePieceEmojis = new()
        {
            { "King", "🤴" },
            { "Queen", "👸" },
            { "Rook", "🏰" },
            { "Bishop", "🥷" },
            { "Knight", "🏇" },
            { "Pawn", "💂" },
        };

        public void PrintDescription()
        {
            int currentLine = 0;
            Console.SetCursorPosition(40, currentLine++);
            Console.WriteLine("White: ");
            foreach (var piece in whitePieceEmojis)
            {
                Console.SetCursorPosition(40, currentLine++);
                Console.WriteLine($"{piece.Value}: {piece.Key}");
            }
            Console.SetCursorPosition(40, currentLine++);
            Console.WriteLine("Black: ");
            foreach (var piece in blackPieceEmojis)
            {
                Console.SetCursorPosition(40, currentLine++);
                Console.WriteLine($"{piece.Value}: {piece.Key}");
            }
        }
    }
}
