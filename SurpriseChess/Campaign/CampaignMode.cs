namespace SurpriseChess;

public class CampaignNode
{
    public char Id { get; set; } // Node represented by A, B, C...
    public int Difficulty { get; set; } // Difficulty level

    public CampaignNode(char id, int difficulty)
    {
        Id = id;
        Difficulty = difficulty;
    }
}
