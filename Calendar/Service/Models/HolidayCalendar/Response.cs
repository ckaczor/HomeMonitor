using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.HolidayCalendar;

[PublicAPI]
public class Response
{
    public bool Success { get; set; }
    public Data Data { get; set; } = new();
}