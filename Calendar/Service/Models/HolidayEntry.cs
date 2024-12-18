namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models;

public class HolidayEntry
{
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public bool IsToday { get; set; }
    public Duration DurationUntil { get; set; }

    public HolidayEntry(CalendarEntry calendarEntry, TimeZoneInfo timeZoneInfo)
    {
        Name = calendarEntry.Summary;
        Date = TimeZoneInfo.ConvertTime(calendarEntry.Start, timeZoneInfo).Subtract(timeZoneInfo.GetUtcOffset(calendarEntry.Start));
        IsToday = Date.Date == DateTimeOffset.UtcNow.Date;
        DurationUntil = new Duration(Date);
    }
}