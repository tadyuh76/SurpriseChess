namespace SurpriseChess
{
    public class MatchHistoryView
    {
        public void RenderMatchList(List<Match> matches)
        {
            Console.Clear();
            Console.WriteLine("Lịch sử trận đấu:");
            foreach (var match in matches)
            {
                Console.WriteLine($"{match.Id}: {match.Result} on {match.MatchDate.ToShortDateString()}");
            }
            Console.WriteLine("Nhập ID trận để xem lại hoặc dùng backspace để lui về màn hình chính.");
        }

        public int GetSelectedMatchId()
        {
            Console.Write("Chọn ID trận: ");
            if (int.TryParse(Console.ReadLine(), out int selectedId))
            {
                return selectedId;
            }
            return -1;
        }

        public void DisplayError(string message)
        {
            Console.WriteLine($"Lỗi: {message}");
        }
    }
}