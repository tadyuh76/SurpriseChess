using Timer = System.Timers.Timer;

namespace SurpriseChess;

internal class ChessController : IController
{
    private int cursorX = 0, cursorY = 7; // Vị trí con trỏ ban đầu trên bàn cờ
    private readonly ChessModel model; // Model trò chơi (MVC)
    private readonly ChessView view; // View trò chơi (MVC)

    private GameMode gameMode = GameMode.PlayerVsPlayer; // Chế độ chơi mặc định
    private int? difficultyLevel; // Mức độ khó (nếu có)
    private Match? match; // Đối tượng lưu trữ thông tin trận đấu

    private static Timer _timer = null!; // Timer để cập nhật đồng hồ trò chơi

    public ChessController(ChessModel model, ChessView view, GameMode gameMode, int? difficultyLevel = null)
    {
        this.model = model; // Khởi tạo Model
        this.view = view;   // Khởi tạo View
        this.gameMode = gameMode; // Chế độ chơi
        this.difficultyLevel = difficultyLevel; // Mức độ khó

        match = new Match
        {
            MatchDate = DateTime.Now,
            HistoryFEN = new List<string>(), // Danh sách lưu trữ lịch sử nước đi
            Result = "InProgress" // Trạng thái trận đấu
        };
        // Khởi tạo listener cho model mỗi khi có update từ board
        this.model.BoardUpdated += Rerender; // Đăng ký sự kiện để rerender view khi có cập nhật từ model
    }

    // Hàm để rerender view mỗi khi có cập nhật từ model
    private void Rerender()
    {
        view.Render(model, cursorX, cursorY); // Vẽ lại bàn cờ
        model.ChessTimer.PrintRemainingTime(); // In ra thời gian còn lại
    }

    // Chạy trò chơi
    public void Run()
    {
        model.NewGame(gameMode); // Bắt đầu trò chơi mới
        model.ChessTimer.Start(); // Bắt đầu đồng hồ

        StartTimerInterval(); // Bắt đầu đồng hồ định kỳ

        while (model.Result == GameResult.InProgress) // Khi trò chơi đang diễn ra
        {
            Rerender(); // Vẽ lại bàn cờ

            // Ghi lại FEN của trạng thái bàn cờ hiện tại
            string currentFEN = FEN.GetFEN(model.Board, model.GameState);
            match!.AddFEN(currentFEN); // Thêm trạng thái FEN vào lịch sử

            ListenKeyStroke(); // Lắng nghe phím bấm
        }

        // Tắt đồng hồ đếm giờ
        DisposeTimers();

        // Khi trò chơi kết thúc, lưu kết quả và xuất lịch sử
        string finalFEN = FEN.GetFEN(model.Board, model.GameState);
        match!.AddFEN(finalFEN); // Ghi lại FEN cuối cùng

        match!.Result = model.Result.ToString(); // Lưu kết quả trận đấu

        // Loại bỏ các chuỗi FEN trùng lặp 
        List<string> processedHistory = GameHistoryPostProcessor.ProcessGameHistory(match.HistoryFEN);
        match.HistoryFEN = processedHistory; // Cập nhật lịch sử trận đấu

        MatchHistoryManager.SaveMatch(match); // Xuất trận đấu ra file bằng cách sử dụng MatchHistoryManager

        ScreenManager.Instance.NavigateToScreen(
            new EndGameController(
                new EndGameView(),
                model.Result // Chuyển đến màn hình kết thúc trò chơi
            )
        );
    }

    private void StartTimerInterval()
    {
        // Tạo một timer có chu kỳ 1 giây (1000 milliseconds)
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimeInterval!; // Gán handler cho sự kiện
        _timer.AutoReset = true; // Đặt timer tự động khởi động lại
        _timer.Enabled = true; // Bắt đầu timer
    }

    private void OnTimeInterval(Object source, System.Timers.ElapsedEventArgs e)
    {
        model.ChessTimer.PrintRemainingTime(); // In ra thời gian còn lại khi timer tick
    }

    // Lắng nghe các phím bấm
    private void ListenKeyStroke()
    {
        ConsoleKey key = Console.ReadKey(true).Key; // Đọc phím bấm

        // Cập nhật vị trí con trỏ theo hướng di chuyển
        if (key == ConsoleKey.LeftArrow && cursorX > 0) cursorX--; // Di chuyển sang trái
        else if (key == ConsoleKey.RightArrow && cursorX < 7) cursorX++; // Di chuyển sang phải
        else if (key == ConsoleKey.UpArrow && cursorY > 0) cursorY--; // Di chuyển lên trên
        else if (key == ConsoleKey.DownArrow && cursorY < 7) cursorY++; // Di chuyển xuống dưới
        else if (key == ConsoleKey.Enter) HandleBoardClick(new Position(cursorY, cursorX)); // Xử lý nhấp chuột
        else if (key == ConsoleKey.Backspace) HandleNavigateBack(); // Xử lý nhấn phím quay lại
    }

    // Xử lý nhấp chuột vào ô
    private void HandleBoardClick(Position clickedSquare)
    {
        if (model.IsBotsTurn) return;  // Không cho người chơi click nếu là lượt của bot

        // Nếu nhấp vào nước đi hợp lệ, di chuyển đến đó
        if (model.HighlightedMoves.Contains(clickedSquare))
        {
            model.HandleMoveTo(clickedSquare); // Thực hiện nước đi
            return;
        }

        // Nếu nhấp vào quân cờ của mình, chọn nó
        Piece? clickedPiece = model.Board.GetPieceAt(clickedSquare);
        if (clickedPiece?.Color == model.GameState.CurrentPlayerColor)
        {
            model.Select(clickedSquare); // Chọn quân cờ
        }
        else // Bỏ chọn quân cờ
        {
            model.Deselect();
        }
    }

    private void HandleNavigateBack()
    {
        DisposeTimers(); // Dừng tất cả timers
        ScreenManager.Instance.BackToHomeScreen(); // Quay lại màn hình chính
    }

    private void DisposeTimers()
    {
        _timer.Enabled = false; // Tắt timer
        _timer.Stop(); // Dừng timer
        _timer.Dispose(); // Giải phóng tài nguyên
        model.ChessTimer.Dispose(); // Giải phóng đồng hồ trò chơi
    }
}
