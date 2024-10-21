namespace SurpriseChess;

public class HomeModel
{
    public string[] Options { get; } = { "2 người chơi", "Chơi với máy", " Hướng dẫn", " Lịch sử đấu" };
    public int SelectedIndex { get; set; } = 0;
}
