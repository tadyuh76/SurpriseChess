namespace SurpriseChess;

class TutorialController : IController
{
    private readonly TutorialView view;

    public TutorialController(TutorialView view)
    {
        this.view = view;
    }

    public void Run()
    {
        ConsoleKey keyPressed;

        while (true)
        {
            view.Render();
            keyPressed = Console.ReadKey().Key;

            if (keyPressed == ConsoleKey.Backspace) ScreenManager.Instance.BackToHomeScreen();
        }
    }
}