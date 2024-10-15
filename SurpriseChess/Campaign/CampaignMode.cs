namespace SurpriseChess;

public class CampaignNode
{
    public char Id { get; set; } // Node represented by A, B, C...
    public int Difficulty { get; set; } // Difficulty level
    public int NumMoves { get; set; }

    public CampaignNode(char id, int difficulty,int numMoves)
    {
        Id = id;
        Difficulty = difficulty;
        NumMoves = numMoves;
    }
}
