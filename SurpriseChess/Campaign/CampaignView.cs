using System;

namespace SurpriseChess;
public class CampaignView
{

    public void RenderMap(CampaignNode[,] map, (int x, int y) selectedNode)
    {
        Console.Clear();
        Console.CursorVisible = false; // Ẩn con trỏ vĩnh viễn

        // Các thông số và padding như trước
        int horizontalSpacing = 9;
        int verticalSpacing = 2;
        int mapHeight = map.GetLength(0) * (3 + verticalSpacing);
        int mapWidth = (map.GetLength(1) * (7 + horizontalSpacing)) - horizontalSpacing;

        // Header và footer với padding lớn hơn
        string header = "⚔️ 1 người chơi - Bản đồ màn chơi UEH ⚔️";
        string footer = "🎮 Dùng các phím mũi tên để di chuyển| Escape để thoát| Enter để chọn bàn chơi 🎮";

        // Tăng chiều rộng và chiều cao của viền
        int contentWidth = Math.Max(Math.Max(mapWidth, header.Length), footer.Length) + 37;
        int contentHeight = mapHeight + 16;

        int leftPadding = (Console.WindowWidth - contentWidth) / 2;
        int topPadding = (Console.WindowHeight - contentHeight) / 2 + 7 / 4;

        // Vẽ viền trên cùng bằng các ký tự *
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(leftPadding, topPadding);
        DrawStarBorder(contentWidth);

        // In header đã căn giữa
        Console.SetCursorPosition(leftPadding + (contentWidth - header.Length) / 2, topPadding + 2);
        Console.WriteLine(header);

        // Vẽ bản đồ với spacing và căn giữa mới
        int mapLeftPadding = leftPadding + (contentWidth - mapWidth) / 2;
        Console.SetCursorPosition(0, topPadding + 5);

        for (int row = 0; row < map.GetLength(0); row++)
        {
            PrintRowBorder(map, row, selectedNode, "┌─────┐", horizontalSpacing, mapLeftPadding);
            PrintRowContent(map, row, selectedNode, horizontalSpacing, mapLeftPadding);
            PrintRowBorder(map, row, selectedNode, "└─────┘", horizontalSpacing, mapLeftPadding);

            for (int i = 0; i < verticalSpacing; i++) Console.WriteLine();
        }

        // In footer căn giữa
        Console.SetCursorPosition(leftPadding + (contentWidth - footer.Length) / 2, Console.CursorTop + 2);
        Console.WriteLine(footer);

        // Tăng chiều cao phần giữa footer và viền dưới
        Console.SetCursorPosition(leftPadding, Console.CursorTop + 3);

        // Vẽ viền dưới cùng
        DrawStarBorder(contentWidth);

        // Vẽ viền hai bên bằng các ký tự *
        DrawStarVerticalLines(leftPadding, topPadding + 1, Console.CursorTop - 1, contentWidth);

        // Đảm bảo con trỏ được đặt dưới cùng sau khi render xong
        Console.SetCursorPosition(0, Console.CursorTop + 2);
        Console.ResetColor();
        var key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.Enter:
                var selected = map[selectedNode.x, selectedNode.y];
                if (selected != null)
                {
                    // Chuyển ngay sang bàn cờ với số nummove tương ứng
                    StartChessBoard(selected);
                    return;
                }
                break;
            case ConsoleKey.Escape:
                return;
        }

    }
    private void StartChessBoard(CampaignNode node)
    {
        int numMoves = GetStockfishNumMoves(node.Difficulty); // Giả lập số nước đi từ Stockfish

        Console.Clear();

        // Tạo thông điệp và ASCII art tương ứng với độ khó
        string message;
        string asciiArt;
        string guide;

        if (node.Difficulty == 6)
        {
            message = "🚎 Bắt đầu bàn cờ tại UEH - Cơ sở 232/6 Võ Thị Sáu, P. Võ Thị Sáu - Q.3 🚎";
            guide = "Nhấn Enter để tiếp tục";
            asciiArt = @"
      _ _.-'`-._ _
                ;.'________'.;
     _________n.[____________].n_________
    |""_""_""_""||==||==||==||""_""_""_""
    |""""""""""""||..||..||..||""""""""""""
    |LI LI LI LI||LI||LI||LI||LI LI LI LI|
    |.. .. .. ..||..||..||..||.. .. .. ..|
    |LI LI LI LI||LI||LI||LI||LI LI LI LI|
,,;;,;;;,;;;,;;;,;;;,;;;,;;;,;;,;;;,;;;,;;
;;jgs;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;";
            SetCursorPositionAndPrint(7, asciiArt);
        }
        else if (node.Difficulty == 7)
        {
            message = "🚎 Bắt đầu bàn cờ tại UEH - Cơ sở 144 Phạm Đức Sơn, P.16 - Q.8 🚎";
            guide = "Nhấn Enter để tiếp tục";
            asciiArt = @"
     /\
     /\/\
    /\/\/\
   /\/\/\/\
  /\/\/\/\/\
 /\/\/\/\/\/\
/\/\/\/\/\/\/\______________________________
|~~~~~~~~~~~~|/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\
|  /\    /\  | \/\/\/\/\/\/\/\/\/\/\/\/\/\/\
|  \/    \/  |  \/\/\/\/\/\/\/\/\/\/\/\/\/\/
|            |   \/\/\/\/\/\/\/\/\/\/\/\/\/\
|     __     |    \/\/\/\/\/\/\/\/\/\/\/\/\/\
|    /  \    |     |   /\   /\   /\   /\   |
|   |    |   |     |  |  | |  | |  | |  |  |
|   |-   |   |     |  |__| |__| |__| |__|  |
|___|____|___|_____|_______________________|";
            SetCursorPositionAndPrint(5, asciiArt);            // Vị trí ASCII art (dòng 5)
        }
        else
        {
            message = $"🚎 Bắt đầu bàn cờ tại UEH - Cơ sở {node.Id} 🚎";
            guide = "Nhấn Enter để tiếp tục";
            asciiArt = @"
                                         /\
                                               ||______||
                                               || ^  ^ ||
                                               \| |  | |/
                                                |______|
              __                                |  __  |
             /  \       ________________________|_/  \_|__
            / ^^ \     /=========================/ ^^ \===|
           /  []  \   /=========================/  []  \==|
          /________\ /=========================/________\=|
       *  |        |/==========================|        |=|
      *** | ^^  ^^ |---------------------------| ^^  ^^ |--
     *****| []  [] |           _____           | []  [] | |
    *******        |          /_____\          |      * | |
   *********^^  ^^ |  ^^  ^^  |  |  |  ^^  ^^  |     ***| |
  ***********]  [] |  []  []  |  |  |  []  []  | ===***** |
 *************     |         @|__|__|@         |/ |*******|
***************   ***********--=====--**********| *********
***************___*********** |=====| **********|***********
 *************     ********* /=======\ ******** | *********";
            SetCursorPositionAndPrint(4, asciiArt);            // Vị trí ASCII art (dòng 4)
        }

        // Tùy chỉnh vị trí và màu sắc
        SetCursorPositionAndPrint(2, message);            // Vị trí thông điệp (dòng 2)
        SetCursorPositionAndPrint(4, $"Độ khó: {node.Difficulty}"); // Vị trí độ khó (dòng 4)

        Console.ForegroundColor = ConsoleColor.White; // Đặt màu trắng cho ASCII art
        
        SetCursorPositionAndPrint(28, guide);
    }

    // Phương thức hỗ trợ đặt vị trí và in nội dung
    private void SetCursorPositionAndPrint(int top, string text)
    {
        string[] lines = text.Split('\n'); // Chia nội dung theo dòng

        foreach (var line in lines)
        {
            int leftPadding = (Console.WindowWidth - line.Length) / 2;
            Console.SetCursorPosition(leftPadding, top++);
            Console.WriteLine(line);
        }
    }



    // Giả lập gọi API của Stockfish để lấy số nummove theo độ khó
    private int GetStockfishNumMoves(int difficulty)
    {
        // Tùy thuộc vào độ khó, trả về số nước đi từ Stockfish
        return difficulty;
    }

    // Vẽ đường viền ngang bằng các ký tự *
    private void DrawStarBorder(int width)
    {
        for (int i = 0; i < width; i++)
        {
            Console.Write("*");
        }
        Console.WriteLine();
    }

    // Vẽ đường viền dọc bằng các ký tự *
    private void DrawStarVerticalLines(int leftPadding, int startY, int endY, int width)
    {
        for (int y = startY; y < endY; y++)
        {
            Console.SetCursorPosition(leftPadding, y);
            Console.Write("*");

            Console.SetCursorPosition(leftPadding + width - 1, y);
            Console.Write("*");
        }
    }

    // Phương thức vẽ viền hàng
    private void PrintRowBorder(CampaignNode[,] map, int row, (int x, int y) selectedNode, string border, int spacing, int leftPadding)
    {
        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        for (int col = 0; col < map.GetLength(1); col++)
        {
            var node = map[row, col];
            if (node != null)
            {
                if (row == selectedNode.x && col == selectedNode.y)
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write(border);
                Console.ResetColor();
            }
            else
            {
                Console.Write(new string(' ', border.Length));
            }
            Console.Write(new string(' ', spacing));
        }
        Console.WriteLine();
    }

    // Phương thức vẽ nội dung hàng
    private void PrintRowContent(CampaignNode[,] map, int row, (int x, int y) selectedNode, int spacing, int leftPadding)
    {
        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        for (int col = 0; col < map.GetLength(1); col++)
        {
            var node = map[row, col];
            if (node != null)
            {
                if (row == selectedNode.x && col == selectedNode.y)
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write($"│Màn {node.Difficulty}│"); // Thêm khoảng trắng để tăng bề rộng
                Console.ResetColor();
            }
            else
            {
                Console.Write("       "); // Tương ứng với độ rộng mới
            }
            Console.Write(new string(' ', spacing));
        }
        Console.WriteLine();
    }
   



   

}


