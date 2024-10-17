namespace SurpriseChess;

public class CampaignController : IController
{
    private readonly CampaignModel model;
    private readonly CampaignView view;

    public CampaignController(CampaignModel model, CampaignView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Run()
    {
        ConsoleKey key;

        do
        {
            view.Render(model.Nodes, model.CurrentNodeIndex);
            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    model.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    model.MoveDown();
                    break;
                case ConsoleKey.Enter:
                    // Handle selecting the node and navigating to the game screen
                    StartSelectedCampaign();
                    break;
                case ConsoleKey.Backspace:
                    // Return to the home screen
                    ScreenManager.Instance.BackToHomeScreen();
                    return;
            }

        } while (key != ConsoleKey.Backspace);
    }

    private void StartSelectedCampaign()
    {
        var selectedNode = model.Nodes[model.CurrentNodeIndex];
        Console.WriteLine($"\nStarting game at node {selectedNode.Id} with difficulty {selectedNode.Difficulty}...");

        // for placeholder
        ChessController chessController = new ChessController(
            new ChessModel(new Chess960(), new StockFish()),
            new ChessView(),
            GameMode.PlayerVsPlayer,
            selectedNode.Difficulty
        );
        chessController.Run();
    }
}
