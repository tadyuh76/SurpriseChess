namespace SurpriseChess;

public class King : SimpleMovementPiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🤴" : "🦁";

    private static readonly (int, int)[] KingMoveOffsets = new (int, int)[]
    {
            // The king can move to the 8 squares surrounding it
            (1, 1), (1, -1), (-1, 1), (-1, -1),
            (0, 1), (1, 0), (0, -1), (-1, 0)
    };
    
    public King(PieceColor color) : base(color, PieceType.King, KingMoveOffsets) { }

    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        // Get regular moves
        List<Position> moves = base.GetMoves(board, currentPosition, gameState);

        // Get castling moves
        moves.AddRange(GetCastlingMoves(board, currentPosition, gameState));

        return moves;
    }

    // Returns the squares of the rooks where the king can castle
    private List<Position> GetCastlingMoves(IBoardView board, Position currentKingPosition, GameState gameState)
    {
        List<Position> castlingMoves = new();
        if (IsParalyzed) return castlingMoves;

        foreach (CastleDirection direction in Enum.GetValues(typeof(CastleDirection)))
        {
            if (!gameState.CanCastle[Color][direction]) continue;

            Position currentRookPosition = board.RookStartingPositions[Color][direction];
            Piece? rook = board.GetPieceAt(currentRookPosition);
            // Check that the rook is still in its starting position
            if (rook == null || rook.Type != PieceType.Rook || rook.Color != Color) continue;

            // Check that there are no pieces blocking the castling path
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
                // Ignore the current positions of the king and the rook
                if (col == currentKingPosition.Col || col == currentRookPosition.Col) continue;

                if (board.GetPieceAt(new Position(currentKingPosition.Row, col)) != null)
                {
                    isCastlingPathClear = false;
                    break;
                }
            }
            if (isCastlingPathClear)
            {
                castlingMoves.Add(currentRookPosition);  // Player will click on the rook to castle
            }
        }

        return castlingMoves;
    }
}