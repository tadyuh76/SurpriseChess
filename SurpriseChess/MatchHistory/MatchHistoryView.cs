namespace SurpriseChess;

public class MatchHistoryView
{
    public void Render(List<Match> matches)
    {
        Console.WriteLine("Match History:");
        foreach (var match in matches)
        {
            Console.WriteLine($"{match.Id}: {match.Result} on {match.MatchDate.ToShortDateString()}");
        }
        Console.WriteLine("Enter the match number to view the details or press Backspace to go back.");
    }

    public int GetSelectedMatchId()
    {
        Console.Write("Enter match number: ");
        int selectedId;
        if (int.TryParse(Console.ReadLine(), out selectedId))
        {
            return selectedId;
        }
        return -1; // Invalid input
    }
}
