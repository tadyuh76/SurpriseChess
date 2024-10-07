namespace SurpriseChess;

public class CampaignView
{
    public void Render(List<CampaignNode> nodes, int currentNodeIndex)
    {
        Console.Clear();
        Console.WriteLine("Campaign Mode - Navigate the Nodes\n");

        for (int i = 0; i < nodes.Count; i++)
        {
            if (i == currentNodeIndex)
            {
                // Highlight the current node
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{nodes[i].Id}] Difficulty: {nodes[i].Difficulty}");
                Console.ResetColor();
            }
            else
            {
                // Display unselected nodes
                Console.WriteLine($" {nodes[i].Id}  Difficulty: {nodes[i].Difficulty}");
            }
        }
        Console.WriteLine("\nUse arrow keys to move between nodes. Press Enter to select.");
    }
}
