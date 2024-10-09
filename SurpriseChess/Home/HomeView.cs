namespace SurpriseChess;
using System;

public class HomeView
{
    public void Render(HomeModel model)
    {
        Console.Clear();
        int consoleWidth = Console.WindowWidth; // Sử dụng chiều rộng bảng điều khiển động
        Console.CursorVisible = false; // Giấu con trỏ

        DrawCenteredText(consoleWidth,@"
  ____                        _          
/ ___| _   _ _ __ _ __  _ __(_)___  ___ 
\___ \| | | | '__| '_ \| '__| / __|/ _ \
 ___) | |_| | |  | |_) | |  | \__ \  __/
|____/ \__,_|_|  | .__/|_|  |_|___/\___|
                 |_|                    
", ConsoleColor.Cyan);

        // Căn giữa "Home Screen" 
        DrawCenteredText(consoleWidth,"Home Screen", ConsoleColor.White);

        // Căn giữa phần hướng dẫn 
        DrawCenteredText(consoleWidth, "Use Arrow Keys to navigate");
        DrawCenteredText(consoleWidth, "Enter to select");
        DrawCenteredText(consoleWidth, "Backspace to exit");
        Console.WriteLine();



        for (int i = 0; i < model.Options.Length; i++)
        {
            string option = model.Options[i];
            string prefix = (i == model.SelectedIndex) ? "> " : "  ";
            string line = $"{prefix}{option}";

            // Tính khoảng cách để căn giữa
            int spaces = (consoleWidth - line.Length) / 2;
            string centeredLine = new string(' ', Math.Max(0, spaces)) + line;

            if (i == model.SelectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(centeredLine); 
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(centeredLine);  
            }
        }
        
        // Draw "Chess" footer
        DrawCenteredText(consoleWidth,@"
   ____ _                   
 / ___| |__   ___  ___ ___ 
| |   | '_ \ / _ \/ __/ __|
| |___| | | |  __/\__ \__ \
 \____|_| |_|\___||___/___/
", ConsoleColor.Yellow);
    
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
