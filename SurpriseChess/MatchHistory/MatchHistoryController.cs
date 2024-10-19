namespace SurpriseChess
{
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

            while (true)
            {
                view.RenderMatchList(model.Matches);
                int selectedId = view.GetSelectedMatchId();

                if (selectedId == -1) break;

                var selectedMatch = model.GetMatchById(selectedId);
                if (selectedMatch != null)
                {
                    List<string> fenList = selectedMatch.HistoryFEN;
                    var replayModel = new ReplayModel(fenList);
                    var replayView = new ReplayView();
                    var replayController = new ReplayController(replayModel, replayView);
                    ScreenManager.Instance.NavigateToScreen(replayController);
                }
                else
                {
                    view.DisplayError("ID trận đấu không hợp lệ.");
                }
            }
            
        }
    }
}