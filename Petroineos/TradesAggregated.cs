namespace Petroineos;

public class TradesAggregated
{
    public string LocalTime { get; private set; }
    public double Volume { get; private set; }

    public TradesAggregated(string localTime)
    {
        LocalTime = localTime;
    }

    public void AddVolume(double volume)
    {
        Volume += volume;
    }
}