namespace SurpriseChess;

class TutorialController : IController
{
    private readonly TutorialModel model;
    private readonly TutorialView view;

    public TutorialController(TutorialModel model, TutorialView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Run()
    {
        ConsoleKey keyPressed;

        while (true)
        {
            view.Render();
            keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    break;
                /*case ConsoleKey.Enter:
                    SelectOption();
                    break;*/
            }
        }
    }
    private void MoveUp()
    {
        model.SelectedIndex1--;
        if (model.SelectedIndex1 < 0)
        {
            model.SelectedIndex1 = model.Options1.Length - 1;
        }
    }
    private void MoveDown()
    {
        model.SelectedIndex1++;
        if (model.SelectedIndex1 >= model.Options1.Length)
        {
            model.SelectedIndex1 = 0;
        }
    }
}