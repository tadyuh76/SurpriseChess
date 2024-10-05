namespace SurpriseChess;

// Responsible for determining the legality of moves and the game result
public class Arbiter
{
    private readonly Board board;
    private readonly GameState gameState;

    public Arbiter(Board board, GameState gameState)
    {
        this.board = board;
        this.gameState = gameState;
    }

    // Returns all the legal moves from a given position
    // A move is legal if it doesn't leave the player's king in check
    public HashSet<Position> GetLegalMoves(Position source)
    {
        HashSet<Position> legalMoves = new();
        Piece? piece = board.GetPieceAt(source);
        if (piece == null) return legalMoves;  // No piece to be moved

        Position currentKingPosition = LocateKing(piece.Color);
        foreach (Position destination in piece.GetMoves(board, source, gameState))
        {
            if (IsLegalMove(piece, source, destination, currentKingPosition))
            {
                legalMoves.Add(destination);
            }
        }

        return legalMoves;
    }

    private bool IsLegalMove(
        Piece piece,
        Position source,
        Position destination,
        Position currentKingPosition
    )
    {
        if (ChessUtils.IsCastlingMove(piece, board.GetPieceAt(destination)))
        {
            return CanCastleSafely(piece.Color, currentKingPosition, destination.Col);
        }

        return !MoveLeavesKingInCheck(piece, source, destination, currentKingPosition);
    }

    private Position LocateKing(PieceColor color)
    {
        (Position position, Piece _) = board.LocatePieces(color, PieceType.King).First();
        return position;
    }

    // Returns whether the king's castling path is not under attack
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
            if (IsPositionUnderAttack(kingColor, position)) return false;
        }
        return true;
    }

    private bool MoveLeavesKingInCheck(
        Piece piece,
        Position source,
        Position destination,
        Position currentKingPosition
    )
    {
        // Determine the king's position after the move
        Position kingPosition;
        if (piece.Type == PieceType.King) kingPosition = destination;
        else kingPosition = currentKingPosition;

        // Simulate the move on a cloned board
        Board tempBoard = board.Clone();
        tempBoard.MakeMove(source, destination);
        bool isKingInCheck = IsPositionUnderAttack(piece.Color, kingPosition, tempBoard);

        return isKingInCheck;
    }

    private bool IsPositionUnderAttack(
        PieceColor playerColor,
        Position position,
        Board? board = null
    )
    {
        board ??= this.board;

        // Check if any of the opponent's pieces can attack the position
        PieceColor opponentColor = ChessUtils.OpponentColor(playerColor);
        foreach (
            (Position enemyPosition, Piece enemyPiece)
            in board.LocatePieces(opponentColor)
        )
        {
            if (enemyPiece.GetMoves(board, enemyPosition, gameState).Contains(position)) return true;
        }
        return false;
    }

    public GameResult GetGameResult(PieceColor currentPlayerColor)
    {
        if (HasInsufficientMaterial()) return GameResult.DrawByInsufficientMaterial;

        Position kingPosition = LocateKing(currentPlayerColor);
        bool isKingInCheck = IsPositionUnderAttack(currentPlayerColor, kingPosition);
        bool hasLegalMoves = HasLegalMoves(currentPlayerColor, kingPosition);

        // Check if current player is checkmated
        if (isKingInCheck && !hasLegalMoves)
        {
            if (currentPlayerColor == PieceColor.White) return GameResult.BlackWins;
            else return GameResult.WhiteWins;
        }
        // Check the game ends in stalemate
        if (!isKingInCheck && !hasLegalMoves)
        {
            return GameResult.DrawByStalemate;
        }
        return GameResult.InProgress;
    }

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
                    return true;
                }
            }
        }
        return false;
    }

    private bool HasInsufficientMaterial()
    {
        // If only kings (or stuck pawns) are left on the board, checkmate is impossible
        // If there's another type of piece, it can morph and checkmate the opponent
        // (A stuck pawn is a piece that unfortunately morphed into a pawn on the final row)
        foreach (
            (Position position, Piece piece)
            in board.LocatePieces()
        )
        {
            bool isStuckPawn = (
                piece.Type == PieceType.Pawn
                && (position.Row == 0 || position.Row == 7)
            );
            if (piece.Type != PieceType.King && !isStuckPawn) return false;
        }
        return true;
    }
}
