using System;
using System.Collections.Generic;
using System.IO; // Cần thiết để sử dụng StreamReader và StreamWriter

namespace SurpriseChess
{
    // Quản lý lịch sử trận đấu
    public static class MatchHistoryManager
    {
        private static readonly string FilePath = "match_history.txt"; // Đường dẫn cho file lưu trữ

        // Lưu trận đấu vào trong một file
        public static void SaveMatch(Match match)
        {
            // Mở file để ghi với chế độ thêm (append)
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                // Ghi ID trận đấu
                writer.WriteLine($"Match ID: {match.Id}");
                // Ghi ngày giờ trận đấu
                writer.WriteLine($"Date: {match.MatchDate}");
                // Ghi kết quả trận đấu
                writer.WriteLine($"Result: {match.Result}");
                writer.WriteLine("History:");

                // Ghi từng dòng FEN vào file
                foreach (string fen in match.HistoryFEN)
                {
                    writer.WriteLine(fen);
                }

                writer.WriteLine();  // Dòng trống để ngắt giữa các trận khác nhau
            }
        }

        // Tải lịch sử trận đấu
        public static List<Match> LoadMatches()
        {
            var matches = new List<Match>(); // Danh sách để lưu trữ các trận đấu
            // Kiểm tra xem file có tồn tại không
            if (File.Exists(FilePath))
            {
                // Mở file để đọc
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    string? line; // Biến để đọc từng dòng
                    Match? currentMatch = null; // Biến để lưu trận đấu hiện tại
                    List<string> fens = new List<string>(); // Danh sách lưu trữ các dòng FEN

                    // Đọc từng dòng trong file
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Nếu dòng bắt đầu bằng "Match ID:"
                        if (line.StartsWith("Match ID:"))
                        {
                            // Nếu có trận đấu hiện tại, lưu lịch sử FEN vào trận đấu đó
                            if (currentMatch != null)
                            {
                                currentMatch.HistoryFEN = new List<string>(fens);
                                matches.Add(currentMatch); // Thêm trận đấu vào danh sách
                            }

                            // Khởi tạo trận đấu mới
                            currentMatch = new Match
                            {
                                Id = int.Parse(line.Split(": ")[1]) // Lấy ID trận đấu
                            };
                            fens = new List<string>(); // Khởi tạo danh sách FEN mới
                        }
                        // Nếu dòng bắt đầu bằng "Date:"
                        else if (line.StartsWith("Date:"))
                        {
                            // Nếu có trận đấu hiện tại, lưu ngày giờ vào trận đấu đó
                            if (currentMatch != null)
                            {
                                currentMatch.MatchDate = DateTime.Parse(line.Split(": ")[1]); // Lấy ngày
                            }
                        }
                        // Nếu dòng bắt đầu bằng "Result:"
                        else if (line.StartsWith("Result:"))
                        {
                            // Nếu có trận đấu hiện tại, lưu kết quả vào trận đấu đó
                            if (currentMatch != null)
                            {
                                currentMatch.Result = line.Split(": ")[1]; // Lấy kết quả
                            }
                        }
                        // Nếu dòng không phải là trống
                        else if (!string.IsNullOrWhiteSpace(line))
                        {
                            fens.Add(line);  // Thêm dòng FEN vào danh sách
                        }
                    }

                    // Nếu vẫn còn trận đấu hiện tại, thêm vào danh sách
                    if (currentMatch != null)
                    {
                        currentMatch.HistoryFEN = new List<string>(fens);
                        matches.Add(currentMatch);
                    }
                }
            }

            return matches; // Trả về danh sách trận đấu
        }
    }
}
