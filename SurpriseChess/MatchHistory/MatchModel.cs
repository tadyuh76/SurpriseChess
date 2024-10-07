
namespace SurpriseChess;

public class Match
{
    public int Id { get; set; }
    public DateTime MatchDate { get; set; }
    public string Result { get; set; }
    public string[] HistoryFEN { get; set; } // The FEN notation to reload the match state

    // Additional fields related to the match
}
