namespace SurpriseChess;

// Lớp NodeController chịu trách nhiệm điều khiển một node trong chế độ Campaign
internal class NodeController : IController
{
    private readonly NodeView view; // Giao diện hiển thị cho node
    private readonly CampaignNode node; // Node mà controller này quản lý

    // Constructor để khởi tạo NodeController với view và node cụ thể
    public NodeController(NodeView view, CampaignNode node)
    {
        this.view = view;
        this.node = node;
    }

    // Phương thức Run để hiển thị node và chờ người dùng nhấn phím
    public void Run()
    {
        view.Render(node); // Hiển thị thông tin của node
        Console.ReadKey(); // Đợi người dùng nhấn phím
        StartSelectedCampaign(); // Bắt đầu bàn cờ được chọn
    }

    // Phương thức để bắt đầu trò chơi với node được chọn
    private void StartSelectedCampaign()
    {
        // Tạo bot cờ mới với độ khó tương ứng của node
        IChessBot chessBot = new StockFish(node.Difficulty);

        // Tạo một ChessController mới cho trò chơi
        ChessController chessController = new ChessController(
            new ChessModel(new Chess960(), chessBot), // Khởi tạo mô hình cờ
            new ChessView(), // Khởi tạo giao diện cờ
            GameMode.PlayerVsBot, // Chế độ chơi người so với bot
            node.Difficulty // Độ khó của bot
        );

        // Chuyển hướng tới màn hình cờ
        ScreenManager.Instance.NavigateToScreen(chessController);
    }
}
