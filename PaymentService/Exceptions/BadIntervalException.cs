namespace PaymentService.Exceptions;

public class BadIntervalException : Exception
{
    public BadIntervalException(int interval) : base($"Interval cant be zero or less, current interval {interval}")
    {
        
    }
}