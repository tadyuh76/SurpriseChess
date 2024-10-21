namespace SurpriseChess;

public static class GameHistoryPostProcessor
{
    // Phương thức chính xử lý lịch sử trận đấu
    public static List<string> ProcessGameHistory(List<string> originalHistory)
    {
        var deduplicatedHistory = RemoveDuplicates(originalHistory); // Bước 1: loại bỏ bản sao
        UpdateMoveCounts(deduplicatedHistory); // Bước 2: cập nhật số lượt di chuyển
        return deduplicatedHistory; // Trả về lịch sử đã xử lý
    }

    // Phương thức loại bỏ các bản sao từ lịch sử FEN
    private static List<string> RemoveDuplicates(List<string> fenHistory)
    {
        List<string> processedHistory = new List<string>(); // Danh sách để lưu lịch sử đã xử lý
        string? previousFen = null; // Biến để lưu FEN trước đó
        foreach (var fen in fenHistory)
        {
            if (fen != previousFen) // Kiểm tra nếu FEN hiện tại khác FEN trước đó
            {
                processedHistory.Add(fen); // Thêm vào danh sách nếu không trùng lặp
                previousFen = fen; // Cập nhật FEN trước đó
            }
        }
        return processedHistory; // Trả về lịch sử đã loại bỏ bản sao
    }

    // Phương thức cập nhật số lượt di chuyển trong lịch sử FEN
    private static void UpdateMoveCounts(List<string> fenHistory)
    {
        for (int i = 0; i < fenHistory.Count; i++)
        {
            string[] fenParts = fenHistory[i].Split(' '); // Chia FEN thành các phần
            int halfMoveClock = i == 0 ? 0 : int.Parse(fenParts[4]) + 1; // Cập nhật halfMoveClock
            fenParts[4] = halfMoveClock.ToString(); // Gán giá trị mới cho halfMoveClock
            int fullMoveNumber = (i / 2) + 1; // Tính fullMoveNumber
            fenParts[5] = fullMoveNumber.ToString(); // Gán giá trị mới cho fullMoveNumber
            fenHistory[i] = string.Join(" ", fenParts); // Kết hợp các phần thành chuỗi FEN mới
        }
    }
}
