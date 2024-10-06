namespace SurpriseChess;

public class Queen : LongRangePiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "👸" : "🐯";


    private static readonly (int, int)[] QueenDirectionOffsets = 
    {
        (1, 0),   // phải
        (-1, 0),  // trái
        (0, 1),   // xuống
        (0, -1),  // lên
        (1, 1),   // phải dưới
        (-1, 1),  // trái dưới
        (1, -1),  // phải trên
        (-1, -1)  // trái trên
    };

    public Queen(PieceColor color) : base(color, PieceType.Queen, QueenDirectionOffsets) { }
}
