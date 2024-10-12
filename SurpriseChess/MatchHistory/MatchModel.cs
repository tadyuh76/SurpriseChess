namespace SurpriseChess;

public class Match
{
    private static Random random = new Random();

    public int Id { get; set; }
    public DateTime MatchDate { get; set; }
    public string Result { get; set; } = string.Empty;

    public List<string> HistoryFEN { get; set; } = new List<string>();

    public Match()
    {
        HistoryFEN = new List<string>();

        do
        {
            Id = random.Next(1000, 10000); // Generate a 4-digit number
        } while (MatchHistoryModel.usedIds.Contains(Id));

        MatchHistoryModel.usedIds.Add(Id); // Mark the ID as used
    }

    // Method to add FEN strings, ensuring no duplicates
    public void AddFEN(List<string> newFENs)
    {
        foreach (var fen in newFENs)
        {
            if (HistoryFEN.Count == 0 || HistoryFEN[^1] != fen)
            {
                HistoryFEN.Add(fen);
            }
        }
    }

    // Save the match to a text file
    public void SaveToFile(string filePath)
    {
        File.WriteAllLines(filePath, HistoryFEN);
    }
}
