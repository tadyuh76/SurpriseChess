using System.Diagnostics;

namespace SurpriseChess;

public class ChessModel
{
    public Board Board { get; private set; } = null!; // Bàn cờ
    private readonly IBoardSetup boardSetup; // Cấu hình bàn cờ

    private readonly IChessBot chessBot; // Bot cờ
    private Arbiter Arbiter = null!; // Đối tượng Arbiter kiểm tra trạng thái trò chơi
    private EffectApplier EffectApplier = null!; // Áp dụng hiệu ứng cho quân cờ

    private readonly Random random = new(); // Đối tượng Random để chọn nước đi ngẫu nhiên

    public event Action? BoardUpdated; // Sự kiện thông báo khi bàn cờ được cập nhật

    public GameMode GameMode { get; private set; } // Chế độ trò chơi
    public GameState GameState { get; private set; } = null!; // Trạng thái trò chơi
    public GameResult Result { get; set; } // Kết quả trò chơi

    public Position? SelectedPosition { get; private set; } // Vị trí quân cờ được chọn
    public HashSet<Position> HighlightedMoves { get; private set; } = null!; // Các nước đi hợp lệ được đánh dấu
    public ChessTimer ChessTimer { get; private set; } = null!; // Đồng hồ trò chơi

    // Constructor khởi tạo đối tượng ChessModel
    public ChessModel(IBoardSetup boardSetup, IChessBot chessBot)
    {
        this.boardSetup = boardSetup; // Khởi tạo cấu hình bàn cờ.
        this.chessBot = chessBot; // Khởi tạo chess bot.
    }

    // Phương thức khởi tạo trò chơi mới
    public void NewGame(GameMode gameMode)
    {
        Board = new Board(boardSetup); // Khởi tạo bàn cờ mới
        GameMode = gameMode; // Thiết lập chế độ trò chơi
        GameState = new(Board); // Tạo trạng thái trò chơi mới
        Result = GameResult.InProgress; // Trạng thái trò chơi là đang diễn ra
        Arbiter = new Arbiter(Board, GameState); // Khởi tạo Arbiter
        EffectApplier = new EffectApplier(Board); // Khởi tạo EffectApplier
        SelectedPosition = null; // Không có quân nào được chọn
        HighlightedMoves = new HashSet<Position>(); // Không có nước đi nào được đánh dấu
        ChessTimer = new ChessTimer(TimeSpan.FromMinutes(0.1)); // Khởi tạo đồng hồ trò chơi với 15 phút cho mỗi người chơi
    }

    // Phương thức chọn quân cờ
    public void Select(Position position)
    {
        SelectedPosition = position; // Lưu vị trí được chọn
        Debug.Print($"Selecting {FEN.PositionToFEN(position)}"); // In ra vị trí được chọn
        HighlightedMoves = Arbiter.GetLegalMoves(position); // Lấy các nước đi hợp lệ từ vị trí đã chọn
    }

    // Phương thức bỏ chọn quân cờ
    public void Deselect()
    {
        SelectedPosition = null; // Bỏ chọn quân
        HighlightedMoves.Clear(); // Xóa các nước đi đã được đánh dấu
    }

    // Xử lý nước đi tới vị trí đích
    public void HandleMoveTo(Position destination)
    {
        if (SelectedPosition == null) return; // Nếu không có quân nào được chọn thì không làm gì


        GameState.UpdateStateAfterMove(SelectedPosition, destination); // Cập nhật trạng thái trò chơi
        Board.MakeMove(SelectedPosition, destination); // Thực hiện nước đi trên bàn cờ
        Debug.Print($"Moved to {FEN.PositionToFEN(destination)}"); // In ra nước đi đã thực hiện

        EffectApplier.ClearEffects(); // Xóa các hiệu ứng trước đó
        EffectApplier.ApplyEffects(destination); // Áp dụng các hiệu ứng tại vị trí đích

        Result = Arbiter.GetGameResult(GameState.CurrentPlayerColor); // Lấy kết quả trò chơi
        Deselect(); // Bỏ chọn quân cờ

        if (Result != GameResult.InProgress) // Nếu trò chơi đã kết thúc
        {
            ChessTimer.Dispose(); // Dừng đồng hồ
        } else
        {
            ChessTimer.UpdateTurn(); // Cập nhật lượt chơi
        }

        // Kiểm tra nếu bot cần thực hiện nước đi
        if (Result == GameResult.InProgress && IsBotsTurn)
        {
            HandleBotMove(); // Thực hiện nước đi của bot
        }
    }

    // Kiểm tra xem có phải lượt của bot không
    public bool IsBotsTurn => (
        // Bot chỉ di chuyển ở chế độ một người chơi và quy ước bot mặc định là quân đen
        GameMode == GameMode.PlayerVsBot
        && GameState.CurrentPlayerColor == PieceColor.Black
    );

    // Xử lý nước đi của bot
    public async void HandleBotMove()
    {
        (Position source, Position destination) = await GetBotMove(); // Lấy nước đi của bot
        Debug.Print($"bot's gonna move: from {FEN.PositionToFEN(source)} to {FEN.PositionToFEN(destination)}");
        Select(source); // Chọn quân bot
        BoardUpdated!.Invoke(); // Thông báo cập nhật bàn cờ

        await Task.Delay(1000);  // Chờ 1 giây để người chơi có thời gian nhìn thấy nước đi
        HandleMoveTo(destination); // Thực hiện nước đi
        BoardUpdated.Invoke(); // Thông báo cập nhật bàn cờ
    }

    // Lấy nước đi tốt nhất của bot
    private async Task<(Position, Position)> GetBotMove()
    {
        if (chessBot == null) throw new InvalidOperationException("Bot không được khởi tạo.");
        string fen = FEN.GetFEN(Board, GameState); // Lấy FEN của bàn cờ hiện tại
        List<(Position, Position)> bestMoves = await chessBot.GetBestMoves(fen); // Lấy các nước đi tốt nhất từ bot

        // Kiểm tra nếu nước đi của bot là hợp lệ trước khi thực hiện
        Debug.Print($"legal moves at stockfish 1st move ({FEN.PositionToFEN(bestMoves[0].Item1)} {FEN.PositionToFEN(bestMoves[0].Item2)})");
        foreach (var move in Arbiter.GetLegalMoves(bestMoves[0].Item1))
        {
            Debug.Print(FEN.PositionToFEN(move)); // In ra các nước đi hợp lệ
        }
        foreach ((Position source, Position destination) in bestMoves)
        {
            if (Arbiter.GetLegalMoves(source).Contains(destination)) // Kiểm tra tính hợp lệ
            {
                return (source, destination); // Trả về nước đi hợp lệ
            }
        }
        Debug.Print("Bot is choosing a random move");
        // Nếu bot không lấy được nước đi từ Stockfish, chọn một nước đi ngẫu nhiên
        return GetRandomMove();
    }

    // Hàm di chuyển nước đi ngẫu nhiên
    private (Position, Position) GetRandomMove()
    {
        List<(Position, Position)> legalMoves = new();
        foreach ((Position source, Piece _) in Board.LocatePieces(GameState.CurrentPlayerColor))
        {
            foreach (Position destination in Arbiter.GetLegalMoves(source))
            {
                legalMoves.Add((source, destination)); // Thêm các nước đi hợp lệ vào danh sách
            }
        }
        return legalMoves[random.Next(legalMoves.Count)]; // Trả về một nước đi ngẫu nhiên
    }
}
