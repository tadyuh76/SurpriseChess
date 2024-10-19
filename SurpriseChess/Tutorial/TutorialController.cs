namespace SurpriseChess;

class TutorialController : IController
{
    private readonly TutorialView view;

    public TutorialController(TutorialView view)
    {
        //this.model = model;
        this.view = view;
    }

    public void Run()
    {
        ConsoleKey keyPressed;

        while (true)
        {
            view.Render();
            keyPressed = Console.ReadKey().Key;

            if (keyPressed == ConsoleKey.Backspace) HandleNavigateBack();
        }
    }
    private void HandleNavigateBack()
    {
        // Yêu cầu người dung xác nhận thoát trò chơi
        ConsoleKey keyPressed;

        keyPressed = Console.ReadKey().Key;

        if (keyPressed == ConsoleKey.Backspace)
        {
            // Trở về màn hình chính
            ScreenManager.Instance.NavigateToScreen(new HomeController(
                new HomeModel(),
                new HomeView()
            ));
        }

    }
}