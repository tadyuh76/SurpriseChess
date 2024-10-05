namespace SurpriseChess;

public class Rook : LongRangePiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🏰" : "🐻";


    private static readonly (int, int)[] RookDirectionOffsets = 
    {
        (1, 0),   // right
        (-1, 0),  // left
        (0, 1),   // down
        (0, -1)   // up
    };

    public Rook(PieceColor color) : base(color, PieceType.Rook, RookDirectionOffsets) { }
}
