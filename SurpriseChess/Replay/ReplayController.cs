namespace SurpriseChess
{
    public class ReplayController : IController
    {
        private readonly ReplayModel model;
        private readonly ReplayView view;
        private readonly StockFish stockfish;
        private readonly StockfishAnalysisCache analysisCache;


        public ReplayController(ReplayModel model, ReplayView view)
        {
            this.model = model;
            this.view = view;
            this.stockfish = new StockFish();
            this.analysisCache = new StockfishAnalysisCache();
        }


        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                
                DisplayBoardAndAnalysisAsync().Wait();
                isRunning = HandleUserInput();
            }
        }

        private async Task DisplayBoardAndAnalysisAsync()
        {
            try
            {
                string actualNextMove = model.DetermineActualNextMove();
                view.DisplayMoveInfo(actualNextMove, "Loading...");

                List<(Position, Position)> bestMoves = await model.GetBestMovesAsync(stockfish, analysisCache);
                string bestMove = model.ConvertMoveToString(bestMoves.FirstOrDefault());

                view.RenderBoard(model.CurrentBoard, actualNextMove, bestMove);
                view.DisplayMoveInfo(actualNextMove, bestMove + "      ");

            }
            catch (Exception ex)
            {
                view.DisplayError($"Lỗi phân tích nước đi: {ex.Message}");
            }
        }

        private bool HandleUserInput()
        {
           
            ConsoleKeyInfo keyInfo = view.GetUserInput();
            switch (keyInfo.Key)
            {
                case ConsoleKey.RightArrow:
                    model.MoveNext();
                    return true;
                case ConsoleKey.LeftArrow:
                    model.MovePrevious();
                    return true;
                case ConsoleKey.Backspace:
                    return false;
                default:
                    return true;
            }
        }
    }
}