using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace SurpriseChess;

public class StockFish : IChessBot
{
    private static readonly HttpClient client = new() { Timeout = TimeSpan.FromSeconds(5) }; // HttpClient với timeout 5 giây
    private const string ApiUrl = "https://tadyuh76.pythonanywhere.com/best_moves"; // Địa chỉ API để nhận nước đi

    private const int numMoves = 1; // Số lượng nước đi mà bot sẽ yêu cầu
    private readonly int depth; // Độ sâu của tìm kiếm

    public StockFish(int depth)
    {
        this.depth = depth; // Khởi tạo độ sâu
    }

    public async Task<List<(Position, Position)>> GetBestMoves(string fen)
    {
        // Tạo nội dung yêu cầu
        var requestBody = new
        {
            fen, // Trạng thái bàn cờ ở định dạng FEN
            options = new { UCI_Chess960 = true }, // Tùy chọn cho Chess960
            num_moves = numMoves, // Số lượng nước đi muốn nhận
            depth = depth // Độ sâu cho tìm kiếm
        };

        // Chuyển đổi nội dung yêu cầu thành chuỗi JSON
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody), // Chuyển đổi đối tượng thành JSON
            Encoding.UTF8,
            "application/json" // Đặt loại nội dung là JSON
        );

        try
        {
            // Gửi yêu cầu POST đến API
            HttpResponseMessage response = await client.PostAsync(ApiUrl, jsonContent);

            // Xử lý phản hồi
            response.EnsureSuccessStatusCode(); // Đảm bảo phản hồi thành công
            string responseBody = await response.Content.ReadAsStringAsync(); // Đọc nội dung phản hồi
            using JsonDocument doc = JsonDocument.Parse(responseBody); // Phân tích cú pháp JSON
            JsonElement root = doc.RootElement;

            // Lấy và in ra các nước đi tốt nhất
            List<(Position, Position)> bestMoves = new();
            foreach (JsonElement item in root.EnumerateArray())
            {
                if (item.TryGetProperty("Move", out JsonElement moveElement)) // Kiểm tra nếu có thuộc tính "Move"
                {
                    string move = moveElement.GetString()!; // Lấy giá trị của "Move"

                    // Chuyển đổi chuỗi move thành vị trí bắt đầu và kết thúc
                    Position startPosition = FEN.FENToPosition(move[..2]); // Lấy vị trí bắt đầu
                    Position endPosition = FEN.FENToPosition(move[2..4]); // Lấy vị trí kết thúc
                    bestMoves.Add((startPosition, endPosition)); // Thêm vào danh sách
                }
            }

            return bestMoves; // Trả về danh sách các nước đi tốt nhất
        }
        catch (Exception)
        {
            return new List<(Position, Position)>(); // Trả về danh sách rỗng nếu có lỗi
        }
    }
}
