namespace SurpriseChess;

public class Knight : SimpleMovementPiece
{
    public override string DisplaySymbol => this.Color == PieceColor.White ? "🏇" : "🐴";

    // Quân mã có thể di chuyển đến 8 ô theo hình chữ L xung quanh
    private static readonly (int, int)[] KnightMoveOffsets = new (int, int)[]
    {
            (-2, 1), (-1, 2), (1, 2), (2, 1),
            (2, -1), (1, -2), (-1, -2), (-2, -1)
    };

    public Knight(PieceColor color) : base(color, PieceType.Knight, KnightMoveOffsets) { }

}