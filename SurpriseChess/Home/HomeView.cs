namespace SurpriseChess;
using System;

public class HomeView
{
    public void Render(HomeModel model)
    {
        Console.Clear();
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
    }
}
