using System;
using System.Collections.Generic;

namespace SurpriseChess
{
    public class ReplayView
    {
        static string[] columnLabels = { "a", "b", "c", "d", "e", "f", "g", "h" }; // Nhãn cột cho bàn cờ
        static string[] rowLabels = { "8", "7", "6", "5", "4", "3", "2", "1" }; // Nhãn hàng cho bàn cờ
        private string actualMove; // Nước đi thực tế
        private string bestMove; // Nước đi tốt nhất

        // Phương thức hiển thị bàn cờ
        public void RenderBoard(ReplayBoard board, string actualMove, string bestMove)
        {
            Console.Clear(); // Xóa màn hình
            this.actualMove = actualMove; // Gán nước đi thực tế
            this.bestMove = bestMove; // Gán nước đi tốt nhất

            // Vẽ bàn cờ
            DrawBoardReplay(board);

            // Hiển thị tên cột dưới bàn cờ
            DisplayColumnLabels();

            // In ra mô tả từng quân cờ
            PrintDescription();

            // In ra bảng điều khiển
            DisplayNavigationOptions();
        }

        // Vẽ bàn cờ trong chế độ replay
        private void DrawBoardReplay(ReplayBoard board)
        {
            // Render bảng cờ
            for (int row = 0; row < 8; row++)
            {
                Console.Write($" {rowLabels[row]} "); // In nhãn hàng
                for (int col = 0; col < 8; col++)
                {
                    Position currentPosition = new Position(row, col); // Tạo vị trí hiện tại
                    DrawSquareReplay(board, currentPosition); // Vẽ ô hiện tại
                }
                Console.ResetColor(); // Đặt lại màu sắc
                Console.WriteLine();
            }
        }

        // Vẽ ô cụ thể trong chế độ replay
        private void DrawSquareReplay(ReplayBoard board, Position position)
        {
            string squareNotation = $"{(char)('a' + position.Col)}{8 - position.Row}"; // Tính toán ký hiệu ô
            ConsoleColor backgroundColor = GetSquareBackgroundColor(position, squareNotation); // Lấy màu nền
            ConsoleColor foregroundColor = ConsoleColor.Black; // Màu chữ

            Console.BackgroundColor = backgroundColor; // Gán màu nền
            Console.ForegroundColor = foregroundColor; // Gán màu chữ

            Piece? piece = board.GetPieceAt(position); // Lấy quân cờ tại vị trí
            string pieceSymbol = piece?.DisplaySymbol ?? "  "; // Lấy ký hiệu quân cờ hoặc khoảng trắng

            // Điều chỉnh màu chữ để dễ nhìn hơn nếu cần
            if (backgroundColor == ConsoleColor.DarkGreen || backgroundColor == ConsoleColor.DarkYellow)
            {
                Console.ForegroundColor = ConsoleColor.White; // Đặt màu chữ là trắng
            }

            Console.Write($" {pieceSymbol} "); // In quân cờ
            Console.ResetColor(); // Đặt lại màu sắc
        }

        // Kiểm tra xem ô có nằm trong nước đi hay không
        private bool IsPartOfMove(string squareNotation, string move)
        {
            return move.Contains(squareNotation); // Trả về true nếu ô nằm trong nước đi
        }

        // Lấy màu nền cho ô dựa trên vị trí và nước đi
        private ConsoleColor GetSquareBackgroundColor(Position position, string squareNotation)
        {
            if (IsPartOfMove(squareNotation, actualMove)) // Nếu ô thuộc nước đi thực tế
            {
                return ConsoleColor.DarkGreen; // Màu nền xanh đậm
            }
            else if (IsPartOfMove(squareNotation, bestMove)) // Nếu ô thuộc nước đi tốt nhất
            {
                return ConsoleColor.DarkYellow; // Màu nền vàng đậm
            }
            else
            {
                // Màu nền ô cờ
                return (position.Row + position.Col) % 2 == 0 ? ConsoleColor.Gray : ConsoleColor.DarkGray;
            }
        }

        // Hiển thị tên cột dưới bàn cờ
        static void DisplayColumnLabels()
        {
            Console.Write("    "); // Khoảng cách trước nhãn cột
            foreach (string label in columnLabels)
            {
                Console.Write($"{label}   "); // In nhãn cột
            }
            Console.WriteLine(); // Xuống dòng
        }

        // In mô tả từng loại quân cờ với biểu tượng và tên tương ứng
        private void PrintDescription()
        {
            int currentLine = 0; // Dòng hiện tại
            PrintPieceDescription("Vương quốc", ChessUtils.WhitePieceEmojis, currentLine); // Mô tả quân cờ trắng
            PrintPieceDescription("Rừng sâu", ChessUtils.BlackPieceEmojis, currentLine); // Mô tả quân cờ đen
            PrintSpecialSquareDescription(ref currentLine); // Mô tả ô đặc biệt
        }

        // In mô tả ô đặc biệt
        private void PrintSpecialSquareDescription(ref int currentLine)
        {
            int offset = 77; // Điều chỉnh vị trí in trên màn hình

            Console.SetCursorPosition(offset, currentLine++); // Đặt con trỏ tại vị trí in
            Console.WriteLine("Ô đặc biệt:");

            Console.SetCursorPosition(offset, currentLine++);
            Console.BackgroundColor = ConsoleColor.Cyan; // Đặt màu nền cho ô được bảo vệ
            Console.Write("   "); // In một khối màu để mô tả ô được bảo vệ
            Console.ResetColor();
            Console.WriteLine(": Ô được bảo vệ");

            Console.SetCursorPosition(offset, currentLine++);
            Console.BackgroundColor = ConsoleColor.DarkMagenta; // Đặt màu nền cho ô bị trói
            Console.Write("   "); // In một khối màu để mô tả ô bị trói
            Console.ResetColor();
            Console.WriteLine(": Ô bị trói");
        }

        // In mô tả từng loại quân cờ
        private void PrintPieceDescription(string pieceName, Dictionary<string, string> emojisDict, int currentLine)
        {
            int offset = pieceName == "Vương quốc" ? 40 : 60; // Xác định vị trí in
            Console.SetCursorPosition(offset, currentLine++); // Đặt con trỏ tại vị trí in
            Console.WriteLine($"{pieceName}: "); // In tên quân cờ
            foreach (var pieceDescription in emojisDict)
            {
                Console.SetCursorPosition(offset, currentLine++); // Đặt con trỏ tại vị trí in
                Console.WriteLine($"{pieceDescription.Value}: {pieceDescription.Key} "); // In biểu tượng và tên quân cờ
            }
        }

        // In các tùy chọn điều hướng
        public void DisplayNavigationOptions()
        {
            Console.SetCursorPosition(0, 10); // Đặt vị trí con trỏ
            Console.WriteLine("\nĐiều khiển:");
            Console.WriteLine("Mũi tên phải (→) - Nước đi tiếp theo"); // Chỉ dẫn điều khiển
            Console.WriteLine("Mũi tên trái (←) - Nước đi trước"); // Chỉ dẫn điều khiển
            Console.WriteLine("Backspace - Thoát replay"); // Chỉ dẫn điều khiển
        }

        // Lấy thông tin đầu vào từ người dùng
        public ConsoleKeyInfo GetUserInput()
        {
            return Console.ReadKey(true); // Đọc phím bấm mà không hiển thị
        }

        // Hiển thị thông tin nước đi
        public void DisplayMoveInfo(string actualNextMove, string bestNextMove)
        {
            Console.SetCursorPosition(0, 16); // Đặt vị trí con trỏ
            Console.Write("Bước đi thực tế tiếp theo: ");
            Console.BackgroundColor = ConsoleColor.DarkGreen; // Màu nền cho nước đi thực tế
            Console.ForegroundColor = ConsoleColor.White; // Màu chữ cho nước đi thực tế
            Console.WriteLine(actualNextMove); // In nước đi thực tế
            Console.ResetColor(); // Đặt lại màu sắc

            Console.Write("Bước đi tiếp theo tối ưu nhất (Stockfish): ");
            Console.BackgroundColor = ConsoleColor.DarkYellow; // Màu nền cho nước đi tốt nhất
            Console.ForegroundColor = ConsoleColor.Black; // Màu chữ cho nước đi tốt nhất
            Console.WriteLine(bestNextMove); // In nước đi tốt nhất
            Console.ResetColor(); // Đặt lại màu sắc
        }

        // Hiển thị lỗi
        public void DisplayError(string message)
        {
            Console.WriteLine($"Lỗi: {message}"); // In thông báo lỗi
        }
    }
}
