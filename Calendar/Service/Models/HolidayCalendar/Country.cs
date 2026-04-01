using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.HolidayCalendar;

[PublicAPI]
public class Country
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}