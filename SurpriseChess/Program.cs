﻿using System.Text;

namespace SurpriseChess;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        HomeView homeView = new();
        HomeModel homeModel = new();
        HomeController homeController = new(homeModel, homeView);

        ScreenManager.Instance.NavigateToScreen(homeController);
    }
}
