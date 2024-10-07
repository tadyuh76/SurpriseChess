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
        // Create 10 nodes with IDs from 'A' to 'J' and varying difficulty levels
        return new List<CampaignNode>
        {
            new CampaignNode('A', 1),
            new CampaignNode('B', 2),
            new CampaignNode('C', 3),
            new CampaignNode('D', 4),
            new CampaignNode('E', 5),
            new CampaignNode('F', 6),
            new CampaignNode('G', 7),
            new CampaignNode('H', 8),
            new CampaignNode('I', 9),
            new CampaignNode('N', 10)
        };
    }

    public void MoveUp()
    {
        if (CurrentNodeIndex > 0) CurrentNodeIndex--;
    }

    public void MoveDown()
    {
        if (CurrentNodeIndex < Nodes.Count - 1) CurrentNodeIndex++;
    }
}
