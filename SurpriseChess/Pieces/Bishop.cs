namespace SurpriseChess;

public class Bishop : LongRangePiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🥷" : "🦉";

    private static readonly (int, int)[] BishopDirectionOffsets =
    {
        (1, 1),   // bottom-right
        (-1, 1),  // bottom-left
        (1, -1),  // top-right
        (-1, -1)  // top-left
    };

    public Bishop(PieceColor color) : base(color, PieceType.Bishop, BishopDirectionOffsets) { }
}
