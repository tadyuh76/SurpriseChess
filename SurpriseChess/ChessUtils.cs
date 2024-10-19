namespace SurpriseChess;

// Lớp tiện ích cờ vua
public static class ChessUtils
{
    // Phương thức để lấy màu đối thủ
    public static PieceColor OpponentColor(PieceColor color) => (
        color == PieceColor.White ? PieceColor.Black : PieceColor.White
    );

    // Kiểm tra xem đây có phải là nước đi tướng nhập thành hay không
    public static bool IsCastlingMove(Piece pieceAtSource, Piece? pieceAtDestination) => (
        pieceAtSource.Type == PieceType.King  // Kiểm tra xem quân cờ là vua
        && pieceAtDestination != null  // Đích đến không được null
        && pieceAtDestination.Type == PieceType.Rook  // Kiểm tra xem quân cờ tại đích là xe
        && pieceAtDestination.Color == pieceAtSource.Color  // Cùng màu
    );

    // Kiểm tra xem đây có phải là nước đi en passant hay không
    public static bool IsEnPassantMove
    (
        Position source,
        Position destination,
        Piece pieceAtSource,
        Piece? pieceAtDestination
    ) => (
        pieceAtSource.Type == PieceType.Pawn  // Kiểm tra xem quân cờ là tốt
        && destination.Col != source.Col  // Nước đi chéo
        && pieceAtDestination == null  // Không phải là bắt quân cờ theo đường chéo bình thường
    );

    // Kiểm tra xem đây có phải là nước đi thăng cấp tốt hay không
    public static bool IsPawnPromotionMove(Piece piece, Position destination) => (
        piece.Type == PieceType.Pawn  // Kiểm tra xem quân cờ là tốt
        && (destination.Row == 0 || destination.Row == 7)  // Đích đến ở hàng 0 hoặc hàng 7
    );

    // Từ điển lưu trữ vị trí cột sau khi thực hiện nhập thành
    public static readonly Dictionary<PieceType, Dictionary<CastleDirection, int>> ColAfterCastling = new()
    {
        [PieceType.King] = new Dictionary<CastleDirection, int>
        {
            [CastleDirection.KingSide] = 6,  // Vị trí cột của vua bên cánh vua
            [CastleDirection.QueenSide] = 2  // Vị trí cột của vua bên cánh hậu
        },
        [PieceType.Rook] = new Dictionary<CastleDirection, int>
        {
            [CastleDirection.KingSide] = 5,  // Vị trí cột của xe bên cánh vua
            [CastleDirection.QueenSide] = 3  // Vị trí cột của xe bên cánh hậu
        }
    };

    // Từ điển lưu trữ quyền nhập thành ban đầu cho từng màu cờ
    public static readonly Dictionary<PieceColor, Dictionary<CastleDirection, bool>> InitialCastlingRights = new()
    {
        [PieceColor.White] = new Dictionary<CastleDirection, bool>
        {
            [CastleDirection.KingSide] = true,  // Quyền nhập thành cánh vua cho quân trắng
            [CastleDirection.QueenSide] = true  // Quyền nhập thành cánh hậu cho quân trắng
        },
        [PieceColor.Black] = new Dictionary<CastleDirection, bool>
        {
            [CastleDirection.KingSide] = true,  // Quyền nhập thành cánh vua cho quân đen
            [CastleDirection.QueenSide] = true  // Quyền nhập thành cánh hậu cho quân đen
        }
    };

    // Từ điển biểu tượng cảm xúc cho các quân cờ đen
    public static Dictionary<string, string> BlackPieceEmojis = new()
    {
        { "Vua", "🦁" },
        { "Hậu", "🐯" },
        { "Xe", "🐻" },
        { "Tượng", "🦉" },
        { "Mã", "🐴" },
        { "Tốt", "🐹" },
    };

    // Từ điển biểu tượng cảm xúc cho các quân cờ trắng
    public static Dictionary<string, string> WhitePieceEmojis = new()
    {
        { "Vua", "🤴" },
        { "Hậu", "👸" },
        { "Xe", "🏰" },
        { "Tượng", "🥷" },
        { "Mã", "🏇" },
        { "Tốt", "💂" },

    };
    public static Dictionary<PieceType, int> PiecePoints = new()
    {
        { PieceType.Pawn, 1 },       // Quân tốt
        { PieceType.Knight, 3 },     // Quân mã
        { PieceType.Bishop, 3 },     // Quân tượng
        { PieceType.Rook, 5 },       // Quân xe
        { PieceType.Queen, 9 },      // Quân hậu
        { PieceType.King, 0 }        // Quân vua không được tính điểm vì không thể bị bắt
    };
}
