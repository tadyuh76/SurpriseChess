namespace SurpriseChess;

// Cài đặt bàn cờ theo kiểu Chess960
public class Chess960 : IBoardSetup
{
    private static readonly Random random = new();

    // Thiết lập bàn cờ cho trò chơi
    public void SetUp(Board board)
    {
        PieceType[] backRank = CreateRandomBackRank(); // Tạo hàng sau ngẫu nhiên
        for (int col = 0; col < 8; col++)
        {
            // Đặt quân tốt
            board.SetPieceAt(new Position(6, col), PieceFactory.Create(PieceColor.White, PieceType.Pawn));
            board.SetPieceAt(new Position(1, col), PieceFactory.Create(PieceColor.Black, PieceType.Pawn));

            // Đặt quân ở hàng sau
            PieceType type = backRank[col];
            board.SetPieceAt(new Position(7, col), PieceFactory.Create(PieceColor.White, type));
            board.SetPieceAt(new Position(0, col), PieceFactory.Create(PieceColor.Black, type));
        }
    }

    // Tạo hàng sau ngẫu nhiên cho cả hai màu quân
    private static PieceType[] CreateRandomBackRank()
    {
        // Bắt đầu với hàng sau trống
        PieceType[] backRank = new PieceType[8];
        List<int> remainingPositions = new() { 0, 1, 2, 3, 4, 5, 6, 7 };

        // Ngẫu nhiên vị trí cho 2 quân tượng màu đối lập
        int bishop1Index = random.Next(0, 4) * 2; // Ô chẵn (chỉ số 0, 2, 4, 6)
        int bishop2Index = random.Next(0, 4) * 2 + 1; // Ô lẻ (chỉ số 1, 3, 5, 7)
        backRank[bishop1Index] = PieceType.Bishop;
        backRank[bishop2Index] = PieceType.Bishop;
        remainingPositions.Remove(bishop1Index);
        remainingPositions.Remove(bishop2Index);

        // Ngẫu nhiên vị trí cho 2 quân mã
        int knight1Index = remainingPositions[random.Next(remainingPositions.Count)];
        backRank[knight1Index] = PieceType.Knight;
        remainingPositions.Remove(knight1Index);
        int knight2Index = remainingPositions[random.Next(remainingPositions.Count)];
        backRank[knight2Index] = PieceType.Knight;
        remainingPositions.Remove(knight2Index);

        // Đặt quân xe ở chỉ số đầu và cuối trong các vị trí còn lại để đảm bảo quân vua nằm giữa chúng
        int rook1Index = remainingPositions.First();
        int rook2Index = remainingPositions.Last();
        backRank[rook1Index] = PieceType.Rook;
        backRank[rook2Index] = PieceType.Rook;
        remainingPositions.RemoveAt(0);
        remainingPositions.RemoveAt(remainingPositions.Count - 1);

        // Ngẫu nhiên vị trí cho quân hậu
        int queenIndex = remainingPositions[random.Next(remainingPositions.Count)];
        backRank[queenIndex] = PieceType.Queen;
        remainingPositions.Remove(queenIndex);

        // Quân vua ở ô cuối cùng
        int kingIndex = remainingPositions[0];
        backRank[kingIndex] = PieceType.King;

        return backRank; // Trả về hàng sau đã được thiết lập
    }
}
