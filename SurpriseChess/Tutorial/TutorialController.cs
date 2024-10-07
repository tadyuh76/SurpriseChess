namespace SurpriseChess;

class TutorialController : IController
{
    private TutorialModel _model;
    private TutorialView _view;

    public TutorialController(TutorialModel model, TutorialView view)
    {
        _model = model;
        _view = view;
    }

    public void Run()
    {
        _view.Render();
    }
}