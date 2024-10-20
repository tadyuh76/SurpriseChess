using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

namespace SurpriseChess
{
    public class ReplayController : IController
    {
        private readonly ReplayModel model;
        private readonly ReplayView view;
        private readonly StockFish stockfish;
        private readonly Stopwatch inputCooldown;
        private const int CooldownMilliseconds = 500; // Cooldown 500ms giữa các lần nhấn phím

        public ReplayController(ReplayModel model, ReplayView view)
        {
            this.model = model;
            this.view = view;
            this.stockfish = new StockFish(depth: 10);
            this.inputCooldown = new Stopwatch();
            this.inputCooldown.Start(); // Bắt đầu đo thời gian ngay khi khởi tạo
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                DisplayBoardAndAnalysisAsync(); // Hiển thị bàn cờ và phân tích nước đi
                isRunning = HandleUserInput();  // Xử lý đầu vào từ người dùng
            }
        }

        public void DisplayBoardAndAnalysisAsync()
        {
            string actualNextMove = model.DetermineActualNextMove();

            // Hiển thị bàn cờ ngay lập tức với nước đi hiện tại
            view.RenderBoard(model.CurrentBoard, actualNextMove, "");
            view.DisplayMoveInfo(actualNextMove, "Đang tải...");

            // Bắt đầu phân tích nước đi trong nền (background)
            GetBestMove();
        }

        public async void GetBestMove()
        {
            var bestMoves = await stockfish.GetBestMoves(model.GetCurrentFEN());
            
            string bestMove = bestMoves.Count == 0 ? "None" : model.ConvertMoveToString(bestMoves[0]);
            string actualNextMove = model.DetermineActualNextMove();

            // Cập nhật bàn cờ và thông tin nước đi tốt nhất
            view.RenderBoard(model.CurrentBoard, actualNextMove, bestMove);
            view.DisplayMoveInfo(actualNextMove, bestMove);
        }

        private bool HandleUserInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            // Kiểm tra xem thời gian cooldown đã đủ chưa
            if (inputCooldown.ElapsedMilliseconds < CooldownMilliseconds)
            {
                // Nếu chưa đủ thời gian, bỏ qua lần nhập này
                return true;
            }

            // Chỉ reset và bắt đầu lại đồng hồ nếu nhập vào hợp lệ
            bool inputHandled = false;

            switch (keyInfo.Key)
            {
                case ConsoleKey.RightArrow:
                    model.MoveNext();  // Chuyển đến nước đi tiếp theo
                    inputHandled = true;
                    break;
                case ConsoleKey.LeftArrow:
                    model.MovePrevious();  // Quay lại nước đi trước
                    inputHandled = true;
                    break;
                case ConsoleKey.Backspace:
                    ScreenManager.Instance.BackToHomeScreen();
                    break; // Thoát vòng lặp Run
            }

            // Chỉ restart lại cooldown nếu đã xử lý nhập vào thành công
            if (inputHandled)
            {
                inputCooldown.Restart();
            }

            return true;
        }
    }
}
