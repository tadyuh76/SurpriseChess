
using System.Text;

namespace SurpriseChess;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // IBoardSetup boardSetup = new Chess960();
        // ChessModel model = new(boardSetup);
        // ChessView view = new();
        // ChessController controller = new(model, view);

        //controller.Run();
        HomeView homeView = new();
        HomeModel homeModel = new();
        HomeController homeController = new(homeModel, homeView);
        // homeController.Run();
        ScreenManager.Instance.NavigateToScreen(homeController);
    }
}
