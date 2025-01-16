namespace PaymentService;

public class PaymentService
{
    private readonly TimeOnly _startTime;
    private readonly TimeOnly _stopTime;
    private readonly TimeSpan _interval;

    public PaymentService(TimeSpan interval, TimeOnly startTime, TimeOnly stopTime)
    {
        _interval = interval;
        _stopTime = stopTime;
        _startTime = startTime;
    }

    public void Run()
    {
        Thread runThread = new Thread(Start)
        {
            Name = "PaymentServiceTread",
            IsBackground = true,
        };
        runThread.Start();
    }

    private static TimeSpan TimeUntilStart(TimeOnly time)
    {
        var now = TimeOnly.Parse(DateTime.Now.ToString("hh:mm:ss"));
        if (now < time)
            return TimeSpan.Zero;
        var diff = time - now;
        return diff;
    }

    private void Start()
    {
        Thread.Sleep(TimeUntilStart(_startTime));
        var work = 0;
        while (true)
        {
            Thread.Sleep(_interval);
            work++;
            Console.WriteLine(work);
        }
    }
}