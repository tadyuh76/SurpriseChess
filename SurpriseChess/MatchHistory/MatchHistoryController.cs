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
                ChessController chessController = new ChessController(
                    new ChessModel(new Chess960(), new StockFish()),
                    new ChessView(),
                    GameMode.PlayerVsPlayer,
                    null
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
