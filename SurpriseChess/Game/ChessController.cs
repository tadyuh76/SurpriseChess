namespace SurpriseChess;

internal class ChessController : IController
{
    private int cursorX = 0, cursorY = 7; // Vị trí con trỏ ban đầu trên bàn cờ
    private readonly ChessModel model; // Model trò chơi (MVC)
    private readonly ChessView view; // View trò chơi (MVC)

    public ChessController(ChessModel model, ChessView view)
    {
        this.model = model; // Khởi tạo Model
        this.view = view;   // Khởi tạo View
    }

    // Chạy trò chơi
    public void Run()
    {
        model.NewGame(GameMode.PlayerVsPlayer); // Bắt đầu trò chơi mới

        while (model.Result == GameResult.InProgress) // Khi trò chơi đang diễn ra
        {
            Board board = model.Board;
            Position? selectedPosition = model.SelectedPosition;
            HashSet<Position> highlightedMoves = model.HighlightedMoves;
            PieceColor currentPlayerColor = model.GameState.CurrentPlayerColor;

            view.Render(board, selectedPosition, highlightedMoves, currentPlayerColor, cursorX, cursorY); // Vẽ bàn cờ
            ListenKeyStroke(); // Lắng nghe phím bấm
        }
    }

    // Lắng nghe các phím bấm
    private void ListenKeyStroke()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(); // Đọc phím bấm

        // Cập nhật vị trí con trỏ theo hướng di chuyển
        if (keyInfo.Key == ConsoleKey.LeftArrow && cursorX > 0) cursorX--;
        else if (keyInfo.Key == ConsoleKey.RightArrow && cursorX < 7) cursorX++;
        else if (keyInfo.Key == ConsoleKey.UpArrow && cursorY > 0) cursorY--;
        else if (keyInfo.Key == ConsoleKey.DownArrow && cursorY < 7) cursorY++;
        else if (keyInfo.Key == ConsoleKey.Enter) HandleBoardClick(new Position(cursorY, cursorX));
        else if (keyInfo.Key == ConsoleKey.Backspace) HandleNavigateBack();
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
        ConsoleKey keyPressed;
        do
        {
            keyPressed = Console.ReadKey(true).Key;

            if (keyPressed == ConsoleKey.Backspace)
            {
                // Return to Home Screen
                ScreenManager.Instance.NavigateToScreen(new HomeController(
                    new HomeModel(),
                    new HomeView()
                ));
                break;
            }

            // Implement additional key handling if needed
        } while (true);
    }
}
