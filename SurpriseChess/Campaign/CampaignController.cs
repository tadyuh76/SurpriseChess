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
                case ConsoleKey.LeftArrow:
                    model.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    model.MoveRight();
                    break;
                case ConsoleKey.Enter:
                    // Handle selecting the node and navigating to the game screen
                    StartSelectedCampaign();
                    break;
                case ConsoleKey.Backspace:
                    // Return to the home screen
                    return;
            }

        } while (key != ConsoleKey.Backspace);
    }

    private void StartSelectedCampaign()
    {
        var selectedNode = model.Nodes[model.CurrentNodeIndex];
        Console.WriteLine($"\nStarting game at node {selectedNode.Id} with difficulty {selectedNode.Difficulty}...");

        // Call the game controller to start the game at the selected difficulty level
        // Navigate to the Game Screen with the selected match details
        // ChessController chessController = new ChessController(
        //    new ChessModel(
        //        // convert FENs to BoardSetups
        //        // then check if the model receives the list of board setups,
        //        // it will render the UI for viewing history, not for playing the game.
        //    ),
        //    new ChessView()
        //);

        // for placeholder
        ChessController chessController = new ChessController(
            new ChessModel(new Chess960()),
            new ChessView(),
            GameMode.PlayerVsPlayer,
            selectedNode.Difficulty
        );
        chessController.Run();
    }
}
