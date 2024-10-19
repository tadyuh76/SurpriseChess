using System;
using System.Threading;
using System.Threading.Tasks;

namespace SurpriseChess.Game
{
    public class ChessTimer
    {
        private TimeSpan whiteTime;
        private TimeSpan blackTime;
        private CancellationTokenSource cancellationTokenSource;
        private int currentPlayer; // 1: White, 2: Black

        public event Action<int> TimeExpired; // Sự kiện khi thời gian hết
        public event Action<int, TimeSpan> TimeUpdated; // Sự kiện khi thời gian cập nhật

        public ChessTimer(TimeSpan initialTime)
        {
            whiteTime = initialTime;
            blackTime = initialTime;
            currentPlayer = 1;
        }

        public void StartTimer(int player)
        {
            StopTimer(); // Dừng bất kỳ timer nào đang chạy

            cancellationTokenSource = new CancellationTokenSource();
            currentPlayer = player;
            StartTimerInternal(player, cancellationTokenSource.Token);
        }

        private async void StartTimerInternal(int player, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken); // Chờ 1 giây

                    if (player == 1)
                    {
                        whiteTime = whiteTime.Subtract(TimeSpan.FromSeconds(1));
                        TimeUpdated?.Invoke(1, whiteTime);

                        if (whiteTime <= TimeSpan.Zero)
                        {
                            TimeExpired?.Invoke(1);
                            StopTimer();
                            break;
                        }
                    }
                    else
                    {
                        blackTime = blackTime.Subtract(TimeSpan.FromSeconds(1));
                        TimeUpdated?.Invoke(2, blackTime);

                        if (blackTime <= TimeSpan.Zero)
                        {
                            TimeExpired?.Invoke(2);
                            StopTimer();
                            break;
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // Bỏ qua ngoại lệ hủy bỏ
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong StartTimerInternal: {ex.Message}");
            }
        }

        public void StopTimer()
        {
            cancellationTokenSource?.Cancel();
        }

        public TimeSpan GetWhiteTime() => whiteTime;
        public TimeSpan GetBlackTime() => blackTime;

        public int GetCurrentPlayer() => currentPlayer;
    }
}
