namespace SurpriseChess;

public class Pawn : Piece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "💂" : "🐹";

    private static readonly int[] colOffsets = { -1, 1 };
    private readonly int direction;
    private readonly int startRow;

    public Pawn(PieceColor color) : base(color, PieceType.Pawn) {
        if (color == PieceColor.White)  // White pawns start at the bottom and move up
        {
            direction = -1;
            startRow = 6;
        }
        else  // Black pawns start at the top and move down
        {
            direction = 1;
            startRow = 1;
        }
    }

    public override List<Position> GetMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> moves = new();
        if (IsParalyzed) return moves;

        moves.AddRange(GetForwardMoves(board, currentPosition));
        moves.AddRange(GetDiagonalMoves(board, currentPosition, gameState));

        return moves;
    }

    private List<Position> GetForwardMoves(IBoardView board, Position currentPosition)
    {
        List<Position> forwardMoves = new();

        Position oneStepForward = new(currentPosition.Row + direction, currentPosition.Col);
        if (
            Board.IsInsideBoard(oneStepForward)
            && board.GetPieceAt(oneStepForward) == null  // Empty square
        )
        {
            forwardMoves.Add(oneStepForward);
        } else
        {
            return forwardMoves;  // Cannot move 2 steps if cannot move 1 step
        }

        Position twoStepsForward = new(
            currentPosition.Row + 2 * direction,
            currentPosition.Col
        );
        if (currentPosition.Row == startRow  // Can only move 2 steps on the first move
            && board.GetPieceAt(twoStepsForward) == null  // Empty square
        )
        {
            forwardMoves.Add(twoStepsForward);
        }
    
        return forwardMoves;
    }

    private List<Position> GetDiagonalMoves(IBoardView board, Position currentPosition, GameState gameState)
    {
        List<Position> diagonalMoves = new();
        foreach (int dCol in colOffsets)
        {
            Position diagonalSquare = new(
                currentPosition.Row + direction,
                currentPosition.Col + dCol
            );
            if (!Board.IsInsideBoard(diagonalSquare)) continue;

            // Check for an unshielded enemy piece to be captured
            Piece? pieceToCapture = board.GetPieceAt(diagonalSquare);
            if (pieceToCapture != null && pieceToCapture.Color != Color && !pieceToCapture.IsShielded)
            {
                diagonalMoves.Add(diagonalSquare);
                continue;
            }

            // Check for en passant
            if (diagonalSquare != gameState.EnPassantPosition) continue;  // En passant unavailable
            // Check for an unshielded enemy pawn to the left or right
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

        return diagonalMoves;
    }
}
