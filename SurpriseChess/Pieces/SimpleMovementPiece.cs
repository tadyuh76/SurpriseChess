namespace SurpriseChess;

// Lớp cơ sở cho các quân cờ di chuyển đơn giản như quân Mã và Vua
public abstract class SimpleMovementPiece : Piece
{
    private readonly (int, int)[] moveOffsets; // Các giá trị bù cho hàng và cột

    protected SimpleMovementPiece(PieceColor color, PieceType type, (int, int)[] moveOffsets)
        : base(color, type)
    {
        this.moveOffsets = moveOffsets; // Khởi tạo các độ dịch chuyển của nước đi
    }

    // Lấy tất cả các nước đi có thể của quân cờ từ vị trí hiện tại
    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> moves = new(); // Danh sách các nước đi
        if (IsParalyzed) return moves; // Trả về danh sách rỗng nếu quân cờ bị tê liệt

        // Lặp qua tất cả các bù di chuyển
        foreach ((int dRow, int dCol) in moveOffsets)
        {
            // Tính toán vị trí mới
            Position newPosition = new(
                currentPosition.Row + dRow,
                currentPosition.Col + dCol
            );
            if (!Board.IsInsideBoard(newPosition)) continue; // Kiểm tra xem vị trí có hợp lệ không

            Piece? piece = board.GetPieceAt(newPosition); // Lấy quân cờ tại vị trí mới
            // Kiểm tra nếu ô trống hoặc có quân cờ đối phương không được bảo vệ
            if (piece == null || (piece.Color != Color && !piece.IsShielded))
            {
                moves.Add(newPosition); // Thêm nước đi vào danh sách
            }
        }

        return moves; // Trả về danh sách các nước đi
    }
}
