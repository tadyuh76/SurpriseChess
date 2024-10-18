namespace SurpriseChess;

public class CampaignController : IController
{ 
    private readonly CampaignModel model;
    private readonly CampaignView view;

    private CampaignNode[,] map;  // Define a 2D map of campaign nodes
    private (int x, int y) selectedNode = (0, 0); // Start at (0, 0) - Node A

    public CampaignController(CampaignModel model, CampaignView view)
    {
        this.model = model;
        this.view = view;

        // Initialize the map from the nodes in the CampaignModel
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Assuming the map is 3x3 for simplicity
        map = new CampaignNode[3, 3];

        for (int i = 0; i < model.Nodes.Count && i < 9; i++)
        {
            int row = i / 3;
            int col = i % 3;
            map[row, col] = model.Nodes[i]; // Fill the map with campaign nodes
        }
    }

    public void Run()
    {
        ConsoleKey key;

        do
        {
            view.RenderMap(map, selectedNode); // Pass the map and selected node to view
            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveSelection(-1, 0); // Move up
                    break;
                case ConsoleKey.DownArrow:
                    MoveSelection(1, 0); // Move down
                    break;
                case ConsoleKey.LeftArrow:
                    MoveSelection(0, -1); // Move left
                    break;
                case ConsoleKey.RightArrow:
                    MoveSelection(0, 1); // Move right
                    break;
                case ConsoleKey.Enter:
                    StartSelectedCampaign();
                    break;
                case ConsoleKey.Backspace:
                    HandleNavigateBack();
                    return;

            }

        } while (key != ConsoleKey.Backspace);
    }

    private void MoveSelection(int dx, int dy)
    {
        int newX = selectedNode.x + dx;
        int newY = selectedNode.y + dy;

        // Ensure the new selection is within bounds and not null
        if (newX >= 0 && newX < map.GetLength(0) &&
            newY >= 0 && newY < map.GetLength(1) &&
            map[newX, newY] != null)
        {
            selectedNode = (newX, newY);
        }
    }

    private void StartSelectedCampaign()
    {
        var node = map[selectedNode.x, selectedNode.y]; // Get the selected node
        Console.WriteLine($"\nBắt đầy game ở {node.Id} độ khó  {node.Difficulty}...");

        IChessBot chessBot = new StockFish(node.NumMoves);

        ChessController chessController = new ChessController(
            new ChessModel(new Chess960(), chessBot),
            new ChessView(),
            GameMode.PlayerVsBot,
            node.Difficulty
        );
        chessController.Run();
    }

    private void HandleNavigateBack()
    {
        Console.WriteLine("Quay lại màn hình chờ...");
        ScreenManager.Instance.NavigateToScreen(new HomeController(
            new HomeModel(),
            new HomeView()
        ));
    }
}


