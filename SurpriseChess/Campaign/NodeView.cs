namespace SurpriseChess
{
    // Lớp NodeView chịu trách nhiệm hiển thị thông tin của một CampaignNode
    public class NodeView
    {
        private const int BorderWidth = 120; // Chiều rộng của khung hiển thị
        private const int BorderHeight = 29; // Chiều cao của khung hiển thị

        // Phương thức Render để vẽ giao diện cho node
        public void Render(CampaignNode node)
        {
            Console.Clear(); // Xóa màn hình console
            DrawBorder(); // Vẽ khung bao xung quanh

            // Trung tâm ID và Độ khó
            string idText = $"Cơ sở {node.Id}"; // Nội dung ID của node
            string difficultyText = $"Độ khó: {node.Difficulty}"; // Nội dung độ khó của node
            int idPosition = (BorderWidth - idText.Length) / 2; // Tính toán vị trí cho ID
            int difficultyPosition = (BorderWidth - difficultyText.Length) / 2; // Tính toán vị trí cho độ khó

            // Vẽ ASCII art dựa trên độ khó
            DrawASCIIART(GetASCIIArt(node.Difficulty));

            // Hiển thị ID và Độ khó đã được căn giữa
            DrawCenteredText(idText, 2);
            DrawCenteredText(difficultyText, 3);

            // Thông báo cho người dùng nhấn phím để bắt đầu
            DrawCenteredText("Nhấn nút bất kỳ để bắt đầu màn chơi...", BorderHeight - 3);
        }

        // Vẽ khung bao xung quanh
        private void DrawBorder()
        {
            // Vẽ khung trên
            Console.WriteLine(new string('*', BorderWidth));

            // Vẽ phần giữa của khung
            for (int i = 0; i < BorderHeight - 2; i++)
            {
                Console.WriteLine("*" + new string(' ', BorderWidth - 2) + "*");
            }

            // Vẽ khung dưới
            Console.WriteLine(new string('*', BorderWidth));
        }

        // Hiển thị văn bản căn giữa ở hàng cụ thể
        private void DrawCenteredText(string text, int col)
        {
            Console.SetCursorPosition((BorderWidth - text.Length) / 2, col); // Đặt con trỏ tới vị trí căn giữa
            Console.WriteLine(text); // In văn bản
        }

        // Lấy ASCII art dựa trên độ khó
        private List<string> GetASCIIArt(int difficulty)
        {
            return CampaignASCIIArt.Instance.NodeArt[difficulty - 1]; // Trả về ASCII art tương ứng
        }

        // Vẽ ASCII art tại một vị trí nhất định
        private void DrawASCIIART(List<string> lines)
        {
            int startX = (BorderWidth - GetMaxLineLength(lines)) / 2; // Tính toán vị trí X để căn giữa
            int startY = (BorderHeight - lines.Count) / 2; // Tính toán vị trí Y để căn giữa

            // In ASCII art tại vị trí đã tính
            for (int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.WriteLine(lines[i]); // In từng dòng của ASCII art
            }
        }

        // Lấy chiều dài lớn nhất của dòng trong một danh sách các dòng
        static int GetMaxLineLength(List<string> lines)
        {
            int maxLength = 0;
            foreach (string line in lines)
            {
                if (line.Length > maxLength) // Cập nhật chiều dài lớn nhất nếu cần
                {
                    maxLength = line.Length;
                }
            }
            return maxLength; // Trả về chiều dài lớn nhất
        }
    }
}
