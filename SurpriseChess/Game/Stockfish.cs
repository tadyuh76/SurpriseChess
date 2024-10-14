using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SurpriseChess;
public class StockFish : IChessBot
{
    private static readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(5) };
    private const string ApiUrl = "https://tadyuh76.pythonanywhere.com/best_moves";
    
    private readonly int numMoves; // Số nước đi sẽ được lấy từ CampaignNode

    public StockFish(int numMoves)
    {
        this.numMoves = numMoves; // Truyền số nước đi khi khởi tạo
    }

    public async Task<List<(Position, Position)>> GetBestMoves(string fen)
    {
        // Tạo nội dung yêu cầu
        var requestBody = new
        {
            fen,
            options = new { UCI_Chess960 = true },
            num_moves = numMoves // Số nước đi giờ là từ campaign
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json"
        );

        try
        {
            // Gửi yêu cầu POST
            HttpResponseMessage response = await client.PostAsync(ApiUrl, jsonContent);

            // Xử lý phản hồi
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseBody);
            JsonElement root = doc.RootElement;

            // Lấy và in ra các nước đi tốt nhất
            List<(Position, Position)> bestMoves = new();
            Console.WriteLine($"top {numMoves} nước đi tốt nhất từ api :");

            foreach (JsonElement item in root.EnumerateArray())
            {
                if (item.TryGetProperty("Move", out JsonElement moveElement))
                {
                    string move = moveElement.GetString()!;
                    Position startPosition = FEN.FENToPosition(move[..2]);
                    Position endPosition = FEN.FENToPosition(move[2..4]);
                    bestMoves.Add((startPosition, endPosition));

                    // In ra nước đi
                    Console.WriteLine($"{FEN.PositionToFEN(startPosition)} -> {FEN.PositionToFEN(endPosition)}");
                  

                }

            }

            return bestMoves;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching best moves: {ex.Message}");
            return new List<(Position, Position)>();
        }
    }
}










