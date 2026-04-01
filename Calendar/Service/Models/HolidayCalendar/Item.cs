using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.HolidayCalendar;

[PublicAPI]
public class Item
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Excerpt { get; set; }
    public required string Url { get; set; }
    public required string Type { get; set; }
}