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
    DrawByInsufficientMaterial, // Hòa do không đủ quân
     WhiteLosesByTimesUp, // Trắng thua do hết giờ
    BlackLosesByTimesUp  // Đen thua do hết giờ
}

// Các màn hình trong ứng dụng
public enum Screen
{ 
    Home, // Màn hình chính
    Game, // Màn hình chơi
    GameOver, // Màn hình kết thúc trò chơi
    Campaign, // Màn hình chọn bàn chơi (Chế độ chơi với máy)
    Tutorial, // Màn hình hướng dẫn chơi
    MatchHistory // Màn hình xem lịch sử các trận đấu
}
