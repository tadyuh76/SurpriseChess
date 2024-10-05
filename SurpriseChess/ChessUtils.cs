namespace SurpriseChess;

public static class ChessUtils
{
    public static PieceColor OpponentColor(PieceColor color) => (
        color == PieceColor.White ? PieceColor.Black : PieceColor.White
    );

    public static bool IsCastlingMove(Piece pieceAtSource, Piece? pieceAtDestination) => (
        pieceAtSource.Type == PieceType.King
        && pieceAtDestination != null
        && pieceAtDestination.Type == PieceType.Rook
        && pieceAtDestination.Color == pieceAtSource.Color
    );

    public static bool IsEnPassantMove
    (
        Position source,
        Position destination,
        Piece pieceAtSource,
        Piece? pieceAtDestination
    ) => (
        pieceAtSource.Type == PieceType.Pawn
        && destination.Col != source.Col  // Diagonal move
        && pieceAtDestination == null  // Not a regular diagonal capture
    );

    public static bool IsPawnPromotionMove(Piece piece, Position destination) => (
        piece.Type == PieceType.Pawn
        && (destination.Row == 0 || destination.Row == 7)
    );

    public static readonly Dictionary<PieceType, Dictionary<CastleDirection, int>> ColAfterCastling = new()
    {
        [PieceType.King] = new Dictionary<CastleDirection, int>
        {
            [CastleDirection.KingSide] = 6,
            [CastleDirection.QueenSide] = 2
        },
        [PieceType.Rook] = new Dictionary<CastleDirection, int>
        {
            [CastleDirection.KingSide] = 5,
            [CastleDirection.QueenSide] = 3
        }
    };

    public static readonly Dictionary<PieceColor, Dictionary<CastleDirection, bool>> InitialCastlingRights = new()
    {
        [PieceColor.White] = new Dictionary<CastleDirection, bool>
        {
            [CastleDirection.KingSide] = true,
            [CastleDirection.QueenSide] = true
        },
        [PieceColor.Black] = new Dictionary<CastleDirection, bool>
        {
            [CastleDirection.KingSide] = true,
            [CastleDirection.QueenSide] = true
        }
    };
}
