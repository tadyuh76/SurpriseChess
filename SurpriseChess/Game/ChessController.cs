using SurpriseChess.Game;

namespace SurpriseChess;

internal class ChessController : IController
{
    private int cursorX = 0, cursorY = 7; // Vị trí con trỏ ban đầu trên bàn cờ
    private readonly ChessModel model; // Model trò chơi (MVC)
    private readonly ChessView view; // View trò chơi (MVC)

    private GameMode gameMode = GameMode.PlayerVsPlayer;
    private int? difficultyLevel;

    public ChessController(ChessModel model, ChessView view, GameMode gameMode, int? difficultyLevel = null)
    {
        this.model = model; // Khởi tạo Model
        this.view = view;   // Khởi tạo View
        this.gameMode = gameMode; // Chế độ chơi
        this.difficultyLevel = difficultyLevel; // Mức độ khó
    }

    // Chạy trò chơi
    public void Run()
    {
        model.NewGame(gameMode); // Bắt đầu trò chơi mới

        while (model.Result == GameResult.InProgress) // Khi trò chơi đang diễn ra
        {
            Board board = model.Board;
            Position? selectedPosition = model.SelectedPosition;
            HashSet<Position> highlightedMoves = model.HighlightedMoves;
            PieceColor currentPlayerColor = model.GameState.CurrentPlayerColor;

            view.Render(board, selectedPosition, highlightedMoves, currentPlayerColor, cursorX, cursorY); // Vẽ bàn cờ
            ListenKeyStroke(); // Lắng nghe phím bấm
            Thread.Sleep(10);
        }
    }

    // Lắng nghe các phím bấm
    private void ListenKeyStroke()
    {
        ConsoleKey key = Console.ReadKey(true).Key; // Đọc phím bấm

        // Cập nhật vị trí con trỏ theo hướng di chuyển
        if (key == ConsoleKey.LeftArrow && cursorX > 0) cursorX--;
        else if (key == ConsoleKey.RightArrow && cursorX < 7) cursorX++;
        else if (key == ConsoleKey.UpArrow && cursorY > 0) cursorY--;
        else if (key == ConsoleKey.DownArrow && cursorY < 7) cursorY++;
        else if (key == ConsoleKey.Enter) HandleBoardClick(new Position(cursorY, cursorX));
        else if (key == ConsoleKey.Backspace) HandleNavigateBack();
    }

    // Xử lý nhấp chuột vào ô
    private void HandleBoardClick(Position clickedSquare)
    {
        // Nếu nhấp vào nước đi hợp lệ, di chuyển đến đó
        if (model.HighlightedMoves.Contains(clickedSquare))
        {
            model.HandleMoveTo(clickedSquare);
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

    private void HandleNavigateBack()
    {
        // Yêu cầu người dung xác nhận thoát trò chơi
        ConsoleKey keyPressed;

        keyPressed = Console.ReadKey().Key;

        if (keyPressed == ConsoleKey.Backspace)
        {
            //Dừng bộ đếm thời gian
            view.StopTimer();
            // Trở về màn hình chính
            ScreenManager.Instance.NavigateToScreen(new HomeController(
                new HomeModel(),
                new HomeView()
            ));
        }
    }
}
