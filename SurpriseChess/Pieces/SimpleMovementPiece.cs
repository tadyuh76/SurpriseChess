namespace SurpriseChess;

// Used for Knight and King
public abstract class SimpleMovementPiece : Piece
{
    private readonly (int, int)[] moveOffsets;

    protected SimpleMovementPiece(PieceColor color, PieceType type, (int, int)[] moveOffsets)
        : base(color, type)
    {
        this.moveOffsets = moveOffsets;
    }

    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> moves = new();
        if (IsParalyzed) return moves;

        foreach ((int dRow, int dCol) in moveOffsets)
        {
            Position newPosition = new(
                currentPosition.Row + dRow,
                currentPosition.Col + dCol
            );
            if (!Board.IsInsideBoard(newPosition)) continue;

            Piece? piece = board.GetPieceAt(newPosition);
            // Check if square is empty or has an unshielded enemy piece
            if (piece == null || (piece.Color != Color && !piece.IsShielded))
            {
                moves.Add(newPosition);
            }
        }

        return moves;
    }
}
