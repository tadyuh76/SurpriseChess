namespace SurpriseChess;

// Điều khiển lịch sử trận đấu
public class MatchHistoryController : IController
{
    private readonly MatchHistoryModel model; // Mô hình chứa dữ liệu lịch sử trận đấu
    private readonly MatchHistoryView view; // Giao diện để hiển thị lịch sử trận đấu

    // Khởi tạo MatchHistoryController với mô hình và giao diện
    public MatchHistoryController(MatchHistoryModel model, MatchHistoryView view)
    {
        this.model = model; // Gán mô hình
        this.view = view; // Gán giao diện
    }

    // Phương thức chính để chạy điều khiển
    public void Run()
    {
        while (true) // Vòng lặp vô hạn cho đến khi người dùng thoát
        {
            view.RenderMatchList(model.Matches); // Hiển thị danh sách trận đấu
            int selectedId = view.GetSelectedMatchId(); // Lấy ID trận đấu được chọn từ giao diện

            if (selectedId == -1) break; // Nếu không có ID hợp lệ, thoát vòng lặp

            var selectedMatch = model.GetMatchById(selectedId); // Lấy trận đấu theo ID
            if (selectedMatch != null) // Kiểm tra nếu trận đấu tồn tại
            {
                List<string> fenList = selectedMatch.HistoryFEN; // Lấy danh sách FEN của trận đấu
                var replayModel = new ReplayModel(fenList); // Tạo mô hình phát lại với danh sách FEN
                var replayView = new ReplayView(); // Tạo giao diện phát lại
                var replayController = new ReplayController(replayModel, replayView); // Tạo điều khiển phát lại
                ScreenManager.Instance.NavigateToScreen(replayController); // Chuyển sang màn hình phát lại
            }
            else
            {
                view.DisplayError("ID trận đấu không hợp lệ."); // Thông báo lỗi nếu ID không hợp lệ
            }
        }
    }
}
