namespace SurpriseChess;

// Manages the the current player, castling & en passant rights
public class GameState
{
    public PieceColor CurrentPlayerColor { get; private set; }
    public Position? EnPassantPosition { get; private set; }
    public Dictionary<PieceColor, Dictionary<CastleDirection, bool>> CanCastle { get; private set; }
    private readonly Board board;

    public GameState(Board board)
    {
        CurrentPlayerColor = PieceColor.White;
        CanCastle = ChessUtils.InitialCastlingRights;
        this.board = board;
    }

    public void UpdateStateAfterMove(Position source, Position destination)
    {
        Piece? pieceAtSource = board.GetPieceAt(source);
        if (pieceAtSource == null) throw new InvalidOperationException("No piece at source position");

        UpdateEnPassantRights(pieceAtSource, source, destination);
        UpdateCastlingRights(pieceAtSource, source);
        SwitchPlayer();
    }

    private void UpdateCastlingRights(Piece piece, Position source)
    {
        if (piece.Type == PieceType.King)  // King moved, disable castling for player
        {
            CanCastle[piece.Color][CastleDirection.KingSide] = false;
            CanCastle[piece.Color][CastleDirection.QueenSide] = false;
        }
        else if (piece.Type == PieceType.Rook)  // Rook moved, disable castling for that side
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

    private void UpdateEnPassantRights(Piece piece, Position source, Position destination)
    {
        bool isPawnDoubleMove = (
            piece.Type == PieceType.Pawn
            && Math.Abs(source.Row - destination.Row) == 2
        );

        // En passant becomes available if a pawn moves two squares forward
        EnPassantPosition = isPawnDoubleMove
            ? new Position((source.Row + destination.Row) / 2, source.Col)
            : null;
    }

    private void SwitchPlayer()
    {
        CurrentPlayerColor = CurrentPlayerColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }
}
