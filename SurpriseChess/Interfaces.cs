namespace SurpriseChess;

public interface IPrototype<T>
{
    T Clone();
}

public interface IBoardView
{
    Piece? GetPieceAt(Position position);
    Dictionary<Position, Piece> LocatePieces(PieceColor? color = null, PieceType? type = null);
    Dictionary<PieceColor, Dictionary<CastleDirection, Position>> RookStartingPositions { get; }
}

public interface IBoardSetup
{
    void SetUp(Board board);
}

//public interface IChessBot
//{
//    Task<List<(Position, Position)>> GetBestMoves(string fen);
//}