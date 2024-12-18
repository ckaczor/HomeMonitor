using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models;

[PublicAPI]
public class CalendarEntry(Occurrence occurrence)
{
    public string Summary { get; set; } = ((CalendarEvent)occurrence.Source).Summary;
    public DateTimeOffset Start { get; set; } = occurrence.Period.StartTime.AsUtc;
    public DateTimeOffset End { get; set; } = occurrence.Period.EndTime.AsUtc;
}