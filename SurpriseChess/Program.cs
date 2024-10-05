
using System.Text;

namespace SurpriseChess;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        IBoardSetup boardSetup = new Chess960();
        //IChessBot chessBot = new StockFish();

        ChessModel model = new(boardSetup);
        ChessView view = new();
        ChessController controller = new(model, view);
        //ConsoleGraphics g = new();

        controller.Run();
    }
}
