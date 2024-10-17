namespace SurpriseChess;

internal class ChessView
{
    static string[] columnLabels = { "a", "b", "c", "d", "e", "f", "g", "h" };
    static string[] rowLabels = { "8", "7", "6", "5", "4", "3", "2", "1" };

    // Biến lưu trữ thời gian và quân cờ bị bắt
    private static int whiteTimeRemaining = 900; // 15 phút cho Trắng
    private static int blackTimeRemaining = 900; // 15 phút cho Đen

    // Biễn lưu trữ các quân cờ đã bắt của 2 bên
    private static Dictionary<PieceColor, List<Piece>> capturedPieces = new()
        {
            { PieceColor.White, new List<Piece>() },
            { PieceColor.Black, new List<Piece>() }
        };

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

    // Hiển thị lượt chơi hiện tại (Trắng hoặc Đen)
    private void DisplayCurrentTurn(PieceColor currentPlayerColor)
    {
        Console.WriteLine();
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
        //Console.SetCursorPosition(offset, currentLine++);
        //Console.WriteLine($"{pieceName}: ");
        foreach (var pieceDescription in emojisDict)
        {
            Console.SetCursorPosition(offset, currentLine++);
            Console.WriteLine($"{pieceDescription.Value}: {pieceDescription.Key}");
        }
    }

    // Hiển thị quân cờ bị bắt và đồng hồ cho Đen ở trên
    private void DisplayBlackCapturedAndTimer()
    {

        Console.WriteLine($"Rừng sâu đã bắt: {GetCapturedPieces(PieceColor.White)}");
        Console.WriteLine($"Thời gian còn lại của Rừng sâu: {FormatTime(blackTimeRemaining)}");
        Console.WriteLine(); // Dòng trống để cách biệt với bàn cờ
    }

    // Hiển thị quân cờ bị bắt và đồng hồ cho Trắng ở dưới
    private void DisplayWhiteCapturedAndTimer()
    {

        Console.WriteLine(); // Dòng trống để cách biệt với bàn cờ
        Console.WriteLine($"Vương quốc đã bắt: {GetCapturedPieces(PieceColor.Black)}");
        Console.WriteLine($"Thời gian còn lại của Vương quốc: {FormatTime(whiteTimeRemaining)}");
    }

    // Lấy danh sách quân cờ bị bắt cho mỗi bên
    private string GetCapturedPieces(PieceColor color)
    {
        return string.Join(", ", capturedPieces[color].Select(p => p.DisplaySymbol));
    }

    // Định dạng thời gian theo phút và giây
    private string FormatTime(int seconds)
    {
        int minutes = seconds / 60;
        int remainingSeconds = seconds % 60;
        return $"{minutes:D2}:{remainingSeconds:D2}";
    }
}
