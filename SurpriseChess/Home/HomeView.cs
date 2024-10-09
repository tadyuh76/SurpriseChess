﻿namespace SurpriseChess;
using System;

public class HomeView
{
    public void Render(HomeModel model)
    {
        Console.Clear();
        int consoleWidth = Console.WindowWidth; // Use dynamic console width
        Console.CursorVisible = false; // Giấu con trỏ

        DrawCenteredText(consoleWidth,@"
  ____                        _          
/ ___| _   _ _ __ _ __  _ __(_)___  ___ 
\___ \| | | | '__| '_ \| '__| / __|/ _ \
 ___) | |_| | |  | |_) | |  | \__ \  __/
|____/ \__,_|_|  | .__/|_|  |_|___/\___|
                 |_|                    
", ConsoleColor.Cyan);

        Console.WriteLine("Home Screen - Use Arrow Keys to navigate, Enter to select, Backspace to exit:\n");

        for (int i = 0; i < model.Options.Length; i++)
        {
            if (i == model.SelectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"> {model.Options[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  {model.Options[i]}");
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
