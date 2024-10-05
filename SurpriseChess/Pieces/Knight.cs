namespace SurpriseChess;

public class Knight : SimpleMovementPiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🏇" : "🐴";

    private static readonly (int, int)[] KnightMoveOffsets = new (int, int)[]
    {
            // The knight can move to the 8 L-shaped squares surrounding it
            (-2, 1), (-1, 2), (1, 2), (2, 1),
            (2, -1), (1, -2), (-1, -2), (-2, -1)
    };

    public Knight(PieceColor color) : base(color, PieceType.Knight, KnightMoveOffsets) { }

}