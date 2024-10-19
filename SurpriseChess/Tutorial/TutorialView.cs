namespace SurpriseChess;
public class TutorialView
{
    public void Render()
    {
        Console.Clear();
        int consoleWidth = Console.WindowWidth; // S? d?ng chi?u r?ng b?ng ?i?u khi?n ??ng
        Console.CursorVisible = false;

        DrawCenteredText(consoleWidth, "✩░▒▓▆▅▃▂▁HƯỚNG DẪN▁▂▃▅▆▓▒░✩", ConsoleColor.DarkGreen);
        Console.WriteLine();
        DrawCenteredText(consoleWidth, "Chào mừng bạn đến với Surprise Chess – phiên bản mới mẻ và thú vị của cờ vua truyền thống!", ConsoleColor.Green);
        DrawCenteredText(consoleWidth, "Surprise Chess là có lối chơi tương tự như cờ vua nhưng có thêm những tính năng thú vị khác. " +
            "\nNếu bạn là người yêu thích với những trải nghiệm bất ngờ thì Surprise Chess là trò chơi mà bạn không thể nào bỏ qua!!!");
        Console.WriteLine();
        
        DrawCenteredText(consoleWidth, "CẬP NHẬT", ConsoleColor.Green);
        int originalTop = Console.CursorTop;// Tọa độ y của chữ cái đầu tiên
        DrawCenteredText(consoleWidth, "- Quân cờ được đại diện bằng các emoji tượng trưng cho hai phe: Vương quốc 🏰 và Rừng sâu 🌳"); 
        Console.SetCursorPosition(14, originalTop + 2); 
        Console.WriteLine("- Quân cờ có thể nhận được các hiệu ứng đặc biệt:");
        Console.WriteLine() ;
        for (int i = 3; i < 7; i++) 
        {
            Console.SetCursorPosition(20, originalTop + i);
            switch (i) 
            {
                case 3: Console.WriteLine("+ Bảo vệ: Quân cờ được bảo vệ khỏi một lần tấn công.");
                    break;
                case 4:
                    Console.WriteLine("+ Tàng hình: Quân cờ trở nên vô hình trong một số lượt đi nhất định.");
                    break;
                case 5:
                    Console.WriteLine("+ Tê liệt: Quân cờ không thể di chuyển trong một số lượt.");
                    break;
                case 6:
                    Console.WriteLine("+ Biến đổi: Quân cờ biến thành một quân cờ khác.");
                    break;
                default:
                    break;
            }
        }
        for (int i = 8; i < 19; i+=2)
        {
            Console.SetCursorPosition(14, originalTop + i);
            switch (i)
            {
                case 8:
                    Console.WriteLine("- Hiển thị quân cờ bị bắt và tính điểm");
                    break;
                case 10:
                    Console.WriteLine("- Tạo API dựa trên Stockfish với các độ khó khác nhau.");
                    break;
                case 12:
                    Console.WriteLine("- Chọn bàn chơi dựa trên các bản đồ cơ sở của UEH.");
                    break;
                case 14:
                    Console.WriteLine("- Tích hợp đồng hồ đếm giờ cho mỗi người chơi.");
                    break;
                case 16:
                    Console.WriteLine("- Lưu và xem lịch sử ván cờ.");
                    break;
                case 18:
                    Console.WriteLine("- Hiển thị nước đi tốt nhất.");
                    break;
                default:
                    break;
            }
        }
        Console.WriteLine();
        DrawCenteredText(consoleWidth, "Backspace to exit");
    }
    private void DrawCenteredText(int consoleWidth, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color;
        string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            int spaces = (consoleWidth - line.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(0, spaces)) + line);
        }
        Console.ResetColor();
    }
}