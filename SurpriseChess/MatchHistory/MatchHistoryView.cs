namespace SurpriseChess;

public class MatchHistoryView
{
    // Render list lịch sử để chọn
    public void RenderMatchList(List<Match> matches)
    {
        Console.Clear();
        Console.WriteLine("Match History:");
        foreach (var match in matches)
        {
            Console.WriteLine($"{match.Id}: {match.Result} on {match.MatchDate.ToShortDateString()}");
        }
        Console.WriteLine("Nhập ID trận để xem lại hoặc dùng backspace để lui về màn hình chính.");
    }

    // Lấy matchID được chọn từ user
    public int GetSelectedMatchId()
    {
        Console.Write("Chọn ID trận: ");
        if (int.TryParse(Console.ReadLine(), out int selectedId))
        {
            return selectedId;
        }
        return -1; // input lỗi
    }
}
