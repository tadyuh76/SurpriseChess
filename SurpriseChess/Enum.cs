namespace SurpriseChess;

// Các loại quân cờ trong trò chơi
public enum PieceType { King, Queen, Rook, Bishop, Knight, Pawn }

// Màu sắc quân cờ
public enum PieceColor { White, Black }

// Hướng nhập thành
public enum CastleDirection { KingSide, QueenSide };

// Chế độ trò chơi
public enum GameMode { PlayerVsPlayer, PlayerVsBot }

// Kết quả của trò chơi
public enum GameResult
{
    InProgress, // Trò chơi đang diễn ra
    WhiteWins,  // Trắng thắng
    BlackWins,  // Đen thắng
    DrawByStalemate, // Hòa do không còn nước đi hợp lệ
    DrawByInsufficientMaterial // Hòa do không đủ quân
}

// Các màn hình trong ứng dụng
public enum Screen { Home, Game, GameOver }
