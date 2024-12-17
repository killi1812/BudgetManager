namespace Data.Enums;

public enum Frequency
{
    Once,
    Daily,
    Weakly,
    Monthly,
    Yearly,
    
}

public class FrequencyHelper
{
    public static Frequency Parse(string text)
    {
        return text.ToLower() switch
        {
            "once" => Frequency.Once,
            "daily" => Frequency.Daily,
            "weakly" => Frequency.Weakly,
            "Monthly" => Frequency.Monthly,
            "Yearly" => Frequency.Yearly,
            _ => throw new Exception($"Frequency not supported {text}")
        };
    }
}
