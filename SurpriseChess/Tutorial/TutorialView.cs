namespace SurpriseChess;
public class TutorialView
{
    public void Render()
    {
        Console.Clear();
        int consoleWidth = Console.WindowWidth; // S? d?ng chi?u r?ng b?ng ?i?u khi?n ??ng
        Console.CursorVisible = false;
        DrawCenteredText(consoleWidth,"Hướng dẫn", ConsoleColor.DarkGreen);
        DrawCenteredText(consoleWidth, "Chào mừng bạn đến với Surprise Chess – phiên bản mới mẻ và thú vị của cờ vua truyền thống!", ConsoleColor.DarkGreen);
        DrawCenteredText(consoleWidth, "Quân cờ không còn là trắng và đen nữa, mà được đại diện bằng các emoji tượng trưng cho hai phe: Vương quốc 🏰Rừng sâu 🌳", ConsoleColor.DarkGreen);
        
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