using SurpriseChess.FEN;
using SurpriseChess;

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
        view.RenderMatchList(model.Matches);
        int selectedId = view.GetSelectedMatchId();
        if (selectedId > 0)
        {
            var selectedMatch = model.Matches.FirstOrDefault(m => m.Id == selectedId);
            if (selectedMatch != null)
            {
                string filePath = "match_history.txt";
                List<string> fenList;
                try
                {
                    fenList = FEN.FENParser.LoadFENFromFileByMatchId(filePath, selectedId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading match: {ex.Message}");
                    return;
                }

                var replayBoard = new ReplayBoard(fenList[0]); // Start with the first move's FEN
                var replayView = new ReplayView();
                var replayController = new ReplayController(fenList, replayView);
                ScreenManager.Instance.NavigateToScreen(replayController);
            }
            else
            {
                Console.WriteLine("Invalid match number.");
            }
        }
    }
}