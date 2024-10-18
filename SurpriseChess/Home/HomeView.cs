namespace SurpriseChess;
using System;
using System.Diagnostics;
using System.Text;

public class HomeView
{
    public void Render(HomeModel model)
    {
        Console.Clear();
        int consoleWidth = Console.WindowWidth; // Sử dụng chiều rộng bảng điều khiển động
        Console.CursorVisible = false; // Giấu con trỏ
        int originalTop = Console.CursorTop;
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
        Console.WriteLine(new string('\n', 2));

        int maxOptionWidth = model.Options.Max(option => option.Length) + 2; // Correct calculation
        int consoleHeight = Console.WindowHeight;
        int topOffset = (consoleHeight - model.Options.Length * 3) / 2; // Correct top offset calculation (outside loop)


        for (int i = 0; i < model.Options.Length; i++)
        {
            string option = model.Options[i];
            string rectangleTop = $"┌{new string('─', maxOptionWidth)}┐";
            
            // Correctly padded and centered line within the middle of the box:
            string rectangleMiddle = $"│{new string(' ', Math.Max(0, (maxOptionWidth - option.Length) / 2))}{option}{new string(' ', Math.Max(0, (maxOptionWidth - option.Length + 1) / 2))}│";
            string rectangleBottom = $"└{new string('─', maxOptionWidth)}┘";


            int currentTop = topOffset + i * 3;

            if (currentTop >= 0 && currentTop < consoleHeight - 2)
            {
                ConsoleColor foregroundColor = (i == model.SelectedIndex) ? ConsoleColor.Green : ConsoleColor.White;
                DrawCenteredText(consoleWidth, rectangleTop, foregroundColor);               
                DrawCenteredText(consoleWidth, rectangleMiddle, foregroundColor); // Draw the combined box and text
                Console.ResetColor();
                DrawCenteredText(consoleWidth, rectangleBottom, foregroundColor);
            }
        }
        Console.WriteLine();
        RenderInstructions();
    }

    private void RenderInstructions()
    {
        int consoleWidth = Console.WindowWidth;
        int instructionTop = Console.WindowHeight - 8;

        DrawCenteredText(consoleWidth, "Dùng ↑ ↓ → ← để điều hướng");
        DrawCenteredText(consoleWidth, "Nhấn Enter để chọn");
        DrawCenteredText(consoleWidth, "Nhấn Backspace để thoát");
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
    
    private void DrawAtCursor(int left, int top, string text, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.ForegroundColor = color;
        string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        int currentTop = top;

        foreach (string line in lines)
        {
            Console.SetCursorPosition(left, currentTop);
            Console.Write(line);
            currentTop++;
        }
        Console.ResetColor();
    }

}
