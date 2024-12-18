namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models;

public class Duration
{
    public int Days { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }

    public Duration(DateTimeOffset date)
    {
        var now = DateTimeOffset.Now;
        var duration = date - now;
        Days = duration.Days;
        Hours = duration.Hours;
        Minutes = duration.Minutes;
        Seconds = duration.Seconds;
    }
}