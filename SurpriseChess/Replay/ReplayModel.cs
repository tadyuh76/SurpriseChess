using SurpriseChess;

public class ReplayModel
{
    public List<string> FENList { get; }
    public int CurrentMoveIndex { get; private set; } = 1;
    public ReplayBoard CurrentBoard { get; private set; }

    public ReplayModel(List<string> fenList)
    {
        FENList = fenList;
        UpdateCurrentBoard();
    }

    public void MoveNext()
    {
        if (CurrentMoveIndex < FENList.Count - 1)
        {
            CurrentMoveIndex++;
            UpdateCurrentBoard();
        }
    }

    public void MovePrevious()
    {
        if (CurrentMoveIndex > 1)
        {
            CurrentMoveIndex--;
            UpdateCurrentBoard();
        }
    }

    private void UpdateCurrentBoard()
    {
        CurrentBoard = new ReplayBoard(FENList[CurrentMoveIndex]);
    }

    public string GetCurrentFEN() => FENList[CurrentMoveIndex];

    public string DetermineActualNextMove()
    {
        if (CurrentMoveIndex < FENList.Count - 1)
        {
            string currentFEN = FENList[CurrentMoveIndex];
            string nextFEN = FENList[CurrentMoveIndex + 1];
            return CalculateMove(currentFEN, nextFEN);
        }
        return "None";
    }

    private string CalculateMove(string currentFEN, string nextFEN)
    {
        string[] currentParts = currentFEN.Split(' ');
        string[] nextParts = nextFEN.Split(' ');
        string[] currentRows = currentParts[0].Split('/');
        string[] nextRows = nextParts[0].Split('/');

        string fromSquare = "";
        string toSquare = "";

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                char? currentPiece = GetPieceAt(currentRows, row, col); 
                char? nextPiece = GetPieceAt(nextRows, row, col);

                if (currentPiece != nextPiece)
                {
                    string square = $"{(char)('a' + col)}{8 - row}";

                    // Nếu ô hiện tại không còn quân (vị trí đích)
                    if (currentPiece.HasValue && !nextPiece.HasValue)
                    {
                        fromSquare = square;
                    }
                    // Nếu ô tương lai không còn quân (vị trí di chuyển)
                    // Hoặc cả 2 ô đều có quân nhưng khác giá trị (vị trí có quân bị bắt)
                    else if (
                        (!currentPiece.HasValue && nextPiece.HasValue) ||
                        (currentPiece.HasValue && nextPiece.HasValue)
                    )
                    {
                        toSquare = square;
                    } 

                    // Chỉ trả về giá trị khi đảm bảo xác định đủ vị trí đích và vị trí di chuyển
                    if (!string.IsNullOrEmpty(fromSquare) && !string.IsNullOrEmpty(toSquare))
                    {
                        return $"{fromSquare}{toSquare}";
                    }
                }
            }
        }

        return "None";
    }

    private char GetActiveColor(string fen)
    {
        return fen.Split(' ')[1][0];
    }

    private char? GetPieceAt(string[] rows, int row, int col)
    {
        int currentCol = 0;
        foreach (char c in rows[row])
        {
            if (char.IsDigit(c))
            {
                currentCol += int.Parse(c.ToString());
            }
            else
            {
                if (currentCol == col) return c;
                currentCol++;
            }
            if (currentCol > col) break;
        }
        return null;
    }

    public Position GetCurrentPosition()
    {
        return FEN.FENToPosition(GetCurrentFEN());
    }

    public async Task<List<(Position, Position)>> GetBestMovesAsync(StockFish stockfish, StockfishAnalysisCache analysisCache)
    {
        string currentFen = GetCurrentFEN();
        //var cachedMoves = analysisCache.GetCachedAnalysis(currentFen);
        //if (cachedMoves != null)
        //{
        //    return cachedMoves;
        //}
        var bestMoves = await stockfish.GetBestMoves(currentFen);
        //analysisCache.CacheAnalysis(currentFen, bestMoves);
        return bestMoves;
    }

    public string ConvertMoveToString((Position, Position) move)
    {
        //if (move == default)
        //    return "None";
        return $"{FEN.PositionToFEN(move.Item1)}{FEN.PositionToFEN(move.Item2)}";
    }
}