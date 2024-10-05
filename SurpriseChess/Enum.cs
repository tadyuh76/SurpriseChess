namespace SurpriseChess;

public enum PieceType { King, Queen, Rook, Bishop, Knight, Pawn }

public enum PieceColor { White, Black }

public enum CastleDirection { KingSide, QueenSide };

public enum GameMode { PlayerVsPlayer, PlayerVsBot }

public enum GameResult
{
    InProgress,
    WhiteWins,
    BlackWins,
    DrawByStalemate,
    DrawByInsufficientMaterial
}

public enum Screen { Home, Game, GameOver }
