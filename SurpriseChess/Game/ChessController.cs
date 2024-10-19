using SurpriseChess.MatchHistory;

namespace SurpriseChess;

internal class ChessController : IController
{
    private int cursorX = 0, cursorY = 7; // Vị trí con trỏ ban đầu trên bàn cờ
    private readonly ChessModel model; // Model trò chơi (MVC)
    private readonly ChessView view; // View trò chơi (MVC)

    private GameMode gameMode = GameMode.PlayerVsPlayer;
    private int? difficultyLevel;
    private Match? match;

    public ChessController(ChessModel model, ChessView view, GameMode gameMode, int? difficultyLevel = null)
    {
        this.model = model; // Khởi tạo Model
        this.view = view;   // Khởi tạo View
        this.gameMode = gameMode; // Chế độ chơi
        this.difficultyLevel = difficultyLevel; // Mức độ khó

        match = new Match
        {
            MatchDate = DateTime.Now,
            HistoryFEN = new List<string>(), // Khởi tạo danh sách trống ban đầu
            Result = "InProgress" // Trạng thái trận đấu ban đầu
        };
    }
   
    // Chạy trò chơi
    public void Run()
    {
        model.NewGame(gameMode); // Bắt đầu trò chơi mới

        while (model.Result == GameResult.InProgress) // Khi trò chơi đang diễn ra
        {
            RenderView();

            // Ghi lại FEN của trạng thái bàn cờ hiện tại
            string currentFEN = FEN.GetFEN(model.Board, model.GameState);

            if (match != null)
            {
                // Sử dụng AddFEN với một danh sách FEN (dù chỉ là một FEN ở đây)
                match!.AddFEN(new List<string> { currentFEN });
            }

            ListenKeyStroke(); // Lắng nghe phím bấm

        }
        if (match != null)
        {
            // Khi trò chơi kết thúc, lưu kết quả và xuất lịch sử
            match.Result = model.Result.ToString();
            // Loại bỏ các chuỗi FEN trùng lặp vaf
            List<string> processedHistory = GameHistoryPostProcessor.ProcessGameHistory(match.HistoryFEN);

            MatchHistoryManager.SaveMatch(match); // Xuất trận đấu ra file bằng cách sử dụng MatchHistoryManager
        }
        // Hiển thị màn hình kết thúc bàn cờ dựa với kết quả tương ứng
        ScreenManager.Instance.NavigateToScreen(
            new EndGameController(
                new EndGameView(),
                model.Result
            )    
        );

    }

    private void RenderView()
    {
        Board board = model.Board;
        Position? selectedPosition = model.SelectedPosition;
        HashSet<Position> highlightedMoves = model.HighlightedMoves;
        PieceColor currentPlayerColor = model.GameState.CurrentPlayerColor;

        view.Render(board, selectedPosition, highlightedMoves, currentPlayerColor, cursorX, cursorY); // Vẽ bàn cờ
    }

    // Lắng nghe các phím bấm
    private void ListenKeyStroke()
    {
        ConsoleKey key = Console.ReadKey().Key; // Đọc phím bấm

        // Cập nhật vị trí con trỏ theo hướng di chuyển
        if (key == ConsoleKey.LeftArrow && cursorX > 0) cursorX--;
        else if (key == ConsoleKey.RightArrow && cursorX < 7) cursorX++;
        else if (key == ConsoleKey.UpArrow && cursorY > 0) cursorY--;
        else if (key == ConsoleKey.DownArrow && cursorY < 7) cursorY++;
        else if (key == ConsoleKey.Enter) HandleBoardClick(new Position(cursorY, cursorX));
        else if (key == ConsoleKey.Backspace) ScreenManager.Instance.BackToHomeScreen();
    }

    // Xử lý nhấp chuột vào ô
    private async Task HandleBoardClick(Position clickedSquare)
    {
        if (model.IsBotsTurn) return;  // Không cho người chơi click nếu là lượt của bot

        // Nếu nhấp vào nước đi hợp lệ, di chuyển đến đó
        if (model.HighlightedMoves.Contains(clickedSquare))
        {
            await model.HandleMoveTo(clickedSquare);
            RenderView();
            return;
        }

        // Nếu nhấp vào quân cờ của mình, chọn nó
        Piece? clickedPiece = model.Board.GetPieceAt(clickedSquare);
        if (clickedPiece?.Color == model.GameState.CurrentPlayerColor)
        {
            model.Select(clickedSquare);
        }
        else // Bỏ chọn quân cờ
        {
            model.Deselect();
        }
    }    
}
