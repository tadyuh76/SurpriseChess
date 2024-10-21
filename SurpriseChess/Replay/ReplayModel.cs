namespace SurpriseChess;

public class ReplayModel
{
    public List<string> FENList { get; } // Danh sách các chuỗi FEN
    public int CurrentMoveIndex { get; private set; } = 1; // Chỉ số di chuyển hiện tại
    public ReplayBoard CurrentBoard { get; private set; } = null!; // Bảng cờ hiện tại

    // Hàm khởi tạo ReplayModel với danh sách FEN
    public ReplayModel(List<string> fenList)
    {
        FENList = fenList; // Gán danh sách FEN
        UpdateCurrentBoard(); // Cập nhật bảng cờ hiện tại
    }

    // Phương thức để di chuyển tới bước tiếp theo
    public void MoveNext()
    {
        // Kiểm tra nếu có bước tiếp theo
        if (CurrentMoveIndex < FENList.Count - 1)
        {
            CurrentMoveIndex++; // Tăng chỉ số di chuyển
            UpdateCurrentBoard(); // Cập nhật bảng cờ
        }
    }

    // Phương thức để di chuyển về bước trước
    public void MovePrevious()
    {
        // Kiểm tra nếu có bước trước
        if (CurrentMoveIndex > 1)
        {
            CurrentMoveIndex--; // Giảm chỉ số di chuyển
            UpdateCurrentBoard(); // Cập nhật bảng cờ
        }
    }

    // Phương thức cập nhật bảng cờ hiện tại
    private void UpdateCurrentBoard()
    {
        CurrentBoard = new ReplayBoard(FENList[CurrentMoveIndex]); // Tạo bảng cờ mới từ chuỗi FEN hiện tại
    }

    // Phương thức để lấy chuỗi FEN hiện tại
    public string GetCurrentFEN() => FENList[CurrentMoveIndex];

    // Phương thức xác định nước đi thực tế tiếp theo
    public string DetermineActualNextMove()
    {
        // Kiểm tra nếu có nước đi tiếp theo
        if (CurrentMoveIndex < FENList.Count - 1)
        {
            string currentFEN = FENList[CurrentMoveIndex]; // FEN hiện tại
            string nextFEN = FENList[CurrentMoveIndex + 1]; // FEN tiếp theo
            return CalculateMove(currentFEN, nextFEN); // Tính toán nước đi
        }
        return "None"; // Trả về "None" nếu không có nước đi
    }

    // Phương thức tính toán nước đi giữa hai chuỗi FEN
    private string CalculateMove(string currentFEN, string nextFEN)
    {
        string[] currentParts = currentFEN.Split(' '); // Phân tách FEN hiện tại
        string[] nextParts = nextFEN.Split(' '); // Phân tách FEN tiếp theo
        string[] currentRows = currentParts[0].Split('/'); // Tách hàng trong FEN hiện tại
        string[] nextRows = nextParts[0].Split('/'); // Tách hàng trong FEN tiếp theo

        string fromSquare = ""; // Ô khởi đầu của nước đi
        string toSquare = ""; // Ô kết thúc của nước đi

        // Vòng lặp qua từng hàng và cột để tìm sự khác biệt
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                char? currentPiece = GetPieceAt(currentRows, row, col); // Lấy quân cờ hiện tại
                char? nextPiece = GetPieceAt(nextRows, row, col); // Lấy quân cờ tiếp theo

                // So sánh quân cờ giữa FEN hiện tại và FEN tiếp theo
                if (currentPiece != nextPiece)
                {
                    string square = $"{(char)('a' + col)}{8 - row}"; // Tính toán tên ô (ví dụ: a1, b2)

                    // Nếu ô hiện tại không còn quân (vị trí đích)
                    if (currentPiece.HasValue && !nextPiece.HasValue)
                    {
                        fromSquare = square; // Ghi nhận ô khởi đầu
                    }
                    // Nếu ô tương lai có quân mới (vị trí di chuyển)
                    // Hoặc cả 2 ô đều có quân nhưng khác giá trị (vị trí có quân bị bắt)
                    else if (
                        (!currentPiece.HasValue && nextPiece.HasValue) ||
                        (currentPiece.HasValue && nextPiece.HasValue)
                    )
                    {
                        toSquare = square; // Ghi nhận ô kết thúc
                    }

                    // Chỉ trả về giá trị khi đã xác định đủ vị trí đích và vị trí khởi đầu
                    if (!string.IsNullOrEmpty(fromSquare) && !string.IsNullOrEmpty(toSquare))
                    {
                        return $"{fromSquare}{toSquare}"; // Trả về nước đi
                    }
                }
            }
        }

        return "None"; // Trả về "None" nếu không tìm thấy nước đi
    }

    // Phương thức lấy quân cờ tại vị trí hàng và cột cụ thể
    private char? GetPieceAt(string[] rows, int row, int col)
    {
        int currentCol = 0; // Khởi tạo cột hiện tại
        foreach (char c in rows[row])
        {
            if (char.IsDigit(c)) // Nếu ký tự là số, tăng cột hiện tại
            {
                currentCol += int.Parse(c.ToString());
            }
            else
            {
                if (currentCol == col) return c; // Nếu đã tới cột cần tìm, trả về quân cờ
                currentCol++; // Tăng cột hiện tại
            }
            if (currentCol > col) break; // Nếu đã vượt quá cột cần tìm, dừng lại
        }
        return null; // Trả về null nếu không tìm thấy quân cờ
    }

    // Phương thức chuyển đổi nước đi thành chuỗi
    public string ConvertMoveToString((Position, Position) move)
    {
        return $"{FEN.PositionToFEN(move.Item1)}{FEN.PositionToFEN(move.Item2)}"; // Trả về chuỗi nước đi
    }
}
