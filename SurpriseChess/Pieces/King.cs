using System.Diagnostics;

namespace SurpriseChess;

// Lớp đại diện cho quân vua trong trò chơi 
public class King : SimpleMovementPiece
{
    // Biểu tượng hiển thị cho quân vua tùy thuộc vào màu sắc
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🤴" : "🦁";

    // Quân vua có thể di chuyển đến 8 ô xung quanh
    private static readonly (int, int)[] KingMoveOffsets = new (int, int)[]
    {
            (1, 1), (1, -1), (-1, 1), (-1, -1),
            (0, 1), (1, 0), (0, -1), (-1, 0)
    };

    // Khởi tạo quân vua với màu sắc
    public King(PieceColor color) : base(color, PieceType.King, KingMoveOffsets) { }

    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        // Lấy các nước di chuyển bình thường
        List<Position> moves = base.GetMoves(board, currentPosition, gameState);

        // Lấy các nước đi của xe khi nhập thành
        moves.AddRange(GetCastlingMoves(board, currentPosition, gameState));
        
        return moves;
    }

    // Trả về các ô của các quân xe mà quân vua có thể thành
    private List<Position> GetCastlingMoves(IBoardView board, Position currentKingPosition, GameState gameState)
    {
        List<Position> castlingMoves = new();
        if (IsParalyzed) return castlingMoves;  // Nếu quân vua không thể di chuyển

        // Duyệt qua các hướng thành
        foreach (CastleDirection direction in Enum.GetValues(typeof(CastleDirection)))
        {
            // Kiểm tra quyền thành
            if (!gameState.CanCastle[Color][direction]) continue;

            Position currentRookPosition = board.RookStartingPositions[Color][direction];
            Piece? rook = board.GetPieceAt(currentRookPosition);
            // Kiểm tra rằng quân xe vẫn ở vị trí khởi đầu
            if (rook == null || rook.Type != PieceType.Rook || rook.Color != Color) continue;
            // Kiểm tra xem có quân nào chắn đường thành không
            bool isCastlingPathClear = true;
            int[] cols = {
                currentKingPosition.Col,
                currentRookPosition.Col,
                ChessUtils.ColAfterCastling[PieceType.King][direction],
                ChessUtils.ColAfterCastling[PieceType.Rook][direction]
            };
            int leftMostCol = cols.Min();
            int rightMostCol = cols.Max();
            for (int col = leftMostCol; col <= rightMostCol; col++)
            {
                // Bỏ qua vị trí hiện tại của quân vua và quân xe
                if (col == currentKingPosition.Col || col == currentRookPosition.Col) continue;

                // Nếu có quân chắn đường thành
                if (board.GetPieceAt(new Position(currentKingPosition.Row, col)) != null)
                {
                    isCastlingPathClear = false;
                    break;
                }
            }
            if (isCastlingPathClear)
            {
                castlingMoves.Add(currentRookPosition);  // Người chơi sẽ nhấn vào quân xe để thành
            }
        }

        return castlingMoves;
    }
}
