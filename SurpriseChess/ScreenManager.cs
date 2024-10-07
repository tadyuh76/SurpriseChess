namespace SurpriseChess;

// Singleton Design Pattern
public class ScreenManager
{
    // Biến static để lưu trữ duy nhất một instance của ScreenManager
    private static ScreenManager? _instance;

    // Constructor được đặt private để ngăn việc tạo mới đối tượng từ bên ngoài
    private ScreenManager() { }

    // Truy cập đến instance duy nhất của ScreenManager
    public static ScreenManager Instance
    {
        get
        {
            // Nếu instance chưa tồn tại, tạo mới một instance
            if (_instance == null)
                _instance = new ScreenManager();
            return _instance;
        }
    }

    // Điều hướng đến màn hình mong muốn bằng cách gọi controller tương ứng
    public void NavigateToScreen(IController screenController)
    {
        // Thực hiện phương thức Run của controller màn hình
        screenController.Run();
    }
}