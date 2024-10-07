namespace SurpriseChess;

// Chịu trách nhiệm xác định tính hợp lệ của các nước đi và kết quả của trò chơi
public class Arbiter
{
    private readonly Board board; // Bàn cờ hiện tại
    private readonly GameState gameState; // Trạng thái của trò chơi

    // Khởi tạo Arbiter với bàn cờ và trạng thái trò chơi
    public Arbiter(Board board, GameState gameState)
    {
        this.board = board;
        this.gameState = gameState;
    }

    // Trả về tất cả các nước đi hợp lệ từ một vị trí cho trước
    // Một nước đi là hợp lệ nếu không để vua của người chơi bị chiếu
    public HashSet<Position> GetLegalMoves(Position source)
    {
        HashSet<Position> legalMoves = new();
        Piece? piece = board.GetPieceAt(source);
        if (piece == null) return legalMoves;  // Không có quân cờ nào để di chuyển

        Position currentKingPosition = LocateKing(piece.Color);
        foreach (Position destination in piece.GetMoves(board, source, gameState))
        {
            if (IsLegalMove(piece, source, destination, currentKingPosition))
            {
                legalMoves.Add(destination); // Thêm nước đi hợp lệ vào danh sách
            }
        }

        return legalMoves; // Trả về danh sách các nước đi hợp lệ
    }

    // Kiểm tra tính hợp lệ của một nước đi
    private bool IsLegalMove(
        Piece piece,
        Position source,
        Position destination,
        Position currentKingPosition
    )
    {
        if (ChessUtils.IsCastlingMove(piece, board.GetPieceAt(destination)))
        {
            return CanCastleSafely(piece.Color, currentKingPosition, destination.Col); // Kiểm tra nhập thành an toàn
        }

        return !MoveLeavesKingInCheck(piece, source, destination, currentKingPosition); // Kiểm tra không để vua bị chiếu
    }

    // Tìm vị trí của vua
    private Position LocateKing(PieceColor color)
    {
        (Position position, Piece _) = board.LocatePieces(color, PieceType.King).First();
        return position; // Trả về vị trí của vua
    }

    // Kiểm tra xem đường đi của vua trong nhập thành có bị tấn công không
    private bool CanCastleSafely(
        PieceColor kingColor,
        Position currentKingPosition,
        int destinationCol
    )
    {
        int leftCol = Math.Min(currentKingPosition.Col, destinationCol);
        int rightCol = Math.Max(currentKingPosition.Col, destinationCol);
        for (int col = leftCol; col <= rightCol; col++)
        {
            Position position = new(currentKingPosition.Row, col);
            if (IsPositionUnderAttack(kingColor, position)) return false; // Có quân địch tấn công
        }
        return true; // Nhập thành an toàn
    }

    // Kiểm tra xem nước đi có để vua bị chiếu không
    private bool MoveLeavesKingInCheck(
        Piece piece,
        Position source,
        Position destination,
        Position currentKingPosition
    )
    {
        // Xác định vị trí vua sau khi di chuyển
        Position kingPosition;
        if (piece.Type == PieceType.King) kingPosition = destination; // Nếu là vua, vị trí là điểm đến
        else kingPosition = currentKingPosition; // Ngược lại, giữ nguyên

        // Mô phỏng nước đi trên một bản sao của bàn cờ
        Board tempBoard = board.Clone();
        tempBoard.MakeMove(source, destination); // Thực hiện nước đi
        bool isKingInCheck = IsPositionUnderAttack(piece.Color, kingPosition, tempBoard); // Kiểm tra vua có bị chiếu không

        return isKingInCheck; // Trả về kết quả kiểm tra
    }

    // Kiểm tra xem một vị trí có bị tấn công không
    private bool IsPositionUnderAttack(
        PieceColor playerColor,
        Position position,
        Board? board = null
    )
    {
        board ??= this.board; // Sử dụng bàn cờ hiện tại nếu không có bàn cờ khác

        // Kiểm tra xem bất kỳ quân cờ đối phương nào có thể tấn công vị trí đó không
        PieceColor opponentColor = ChessUtils.OpponentColor(playerColor);
        foreach (
            (Position enemyPosition, Piece enemyPiece)
            in board.LocatePieces(opponentColor)
        )
        {
            if (enemyPiece.GetMoves(board, enemyPosition, gameState).Contains(position)) return true; // Có quân tấn công
        }
        return false; // Không có quân tấn công
    }

    // Trả về kết quả của trò chơi
    public GameResult GetGameResult(PieceColor currentPlayerColor)
    {
        if (HasInsufficientMaterial()) return GameResult.DrawByInsufficientMaterial; // Kết thúc hòa do không đủ quân

        Position kingPosition = LocateKing(currentPlayerColor);
        bool isKingInCheck = IsPositionUnderAttack(currentPlayerColor, kingPosition); // Kiểm tra vua có bị chiếu không
        bool hasLegalMoves = HasLegalMoves(currentPlayerColor, kingPosition); // Kiểm tra có nước đi hợp lệ không

        // Kiểm tra nếu người chơi hiện tại bị chiếu hết
        if (isKingInCheck && !hasLegalMoves)
        {
            if (currentPlayerColor == PieceColor.White) return GameResult.BlackWins; // Trắng thua
            else return GameResult.WhiteWins; // Đen thua
        }
        // Kiểm tra xem trận đấu có kết thúc hòa không
        if (!isKingInCheck && !hasLegalMoves)
        {
            return GameResult.DrawByStalemate; // Hòa do không có nước đi
        }
        return GameResult.InProgress; // Trò chơi vẫn tiếp tục
    }

    // Kiểm tra có nước đi hợp lệ không
    private bool HasLegalMoves(
        PieceColor playerColor,
        Position currentKingPosition
    )
    {
        foreach (
            (Position source, Piece piece)
            in board.LocatePieces(playerColor)
        )
        {
            foreach (Position destination in piece.GetMoves(board, source, gameState))
            {
                if (!MoveLeavesKingInCheck(piece, source, destination, currentKingPosition))
                {
                    return true; // Có nước đi hợp lệ
                }
            }
        }
        return false; // Không có nước đi hợp lệ
    }

    // Kiểm tra nếu có quá ít quân cờ
    private bool HasInsufficientMaterial()
    {
        // Nếu chỉ còn vua (hoặc quân tốt bị kẹt), không thể chiếu hết
        foreach (
            (Position position, Piece piece)
            in board.LocatePieces()
        )
        {
            bool isStuckPawn = (
                piece.Type == PieceType.Pawn
                && (position.Row == 0 || position.Row == 7)
            );
            if (piece.Type != PieceType.King && !isStuckPawn) return false; // Có quân khác ngoài vua
        }
        return true; // Không đủ quân
    }
}
