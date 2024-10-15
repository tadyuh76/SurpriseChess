using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SurpriseChess.Game; // Thêm namespace để sử dụng ChessTimer

namespace SurpriseChess
{
    internal class ChessView
    {
        static string[] columnLabels = { "a", "b", "c", "d", "e", "f", "g", "h" };
        static string[] rowLabels = { "8", "7", "6", "5", "4", "3", "2", "1" };

        // Biến lưu trữ các quân cờ đã bắt của 2 bên
        private static Dictionary<PieceColor, List<Piece>> capturedPieces = new()
        {
            { PieceColor.White, new List<Piece>() },
            { PieceColor.Black, new List<Piece>() }
        };

        private ChessTimer chessTimer;

        // Vị trí của thời gian trên console
        private (int left, int top) WhiteTimerPosition;
        private (int left, int top) BlackTimerPosition;

        // Đối tượng khóa để đảm bảo an toàn luồng
        private readonly object ConsoleLock = new object();

        public ChessView(TimeSpan initialTime)
        {
            chessTimer = new ChessTimer(initialTime);
            chessTimer.TimeUpdated += ChessTimer_TimeUpdated;
            chessTimer.TimeExpired += ChessTimer_TimeExpired;
        }

        // Phương thức hiển thị toàn bộ bàn cờ, bao gồm các quân cờ và các ô được đánh dấu
        public void Render(Board board, Position? selectedPosition, HashSet<Position> highlightedMoves, PieceColor currentPlayerColor, int cursorX, int cursorY)
        {
            Console.Clear();

            // Hiển thị bộ đếm quân cờ đã bị bắt và đồng hồ
            DisplayBlackCapturedAndTimer();

            // Vẽ bàn cờ vua
            DrawBoard(board, selectedPosition, highlightedMoves, cursorX, cursorY);

            // Hiển thị tên cột dưới bàn cờ
            DisplayColumnLabels();

            // Hiển thị bộ đếm quân cờ đã bị bắt và đồng hồ của Trắng
            DisplayWhiteCapturedAndTimer();

            // Hiển thị lượt chơi hiện tại
            DisplayCurrentTurn(currentPlayerColor);

            // In mô tả cho các quân cờ
            PrintDescription();

            // Bắt đầu đồng hồ cho người chơi hiện tại
            StartTimerForCurrentPlayer(currentPlayerColor);
        }

        private void DrawBoard(Board board, Position? selectedPosition, HashSet<Position> highlightedMoves, int cursorX, int cursorY)
        {
            // Vòng lặp để vẽ từng hàng của bàn cờ
            for (int row = 0; row < 8; row++)
            {
                Console.Write($" {rowLabels[row]} ");
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
        }

        // Hiển thị tên cột dưới bàn cờ
        static void DisplayColumnLabels()
        {
            Console.Write("    ");
            foreach (string label in columnLabels)
            {
                Console.Write($" {label}  ");
            }
            Console.WriteLine();
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
            bool isCursor = (position.Col == cursorX && position.Row == cursorY);
            bool isHighlighted = highlightedMoves.Contains(position);
            bool isSelected = (selectedPosition == position);

            if (isCursor)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            else if (isHighlighted)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            else if (isSelected)
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else if ((position.Row + position.Col) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
        }

        // Hiển thị lượt chơi hiện tại (Vương quốc hoặc Rừng sâu)
        private void DisplayCurrentTurn(PieceColor currentPlayerColor)
        {
            Console.SetCursorPosition(40, 3);
            Console.WriteLine(currentPlayerColor == PieceColor.White ? "Lượt chơi của Vương quốc" : "Lượt chơi của Rừng sâu");
        }

        // In mô tả cho các quân cờ với biểu tượng tương ứng
        private void PrintDescription()
        {
            int currentLine = 5;
            PrintPieceDescription("Vương quốc", ChessUtils.WhitePieceEmojis, currentLine);
            PrintPieceDescription("Rừng sâu", ChessUtils.BlackPieceEmojis, currentLine);
        }

        // In mô tả từng loại quân cờ với biểu tượng và tên tương ứng
        private void PrintPieceDescription(string pieceName, Dictionary<string, string> emojisDict, int currentLine)
        {
            int offset = pieceName == "Vương quốc" ? 40 : 60;
            foreach (var pieceDescription in emojisDict)
            {
                Console.SetCursorPosition(offset, currentLine++);
                Console.WriteLine($"{pieceDescription.Value}: {pieceDescription.Key}");
            }
        }

        // Hiển thị quân cờ bị bắt và đồng hồ cho Rừng sâu ở trên
        private void DisplayBlackCapturedAndTimer()
        {
            lock (ConsoleLock)
            {
                Console.WriteLine($"Rừng sâu đã bắt: {GetCapturedPieces(PieceColor.White)}");

                // Lưu vị trí của đồng hồ
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                BlackTimerPosition = (left + 25, top);

                Console.Write($"Thời gian còn lại của Rừng sâu: ");
                Console.WriteLine(FormatTime(chessTimer.GetBlackTime()));
                Console.WriteLine(); // Dòng trống để cách biệt với bàn cờ
            }
        }

        // Hiển thị quân cờ bị bắt và đồng hồ cho Vương quốc ở dưới
        private void DisplayWhiteCapturedAndTimer()
        {
            lock (ConsoleLock)
            {
                Console.WriteLine(); // Dòng trống để cách biệt với bàn cờ
                Console.WriteLine($"Vương quốc đã bắt: {GetCapturedPieces(PieceColor.Black)}");

                // Lưu vị trí của đồng hồ
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                WhiteTimerPosition = (left + 25, top);

                Console.Write($"Thời gian còn lại của Vương quốc: ");
                Console.WriteLine(FormatTime(chessTimer.GetWhiteTime()));
            }
        }

        // Lấy danh sách quân cờ bị bắt cho mỗi bên
        private string GetCapturedPieces(PieceColor color)
        {
            return string.Join(", ", capturedPieces[color].Select(p => p.DisplaySymbol));
        }

        // Định dạng thời gian theo phút và giây
        private string FormatTime(TimeSpan time)
        {
            return time.ToString(@"mm\:ss");
        }

        // Hàm xử lý sự kiện TimeUpdated
        private void ChessTimer_TimeUpdated(int playerNumber, TimeSpan remainingTime)
        {
            string timeString = FormatTime(remainingTime);

            if (playerNumber == 1)
            {
                UpdateTimerDisplay(WhiteTimerPosition.left, WhiteTimerPosition.top, timeString);
            }
            else
            {
                UpdateTimerDisplay(BlackTimerPosition.left, BlackTimerPosition.top, timeString);
            }
        }

        // Hàm cập nhật hiển thị thời gian
        private void UpdateTimerDisplay(int left, int top, string timeString)
        {
            lock (ConsoleLock)
            {
                Console.SetCursorPosition(left, top);
                Console.Write(timeString + "  "); // Xóa các ký tự dư thừa
            }
        }

        // Hàm xử lý sự kiện TimeExpired
        private void ChessTimer_TimeExpired(int playerNumber)
        {
            lock (ConsoleLock)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 2);
                Console.WriteLine($"\nThời gian của {(playerNumber == 1 ? "Vương quốc" : "Rừng sâu")} đã hết!");
                Console.WriteLine("Nhấn phím bất kỳ để thoát...");
            }

            // Dừng chương trình hoặc thực hiện hành động kết thúc trò chơi
            Console.ReadKey();
            Environment.Exit(0);
        }

        // Bắt đầu đồng hồ cho người chơi hiện tại
        private void StartTimerForCurrentPlayer(PieceColor currentPlayerColor)
        {
            int playerNumber = currentPlayerColor == PieceColor.White ? 1 : 2;
            chessTimer.StartTimer(playerNumber);
        }

        // Gọi hàm này khi người chơi hoàn thành lượt đi
        public void OnPlayerMoveCompleted(PieceColor currentPlayerColor)
        {
            // Chuyển sang người chơi tiếp theo
            PieceColor nextPlayerColor = currentPlayerColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
            StartTimerForCurrentPlayer(nextPlayerColor);
        }

        // Thêm các phương thức khác nếu cần thiết
    }
}
