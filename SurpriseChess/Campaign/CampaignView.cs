using Microsoft.VisualBasic;
using System;

namespace SurpriseChess
{
    public class CampaignView
    {
        private const int BorderWidth = 120;
        private const int BorderHeight = 28;

        public void Render(CampaignModel model)
        {
            Console.Clear();
            DrawBorder();

            // Define positions for the grid cells
            int cellWidth = 12; // Each cell will be 7 characters wide
            int cellHeight = 6; // Each cell will be 3 characters tall

            int startRow = (BorderHeight - cellHeight * 3) / 2; // Starting row for the grid
            int startCol = (BorderWidth - cellWidth * 3) / 2; // Starting column for the grid (21 chars for 3 items)

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Calculate position in the console
                    int currentRow = startRow + row * cellHeight + 1; // +1 for the border
                    int currentCol = startCol + col * cellWidth;

                    // Draw the ASCII square for each cell
                    DrawCell(model, row, col, currentRow, currentCol);
                }
            }

            string info = "Use arrow keys to navigate the grid. Press 'Esc' to exit.";
            Console.SetCursorPosition((BorderWidth - info.Length) / 2, BorderHeight - 4); // Move cursor to bottom of the border
            Console.WriteLine(info);
        }

        private void DrawCell(CampaignModel model, int row, int col, int currentRow, int currentCol)
        {
            if (row == model.SelectedRow && col == model.SelectedCol)
            {
                Console.ForegroundColor = ConsoleColor.Green; // Set color to green for selected
            }

            // Draw the top border of the cell
            Console.SetCursorPosition(currentCol, currentRow);
            Console.Write("+--------+");

            // Draw the content of the cell
            Console.SetCursorPosition(currentCol, currentRow + 1);
            Console.Write($"|        |");

            string nodeName = $"CS {model.CampaignGrid[row, col].Id}";
            Console.SetCursorPosition(currentCol + (10 - nodeName.Length) / 2, currentRow + 1);
            Console.Write(nodeName);

            // Draw the bottom border of the cell
            Console.SetCursorPosition(currentCol, currentRow + 2);
            Console.Write("+--------+");

            Console.ResetColor(); // Reset color for other text
        }

        private void DrawBorder()
        {
            // Draw the top border
            Console.WriteLine(new string('*', BorderWidth));

            // Draw the middle part of the border
            for (int i = 0; i < BorderHeight - 2; i++)
            {
                Console.WriteLine("*" + new string(' ', BorderWidth - 2) + "*");
            }

            // Draw the bottom border
            Console.WriteLine(new string('*', BorderWidth));
        }
    }
}
