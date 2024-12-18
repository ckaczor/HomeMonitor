using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models;

[PublicAPI]
public class HolidayResponse(HolidayEntry holidayEntry, TimeZoneInfo timeZoneInfo)
{
    public DateTimeOffset ResponseTime { get; set; } = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZoneInfo);
    public HolidayEntry? Event { get; set; } = holidayEntry;
}