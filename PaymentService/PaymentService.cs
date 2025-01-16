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

    private void Start()
    {
        int work = 0;
        while (true)
        {
            Thread.Sleep(_interval);
            work++;
        }
    }
}