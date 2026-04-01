using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.HolidayCalendar;

[PublicAPI]
public class Data
{
    public IEnumerable<Item> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int TotalPages { get; set; }
}