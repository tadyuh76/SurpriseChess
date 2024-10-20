namespace SurpriseChess
{
    // Lớp CampaignView chịu trách nhiệm cho việc hiển thị lưới các node
    public class CampaignView
    {
        private const int BorderWidth = 120; // Độ rộng của khung viền
        private const int BorderHeight = 28; // Độ cao của khung viền

        // Phương thức Render để hiển thị mô hình
        public void Render(CampaignModel model)
        {
            Console.Clear(); // Xóa màn hình console
            DrawBorder(); // Vẽ khung viền

            DrawCenteredText("Chế độ Chiến dịch - Bản đồ UEH", 3);

            // Định nghĩa kích thước cho các ô trong lưới
            int cellWidth = 12; // Mỗi ô có chiều rộng 12 ký tự
            int cellHeight = 6; // Mỗi ô có chiều cao 6 ký tự

            // Tính toán vị trí bắt đầu cho lưới
            int startRow = (BorderHeight - cellHeight * 3) / 2; // Hàng bắt đầu cho lưới
            int startCol = (BorderWidth - cellWidth * 3) / 2; // Cột bắt đầu cho lưới

            // Vẽ lưới 3x3
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    // Tính toán vị trí hiện tại trong console
                    int currentRow = startRow + row * cellHeight + 1; // +1 cho viền
                    int currentCol = startCol + col * cellWidth;

                    // Vẽ ô cho mỗi cell
                    DrawCell(model, row, col, currentRow, currentCol);
                }
            }

            // Hiển thị thông tin hướng dẫn sử dụng
            DrawCenteredText("Dùng ↑ ↓ → ← để điều hướng", BorderHeight - 5);
            DrawCenteredText("Nhấn Enter để chọn", BorderHeight - 4);
            DrawCenteredText("Nhấn Backspace để thoát", BorderHeight - 3);
        }

        private void DrawCenteredText(string text, int col)
        {
            Console.SetCursorPosition((BorderWidth - text.Length) / 2, col); // Đặt con trỏ tới vị trí căn giữa
            Console.WriteLine(text); // In văn bản
        }

        // Phương thức vẽ mỗi ô trong lưới
        private void DrawCell(CampaignModel model, int row, int col, int currentRow, int currentCol)
        {
            // Kiểm tra nếu ô hiện tại được chọn
            if (row == model.SelectedRow && col == model.SelectedCol)
            {
                Console.ForegroundColor = ConsoleColor.Green; // Đặt màu cho ô được chọn
            }

            // Vẽ viền trên của ô
            Console.SetCursorPosition(currentCol, currentRow);
            Console.Write("+--------+");

            // Vẽ nội dung của ô
            Console.SetCursorPosition(currentCol, currentRow + 1);
            Console.Write($"|        |");

            string nodeName = $"CS {model.CampaignGrid[row, col].Id}"; // Tên của node
            Console.SetCursorPosition(currentCol + (10 - nodeName.Length) / 2, currentRow + 1); // Căn giữa tên node
            Console.Write(nodeName); // Hiển thị tên node

            // Vẽ viền dưới của ô
            Console.SetCursorPosition(currentCol, currentRow + 2);
            Console.Write("+--------+");

            Console.ResetColor(); // Đặt lại màu cho các văn bản khác
        }

        // Phương thức vẽ khung viền
        private void DrawBorder()
        {
            // Vẽ viền trên
            Console.WriteLine(new string('*', BorderWidth));

            // Vẽ phần giữa của khung
            for (int i = 0; i < BorderHeight - 2; i++)
            {
                Console.WriteLine("*" + new string(' ', BorderWidth - 2) + "*"); // Vẽ các dòng trống giữa khung
            }

            // Vẽ viền dưới
            Console.WriteLine(new string('*', BorderWidth));
        }
    }
}