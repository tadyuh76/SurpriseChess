using SurpriseChess;

public class ReplayView
{
    public void RenderBoard(ReplayBoard board)
    {
        for (int row = 7; row >= 0; row--)
        {
            for (int col = 0; col < 8; col++)
            {
                Piece? piece = board.GetPieceAt(new Position(row, col));
                Console.Write(piece != null ? piece.DisplaySymbol + " " : ". ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine("Use right arrow for next move, left arrow for previous move, or backspace to exit.");
    }
}