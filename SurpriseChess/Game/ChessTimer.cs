using System.Timers;
using Timer = System.Timers.Timer;

public class ChessTimer
{
    // Biến hẹn giờ cho người chơi trắng và đen
    private readonly Timer WhiteTimer;
    private readonly Timer BlackTimer;

    // Thời gian còn lại cho người chơi trắng và đen, công khai để truy cập từ bên ngoài lớp
    public TimeSpan WhiteTime;
    public TimeSpan BlackTime;

    // Xác định lượt hiện tại: true nếu là lượt của trắng, false nếu là lượt của đen
    private bool IsWhiteTurn;

    // Hàm khởi tạo, thiết lập thời gian ban đầu và cài đặt các sự kiện cho bộ đếm giờ
    public ChessTimer(TimeSpan initialTime)
    {
        WhiteTime = initialTime;
        BlackTime = initialTime;
        IsWhiteTurn = true; // Bắt đầu với lượt của trắng

        WhiteTimer = new Timer(1000); // Khoảng thời gian 1 giây
        WhiteTimer.Elapsed += (sender, e) => UpdateTimer(ref WhiteTime);

        BlackTimer = new Timer(1000);
        BlackTimer.Elapsed += (sender, e) => UpdateTimer(ref BlackTime);
    }

    // Bắt đầu đếm giờ cho người chơi hiện tại
    public void Start()
    {
        if (IsWhiteTurn)
            WhiteTimer.Start();
        else
            BlackTimer.Start();
    }

    // Dừng đếm giờ cho người chơi hiện tại
    public void Stop()
    {
        WhiteTimer.Stop();
        BlackTimer.Stop();
    }

    // Giải phóng tài nguyên khi không sử dụng bộ đếm giờ nữa
    public void Dispose()
    {
        Stop();
        WhiteTimer.Dispose();
        BlackTimer.Dispose();
    }

    // Chuyển lượt cho người chơi khác
    public void UpdateTurn()
    {
        Stop(); // Dừng bộ đếm giờ của người chơi hiện tại

        IsWhiteTurn = !IsWhiteTurn; // Đổi lượt

        Start(); // Bắt đầu đếm giờ cho người chơi tiếp theo
    }

    // Lấy thời gian còn lại của người chơi hiện tại
    public TimeSpan GetRemainingTime()
    {
        return IsWhiteTurn ? WhiteTime : BlackTime;
    }

    // Cập nhật thời gian cho bộ đếm giờ
    private void UpdateTimer(ref TimeSpan time)
    {
        if (time.TotalSeconds > 0)
        {
            time = time.Subtract(TimeSpan.FromSeconds(1)); // Giảm 1 giây
        }
        else
        {
            Stop(); // Dừng bộ đếm giờ nếu hết thời gian
        }
    }

    // Hiển thị thời gian còn lại của cả hai người chơi (dành cho mục đích debug)
    public void PrintRemainingTime()
    {
        string blackTimerText = "Thời gian còn lại của Rừng sâu: ";
        string whiteTimerText = "Thời gian còn lại của Vương quốc: ";

        Console.SetCursorPosition(blackTimerText.Length, 1);
        Console.WriteLine(BlackTime);

        Console.SetCursorPosition(whiteTimerText.Length, 14);
        Console.WriteLine(WhiteTime);
    }
}
