namespace SurpriseChess;

public class CampaignController : IController
{
    private readonly CampaignModel model;
    private readonly CampaignView view;

    private CampaignNode[,] map;  
    private (int x, int y) selectedNode = (0, 0); // Bắt đầu ở  (0, 0) - Node A

    public CampaignController(CampaignModel model, CampaignView view)
    {
        this.model = model;
        this.view = view;

        // Khởi tạo map từ các node trong CampaignNode
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Tạo map dạng 3x3
        map = new CampaignNode[3, 3];

        for (int i = 0; i < model.Nodes.Count && i < 9; i++)
        {
            int row = i / 3;
            int col = i % 3;
            map[row, col] = model.Nodes[i]; // Map sẽ hiển thị các node tương ứng
        }
    }

    public void Run()
    {
        ConsoleKey key;

        do
        {
            view.RenderMap(map, selectedNode); 
            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveSelection(-1, 0); // Di chuyển lên
                    break;
                case ConsoleKey.DownArrow:
                    MoveSelection(1, 0); // Di chuyển xuống
                    break;
                case ConsoleKey.LeftArrow:
                    MoveSelection(0, -1); // Sang trái
                    break;
                case ConsoleKey.RightArrow:
                    MoveSelection(0, 1); // Sang phải
                    break;
                case ConsoleKey.Enter:
                    StartSelectedCampaign();
                    break;
                case ConsoleKey.Backspace:
                    // Trở về màn hình chính
                    ScreenManager.Instance.BackToHomeScreen();
                    return;

            }

        } while (key != ConsoleKey.Backspace);
    }

    private void MoveSelection(int dx, int dy)
    {
        int newX = selectedNode.x + dx;
        int newY = selectedNode.y + dy;
        
        if (newX >= 0 && newX < map.GetLength(0) &&
            newY >= 0 && newY < map.GetLength(1) &&
            map[newX, newY] != null)
        {
            selectedNode = (newX, newY);
        }
    }

    private void StartSelectedCampaign()
    {
        var node = map[selectedNode.x, selectedNode.y]; // Bắt đầu game ở Node được chọn

        IChessBot chessBot = new StockFish(node.Depth);

        ChessController chessController = new ChessController(
            new ChessModel(new Chess960(), chessBot),
            new ChessView(),
            GameMode.PlayerVsBot,
            node.Difficulty
        );
        ScreenManager.Instance.NavigateToScreen(chessController);
    }
}
