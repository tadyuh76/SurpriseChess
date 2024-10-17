using SurpriseChess;

public class EffectApplier
{
    private readonly Board board;
    private readonly Random random = new();

    private const float MorphChance = 0.2f;
    private const float InvisibilityChance = 0.3f;
    private const float ParalysisChance = 0.4f;
    private const float ShieldChance = 0.5f;

    public EffectApplier(Board board)
    {
        this.board = board;
    }
    public void ApplyEffects(Position movedPosition)
    {
        Piece? movedPiece = board.GetPieceAt(movedPosition);
        if (movedPiece == null) return;

        ApplyMorphEffect(movedPiece, movedPosition);
        ApplyInvisibilityEffect(movedPiece.Color);
        ApplyParalysisEffect(ChessUtils.OpponentColor(movedPiece.Color));
        ApplyShieldEffect(movedPiece.Color);
    }

    public void ClearEffects()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board.GetPieceAt(new Position(row, col));
                if (piece == null) continue;

                piece.IsInvisible = false;
                piece.IsParalyzed = false;
                piece.IsShielded = false;
            }
        }
    }

    private void ApplyMorphEffect(Piece movedPiece, Position movedPosition)
    {
        //Đảm bảo rằng con Vua không bị biến đổi 
        if (movedPiece.Type == PieceType.King || random.NextDouble() > MorphChance) return;

        PieceType newType = (PieceType)random.Next(1, 6); // Con khác không thể biển đổi thành con vua
        Piece newPiece = PieceFactory.Create(movedPiece.Color, newType);
        board.SetPieceAt(movedPosition, newPiece);
    }

    private void ApplyInvisibilityEffect(PieceColor color)
    {
        if (random.NextDouble() > InvisibilityChance) return;

        Dictionary<Position, Piece> piecesPositions = board.LocatePieces(color);
        foreach (Piece piece in piecesPositions.Values)
        {
            piece.IsInvisible = true;
        }
    }

    private void ApplyParalysisEffect(PieceColor color)
    {
        if ( random.NextDouble() > ParalysisChance) return;

        Dictionary<Position, Piece> piecesPositions = board.LocatePieces(color);
        Piece randomPiece = GetRandomPieceExcludingKing(piecesPositions);
        
            randomPiece.IsParalyzed = true;
        
        
    }

    private void ApplyShieldEffect(PieceColor color)
    {
        if (random.NextDouble() > ShieldChance) return;

        Dictionary<Position, Piece> piecesPositions = board.LocatePieces(color);
        Piece randomPiece = GetRandomPiece(piecesPositions);
        randomPiece.IsShielded = true;
    }

    private Piece GetRandomPiece(Dictionary<Position, Piece> piecesPositions) => (
        piecesPositions.Values.ElementAt(random.Next(piecesPositions.Count))
    );
    private Piece GetRandomPieceExcludingKing(Dictionary<Position, Piece> piecesPositions)
    {
        //tạo một danh sách những quân mà không chứa vua
        var nonKingPieces = piecesPositions.Values.Where(piece => piece.Type != PieceType.King).ToList();
        return nonKingPieces.ElementAt(random.Next(nonKingPieces.Count));
    }
}