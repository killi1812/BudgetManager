using PaymentService.Exceptions;

namespace PaymentService.Builder;

public interface IPaymentServiceBuilder
{
    public IPaymentServiceBuilder WithCheckInterval(int interval);
    public IPaymentServiceBuilder WithStartTime(TimeOnly startTime);
    public IPaymentServiceBuilder WithStopTime(TimeOnly stopTime);
    public PaymentService Build();
}

public class PaymentServiceBuilder : IPaymentServiceBuilder
{
    private TimeOnly? _startTime = null;
    private TimeOnly? _stopTime = null;
    private TimeSpan? _interval = null;

    public IPaymentServiceBuilder WithCheckInterval(int interval)
    {
        if (interval <= 0)
            throw new BadIntervalException(interval);

        _interval = TimeSpan.FromMinutes(interval);
        return this;
    }

    public IPaymentServiceBuilder WithCheckInterval(TimeSpan interval)
    {
        _interval = interval;
        return this;
    }
    public IPaymentServiceBuilder WithStartTime(TimeOnly startTime)
    {
        if (_stopTime < startTime)
            throw new TimeOverlapException(startTime, _stopTime.Value);

        _startTime = startTime;
        return this;
    }

    public IPaymentServiceBuilder WithStopTime(TimeOnly stopTime)
    {
        if (_startTime > stopTime)
            throw new TimeOverlapException(_startTime.Value, stopTime);

        _stopTime = stopTime;
        return this;
    }

    public PaymentService Build()
    {
        return new PaymentService(
            _interval ?? TimeSpan.FromMinutes(60),
            _startTime ?? new TimeOnly(0, 1),
            _stopTime ?? new TimeOnly(23, 59));
    }
}