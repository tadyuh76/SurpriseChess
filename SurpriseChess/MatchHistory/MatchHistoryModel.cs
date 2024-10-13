namespace SurpriseChess;

public class MatchHistoryModel
{
    public static HashSet<int> usedIds = new HashSet<int>(); // Ghi nhớ những ID nào đã dùng
    public List<Match> Matches { get; private set; }

    public MatchHistoryModel()
    {
        Matches = LoadMatchHistory();
    }

    // Load toàn bộ list lịch sử các trận
    private List<Match> LoadMatchHistory()
    {
        // Load từng trận
        var matches = MatchHistory.MatchHistoryManager.LoadMatches();  // Dùng MatchHistoryManager

        if (matches == null || matches.Count == 0)
        {
            // nếu không có trận nào dc tìm thì thêm vào placeholder
            return new List<Match>
            {
                new Match { Id = 1, MatchDate = DateTime.Now.AddDays(-5), Result = "1-0" },
                new Match { Id = 2, MatchDate = DateTime.Now.AddDays(-3), Result = "0-1" }
            };
        }
        return matches;
    }
}

