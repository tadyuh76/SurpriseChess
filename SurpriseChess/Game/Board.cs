using System.Diagnostics;

namespace SurpriseChess;

// Lớp Position đại diện cho vị trí của quân cờ trên bàn cờ
public record Position(int Row, int Col);

public class Board : IBoardView, IPrototype<Board>
{
    private readonly Piece?[,] board = new Piece?[8, 8]; // Bàn cờ với 8 hàng và 8 cột
    private Dictionary<PieceColor, List<Piece>> capturedPieces = new Dictionary<PieceColor, List<Piece>>()
    {
        { PieceColor.White, new List<Piece>() },  // Lưu quân trắng bị bắt
        { PieceColor.Black, new List<Piece>() }   // Lưu quân đen bị bắt
    };
    private Dictionary<PieceColor, int> playerScores = new()
{
    { PieceColor.White, 0 },  // Điểm của người chơi quân trắng
    { PieceColor.Black, 0 }   // Điểm của người chơi quân đen
};
    public Dictionary<PieceColor, Dictionary<CastleDirection, Position>> RookStartingPositions { get; } = new()
    {
        [PieceColor.White] = new(),
        [PieceColor.Black] = new()
    };
    private readonly Random random = new(); // Khởi tạo đối tượng Random để xử lý các nước đi đặc biệt

    // Khởi tạo bàn cờ với cấu hình từ IBoardSetup
    public Board(IBoardSetup boardSetup)
    {
        boardSetup.SetUp(this);
        RookStartingPositions = GetRookStartingPositions(); // Lấy vị trí khởi đầu của quân tốt
    }

    // Hàm khởi tạo bản sao của bàn cờ
    private Board(Board boardState)
    {
        // Sao chép quân cờ
        foreach ((Position position, Piece piece) in boardState.LocatePieces())
        {
            board[position.Row, position.Col] = piece.Clone(); // Nhân bản quân cờ
        }

        // Sao chép vị trí khởi đầu của quân tốt
        foreach (var (color, directions) in boardState.RookStartingPositions)
        {
            foreach (var (direction, position) in directions)
            {
                RookStartingPositions[color][direction] = new Position(position.Row, position.Col);
            }
        }
    }

    // Lấy quân cờ tại vị trí đã cho
    public Piece? GetPieceAt(Position position) => board[position.Row, position.Col];

    // Đặt quân cờ tại vị trí đã cho
    public void SetPieceAt(Position position, Piece piece) => board[position.Row, position.Col] = piece;

    // Kiểm tra xem vị trí có nằm trong bàn cờ hay không
    public static bool IsInsideBoard(Position position) => (
        position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8
    );

    // Thực hiện nước đi từ vị trí nguồn đến vị trí đích
    public void MakeMove(Position source, Position destination)
    {
        Piece? pieceAtSource = board[source.Row, source.Col];
        if (pieceAtSource == null) throw new InvalidOperationException("Không có quân cờ ở vị trí nguồn");

        Piece? pieceAtDestination = board[destination.Row, destination.Col];

        if (ChessUtils.IsCastlingMove(pieceAtSource, pieceAtDestination))
        {
            // Xử lý nước đi nhập thành
            Castle(pieceAtSource, pieceAtDestination!, source, destination);
        }
        else
        {
            // Xử lý nước đi bình thường
            board[source.Row, source.Col] = null; // Xóa quân cờ tại vị trí nguồn
            board[destination.Row, destination.Col] = pieceAtSource; // Đặt quân cờ tại vị trí đích

            // Nếu có quân cờ tại vị trí đích, nghĩa là quân đó bị bắt
            if (pieceAtDestination != null)
            {
                CapturePiece(pieceAtDestination);  // Lưu quân bị bắt vào danh sách
            }

            // Xử lý các nước đi đặc biệt của quân tốt
            if (ChessUtils.IsEnPassantMove(source, destination, pieceAtSource, pieceAtDestination))
            {
                // Quân tốt bị bắt nằm ngay phía sau quân vừa di chuyển, xóa nó khỏi bàn cờ
                Piece? capturedPawn = board[source.Row, destination.Col];
                if (capturedPawn != null)
                {
                    CapturePiece(capturedPawn);  // Lưu quân tốt bị bắt qua đường vào danh sách
                    board[source.Row, destination.Col] = null;  // Xóa quân tốt khỏi bàn cờ
                }
            }
            else if (ChessUtils.IsPawnPromotionMove(pieceAtSource, destination))
            {
                PieceType newType = (PieceType)random.Next(1, 5);  // Chọn ngẫu nhiên loại quân mới
                board[destination.Row, destination.Col] = PieceFactory.Create(
                    pieceAtSource.Color, newType
                );
            }
        }
    }

    private void CapturePiece(Piece piece)
    {
        PieceColor opponentColor = piece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        capturedPieces[opponentColor].Add(piece);  // Thêm quân bị bắt vào danh sách của đối thủ
        playerScores[opponentColor] += ChessUtils.PiecePoints[piece.Type]; //Cộng điểm
    }  
    public List<Piece> GetCapturedPieces(PieceColor color)
    {
        return capturedPieces[color]; //danh sách các quân cờ bị bắt của từng màu
    }
    public int GetPlayerScore(PieceColor color)
    {
        return playerScores[color]; //điểm của người chơi theo màu
    }

    // Xác định vị trí các quân cờ trên bàn cờ
    public Dictionary<Position, Piece> LocatePieces(PieceColor? color = null, PieceType? type = null)
    {
        Dictionary<Position, Piece> piecesPositions = new();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board[row, col];
                if (piece == null) continue;

                bool matchesColor = color == null || piece.Color == color;
                bool matchesType = type == null || piece.Type == type;
                if (matchesColor && matchesType)
                {
                    piecesPositions[new Position(row, col)] = piece; // Lưu vị trí quân cờ
                }
            }
        }
        return piecesPositions;
    }

    // Nhân bản bàn cờ
    public Board Clone() => new(this);

    // Lấy vị trí khởi đầu của các quân tốt
    private Dictionary<PieceColor, Dictionary<CastleDirection, Position>> GetRookStartingPositions()
    {
        // Lưu trữ vị trí ban đầu của quân tốt
        Position[] rookPositions = LocatePieces(type: PieceType.Rook).Keys.ToArray();
        return new Dictionary<PieceColor, Dictionary<CastleDirection, Position>>
        {
            [PieceColor.Black] = new Dictionary<CastleDirection, Position>
            {
                [CastleDirection.QueenSide] = rookPositions[0],
                [CastleDirection.KingSide] = rookPositions[1]
            },
            [PieceColor.White] = new Dictionary<CastleDirection, Position>
            {
                [CastleDirection.QueenSide] = rookPositions[2],
                [CastleDirection.KingSide] = rookPositions[3]
            }
        };
    }

    // Xử lý nước đi nhập thành
    private void Castle(Piece king, Piece rook, Position kingPosition, Position rookPosition)
    {
        // Nếu quân tốt nằm bên phải quân vua, đó là nhập thành bên cánh vua; ngược lại là bên cánh hậu
        CastleDirection direction = kingPosition.Col < rookPosition.Col
            ? CastleDirection.KingSide
            : CastleDirection.QueenSide;
        int kingColAfterCastling = ChessUtils.ColAfterCastling[PieceType.King][direction];
        int rookColAfterCastling = ChessUtils.ColAfterCastling[PieceType.Rook][direction];

        board[kingPosition.Row, kingPosition.Col] = null; // Xóa quân vua
        board[rookPosition.Row, rookPosition.Col] = null; // Xóa quân xe

        board[kingPosition.Row, kingColAfterCastling] = king; // Đặt quân vua vào vị trí mới
        board[rookPosition.Row, rookColAfterCastling] = rook; // Đặt quân xe vào vị trí mới
    }
}
