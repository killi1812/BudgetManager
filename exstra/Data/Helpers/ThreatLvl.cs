namespace Data.Helpers;

public enum ThreatLvl
{
    Low = 1,
    Medium = 2,
    High = 3
}

public static class ThreatLvlHelper
{
    public static ThreatLvl ToThreatLvl(int lvl)
    {
        return lvl switch
        {
            1 => ThreatLvl.Low,
            2 => ThreatLvl.Medium,
            3 => ThreatLvl.High,
            _ => ThreatLvl.Low
        };
    }

    public static int ToInt(this ThreatLvl lvl)
    {
        return lvl switch
        {
            ThreatLvl.Low => 1,
            ThreatLvl.Medium => 2,
            ThreatLvl.High => 3,
            _ => 0
        };
    }
}