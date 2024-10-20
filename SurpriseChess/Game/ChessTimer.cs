using System.Timers;
using Timer = System.Timers.Timer;

public class ChessTimer
{
    private readonly Timer _whiteTimer;
    private readonly Timer _blackTimer;
    private TimeSpan _whiteTime;
    private TimeSpan _blackTime;
    private bool _isWhiteTurn;

    public ChessTimer(TimeSpan initialTime)
    {
        _whiteTime = initialTime;
        _blackTime = initialTime;
        _isWhiteTurn = true; // Start with White's turn

        _whiteTimer = new Timer(1000); // 1 second intervals
        _whiteTimer.Elapsed += (sender, e) => UpdateTimer(ref _whiteTime);

        _blackTimer = new Timer(1000);
        _blackTimer.Elapsed += (sender, e) => UpdateTimer(ref _blackTime);
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
        return _isWhiteTurn ? _whiteTime : _blackTime;
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
            Console.WriteLine($"{(_isWhiteTurn ? "White" : "Black")} time is up!");
        }
    }

    // Debugging output for remaining time
    public void PrintRemainingTime()
    {
        string blackTimerText = "Thời gian còn lại của Rừng sâu: ";
        string whiteTimerText = "Thời gian còn lại của Vương quốc: ";

        Console.SetCursorPosition(blackTimerText.Length, 1);
        Console.WriteLine(_blackTime);
        
        Console.SetCursorPosition(whiteTimerText.Length, 14);
        Console.WriteLine(_whiteTime);
    }
}
