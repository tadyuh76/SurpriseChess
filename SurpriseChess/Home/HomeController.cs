namespace SurpriseChess;

public class HomeController : IController
{
    private readonly HomeModel model;
    private readonly HomeView view;

    public HomeController(HomeModel model, HomeView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Run()
    {
        ConsoleKey keyPressed;

        while (true)
        {
            view.Render(model);
            keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    break;
                case ConsoleKey.Enter:
                    SelectOption();
                    break;
            }
        }
    }

    private void MoveUp()
    {
        model.SelectedIndex--;
        if (model.SelectedIndex < 0)
        {
            model.SelectedIndex = model.Options.Length - 1;
        }
    }

    private void MoveDown()
    {
        model.SelectedIndex++;
        if (model.SelectedIndex >= model.Options.Length)
        {
            model.SelectedIndex = 0;
        }
    }

    private void SelectOption()
    {
        switch (model.SelectedIndex)
        {
            case 0:
                ScreenManager.Instance.NavigateToScreen(new ChessController(
                    new ChessModel(new Chess960()),
                    new ChessView(),
                    GameMode.PlayerVsPlayer,
                    null
                ));
                break;
            case 1:
                ScreenManager.Instance.NavigateToScreen(new CampaignController(
                    new CampaignModel(),
                    new CampaignView()
                ));
                break;
            case 2:
                ScreenManager.Instance.NavigateToScreen(new TutorialController(
                    new TutorialModel(),
                    new TutorialView()
                ));
                break;
            case 3:
                ScreenManager.Instance.NavigateToScreen(new MatchHistoryController(
                    new MatchHistoryModel(),
                    new MatchHistoryView()
                ));
                break;
        }
    }
}
