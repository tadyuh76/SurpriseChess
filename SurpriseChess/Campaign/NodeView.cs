
namespace SurpriseChess
{
    public class NodeView
    {
        private const int BorderWidth = 120;
        private const int BorderHeight = 29;

        public void Render(CampaignNode node)
        {
            Console.Clear();
            DrawBorder();

            // Center the ID and Difficulty
            string idText = $"Cơ sở {node.Id}";
            string difficultyText = $"Độ khó: {node.Difficulty}";
            int idPosition = (BorderWidth - idText.Length) / 2;
            int difficultyPosition = (BorderWidth - difficultyText.Length) / 2;


            // Draw ASCII art based on difficulty
            DrawASCIIART(GetASCIIArt(node.Difficulty));

            // Display the ID and Difficulty centered
            DrawCenteredText(idText, 2);
            DrawCenteredText(difficultyText, 3);

            DrawCenteredText("Nhấn nút bất kỳ để bắt đầu màn chơi...", BorderHeight - 3);
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

        private void DrawCenteredText(string text, int col)
        {
            Console.SetCursorPosition((BorderWidth - text.Length) / 2, col);
            Console.WriteLine(text);
        }

        private List<string> GetASCIIArt(int difficulty)
        {
            var instance = new CampaignASCIIArt();
            return instance.NodeArt[difficulty - 1];
        }

        private void DrawASCIIART(List<string> lines)
        {
            int startX = (BorderWidth - GetMaxLineLength(lines)) / 2;
            int startY = (BorderHeight - lines.Count) / 2;

            // Print the ASCII art centered
            for (int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.WriteLine(lines[i]);
            }
        }

        static int GetMaxLineLength(List<string> lines)
        {
            int maxLength = 0;
            foreach (string line in lines)
            {
                if (line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }
            return maxLength;
        }
    }
}
