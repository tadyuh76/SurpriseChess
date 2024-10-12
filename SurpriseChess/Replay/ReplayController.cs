using System;
using System.Collections.Generic;

namespace SurpriseChess
{
    public class ReplayController : IController
    {
        private readonly List<string> fenList;
        private readonly ReplayView view;
        private ReplayBoard currentBoard;
        private int currentMoveIndex;

        public ReplayController(List<string> fenList, ReplayView view)
        {
            this.fenList = fenList;
            this.view = view;
            this.currentMoveIndex = 0;
            this.currentBoard = new ReplayBoard(fenList[0]);
        }

        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                view.RenderBoard(currentBoard);
                view.DisplayNavigationOptions();

                ConsoleKeyInfo keyInfo = view.GetUserInput();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (currentMoveIndex < fenList.Count - 1)
                        {
                            currentMoveIndex++;
                            currentBoard = new ReplayBoard(fenList[currentMoveIndex]);
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (currentMoveIndex > 0)
                        {
                            currentMoveIndex--;
                            currentBoard = new ReplayBoard(fenList[currentMoveIndex]);
                        }
                        break;

                    case ConsoleKey.Backspace:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}