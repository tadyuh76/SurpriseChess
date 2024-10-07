namespace SurpriseChess;

public class MatchHistoryController : IController
{
    private readonly MatchHistoryModel model;
    private readonly MatchHistoryView view;

    public MatchHistoryController(MatchHistoryModel model, MatchHistoryView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Run()
    {
        view.Render(model.Matches);

        int selectedId = view.GetSelectedMatchId();
        if (selectedId > 0)
        {
            var selectedMatch = model.Matches.FirstOrDefault(m => m.Id == selectedId);
            if (selectedMatch != null)
            {
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
                   new ChessView()
                );
                chessController.Run();
            }
            else
            {
                Console.WriteLine("Invalid match number.");
            }
        }
    }
}
