using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Linq;

namespace SurpriseChess
{
    public class ReplayController : IController
    {
        private readonly ReplayModel model;
        private readonly ReplayView view;
        private readonly StockFish stockfish;
        private readonly StockfishAnalysisCache analysisCache;
        private readonly Stopwatch inputCooldown;
        private const int CooldownMilliseconds = 1200; // 1.2 second input cooldown

        public ReplayController(ReplayModel model, ReplayView view)
        {
            this.model = model;
            this.view = view;
            this.stockfish = new StockFish(depth: 10);
            this.analysisCache = new StockfishAnalysisCache();
            this.inputCooldown = new Stopwatch();
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                DisplayBoardAndAnalysisAsync();
                isRunning = HandleUserInput();
            }
        }

        public void DisplayBoardAndAnalysisAsync()
        {
            string actualNextMove = model.DetermineActualNextMove();

            // Render the board immediately with the actual move
            view.RenderBoard(model.CurrentBoard, actualNextMove, "");
            view.DisplayMoveInfo(actualNextMove, "Loading...");

            // Start the analysis in the background
            Task.Run(async () =>
            {
                try
                {
                    List<(Position, Position)> bestMoves = await model.GetBestMovesAsync(stockfish, analysisCache);
                    string bestMove = model.ConvertMoveToString(bestMoves.FirstOrDefault());

                    // Update the board and move info with the best move
                    view.RenderBoard(model.CurrentBoard, actualNextMove, bestMove);
                    view.DisplayMoveInfo(actualNextMove, bestMove);
                }
                catch (Exception ex)
                {
                    view.DisplayError($"Lỗi phân tích nước đi: {ex.Message}");
                }
            });
        }

        private bool HandleUserInput()
        {
            if (inputCooldown.IsRunning && inputCooldown.ElapsedMilliseconds < CooldownMilliseconds)
            {
                // If the cooldown period hasn't elapsed, wait for the remaining time
                int remainingTime = CooldownMilliseconds - (int)inputCooldown.ElapsedMilliseconds;
                Thread.Sleep(remainingTime);
            }

            // Reset and start the cooldown timer
            inputCooldown.Restart();

            ConsoleKeyInfo keyInfo = view.GetUserInput();
            bool inputHandled = false;

            switch (keyInfo.Key)
            {
                case ConsoleKey.RightArrow:
                    model.MoveNext();
                    inputHandled = true;
                    break;
                case ConsoleKey.LeftArrow:
                    model.MovePrevious();
                    inputHandled = true;
                    break;
                case ConsoleKey.Backspace:
                    return false;
            }

            // If input wasn't handled (i.e., an unrecognized key was pressed),
            // we don't want to enforce the cooldown period
            if (!inputHandled)
            {
                inputCooldown.Reset();
            }

            return true;
        }
    }
}