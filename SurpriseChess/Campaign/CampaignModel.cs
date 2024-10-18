namespace SurpriseChess;

public class CampaignModel
{
    public List<CampaignNode> Nodes { get; private set; }
    public int CurrentNodeIndex { get; set; } // Tracks the currently selected node

    public CampaignModel()
    {
        Nodes = CreateCampaignNodes();
        CurrentNodeIndex = 0; // Start at the first node (A)
    }

    private List<CampaignNode> CreateCampaignNodes()
    {
      // Tạo 10 nodes với các ID từ 'A' đến 'J', độ khó tăng dần và số moves tương ứng
        var nodes = new List<CampaignNode>();

        for (int i = 0; i < 10; i++)
        {
            char id = (char)('A' + i); // Tạo ID từ 'A' đến 'I'
            int difficulty = i + 1;    // Độ khó từ 1 đến 10
            int numMoves = i + 1;      // Số move cũng từ 1 đến 10

            nodes.Add(new CampaignNode(id, difficulty, numMoves));
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
