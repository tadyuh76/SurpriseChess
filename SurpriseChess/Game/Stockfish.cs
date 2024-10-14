namespace SurpriseChess;

public class StockFish : IChessBot
{
    private static readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(5) };
    private const int NumMoves = 3;

    public Task<List<(Position, Position)>> GetBestMoves(string fen)
    {
        throw new NotImplementedException();
    }
}