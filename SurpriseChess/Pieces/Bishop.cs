namespace SurpriseChess;

public class Bishop : LongRangePiece
{
    // Biểu tượng hiển thị cho quân vua tùy thuộc vào màu sắc
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🥷" : "🦉";

    // Quân tượng có thể di chuyển chéo theo 4 hướng
    private static readonly (int, int)[] BishopDirectionOffsets =
    {
        (1, 1),   // phải dưới
        (-1, 1),  // trái dưới
        (1, -1),  // phải trên
        (-1, -1)  // trái trên
    };

    public Bishop(PieceColor color) : base(color, PieceType.Bishop, BishopDirectionOffsets) { }
}
