using System;
using System.Drawing;

namespace SurpriseChess
{
    internal class ChessView
    {
        // Phương thức hiển thị toàn bộ bàn cờ, bao gồm các quân cờ và các ô được đánh dấu
        public void DrawBoard(Board board, Position? selectedPosition, HashSet<Position> highlightedMoves, PieceColor currentPlayerColor, int cursorX, int cursorY)
        {
            Console.Clear();

            // Vòng lặp để vẽ từng hàng của bàn cờ
            for (int row = 0; row < 8; row++)
            {
                // Vòng lặp để vẽ từng cột của bàn cờ
                for (int col = 0; col < 8; col++)
                {
                    Position currentPosition = new Position(row, col);
                    // Vẽ từng ô cờ dựa trên trạng thái của nó
                    DrawSquare(board, currentPosition, selectedPosition, highlightedMoves, cursorX, cursorY);
                }

                Console.ResetColor();
                Console.WriteLine();
            }

            // Hiển thị lượt chơi hiện tại
            DisplayCurrentTurn(currentPlayerColor);
            // In mô tả cho các quân cờ
            PrintDescription();
        }

        // Phương thức để vẽ một ô cờ cụ thể
        private void DrawSquare(Board board, Position position, Position? selectedPosition, HashSet<Position> highlightedMoves, int cursorX, int cursorY)
        {
            // Thiết lập màu nền cho ô cờ dựa trên trạng thái của nó
            SetSquareBackgroundColor(position, selectedPosition, highlightedMoves, cursorX, cursorY);

            // Hiển thị quân cờ hoặc khoảng trống nếu không có quân cờ
            Piece? piece = board.GetPieceAt(position);
            Console.Write($" {piece?.DisplaySymbol ?? "  "} ");
        }

        // Phương thức hỗ trợ để thiết lập màu nền của ô cờ
        private void SetSquareBackgroundColor(Position position, Position? selectedPosition, HashSet<Position> highlightedMoves, int cursorX, int cursorY)
        {
            // Kiểm tra xem con trỏ hiện tại có nằm trên ô này không
            bool isCursor = (position.Col == cursorX && position.Row == cursorY);
            // Kiểm tra xem ô này có phải là một nước đi hợp lệ được đánh dấu không
            bool isHighlighted = highlightedMoves.Contains(position);
            // Kiểm tra xem ô này có phải là ô đang được chọn không
            bool isSelected = (selectedPosition == position);

            if (isCursor)
            {
                // Nếu là con trỏ, tô nền màu xanh đậm
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            else if (isHighlighted)
            {
                // Nếu là ô được đánh dấu, tô nền màu vàng
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            else if (isSelected)
            {
                // Nếu là ô đang được chọn, tô nền màu đỏ
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else if ((position.Row + position.Col) % 2 == 0)
            {
                // Nếu là ô sáng, tô nền màu xám
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else
            {
                // Nếu là ô tối, tô nền màu xám đậm
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
        }

        // Hiển thị lượt chơi hiện tại (trắng hoặc đen)
        private void DisplayCurrentTurn(PieceColor currentPlayerColor)
        {
            Console.WriteLine();
            Console.WriteLine(currentPlayerColor == PieceColor.White ? "Lượt chơi của Vương quốc" : "Lượt chơi của Rừng sâu");
        }

        // In mô tả cho các quân cờ với biểu tượng tương ứng
        private void PrintDescription()
        {
            int currentLine = 0;
            PrintPieceDescription("Vương quốc", ChessUtils.WhitePieceEmojis, ref currentLine);
            PrintPieceDescription("Rừng sâu", ChessUtils.BlackPieceEmojis, ref currentLine);
        }

        // In mô tả từng loại quân cờ với biểu tượng và tên tương ứng
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
    }
}
