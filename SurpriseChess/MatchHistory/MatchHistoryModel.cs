namespace SurpriseChess;

public class MatchHistoryModel
{
    public static HashSet<int> usedIds = new HashSet<int>(); // Keep track of used IDs
    public List<Match> Matches { get; private set; }

    public MatchHistoryModel()
    {
        Matches = LoadMatchHistory();
    }

    // Load match history (could be from file or DB)
    private List<Match> LoadMatchHistory()
    {
        // Loading actual matches saved in the file
        var matches = MatchHistory.MatchHistoryManager.LoadMatches();  // Load matches from the file using MatchHistoryManager

        if (matches == null || matches.Count == 0)
        {
            // If no matches are found, you can either return an empty list or some placeholder data
            return new List<Match>
            {
                new Match { Id = 1, MatchDate = DateTime.Now.AddDays(-5), Result = "1-0" },
                new Match { Id = 2, MatchDate = DateTime.Now.AddDays(-3), Result = "0-1" }
            };
        }

        return matches;
    }

    // Save a specific match
    public void SaveMatch(Match match, string filePath)
    {
        match.SaveToFile(filePath); // Save match as a FEN text file
    }


}

