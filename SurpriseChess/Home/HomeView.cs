namespace SurpriseChess;
using System;
using System.Text;

public class HomeView
{
    public void Render(HomeModel model)
    {
        Console.Clear();
        int consoleWidth = Console.WindowWidth; // Sử dụng chiều rộng bảng điều khiển động
        Console.CursorVisible = false; // Giấu con trỏ

        DrawCenteredText(consoleWidth, @" ____                        _           
/ ___| _   _ _ __ _ __  _ __(_)___  ___  
\___ \| | | | '__| '_ \| '__| / __|/ _ \ 
 ___) | |_| | |  | |_) | |  | \__ \  __/ 
|____/ \__,_|_|  | .__/|_|  |_|___/\___| 
                 |_|                     ", ConsoleColor.Cyan);

       /* DrawCenteredText(consoleWidth, @"  ____ _                   
 / ___| |__   ___  ___ ___ 
| |   | '_ \ / _ \/ __/ __|
| |___| | | |  __/\__ \__ \
 \____|_| |_|\___||___/___/", ConsoleColor.Yellow);*/
        //Console.WriteLine();

        int maxOptionWidth = model.Options.Max(option => option.Length) + 2; // Correct calculation

        int consoleHeight = Console.WindowHeight;
        int topOffset = (consoleHeight - model.Options.Length * 3) / 2; // Correct top offset calculation (outside loop)


        for (int i = 0; i < model.Options.Length; i++)
        {
            string option = model.Options[i];
            string prefix = (i == model.SelectedIndex) ? ">" : "";
            string line = $"{prefix}{option}";
            //Console.ForegroundColor = (i == model.SelectedIndex) ? ConsoleColor.Green : ConsoleColor.White;
            string rectangleTop = $"┌{new string('─', maxOptionWidth)}┐";
            // Correctly padded and centered line within the middle of the box:
            string rectangleMiddle = $"│{new string(' ', Math.Max(0, (maxOptionWidth - option.Length) / 2))}{line}{new string(' ', Math.Max(0, (maxOptionWidth - option.Length + 1) / 2))}│";
            string rectangleBottom = $"└{new string('─', maxOptionWidth)}┘";


            int currentTop = topOffset + i * 3;

            if (currentTop >= 0 && currentTop < consoleHeight - 2)
            {
                DrawCenteredText(consoleWidth, rectangleTop);

                Console.ForegroundColor = (i == model.SelectedIndex) ? ConsoleColor.Green : ConsoleColor.White;
                DrawCenteredText(consoleWidth, rectangleMiddle); // Draw the combined box and text
                Console.ResetColor();
                DrawCenteredText(consoleWidth, rectangleBottom);
            }

            Console.WriteLine();
        }


        // Căn giữa phần hướng dẫn 
        DrawCenteredText(consoleWidth, "Use Arrow Keys to navigate");
        DrawCenteredText(consoleWidth, "Enter to select");
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
