using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurpriseChess.Game
{
    public class ChessTimer
    {
        private TimeSpan player1Time;
        private TimeSpan player2Time;
        private CancellationTokenSource cancellationTokenSource;

        public event Action<int> TimeExpired; // Event to notify when time expires

        public ChessTimer(TimeSpan initialTime)
        {
            player1Time = initialTime;
            player2Time = initialTime;
        }
        public void StartPlayer1Timer()
        {

            StopTimer(); // Stop any running timer

            cancellationTokenSource = new CancellationTokenSource();
            StartTimer(1, cancellationTokenSource.Token);
        }

        public void StartPlayer2Timer()
        {

            StopTimer(); // Stop any running timer

            cancellationTokenSource = new CancellationTokenSource();
            StartTimer(2, cancellationTokenSource.Token);
        }


        private async void StartTimer(int player, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested) // Check for cancellation
            {

                await Task.Delay(1000, cancellationToken); // Wait 1 second
                if (player == 1)
                {
                    player1Time = player1Time.Subtract(TimeSpan.FromSeconds(1));

                    if (player1Time <= TimeSpan.Zero)
                    {

                        TimeExpired?.Invoke(1);
                        StopTimer();
                        break;
                    }


                }

                else
                {
                    player2Time = player2Time.Subtract(TimeSpan.FromSeconds(1));
                    if (player2Time <= TimeSpan.Zero)
                    {
                        TimeExpired?.Invoke(2);
                        StopTimer();
                        break;

                    }
                }

            }


        }



        public void StopTimer()
        {
            cancellationTokenSource?.Cancel();

        }




        public TimeSpan GetPlayer1Time() => player1Time;
        public TimeSpan GetPlayer2Time() => player2Time;




    }
}
