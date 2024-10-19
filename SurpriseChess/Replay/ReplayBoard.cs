using SurpriseChess.FEN;
using SurpriseChess;

public class ReplayBoard
{
    private readonly Piece?[,] board = new Piece?[8, 8]; // 8x8 board

    public ReplayBoard(string fen)
    {
        FEN.LoadPositionFromFEN(fen, this);
    }

    public void SetPieceAt(Position position, Piece piece)
    {
        board[position.Row, position.Col] = piece;
    }

    public Piece? GetPieceAt(Position position) =>
        position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8
            ? board[position.Row, position.Col]
            : null;
}