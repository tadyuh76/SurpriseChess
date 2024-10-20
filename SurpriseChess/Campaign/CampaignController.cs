namespace SurpriseChess;

// Lớp CampaignController, thực hiện giao tiếp giữa model và view trong mô hình MVC
public class CampaignController : IController
{
    // Các trường để lưu giữ model và view
    private readonly CampaignModel model;
    private readonly CampaignView view;

    // Constructor nhận vào model và view
    public CampaignController(CampaignModel model, CampaignView view)
    {
        this.model = model; // Khởi tạo model
        this.view = view;   // Khởi tạo view
    }

    // Phương thức để chạy ứng dụng
    public void Run()
    {
        view.Render(model); // Hiển thị view với dữ liệu từ model
        HandleInput();      // Xử lý đầu vào từ người dùng
    }

    // Phương thức để xử lý đầu vào từ bàn phím
    private void HandleInput()
    {
        while (true) // Vòng lặp vô hạn để liên tục nhận đầu vào
        {
            var key = Console.ReadKey(true).Key; // Đọc phím nhấn từ người dùng

            // Xử lý các phím nhấn khác nhau
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    model.MoveUp(); // Di chuyển lên
                    break;
                case ConsoleKey.DownArrow:
                    model.MoveDown(); // Di chuyển xuống
                    break;
                case ConsoleKey.LeftArrow:
                    model.MoveLeft(); // Di chuyển sang trái
                    break;
                case ConsoleKey.RightArrow:
                    model.MoveRight(); // Di chuyển sang phải
                    break;
                case ConsoleKey.Enter:
                    NavigateToInfoScreen(); // Điều hướng đến màn hình thông tin
                    break;
            }

            view.Render(model); // Cập nhật view sau khi xử lý đầu vào
        }
    }

    // Phương thức để điều hướng đến màn hình thông tin
    private void NavigateToInfoScreen()
    {
        // Lấy node đang được chọn từ lưới chiến dịch
        var node = model.CampaignGrid[model.SelectedRow, model.SelectedCol];

        // Tạo một controller cho node thông tin và điều hướng đến màn hình đó
        var nodeInfoController = new NodeController(new NodeView(), node);
        ScreenManager.Instance.NavigateToScreen(nodeInfoController);
    }
}
