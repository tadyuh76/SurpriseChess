public class ReplayModel
{
    private List<string> FENList { get; } = new();
    private int CurrentMoveIndex { get; set; } = 0;

    public ReplayModel(List<string> fenList)
    {
        FENList = fenList;
    }

    public bool NextMove()
    {
        if (CurrentMoveIndex < FENList.Count - 1)
        {
            CurrentMoveIndex++;
            return true;
        }
        return false;
    }

    public bool PreviousMove()
    {
        if (CurrentMoveIndex > 0)
        {
            CurrentMoveIndex--;
            return true;
        }
        return false;
    }

    public string GetCurrentFEN() => FENList[CurrentMoveIndex];
}