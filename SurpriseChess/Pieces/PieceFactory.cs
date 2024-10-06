namespace SurpriseChess;

public static class PieceFactory
{
    // Tạo quân cờ tương ứng dựa trên màu sắc và loại
    public static Piece Create(PieceColor color, PieceType type)
    {
        return type switch
        {
            PieceType.King => new King(color),
            PieceType.Queen => new Queen(color),
            PieceType.Rook => new Rook(color),
            PieceType.Knight => new Knight(color),
            PieceType.Bishop => new Bishop(color),
            PieceType.Pawn => new Pawn(color),
            _ => throw new ArgumentException("Invalid piece type")  // Thông báo lỗi nếu loại quân cờ không hợp lệ
        };
    }
}
