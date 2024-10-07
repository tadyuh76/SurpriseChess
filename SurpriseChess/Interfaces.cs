namespace SurpriseChess;

// Giao diện IPrototype cho phép sao chép đối tượng
public interface IPrototype<T>
{
    T Clone(); // Phương thức để tạo bản sao của đối tượng
}

// Giao diện IBoardView định nghĩa các phương thức để xem trạng thái của bàn cờ
public interface IBoardView
{
    Piece? GetPieceAt(Position position); // Lấy quân cờ tại vị trí nhất định (có thể null nếu không có quân)
    Dictionary<Position, Piece> LocatePieces(PieceColor? color = null, PieceType? type = null); // Tìm quân cờ theo màu hoặc loại
    Dictionary<PieceColor, Dictionary<CastleDirection, Position>> RookStartingPositions { get; } // Vị trí khởi đầu của các quân xe
}

// Giao diện IBoardSetup định nghĩa phương thức để thiết lập bàn cờ
public interface IBoardSetup
{
    void SetUp(Board board); // Thiết lập bàn cờ theo quy tắc
}

// Giao diện IChessBot định nghĩa phương thức để lấy các nước đi tốt nhất
public interface IChessBot
{
    Task<List<(Position, Position)>> GetBestMoves(string fen); // Lấy danh sách các nước đi tốt nhất từ chuỗi FEN
}

// Giao diện IController định nghĩa phương thức cho điều khiển
public interface IController
{
    void Run(); // Phương thức chạy điều khiển
}
