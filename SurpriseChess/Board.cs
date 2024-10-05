namespace SurpriseChess;

public record Position(int Row, int Col);

public class Board : IBoardView, IPrototype<Board>
{
    private readonly Piece?[,] board = new Piece?[8, 8];
    public Dictionary<PieceColor, Dictionary<CastleDirection, Position>> RookStartingPositions { get; } = new()
    {
        [PieceColor.White] = new(),
        [PieceColor.Black] = new()
    };
    private readonly Random random = new();

    public Board(IBoardSetup boardSetup)
    {
        boardSetup.SetUp(this);
        RookStartingPositions = GetRookStartingPositions();
    }

    private Board(Board boardState)
    {
        // Copy pieces
        foreach ((Position position, Piece piece) in boardState.LocatePieces())
        {
            board[position.Row, position.Col] = piece.Clone();
        }

        // Copy rook starting positions
        foreach (var (color, directions) in boardState.RookStartingPositions)
        {
            foreach (var (direction, position) in directions)
            {
                RookStartingPositions[color][direction] = new Position(position.Row, position.Col);
            }
        }
    }

    public Piece? GetPieceAt(Position position) => board[position.Row, position.Col];

    public void SetPieceAt(Position position, Piece piece) => board[position.Row, position.Col] = piece;

    public static bool IsInsideBoard(Position position) => (
        position.Row >= 0 && position.Row < 8 && position.Col >= 0 && position.Col < 8
    );

    public void MakeMove(Position source, Position destination)
    {
        Piece? pieceAtSource = board[source.Row, source.Col];
        if (pieceAtSource == null) throw new InvalidOperationException("No piece at source position");

        Piece? pieceAtDestination = board[destination.Row, destination.Col];
        if (ChessUtils.IsCastlingMove(pieceAtSource, pieceAtDestination))
        {
            // Handle castling
            Castle(pieceAtSource, pieceAtDestination!, source, destination);
        }
        else
        {
            // Handle regular move
            board[source.Row, source.Col] = null;
            board[destination.Row, destination.Col] = pieceAtSource;

            // Handle special pawn moves
            if (ChessUtils.IsEnPassantMove(source, destination, pieceAtSource, pieceAtDestination))
            {
                board[source.Row, destination.Col] = null;  // Capture the adjacent enemy pawn
            }
            else if (ChessUtils.IsPawnPromotionMove(pieceAtSource, destination))
            {
                PieceType newType = (PieceType)random.Next(1, 5);  // Exclude king (0) and pawn (6)
                board[destination.Row, destination.Col] = PieceFactory.Create(
                    pieceAtSource.Color, newType
                );
            }
        }
    }

    public Dictionary<Position, Piece> LocatePieces(PieceColor? color = null, PieceType? type = null)
    {
        Dictionary<Position, Piece> piecesPositions = new();
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board[row, col];
                if (piece == null) continue;

                bool matchesColor = color == null || piece.Color == color;
                bool matchesType = type == null || piece.Type == type;
                if (matchesColor && matchesType)
                {
                    piecesPositions[new Position(row, col)] = piece;
                }
            }
        }
        return piecesPositions;
    }

    public Board Clone() => new(this);

    private Dictionary<PieceColor, Dictionary<CastleDirection, Position>> GetRookStartingPositions()
    {
        // Store the initial rook positions to later check if they have moved to update castling rights
        Position[] rookPositions = LocatePieces(type: PieceType.Rook).Keys.ToArray();
        return new Dictionary<PieceColor, Dictionary<CastleDirection, Position>>
        {
            [PieceColor.White] = new Dictionary<CastleDirection, Position>
            {
                [CastleDirection.QueenSide] = rookPositions[0],
                [CastleDirection.KingSide] = rookPositions[1]
            },
            [PieceColor.Black] = new Dictionary<CastleDirection, Position>
            {
                [CastleDirection.QueenSide] = rookPositions[2],
                [CastleDirection.KingSide] = rookPositions[3]
            }
        };
    }

    private void Castle(Piece king, Piece rook, Position kingPosition, Position rookPosition)
    {
        // If the rook is to the right of the king, it's a king-side castle; otherwise, it's a queen-side castle
        CastleDirection direction = kingPosition.Col < rookPosition.Col
            ? CastleDirection.KingSide
            : CastleDirection.QueenSide;
        int kingColAfterCastling = ChessUtils.ColAfterCastling[PieceType.King][direction];
        int rookColAfterCastling = ChessUtils.ColAfterCastling[PieceType.Rook][direction];

        board[kingPosition.Row, kingPosition.Col] = null;
        board[rookPosition.Row, rookPosition.Col] = null;

        board[kingPosition.Row, kingColAfterCastling] = king;
        board[rookPosition.Row, rookColAfterCastling] = rook;
    }
}
