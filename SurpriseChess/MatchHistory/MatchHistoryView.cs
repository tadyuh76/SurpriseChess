namespace SurpriseChess;

// Lớp hiển thị lịch sử trận đấu
public class MatchHistoryView
{
    // Phương thức để hiển thị danh sách các trận đấu
    public void RenderMatchList(List<Match> matches)
    {
        Console.Clear(); // Xóa màn hình
        Console.WriteLine("Lịch sử trận đấu:"); // Hiển thị tiêu đề

        // Lặp qua từng trận đấu và hiển thị thông tin
        foreach (var match in matches)
        {
            Console.WriteLine($"{match.Id}: {match.Result} on {match.MatchDate.ToShortDateString()}"); // Hiển thị ID, kết quả và ngày của trận đấu
        }
        Console.WriteLine("Nhập ID trận để xem lại hoặc dùng backspace để lui về màn hình chính."); // Hướng dẫn người dùng
    }

    // Phương thức để lấy ID trận được chọn từ người dùng
    public int GetSelectedMatchId()
    {
        Console.Write("Chọn ID trận: "); // Nhắc người dùng nhập ID trận
        // Kiểm tra xem người dùng có nhập số hợp lệ không
        if (int.TryParse(Console.ReadLine(), out int selectedId))
        {
            return selectedId; // Trả về ID đã chọn
        }
        return -1; // Trả về -1 nếu không hợp lệ
    }

    // Phương thức để hiển thị thông báo lỗi
    public void DisplayError(string message)
    {
        Console.WriteLine($"Lỗi: {message}"); // Hiển thị thông báo lỗi
    }
}
