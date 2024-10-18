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
            Id = random.Next(1000, 10000); // tạo id random 4 chữ số
        } while (MatchHistoryModel.UsedIds.Contains(Id));

        MatchHistoryModel.UsedIds.Add(Id); // đánh dấu là id đã dc dùng
    }

    // Method để thêm string FEN và ko cho lặp lại
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
}
