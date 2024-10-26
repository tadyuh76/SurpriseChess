using System.Timers;
using Timer = System.Timers.Timer;

public class ChessTimer
{
    private readonly Timer _whiteTimer;
    private readonly Timer _blackTimer;
    public TimeSpan WhiteTime;
    public TimeSpan BlackTime;
    private bool _isWhiteTurn;

    public ChessTimer(TimeSpan initialTime)
    {
        WhiteTime = initialTime;
        BlackTime = initialTime;
        _isWhiteTurn = true; // Start with White's turn

        _whiteTimer = new Timer(1000); // 1 second intervals
        _whiteTimer.Elapsed += (sender, e) => UpdateTimer(ref WhiteTime);

        _blackTimer = new Timer(1000);
        _blackTimer.Elapsed += (sender, e) => UpdateTimer(ref BlackTime);
    }


    // Starts the timer for the current player
    public void Start()
    {
        if (_isWhiteTurn)
            _whiteTimer.Start();
        else
            _blackTimer.Start();
    }

    // Stops the timer for the current player
    public void Stop()
    {
        _whiteTimer.Stop();
        _blackTimer.Stop();
    }

    public void Dispose()
    {
        Stop();
        _whiteTimer.Dispose();
        _blackTimer.Dispose();
    }

    // Switches the turn to the other player
    public void UpdateTurn()
    {
        Stop(); // Stop the current player's timer

        _isWhiteTurn = !_isWhiteTurn; // Switch turn

        Start(); // Start the timer for the next player
    }

    // Gets the remaining time for the current player
    public TimeSpan GetRemainingTime()
    {
        return _isWhiteTurn ? WhiteTime : BlackTime;
    }

    // Updates the timer
    private void UpdateTimer(ref TimeSpan time)
    {
        if (time.TotalSeconds > 0)
        {
            time = time.Subtract(TimeSpan.FromSeconds(1)); // Decrement by 1 second
        }
        else
        {
            Stop(); // Stop the timer if time is up
            Console.WriteLine($"{(_isWhiteTurn ? "Vương quốc" : "Rừng sâu")} đã hết giờ!");
        }
    }

    // Debugging output for remaining time
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
