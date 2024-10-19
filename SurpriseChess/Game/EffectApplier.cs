using SurpriseChess;

public class EffectApplier
{
    private readonly Board board;
    private readonly Random random = new();
    public EffectApplier(Board board)
    {
        this.board = board;
    }
    public void ApplyEffects(Position movedPosition)
    {
        // Tạo một số ngẫu nhiên giữa 0 và 1
        double randomValue = random.NextDouble();

        // Tính ngưỡng cho mỗi hiệu ứng để chọn một hiệu ứng duy nhất
        double cumulativeChance = 0;

        Piece? movedPiece = board.GetPieceAt(movedPosition);
        if (movedPiece == null) return;

        //Kiểm tra hiệu ứng biển đổi (20%)
        cumulativeChance += 0.2;
        if (randomValue < cumulativeChance)
        {
            ApplyMorphEffect(movedPiece, movedPosition);
            return;
        }
        //Kiểm tra hiệu ứng tàng hình (20%)
        cumulativeChance += 0.2;
        if (randomValue < cumulativeChance)
        {
            ApplyInvisibilityEffect(movedPiece.Color);
            return;
        }
        //Kiểm tra hiệu ứng tê liệt (20%)
        cumulativeChance += 0.2;
        if (randomValue < cumulativeChance)
        {
            ApplyParalysisEffect(ChessUtils.OpponentColor(movedPiece.Color));
            return;
        }
        //Kiểm tra hiệu ứng bảo vệ (30%)
        cumulativeChance += 0.3;
        if (randomValue < cumulativeChance)
        {
            ApplyShieldEffect(movedPiece.Color);
            return;
        }
        //không áp dụng hiệu ứng nào (10%)
    }

    //hàm xóa hiệu ứng trước khi áp dụng một hiệu ứng khác
    public void ClearEffects()
    {
        //quét toàn bộ bàn cờ và xóa tất cả hiệu ứng trước khi áp dụng hiệu ứng khác
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
        if (movedPiece.Type == PieceType.King) return;

        PieceType newType = (PieceType)random.Next(1, 6); // Con khác không thể biển đổi thành con vua
        Piece newPiece = PieceFactory.Create(movedPiece.Color, newType);
        board.SetPieceAt(movedPosition, newPiece);
    }

    private void ApplyInvisibilityEffect(PieceColor color)
    {      
        //tạo một danh sách một bên quân cờ để tàng hình
        Dictionary<Position, Piece> piecesPositions = board.LocatePieces(color);
        //tàng hình tất cả các quân của 1 bên
        foreach (Piece piece in piecesPositions.Values)
        {
            piece.IsInvisible = true; 
        }
    }

    private void ApplyParalysisEffect(PieceColor color)
    {
        Dictionary<Position, Piece> piecesPositions = board.LocatePieces(color);
        Piece randomPiece = GetRandomPieceExcludingKing(piecesPositions); //quân ngẫu nhiên bị trói trừ con Vua
        
            randomPiece.IsParalyzed = true;      
    }

    private void ApplyShieldEffect(PieceColor color)
    {
        Dictionary<Position, Piece> piecesPositions = board.LocatePieces(color);
        Piece randomPiece = GetRandomPiece(piecesPositions); //quân ngẫu nhiên được bảo vệ
        randomPiece.IsShielded = true;
    }

    private Piece GetRandomPiece(Dictionary<Position, Piece> piecesPositions) => (
        piecesPositions.Values.ElementAt(random.Next(piecesPositions.Count)) //ngẫu nhiên những lựa chọn quân cờ được áp dụng hiệu ứng
    );
    private Piece GetRandomPieceExcludingKing(Dictionary<Position, Piece> piecesPositions)
    {
        //tạo một danh sách những quân mà không chứa vua
        var nonKingPieces = piecesPositions.Values.Where(piece => piece.Type != PieceType.King).ToList();
        return nonKingPieces.ElementAt(random.Next(nonKingPieces.Count)); //ngẫu nhiên những lựa chọn quân cờ được áp dụng hiệu ứng trừ vua 
    }
}