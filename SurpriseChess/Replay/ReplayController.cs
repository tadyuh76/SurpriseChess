using SurpriseChess;

public class ReplayController : IController
{
    private readonly ReplayModel model;
    private readonly ReplayView view;
    private ReplayBoard board;

    public ReplayController(List<string> fenList, ReplayView view)
    {
        this.model = new ReplayModel(fenList);
        this.view = view;
        this.board = new ReplayBoard(model.GetCurrentFEN());
    }

    public void Run()
    {
        bool exit = false;
        while (!exit)
        {
            board = new ReplayBoard(model.GetCurrentFEN());
            view.RenderBoard(board);

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.RightArrow:
                    if (!model.NextMove())
                        Console.WriteLine("You are at the last move.");
                    break;
                case ConsoleKey.LeftArrow:
                    if (!model.PreviousMove())
                        Console.WriteLine("You are at the first move.");
                    break;
                case ConsoleKey.Backspace:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
    }
}