namespace PaymentService.Builder;

public class TimeOverlapException : Exception
{
    public TimeOverlapException(TimeOnly startTime, TimeOnly stopTime) :
        base(
            $"Start Time can't be after stop time,Start Time:({startTime.ToString("HH:mm:ss")}), StopTime:({stopTime.ToString("HH:mm:ssA")})")
    {
    }
}