namespace SurpriseChess;

public class CampaignNode
{
    public char Id { get; set; } // Các node hiển thị duới các chữ cái A,B,C,...
    public int Difficulty { get; set; } // độ khó màn chơi
    public int Depth { get; set; } // depth request đến Stockfish

    public CampaignNode(char id, int difficulty,int depth)
    {
        Id = id;
        Difficulty = difficulty;
        Depth = depth;
       
    }
}
