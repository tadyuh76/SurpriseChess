using System.Diagnostics;

namespace SurpriseChess;

public class ChessModel
{
    public Board Board { get; private set; } = null!;
    private readonly IBoardSetup boardSetup;

    private readonly IChessBot chessBot;
    private Arbiter arbiter = null!;
    private EffectApplier effectApplier = null!;
 
    private readonly Random random = new();

    public event Action? BoardUpdated;

    public GameMode GameMode { get; private set; }
    public GameState GameState { get; private set; } = null!;
    public GameResult Result { get; private set; }

    public Position? SelectedPosition { get; private set; }
    public HashSet<Position> HighlightedMoves { get; private set; } = null!;

    public ChessModel(IBoardSetup boardSetup, IChessBot chessBot)
    {
        this.boardSetup = boardSetup; // Khởi tạo cấu hình bàn cờ.
        this.chessBot = chessBot; // Khởi tạo chess bot.
    }

    public void NewGame(GameMode gameMode)
    {
        Board = new Board(boardSetup);
        GameMode = gameMode;
        GameState = new(Board);
        Result = GameResult.InProgress;
        arbiter = new Arbiter(Board, GameState);
        effectApplier = new EffectApplier(Board);
        SelectedPosition = null;
        HighlightedMoves = new HashSet<Position>();
        effectApplier = new EffectApplier(Board);

    }

    public void Select(Position position)
    {
        SelectedPosition = position;
        Debug.Print($"Selecting {FEN.PositionToFEN(position)}");
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
        Debug.Print($"Moved to {FEN.PositionToFEN(destination)}");

        effectApplier.ClearEffects();
        effectApplier.ApplyEffects(destination);

        Result = arbiter.GetGameResult(GameState.CurrentPlayerColor);
        Deselect();

        // Check if the bot needs to make a move
        if (Result == GameResult.InProgress && IsBotsTurn)
        {
            HandleBotMove();
        }
    }

    public bool IsBotsTurn => (
        //Bot chỉ di chuyển ở chế độ 1 người chơi và quy ước bot mặc định là quân đen
        GameMode == GameMode.PlayerVsBot
        && GameState.CurrentPlayerColor == PieceColor.Black
    );

    public async void HandleBotMove()
    {
        (Position source, Position destination) = await GetBotMove();
        Debug.Print($"bot's gonna move: from {FEN.PositionToFEN(source)} to {FEN.PositionToFEN(destination)}");
        Select(source);
        BoardUpdated!.Invoke();
        
        await Task.Delay(1000);  // Wait 1s so the player has time to see the move
        HandleMoveTo(destination);
        BoardUpdated.Invoke();
    }


    private async Task<(Position, Position)> GetBotMove()
    {
        if (chessBot == null) throw new InvalidOperationException("Bot không được khởi tạo.");
        string fen = FEN.GetFEN(Board, GameState);
        List<(Position, Position)> bestMoves = await chessBot.GetBestMoves(fen);

        // The bot doesn't know the special effects (paralysis and shield),
        // so check if the bot's move is legal before making it
        Debug.Print($"legal moves at stockfish 1st move ({FEN.PositionToFEN(bestMoves[0].Item1)} {FEN.PositionToFEN(bestMoves[0].Item2)})");
        foreach(var move in arbiter.GetLegalMoves(bestMoves[0].Item1))
        {
            Debug.Print(FEN.PositionToFEN(move));
        }
        foreach ((Position source, Position destination) in bestMoves)
        {
            if (arbiter.GetLegalMoves(source).Contains(destination))
            {
                return (source, destination);
            }
        }
        Debug.Print("Bot is choosing a random move");
        // Nếu bot không lấy được nước đi từ Stockfish, đi một nước ngẫu nhiên
        return GetRandomMove();
    }

    private (Position, Position) GetRandomMove() //Hàm di chuyển nước đi ngẫu nhiên
    {
        List<(Position, Position)> legalMoves = new();
        foreach ((Position source, Piece _) in Board.LocatePieces(GameState.CurrentPlayerColor))
        {
            foreach (Position destination in arbiter.GetLegalMoves(source))
            {
                legalMoves.Add((source, destination));
            }
        }
        return legalMoves[random.Next(legalMoves.Count)];
    }
}
