namespace SurpriseChess;

public class CampaignModel
{
    public List<CampaignNode> Nodes { get; private set; }
    public int CurrentNodeIndex { get; set; } 

    public CampaignModel()
    {
        Nodes = CreateCampaignNodes();
        CurrentNodeIndex = 0; // Bắt đầu từ node A
    }

    private List<CampaignNode> CreateCampaignNodes()
    {
        // Tạo 9 nodes với các ID từ 'A' đến 'I', độ khó tăng dần và số depth  tương ứng
        var nodes = new List<CampaignNode>();

        for (int i = 0; i < 10; i++)
        {
            char id = (char)('A' + i); // Tạo ID từ 'A' đến 'I'
            int difficulty = i + 1;    // Độ khó từ 1 đến 9
            int depth = i + 1;      // depth cũng từ 1 đến 9

            nodes.Add(new CampaignNode(id, difficulty, depth));
        }

        return nodes;
    }

    public void MoveUp()
    {
        if (CurrentNodeIndex > 0) CurrentNodeIndex--;
    }

    public void MoveDown()
    {
        if (CurrentNodeIndex < Nodes.Count - 1) CurrentNodeIndex++;
    }

    public CampaignNode GetCurrentNode()
    {
        return Nodes[CurrentNodeIndex];
    }
}
