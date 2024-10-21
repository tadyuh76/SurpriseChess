namespace SurpriseChess;

class EndGameController: IController
{
    private readonly EndGameView view;
    private readonly GameResult GameResult;

    public EndGameController(EndGameView view, GameResult gameResult)
    {
        this.view = view;
        this.GameResult = gameResult;
    }

    public void Run()
    {
        view.Render(GameResult);
        Console.ReadKey();
        ScreenManager.Instance.NavigateToScreen(new HomeController(
            new HomeModel(),
            new HomeView()
        ));
    }
}

