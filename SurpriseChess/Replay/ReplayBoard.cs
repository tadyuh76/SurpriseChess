namespace SurpriseChess;

public class ReplayBoard
{
    // Khai báo bảng cờ 8x8 chứa quân cờ, có thể null
    private readonly Piece?[,] board = new Piece?[8, 8]; // Bảng cờ 8x8

    // Hàm khởi tạo ReplayBoard với FEN
    public ReplayBoard(string fen)
    {
        FEN.LoadPositionFromFEN(fen, this); // Tải vị trí từ chuỗi FEN
    }

    // Phương thức để thiết lập quân cờ tại vị trí cụ thể
    public void SetPieceAt(Position position, Piece piece)
    {
        board[position.Row, position.Col] = piece; // Thiết lập quân cờ tại vị trí
    }

    // Phương thức để lấy quân cờ tại vị trí cụ thể
    public Piece? GetPieceAt(Position position) =>
        // Kiểm tra vị trí có nằm trong bảng cờ không
        position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8
            ? board[position.Row, position.Col] // Trả về quân cờ tại vị trí nếu hợp lệ
            : null; // Trả về null nếu vị trí không hợp lệ
}
