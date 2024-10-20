namespace SurpriseChess;

// Lớp CampaignModel đại diện cho mô hình dữ liệu của chiến dịch
public class CampaignModel
{
    public List<CampaignNode> Nodes { get; private set; } // Danh sách các node trong chiến dịch

    public int SelectedRow { get; private set; } // Hàng được chọn trong lưới
    public int SelectedCol { get; private set; } // Cột được chọn trong lưới

    public CampaignNode[,] CampaignGrid = new CampaignNode[3, 3]; // Lưới các node chiến dịch 3x3
    private List<string> nodeIds = new List<string> { "A", "B", "C", "D", "E", "H", "I", "N", "VL" }; // Danh sách ID cho các node

    // Constructor khởi tạo mô hình
    public CampaignModel()
    {
        Nodes = CreateCampaignNodes(); // Tạo các node cho chiến dịch
        InitializeMap(); // Khởi tạo lưới các node

        SelectedRow = 0; // Khởi tạo hàng được chọn là 0
        SelectedCol = 0; // Khởi tạo cột được chọn là 0
    }

    // Phương thức khởi tạo lưới các node
    private void InitializeMap()
    {
        for (int i = 0; i < Nodes.Count && i < 9; i++)
        {
            int row = i / 3; // Tính toán hàng từ chỉ số i
            int col = i % 3; // Tính toán cột từ chỉ số i
            CampaignGrid[row, col] = Nodes[i]; // Map sẽ hiển thị các node tương ứng
        }
    }

    // Phương thức tạo các node cho chiến dịch
    private List<CampaignNode> CreateCampaignNodes()
    {
        // Tạo 9 nodes với các ID từ 'A' đến 'I', độ khó tăng dần và số depth tương ứng
        var nodes = new List<CampaignNode>();

        for (int i = 0; i < 9; i++)
        {
            int difficulty = i + 1; // Độ khó từ 1 đến 9

            nodes.Add(new CampaignNode(nodeIds[i], difficulty)); // Thêm node vào danh sách
        }

        return nodes; // Trả về danh sách các node
    }

    // Phương thức di chuyển lên
    public void MoveUp() => SelectedRow = (SelectedRow - 1 + 3) % 3;
    // Phương thức di chuyển xuống
    public void MoveDown() => SelectedRow = (SelectedRow + 1) % 3;
    // Phương thức di chuyển sang trái
    public void MoveLeft() => SelectedCol = (SelectedCol - 1 + 3) % 3;
    // Phương thức di chuyển sang phải
    public void MoveRight() => SelectedCol = (SelectedCol + 1) % 3;
}