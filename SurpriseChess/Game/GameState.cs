namespace SurpriseChess;

// Quản lý người chơi hiện tại, quyền nhập thành và en passant
public class GameState
{
    public PieceColor CurrentPlayerColor { get; private set; } // Màu của người chơi hiện tại
    public Position? EnPassantPosition { get; private set; } // Vị trí en passant nếu có
    public Dictionary<PieceColor, Dictionary<CastleDirection, bool>> CanCastle { get; private set; } // Quyền nhập thành
    private readonly Board board; // Bàn cờ
    public int HalfMoveClock { get; private set; } // Đồng hồ di chuyển (nước đi không ăn quân)
    public int FullMoveNumber { get; private set; } // Số lượt di chuyển hoàn chỉnh

    // Khởi tạo GameState với bàn cờ
    public GameState(Board board)
    {
        CurrentPlayerColor = PieceColor.White; // Người chơi đầu tiên là trắng
        CanCastle = ChessUtils.InitialCastlingRights; // Quyền nhập thành ban đầu
        this.board = board;
        HalfMoveClock = 0; // Khởi tạo đồng hồ di chuyển
        FullMoveNumber = 1; // Khởi tạo số lượt di chuyển
    }

    // Cập nhật trạng thái sau khi một nước đi được thực hiện
    public void UpdateStateAfterMove(Position source, Position destination)
    {
        Piece? pieceAtSource = board.GetPieceAt(source); // Lấy quân cờ tại vị trí nguồn
        Piece? pieceAtDestination = board.GetPieceAt(destination); // Lấy quân cờ tại vị trí đích

        if (pieceAtSource == null) throw new InvalidOperationException("Không có quân cờ ở vị trí nguồn");

        // Cập nhật quyền nhập thành và en passant
        UpdateEnPassantRights(pieceAtSource, source, destination);
        UpdateCastlingRights(pieceAtSource, source);
        UpdateMoveCounters(pieceAtSource, pieceAtDestination);
        SwitchPlayer(); // Chuyển lượt cho người chơi
    }

    // Cập nhật quyền nhập thành dựa vào nước đi
    private void UpdateCastlingRights(Piece piece, Position source)
    {
        if (piece.Type == PieceType.King)  // Nếu quân vua di chuyển, vô hiệu hóa quyền nhập thành
        {
            CanCastle[piece.Color][CastleDirection.KingSide] = false;
            CanCastle[piece.Color][CastleDirection.QueenSide] = false;
        }
        else if (piece.Type == PieceType.Rook)  // Nếu quân xe di chuyển, vô hiệu hóa quyền nhập thành cho bên tương ứng
        {
            if (source == board.RookStartingPositions[piece.Color][CastleDirection.KingSide])
            {
                CanCastle[piece.Color][CastleDirection.KingSide] = false;
            }
            else if (source == board.RookStartingPositions[piece.Color][CastleDirection.QueenSide])
            {
                CanCastle[piece.Color][CastleDirection.QueenSide] = false;
            }
        }
    }

    // Cập nhật quyền en passant khi có quân tốt di chuyển
    private void UpdateEnPassantRights(Piece piece, Position source, Position destination)
    {
        bool isPawnDoubleMove = (
            piece.Type == PieceType.Pawn
            && Math.Abs(source.Row - destination.Row) == 2 // Quân tốt di chuyển 2 ô
        );

        // Nếu quân tốt di chuyển hai ô về phía trước, thiết lập quyền en passant
        EnPassantPosition = isPawnDoubleMove
            ? new Position((source.Row + destination.Row) / 2, source.Col) // Tính toán vị trí en passant
            : null;
    }

    private void UpdateMoveCounters(Piece movingPiece, Piece? capturedPiece)
    {
        // Reset đồng hồ nếu di chuyển quân tốt hoặc ăn quân
        if (movingPiece.Type == PieceType.Pawn || capturedPiece != null)
        {
            HalfMoveClock = 0;
        }
        else
        {
            HalfMoveClock++; // Tăng đồng hồ nếu không có ăn quân
        }

        // Tăng số lượt di chuyển nếu người chơi hiện tại là đen
        if (CurrentPlayerColor == PieceColor.Black)
        {
            FullMoveNumber++;
        }
    }

    // Chuyển lượt cho người chơi
    private void SwitchPlayer()
    {
        CurrentPlayerColor = CurrentPlayerColor == PieceColor.White ? PieceColor.Black : PieceColor.White; // Chuyển đổi giữa trắng và đen
    }
}
