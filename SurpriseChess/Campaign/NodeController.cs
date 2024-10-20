
namespace SurpriseChess;
internal class NodeController : IController
{
    private readonly NodeView view;
    private readonly CampaignNode node;

    public NodeController(NodeView view, CampaignNode node)
    {
        this.view = view;
        this.node = node;
    }

    public void Run()
    {
        view.Render(node);
        Console.ReadKey();
        StartSelectedCampaign();
    }

    private void StartSelectedCampaign()
    {
        // Bắt đầu game ở Node được chọn
        IChessBot chessBot = new StockFish(node.Difficulty);
        ChessController chessController = new ChessController(
            new ChessModel(new Chess960(), chessBot),
            new ChessView(),
            GameMode.PlayerVsBot,
            node.Difficulty
        );
        ScreenManager.Instance.NavigateToScreen(chessController);
    }
}

