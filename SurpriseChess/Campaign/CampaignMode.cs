namespace SurpriseChess;

public class CampaignNode
{
    public string Id { get; set; } // Các node hiển thị duới các chữ cái A,B,C,...
    public int Difficulty { get; set; } // độ khó màn chơi

    public CampaignNode(string id, int difficulty)
    {
        Id = id;
        Difficulty = difficulty;       
    }
}
