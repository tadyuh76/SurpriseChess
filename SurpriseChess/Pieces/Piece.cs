namespace SurpriseChess;

// Lớp trừu tượng đại diện cho một quân cờ trong trò chơi
public abstract class Piece : IPrototype<Piece>
{
    // Màu sắc của quân cờ
    public PieceColor Color { get; }
    // Loại quân cờ
    public PieceType Type { get; }
    // Thuộc tính xác định xem quân có đang được tàng hình không
    public bool IsInvisible { get; set; } = false;
    // Thuộc tính xác định xem quân có bị tê liệt không
    public bool IsParalyzed { get; set; } = false;
    // Thuộc tính xác định xem quân có được bảo vệ không
    public bool IsShielded { get; set; } = false;
    // Ký hiệu hiển thị cho quân cờ (biểu tượng)
    public abstract string DisplaySymbol { get; }

    // Khởi tạo quân cờ với màu sắc và loại
    protected Piece(PieceColor color, PieceType type)
    {
        Color = color;
        Type = type;
    }

    // Lấy tất cả các nước di chuyển có thể của quân từ một vị trí dựa trên trạng thái bàn cờ
    // Không xem xét liệu vua có đang bị tấn công hay không
    public abstract List<Position> GetMoves(
        IBoardView board, Position currentPosition, GameState gameState
    );

    // Tạo một bản sao sâu của quân cờ
    public Piece Clone()
    {
        Piece newPiece = PieceFactory.Create(Color, Type);
        // Sao chép thuộc tính của quân cờ
        newPiece.IsInvisible = IsInvisible;  
        newPiece.IsParalyzed = IsParalyzed;
        newPiece.IsShielded = IsShielded; 

        return newPiece;  // Trả về bản sao mới
    }
}
