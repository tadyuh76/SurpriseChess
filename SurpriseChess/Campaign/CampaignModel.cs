using System.Reflection;

namespace SurpriseChess;

public class CampaignModel
{
    public List<CampaignNode> Nodes { get; private set; }

    public int SelectedRow { get; private set; }
    public int SelectedCol { get; private set; }

    public CampaignNode[,] CampaignGrid = new CampaignNode[3, 3];
    private List<string> nodeIds = new List<string> { "A", "B", "C", "D", "E", "H", "I", "N", "VL" };

    public CampaignModel()
    {
        Nodes = CreateCampaignNodes();
        InitializeMap();

        SelectedRow = 0;
        SelectedCol = 0;
    }

    private void InitializeMap()
    {
        for (int i = 0; i < Nodes.Count && i < 9; i++)
        {
            int row = i / 3;
            int col = i % 3;
            CampaignGrid[row, col] = Nodes[i]; // Map sẽ hiển thị các node tương ứng
        }
    }

    private List<CampaignNode> CreateCampaignNodes()
    {
        // Tạo 9 nodes với các ID từ 'A' đến 'I', độ khó tăng dần và số depth  tương ứng
        var nodes = new List<CampaignNode>();

        for (int i = 0; i < 9; i++)
        {
            int difficulty = i + 1;    // Độ khó từ 1 đến 9

            nodes.Add(new CampaignNode(nodeIds[i], difficulty));
        }

        return nodes;
    }

    public void MoveUp() => SelectedRow = (SelectedRow - 1 + 3) % 3;
    public void MoveDown() => SelectedRow = (SelectedRow + 1) % 3;
    public void MoveLeft() => SelectedCol = (SelectedCol - 1 + 3) % 3;
    public void MoveRight() => SelectedCol = (SelectedCol + 1) % 3;
}
