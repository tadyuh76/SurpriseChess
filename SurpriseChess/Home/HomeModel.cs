namespace SurpriseChess;

public class HomeModel
{
    public string[] Options { get; } = { "Player vs Player", "Campaign Mode", "Tutorial", "Match History" };
    public int SelectedIndex { get; set; } = 0;
}
