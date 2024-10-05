namespace SurpriseChess;

public class Chess960 : IBoardSetup
{
    private static readonly Random random = new();

    public void SetUp(Board board)
    {
        PieceType[] backRank = CreateRandomBackRank();
        for (int col = 0; col < 8; col++)
        {
            // Place pawns
            board.SetPieceAt(new Position(6, col), PieceFactory.Create(PieceColor.White, PieceType.Pawn));
            board.SetPieceAt(new Position(1, col), PieceFactory.Create(PieceColor.Black, PieceType.Pawn));

            // Place back rank pieces
            PieceType type = backRank[col];
            board.SetPieceAt(new Position(7, col), PieceFactory.Create(PieceColor.White, type));
            board.SetPieceAt(new Position(0, col), PieceFactory.Create(PieceColor.Black, type));
        }
    }

    private static PieceType[] CreateRandomBackRank()
    {
        // Start with an empty back rank
        PieceType[] backRank = new PieceType[8];
        List<int> remainingPositions = new() { 0, 1, 2, 3, 4, 5, 6, 7 };

        // Randomize positions for 2 opposite color bishops
        int bishop1Index = random.Next(0, 4) * 2; // Even square (indices 0, 2, 4, 6)
        int bishop2Index = random.Next(0, 4) * 2 + 1; // Odd square (indices 1, 3, 5, 7)
        backRank[bishop1Index] = PieceType.Bishop;
        backRank[bishop2Index] = PieceType.Bishop;
        remainingPositions.Remove(bishop1Index);
        remainingPositions.Remove(bishop2Index);

        // Randomize positions for 2 knights
        int knight1Index = remainingPositions[random.Next(remainingPositions.Count)];
        backRank[knight1Index] = PieceType.Knight;
        remainingPositions.Remove(knight1Index);
        int knight2Index = remainingPositions[random.Next(remainingPositions.Count)];
        backRank[knight2Index] = PieceType.Knight;
        remainingPositions.Remove(knight2Index);

        // Place rooks in the first and last indices of the remaining positions to ensure the king goes in between them
        int rook1Index = remainingPositions.First();
        int rook2Index = remainingPositions.Last();
        backRank[rook1Index] = PieceType.Rook;
        backRank[rook2Index] = PieceType.Rook;
        remainingPositions.RemoveAt(0);
        remainingPositions.RemoveAt(remainingPositions.Count - 1);

        // Randomize position for queen
        int queenIndex = remainingPositions[random.Next(remainingPositions.Count)];
        backRank[queenIndex] = PieceType.Queen;
        remainingPositions.Remove(queenIndex);

        // King in last slot
        int kingIndex = remainingPositions[0];
        backRank[kingIndex] = PieceType.King;

        return backRank;
    }
}