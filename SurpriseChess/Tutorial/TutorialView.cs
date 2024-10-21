namespace SurpriseChess;

public class TutorialView
{
    public void Render()
    {
        Console.Clear(); // Xóa màn hình
        int consoleWidth = Console.WindowWidth; // Lấy chiều rộng của console
        Console.CursorVisible = false; // Ẩn con trỏ

        // Hiển thị tiêu đề
        DrawCenteredText(consoleWidth, "✩░▒▓▆▅▃▂▁HƯỚNG DẪN▁▂▃▅▆▓▒░✩", ConsoleColor.DarkGreen);
        Console.WriteLine();
        DrawCenteredText(consoleWidth, "Chào mừng bạn đến với Surprise Chess – phiên bản mới mẻ và thú vị của cờ vua truyền thống!", ConsoleColor.Green);
        DrawCenteredText(consoleWidth, "Surprise Chess có lối chơi tương tự như cờ vua nhưng có thêm những tính năng thú vị khác. " +
            "\nNếu bạn yêu thích trải nghiệm bất ngờ thì Surprise Chess là trò chơi mà bạn không thể nào bỏ qua!!!");
        Console.WriteLine();

        // Cập nhật tính năng mới
        DrawCenteredText(consoleWidth, "CẬP NHẬT", ConsoleColor.Green);
        int originalTop = Console.CursorTop; // Lưu tọa độ y hiện tại

        // Mô tả quân cờ
        DrawCenteredText(consoleWidth, "- Quân cờ được đại diện bằng các emoji: Vương quốc 🏰 và Rừng sâu 🌳");
        Console.SetCursorPosition(14, originalTop + 2);
        Console.WriteLine("- Quân cờ có thể nhận được các hiệu ứng đặc biệt:");

        // Mô tả các hiệu ứng đặc biệt
        string[] specialEffects =
        {
            "+ Bảo vệ: Quân cờ được bảo vệ khỏi một lần tấn công.",
            "+ Tàng hình: Quân cờ trở nên vô hình trong một số lượt đi nhất định.",
            "+ Tê liệt: Quân cờ không thể di chuyển trong một số lượt.",
            "+ Biến đổi: Quân cờ biến thành một quân cờ khác."
        };

        // In ra các hiệu ứng đặc biệt
        PrintList(originalTop, specialEffects, 3);

        // Mô tả các tính năng mới
        string[] newFeatures =
        {
            "- Hiển thị quân cờ bị bắt và tính điểm",
            "- Tạo API dựa trên Stockfish với các độ khó khác nhau.",
            "- Chọn bàn chơi dựa trên các bản đồ cơ sở của UEH.",
            "- Tích hợp đồng hồ đếm giờ cho mỗi người chơi.",
            "- Lưu và xem lịch sử ván cờ.",
            "- Hiển thị nước đi tốt nhất."
        };

        // In ra các tính năng mới
        PrintList(originalTop, newFeatures, 8);

        Console.WriteLine(); // Xuống dòng
        DrawCenteredText(consoleWidth, "Backspace để thoát"); // Hướng dẫn thoát
    }

    // Phương thức in văn bản căn giữa với màu sắc
    private void DrawCenteredText(int consoleWidth, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color; // Đặt màu chữ
        string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // Tách văn bản thành các dòng
        foreach (string line in lines)
        {
            int spaces = (consoleWidth - line.Length) / 2; // Tính khoảng cách để căn giữa
            Console.WriteLine(new string(' ', Math.Max(0, spaces)) + line); // In dòng căn giữa
        }
        Console.ResetColor(); // Đặt lại màu sắc
    }

    // Phương thức in danh sách văn bản với tọa độ cụ thể
    private void PrintList(int originalTop, string[] items, int startLine)
    {
        for (int i = 0; i < items.Length; i++)
        {
            Console.SetCursorPosition(20, originalTop + startLine + i); // Đặt vị trí con trỏ
            Console.WriteLine(items[i]); // In từng mục trong danh sách
        }
    }
}
