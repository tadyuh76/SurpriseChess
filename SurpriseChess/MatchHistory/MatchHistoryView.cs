namespace SurpriseChess;

public class MatchHistoryView
{
    // Render the list of matches for selection
    public void RenderMatchList(List<Match> matches)
    {
        Console.Clear();
        Console.WriteLine("Match History:");
        foreach (var match in matches)
        {
            Console.WriteLine($"{match.Id}: {match.Result} on {match.MatchDate.ToShortDateString()}");
        }
        Console.WriteLine("Enter the match number to view the details or press Backspace to go back.");
    }

    // Get the selected match ID from the user
    public int GetSelectedMatchId()
    {
        Console.Write("Enter match number: ");
        if (int.TryParse(Console.ReadLine(), out int selectedId))
        {
            return selectedId;
        }
        return -1; // Invalid input
    }

    public void DisplayBoard(Board board)
    {
        Console.Clear();
        Console.WriteLine("Current Board State:");

        for (int row = 7; row >= 0; row--)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board.GetPieceAt(new Position(row, col));
                if (piece != null)
                {
                    Console.Write($"{GetPieceSymbol(piece)} ");
                }
                else
                {
                    Console.Write(". ");
                }
            }
            Console.WriteLine();  // Move to the next row
        }
    }

    private string GetPieceSymbol(Piece piece)
    {
        // Returning a symbol to represent the piece
        return piece.Color == PieceColor.White
            ? piece.Type switch
            {
                PieceType.Pawn => "P",
                PieceType.Knight => "N",
                PieceType.Bishop => "B",
                PieceType.Rook => "R",
                PieceType.Queen => "Q",
                PieceType.King => "K",
                _ => "?"
            }
            : piece.Type switch
            {
                PieceType.Pawn => "p",
                PieceType.Knight => "n",
                PieceType.Bishop => "b",
                PieceType.Rook => "r",
                PieceType.Queen => "q",
                PieceType.King => "k",
                _ => "?"
            };
    }
}
