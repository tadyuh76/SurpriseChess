namespace SurpriseChess;

class EndGameView
{
    public void Render(GameResult gameResult)
    {
        Console.Clear();
        DrawBorder();
        DrawTitle();
        DrawChessPieces();
        DrawMessage(gameResult);
    }

    private void DrawBorder()
    {
        string border = new string('*', 120);
        Console.WriteLine(border);

        for (int i = 0; i < 28; i++)
        {
            Console.WriteLine("*" + new string(' ', 118) + "*");
        }

        Console.WriteLine(border);
    }

    private void DrawTitle()
    {
        string title = "=== GAME OVER ===";
        int titlePosition = (120 - title.Length) / 2;
        Console.SetCursorPosition(titlePosition, 2);
        Console.WriteLine(title);
    }

    private void DrawChessPieces()
    {
        // ASCII chess pieces - simple and centered
        string[] pieces = {
            @"                                             .::.",
            @"                                  _()_       _::_",
            @"                        _O      _/____\_   _/____\_     _O",
            @" _  _  _     ^^__      / //\    \      /   \      /    / //\      ^^__     _  _  _",
            @"| || || |   /  - \_   {     }    \____/     \____/    {     }    /  - \_  | || || |",
            @"|_______| <|    __<    \___/     (____)     (____)     \___/   <|    __<  |_______|",
            @"\__ ___ / <|    \      (___)      |  |       |  |      (___)   <|    \    \__ ___ /",
            @" |___|_|  <|     \      |_|       |__|       |__|       |_|    <|     \    |___|_|",
            @" |_|___|  <|______\    /   \     /    \     /    \     /   \   <|______\   |_|___|",
            @" |___|_|   _|____|_   (_____)   (______)   (______)   (_____)   _|____|_   |___|_|",
            @"(_______) (________) (_______) (________) (________) (_______) (________) (_______)",
            @"/_______\ /________\ /_______\ /________\ /________\ /_______\ /________\ /_______\",
        };

        int startRow = 6;
        int startColumn = (120 - pieces[11].Length) / 2;

        Console.ForegroundColor = ConsoleColor.Yellow;
        foreach (string line in pieces)
        {
            Console.SetCursorPosition(startColumn, startRow);
            Console.WriteLine(line);
            startRow++;
        }
        Console.ResetColor();
    }

    private void DrawMessage(GameResult gameResult)
    {
        string message = "Cảm ơn bạn đã chơi Surprise Chess!";
        string result = gameResult switch
        {
            GameResult.WhiteWins => "Vương quốc thắng!",
            GameResult.BlackWins => "Rừng sâu thắng!",
            GameResult.DrawByStalemate => "Hoà do không còn nước đi hợp lệ!",
            GameResult.DrawByInsufficientMaterial => "Hoà do không còn đủ quân cờ!",
            GameResult.WhiteOutOfTime => "Vương quốc thua do hết thời gian!",
            GameResult.BlackOutOfTime => "Rừng sâu thua do hết thời gian!",
            _ => "Đã có lỗi xảy ra!"
        };
        string exit = "Nhấn phím bất kỳ để trở về màn hình chính...";

        int messagePosition = (120 - message.Length) / 2;
        int resultPosition = (120 - result.Length) / 2;
        int exitPosition = (120 - exit.Length) / 2;

        Console.SetCursorPosition(messagePosition, 20);
        Console.WriteLine(message);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(resultPosition, 22);
        Console.WriteLine(result);
        Console.ResetColor();

        Console.SetCursorPosition(exitPosition, 24);
        Console.Write(exit);
    }
}

