namespace SurpriseChess;

// Lớp trừu tượng cho các quân cờ di chuyển 'vô hạn': Tượng, Xe, Hậu
public abstract class LongRangePiece : Piece
{
    // Mảng chứa hướng di chuyển
    private readonly (int, int)[] directionOffsets;

    // Khởi tạo quân cờ với màu sắc, loại, và các hướng di chuyển
    protected LongRangePiece(
        PieceColor color,
        PieceType type,
        (int, int)[] directionOffsets
    ) : base(color, type)
    {
        this.directionOffsets = directionOffsets;
    }

    // Phương thức lấy nước đi hợp lệ của quân cờ
    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> moves = new();
        if (IsParalyzed) return moves;  // Nếu bị tê liệt, không di chuyển

        // Lặp qua các hướng di chuyển
        foreach ((int dRow, int dCol) in directionOffsets)
        {
            // Tiến từng bước trong hướng đó
            for (int step = 1; step < 8; step++)
            {
                Position newPosition = new(
                    currentPosition.Row + dRow * step,
                    currentPosition.Col + dCol * step
                );

                // Kiểm tra vị trí mới có nằm trong bàn cờ không
                if (!Board.IsInsideBoard(newPosition)) break;

                Piece? piece = board.GetPieceAt(newPosition);
                // Kiểm tra ô trống hoặc có quân địch không được bảo vệ
                if (piece == null || (piece.Color != Color && !piece.IsShielded))
                {
                    moves.Add(newPosition);  // Thêm nước đi hợp lệ
                }

                // Nếu có quân cờ, ngừng di chuyển theo hướng này
                if (piece != null) break;
            }
        }

        return moves;  // Trả về danh sách nước đi hợp lệ
    }
}
