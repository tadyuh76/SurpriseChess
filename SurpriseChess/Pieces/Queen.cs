namespace SurpriseChess;

public class Queen : LongRangePiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "👸" : "🐯";


    private static readonly (int, int)[] QueenDirectionOffsets = 
    {
        (1, 0),   // right
        (-1, 0),  // left
        (0, 1),   // down
        (0, -1),  // up
        (1, 1),   // bottom-right
        (-1, 1),  // bottom-left
        (1, -1),  // top-right
        (-1, -1)  // top-left
    };

    public Queen(PieceColor color) : base(color, PieceType.Queen, QueenDirectionOffsets) { }
}
