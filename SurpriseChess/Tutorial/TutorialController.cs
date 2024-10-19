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
        view.Render();
    }
}