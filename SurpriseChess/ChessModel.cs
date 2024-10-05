namespace SurpriseChess;

public class ChessModel
{
    public Board Board { get; private set; } = null!;
    private readonly IBoardSetup boardSetup;
    //private readonly IChessBot chessBot;
    private Arbiter arbiter = null!;
    //private EffectApplier effectApplier = null!;
    private readonly Random random = new();

    public GameMode GameMode { get; private set; }
    public GameState GameState { get; private set; } = null!;
    public GameResult Result { get; private set; }

    public Position? SelectedPosition { get; private set; }
    public HashSet<Position> HighlightedMoves { get; private set; } = null!;

    public ChessModel(IBoardSetup boardSetup)
    {
        this.boardSetup = boardSetup;
    }

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

    public void Select(Position position)
    {
        SelectedPosition = position;
        HighlightedMoves = arbiter.GetLegalMoves(position);
    }

    public void Deselect()
    {
        SelectedPosition = null;
        HighlightedMoves.Clear();
    }

    public void HandleMoveTo(Position destination)
    {
        if (SelectedPosition == null) return;

        GameState.UpdateStateAfterMove(SelectedPosition, destination);
        Board.MakeMove(SelectedPosition, destination);

        Result = arbiter.GetGameResult(GameState.CurrentPlayerColor);
        Deselect();
    }

}
