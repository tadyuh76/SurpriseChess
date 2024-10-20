namespace SurpriseChess;

public class HomeController : IController
{
    private readonly HomeModel model; // Mô hình chứa dữ liệu cho giao diện chính
    private readonly HomeView view; // Giao diện chính

    public HomeController(HomeModel model, HomeView view)
    {
        this.model = model; // Khởi tạo mô hình
        this.view = view; // Khởi tạo giao diện
    }

    // Chạy vòng lặp chính của giao diện chính
    public void Run()
    {
        ConsoleKey keyPressed;

        while (true) // Vòng lặp vô tận để chờ người dùng tương tác
        {
            view.Render(model); // Hiển thị giao diện
            keyPressed = Console.ReadKey().Key; // Đọc phím người dùng nhấn

            switch (keyPressed) // Xử lý phím nhấn
            {
                case ConsoleKey.UpArrow: // Di chuyển lên
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow: // Di chuyển xuống
                    MoveDown();
                    break;
                case ConsoleKey.Enter: // Chọn tùy chọn
                    SelectOption();
                    break;
            }
        }
    }

    // Di chuyển lên một tùy chọn
    private void MoveUp()
    {
        model.SelectedIndex--; // Giảm chỉ số được chọn
        if (model.SelectedIndex < 0) // Nếu chỉ số dưới 0, quay lại tùy chọn cuối
        {
            model.SelectedIndex = model.Options.Length - 1; // Đặt lại chỉ số
        }
    }

    // Di chuyển xuống một tùy chọn
    private void MoveDown()
    {
        model.SelectedIndex++; // Tăng chỉ số được chọn
        if (model.SelectedIndex >= model.Options.Length) // Nếu vượt quá số tùy chọn, quay lại đầu
        {
            model.SelectedIndex = 0; // Đặt lại chỉ số
        }
    }

    // Xử lý chọn tùy chọn
    private void SelectOption()
    {
        switch (model.SelectedIndex) // Tùy theo chỉ số được chọn
        {
            case 0: // Chơi cờ với người chơi khác
                ScreenManager.Instance.NavigateToScreen(new ChessController(
                    new ChessModel(new Chess960(), null!), // Mô hình trò chơi
                    new ChessView(), // Giao diện trò chơi
                    GameMode.PlayerVsPlayer, // Chế độ chơi
                    null
                ));
                break;
            case 1: // Chế độ Campaign
                ScreenManager.Instance.NavigateToScreen(new CampaignController(
                    new CampaignModel(), // Mô hình Campaign
                    new CampaignView() // Giao diện Campaign
                ));
                break;
            case 2: // Hướng dẫn
                ScreenManager.Instance.NavigateToScreen(new TutorialController(
                    new TutorialView() // Giao diện hướng dẫn
                ));
                break;
            case 3: // Lịch sử trận đấu
                ScreenManager.Instance.NavigateToScreen(new MatchHistoryController(
                    new MatchHistoryModel(), // Mô hình lịch sử trận đấu
                    new MatchHistoryView() // Giao diện lịch sử trận đấu
                ));
                break;
        }
    }
}
