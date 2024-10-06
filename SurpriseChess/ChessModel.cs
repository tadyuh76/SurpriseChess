namespace SurpriseChess;

public class ChessModel
{
    public Board Board { get; private set; } = null!;
    private readonly IBoardSetup boardSetup;
    private Arbiter arbiter = null!;
    private readonly Random random = new();

    public GameMode GameMode { get; private set; }
    public GameState GameState { get; private set; } = null!;
    public GameResult Result { get; private set; }

    public Position? SelectedPosition { get; private set; }
    public HashSet<Position> HighlightedMoves { get; private set; } = null!;

    public ChessModel(IBoardSetup boardSetup)
    {
        this.boardSetup = boardSetup; // Khởi tạo cấu hình bàn cờ.
    }

    // Bắt đầu một trò chơi mới với chế độ đã chỉ định.
    public void NewGame(GameMode gameMode)
    {
        Board = new Board(boardSetup);
        GameMode = gameMode;
        GameState = new(Board);
        Result = GameResult.InProgress;
        arbiter = new Arbiter(Board, GameState);
        SelectedPosition = null;
        HighlightedMoves = new HashSet<Position>();
    }

    // Chọn một quân cờ tại vị trí cho trước và làm nổi bật các nước đi hợp lệ.
    public void Select(Position position)
    {
        SelectedPosition = position;
        HighlightedMoves = arbiter.GetLegalMoves(position);
    }

    // Bỏ chọn quân cờ hiện tại và xóa các nước đi gợi ý.
    public void Deselect()
    {
        SelectedPosition = null;
        HighlightedMoves.Clear();
    }

    // Di chuyển quân cờ được chọn tới vị trí đích đã chỉ định và cập nhật trạng thái trò chơi.
    public void HandleMoveTo(Position destination)
    {
        if (SelectedPosition == null) return;

        GameState.UpdateStateAfterMove(SelectedPosition, destination);
        Board.MakeMove(SelectedPosition, destination);
        Result = arbiter.GetGameResult(GameState.CurrentPlayerColor);
        Deselect();
    }
}
