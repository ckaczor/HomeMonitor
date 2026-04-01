using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models;

[PublicAPI]
public class NationalDay
{
    public required string Name { get; set; }
    public required string Url { get; set; }
    public required string Excerpt { get; set; }
    public required string Type { get; set; }

    [SetsRequiredMembers]
    public NationalDay(HolidayCalendar.Item item)
    {
        Name = item.Name;
        Url = item.Url;
        Excerpt = item.Excerpt;
        Type = item.Type;
    }

    [SetsRequiredMembers]
    public NationalDay(DaysOfTheYear.Entry entry)
    {
        Name = entry.Name;
        Url = entry.Url;
        Excerpt = entry.Excerpt;
        Type = entry.Type;
    }
}