namespace SurpriseChess;

// Lớp đại diện cho quân cờ tốt trong trò chơi
public class Pawn : Piece
{
    // Ký hiệu hiển thị cho quân tốt (tùy theo màu sắc)
    public override string DisplaySymbol => this.Color == PieceColor.White ? "💂" : "🐹";

    // Các giá trị thay đổi cột cho nước đi chéo
    private static readonly int[] colOffsets = { -1, 1 };
    // Hướng di chuyển của quân tốt (1 cho đi xuống, -1 cho đi lên)
    private readonly int direction;
    // Hàng bắt đầu của quân tốt
    private readonly int startRow;

    // Khởi tạo quân tốt với màu sắc
    public Pawn(PieceColor color) : base(color, PieceType.Pawn)
    {
        if (color == PieceColor.White)  // Tốt trắng bắt đầu ở dưới và di chuyển lên
        {
            direction = -1;
            startRow = 6;
        }
        else  // Tốt đen bắt đầu ở trên và di chuyển xuống
        {
            direction = 1;
            startRow = 1;
        }
    }

    // Lấy tất cả các nước di chuyển có thể của quân tốt
    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> moves = new();
        if (IsParalyzed) return moves;  // Nếu quân tốt bị tê liệt, không có nước đi nào

        // Lấy các nước đi thẳng
        moves.AddRange(GetForwardMoves(board, currentPosition));
        // Lấy các nước đi chéo
        moves.AddRange(GetDiagonalMoves(board, currentPosition, gameState));

        return moves;
    }

    // Lấy các nước đi thẳng của quân tốt
    private List<Position> GetForwardMoves(IBoardView board, Position currentPosition)
    {
        List<Position> forwardMoves = new();

        // Nước đi 1 ô về phía trước
        Position oneStepForward = new(currentPosition.Row + direction, currentPosition.Col);
        if (
            Board.IsInsideBoard(oneStepForward)
            && board.GetPieceAt(oneStepForward) == null  // Ô trống
        )
        {
            forwardMoves.Add(oneStepForward);
        }
        else
        {
            return forwardMoves;  // Không thể đi 2 bước nếu không thể đi 1 bước
        }

        // Nước đi 2 ô về phía trước (chỉ trong lần di chuyển đầu tiên)
        Position twoStepsForward = new(
            currentPosition.Row + 2 * direction,
            currentPosition.Col
        );
        if (currentPosition.Row == startRow  // Chỉ có thể đi 2 bước trong lần di chuyển đầu tiên
            && board.GetPieceAt(twoStepsForward) == null  // Ô trống
        )
        {
            forwardMoves.Add(twoStepsForward);
        }

        return forwardMoves;
    }

    // Lấy các nước đi chéo của quân tốt
    private List<Position> GetDiagonalMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> diagonalMoves = new();
        foreach (int dCol in colOffsets)
        {
            Position diagonalSquare = new(
                currentPosition.Row + direction,
                currentPosition.Col + dCol
            );
            if (!Board.IsInsideBoard(diagonalSquare)) continue;  // Nếu ô không nằm trong bàn cờ

            // Kiểm tra xem có quân địch không được bảo vệ để ăn
            Piece? pieceToCapture = board.GetPieceAt(diagonalSquare);
            if (pieceToCapture != null && pieceToCapture.Color != Color && !pieceToCapture.IsShielded)
            {
                diagonalMoves.Add(diagonalSquare);
                continue;
            }

            // Kiểm tra nước đi en passant
            if (diagonalSquare != gameState.EnPassantPosition) continue;  // En passant không khả dụng
            // Kiểm tra xem có quân tốt địch không được bảo vệ ở ô bên trái hoặc bên phải
            Position adjacentSquare = new(currentPosition.Row, currentPosition.Col + dCol);
            Piece? adjacentPiece = board.GetPieceAt(adjacentSquare);
            if (
                adjacentPiece != null
                && adjacentPiece.Type == PieceType.Pawn
                && adjacentPiece.Color != Color
                && !adjacentPiece.IsShielded
            )
            {
                diagonalMoves.Add(diagonalSquare);
            }
        }

        return diagonalMoves;  // Trả về các nước đi chéo
    }
}
