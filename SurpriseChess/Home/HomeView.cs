namespace SurpriseChess;

public class HomeView
{
    // Phương thức Render chịu trách nhiệm hiển thị giao diện chính
    public void Render(HomeModel model)
    {
        Console.Clear(); // Xóa màn hình console
        int consoleWidth = Console.WindowWidth; // Lấy chiều rộng của console
        Console.CursorVisible = false; // Ẩn con trỏ
        int originalTop = Console.CursorTop; // Lưu vị trí con trỏ hiện tại

        // Vẽ logo trò chơi
        DrawAtCursor(25, originalTop, @" ____                        _          
/ ___| _   _ _ __ _ __  _ __(_)___  ___ 
\___ \| | | | '__| '_ \| '__| / __|/ _ \
 ___) | |_| | |  | |_) | |  | \__ \  __/
|____/ \__,_|_|  | .__/|_|  |_|___/\___|
                 |_|                    ", ConsoleColor.Cyan);

        DrawAtCursor(67, originalTop, @"  ____ _                   
 / ___| |__   ___  ___ ___ 
| |   | '_ \ / _ \/ __/ __|
| |___| | | |  __/\__ \__ \
 \____|_| |_|\___||___/___/", ConsoleColor.Yellow);

        Console.WriteLine(new string('\n', 2)); // Thêm khoảng trắng giữa logo và các tùy chọn

        int maxOptionWidth = model.Options.Max(option => option.Length) + 2; // Tính chiều rộng tối đa cho các tùy chọn
        int consoleHeight = Console.WindowHeight; // Lấy chiều cao của console
        int topOffset = (consoleHeight - model.Options.Length * 3) / 2; // Tính khoảng cách từ trên xuống cho các tùy chọn

        // Vẽ các tùy chọn trong menu
        for (int i = 0; i < model.Options.Length; i++)
        {
            string option = model.Options[i];
            string rectangleTop = $"┌{new string('─', maxOptionWidth)}┐";
            string rectangleMiddle = $"│{new string(' ', Math.Max(0, (maxOptionWidth - option.Length) / 2))}{option}{new string(' ', Math.Max(0, (maxOptionWidth - option.Length + 1) / 2))}│";
            string rectangleBottom = $"└{new string('─', maxOptionWidth)}┘";

            int currentTop = topOffset + i * 3;

            if (currentTop >= 0 && currentTop < consoleHeight - 2) // Kiểm tra vị trí hợp lệ để vẽ
            {
                ConsoleColor foregroundColor = (i == model.SelectedIndex) ? ConsoleColor.Green : ConsoleColor.White; // Đặt màu sắc cho tùy chọn đang chọn
                DrawCenteredText(consoleWidth, rectangleTop, foregroundColor);
                DrawCenteredText(consoleWidth, rectangleMiddle, foregroundColor); // Vẽ tùy chọn
                Console.ResetColor();
                DrawCenteredText(consoleWidth, rectangleBottom, foregroundColor);
            }
        }
        Console.WriteLine();
        RenderInstructions(); // Vẽ hướng dẫn cho người dùng
    }

    // Phương thức vẽ hướng dẫn cho người dùng
    private void RenderInstructions()
    {
        int consoleWidth = Console.WindowWidth;
        int instructionTop = Console.WindowHeight - 8; // Vị trí cho hướng dẫn

        DrawCenteredText(consoleWidth, "Dùng ↑ ↓ → ← để điều hướng"); // Hướng dẫn điều hướng
        DrawCenteredText(consoleWidth, "Nhấn Enter để chọn"); // Hướng dẫn chọn
        DrawCenteredText(consoleWidth, "Nhấn Backspace để thoát"); // Hướng dẫn thoát
    }

    // Phương thức vẽ văn bản vào giữa màn hình
    private void DrawCenteredText(int consoleWidth, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color; // Đặt màu chữ
        string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // Chia văn bản thành các dòng
        foreach (string line in lines)
        {
            int spaces = (consoleWidth - line.Length) / 2; // Tính khoảng trắng trước dòng
            Console.WriteLine(new string(' ', Math.Max(0, spaces)) + line); // Vẽ dòng với khoảng trắng
        }
        Console.ResetColor(); // Đặt lại màu chữ về mặc định
    }

    // Phương thức vẽ văn bản ở vị trí con trỏ cụ thể
    private void DrawAtCursor(int left, int top, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color; // Đặt màu chữ
        string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // Chia văn bản thành các dòng
        int currentTop = top;

        foreach (string line in lines)
        {
            Console.SetCursorPosition(left, currentTop); // Đặt vị trí con trỏ
            Console.Write(line); // Vẽ văn bản
            currentTop++; // Tăng vị trí con trỏ
        }
        Console.ResetColor(); // Đặt lại màu chữ về mặc định
    }
}
