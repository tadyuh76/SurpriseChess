namespace SurpriseChess;

// Used for pieces that move 'infinitely' in different directions: Bishop, Rook, and Queen
public abstract class LongRangePiece : Piece
{
    private readonly (int, int)[] directionOffsets;

    protected LongRangePiece(
        PieceColor color,
        PieceType type,
        (int, int)[] directionOffsets
    ) : base(color, type)
    {
        this.directionOffsets = directionOffsets;
    }

    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> moves = new();
        if (IsParalyzed) return moves;

        foreach ((int dRow, int dCol) in directionOffsets)
        {
            for (int step = 1; step < 8; step++)
            {
                Position newPosition = new(
                    currentPosition.Row + dRow * step,
                    currentPosition.Col + dCol * step
                );
                if (!Board.IsInsideBoard(newPosition)) break;

                Piece? piece = board.GetPieceAt(newPosition);
                // Check if square is empty or has an unshielded opponent piece
                if (piece == null || (piece.Color != Color && !piece.IsShielded))
                {
                    moves.Add(newPosition);
                }

                // If there is a piece, the path is now blocked and the piece cannot move further in this direction
                if (piece != null) break;
            }
        }

        return moves;
    }
}