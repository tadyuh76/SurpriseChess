namespace SurpriseChess;

public class MatchHistoryModel
{
    public List<Match> Matches { get; private set; }

    public MatchHistoryModel()
    {
        // Load match history from file or database
        Matches = LoadMatchHistory();
    }

    private List<Match> LoadMatchHistory()
    {
        // Logic to load matches from saved files or other storage
        return new List<Match>
        {
            new Match { Id = 1, MatchDate = DateTime.Now.AddDays(-5), Result = "1-0", HistoryFEN = [] },
            new Match { Id = 2, MatchDate = DateTime.Now.AddDays(-3), Result = "0-1", HistoryFEN = [] },
        };
    }
}
