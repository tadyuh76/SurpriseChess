namespace SurpriseChess;

public class Rook : LongRangePiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🏰" : "🐻";


    private static readonly (int, int)[] RookDirectionOffsets = 
    {
        (1, 0),   // phải
        (-1, 0),  // trái
        (0, 1),   // xuống
        (0, -1)   // lên
    };

    public Rook(PieceColor color) : base(color, PieceType.Rook, RookDirectionOffsets) { }
}
