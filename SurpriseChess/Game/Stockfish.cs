
using SurpriseChess.FEN;
using SurpriseChess;
using System.Text.Json;
using System.Text;

public class StockFish : IChessBot
{
    private static readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(5) };
    private const string ApiUrl = "https://tadyuh76.pythonanywhere.com/best_moves";
    private const int NumMoves = 3;

    public async Task<List<(Position, Position)>> GetBestMoves(string fen)
    {
        // Create the request body
        var requestBody = new
        {
            fen,
            options = new { UCI_Chess960 = true },
            num_moves = NumMoves
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        try
        {
            // Send POST request
            HttpResponseMessage response = await client.PostAsync(ApiUrl, jsonContent);

            // Handle response
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseBody);
            JsonElement root = doc.RootElement;

            // Iterate over the JSON array and extract moves
            List<(Position, Position)> bestMoves = new();
            foreach (JsonElement item in root.EnumerateArray())
            {
                if (item.TryGetProperty("Move", out JsonElement moveElement))
                {
                    string move = moveElement.GetString()!;
                    // Convert string representation of move to Position type
                    Position startPosition = FEN.FENToPosition(move[..2]);
                    Position endPosition = FEN.FENToPosition(move[2..4]);
                    bestMoves.Add((startPosition, endPosition));
                }
            }

            return bestMoves;
        }
        catch (Exception)
        {
            return new List<(Position, Position)>();
        }
    }
}
public class StockfishAnalysisCache
{
    private readonly Dictionary<string, List<(Position, Position)>> cache = new();
    private readonly int maxCacheSize;

    public StockfishAnalysisCache(int maxCacheSize = 1000)
    {
        this.maxCacheSize = maxCacheSize;
    }

    public List<(Position, Position)> GetCachedAnalysis(string fen)
    {
        if (cache.TryGetValue(fen, out var analysis))
        {
            // Move the accessed item to the end of the dictionary to implement LRU
            cache.Remove(fen);
            cache[fen] = analysis;
            return analysis;
        }
        return null;
    }

    public void CacheAnalysis(string fen, List<(Position, Position)> analysis)
    {
        if (cache.Count >= maxCacheSize)
        {
            // Remove the least recently used item (first item in the dictionary)
            cache.Remove(cache.Keys.First());
        }
        cache[fen] = analysis;
    }
}