namespace SurpriseChess;

public class HomeModel
{
    public string[] Options { get; } = { "2 người chơi", "1 người chơi", " Hướng dẫn", " Lịch sử đấu" };
    public int SelectedIndex { get; set; } = 0;
}
