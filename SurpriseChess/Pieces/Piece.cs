namespace SurpriseChess;

public abstract class Piece : IPrototype<Piece>
{
    public PieceColor Color { get; }
    public PieceType Type { get; }
    public bool IsInvisible { get; set; } = false;
    public bool IsParalyzed { get; set; } = false;
    public bool IsShielded { get; set; } = false;
    public abstract string DisplaySymbol { get; }

    protected Piece(PieceColor color, PieceType type)
    {
        Color = color;
        Type = type;
    }

    // Gets all the possible moves of a piece from a position given the board state
    // Does not take into account king safety 
    public abstract List<Position> GetMoves(
        IBoardView board, Position currentPosition, GameState gameState
    );

    // Creates a deep copy of the piece
    public Piece Clone()
    {
        Piece newPiece = PieceFactory.Create(Color, Type);
        newPiece.IsInvisible = IsInvisible;
        newPiece.IsParalyzed = IsParalyzed;
        newPiece.IsShielded = IsShielded;

        return newPiece;
    }
}