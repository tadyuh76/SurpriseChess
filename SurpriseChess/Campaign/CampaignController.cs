using System.Diagnostics;
using System.Xml.Linq;

namespace SurpriseChess;

public class CampaignController : IController
{
    private readonly CampaignModel model;
    private readonly CampaignView view;

    public CampaignController(CampaignModel model, CampaignView view)
    {
        this.model = model;
        this.view = view;
    }

    public void Run()
    {
        view.Render(model);
        HandleInput();
    }

    private void HandleInput()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    model.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    model.MoveDown();
                    break;
                case ConsoleKey.LeftArrow:
                    model.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    model.MoveRight();
                    break;
                case ConsoleKey.Enter:
                    NavigateToInfoScreen();
                    break;
            }

            view.Render(model);
        }
    }

    private void NavigateToInfoScreen()
    {
        // Bắt đầu game ở Node được chọn
        var node = model.CampaignGrid[model.SelectedRow, model.SelectedCol];

        var nodeInfoController = new NodeController(new NodeView(), node);
        ScreenManager.Instance.NavigateToScreen(nodeInfoController);
    }
}
