using ChrisKaczor.HomeMonitor.Calendar.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Controllers;

[Route("events")]
[ApiController]
public class HolidayController(IConfiguration configuration, HttpClient httpClient) : ControllerBase
{
    [HttpGet("next")]
    public async Task<ActionResult<IEnumerable<CalendarEntry>>> GetNext([FromQuery] string timezone = "Etc/UTC")
    {
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);

        var data = await httpClient.GetStringAsync(configuration["Calendar:HolidayUrl"]);

        var calendar = Ical.Net.Calendar.Load(data);

        var start = DateTimeOffset.Now.Date;
        var end = start.AddYears(1);

        var calendarEntries = calendar
            .GetOccurrences(start, end)
            .Select(o => new CalendarEntry(o))
            .OrderBy(ce => ce.Start);

        var nextCalendarEntry = calendarEntries.First();

        var holidayEntry = new HolidayEntry(nextCalendarEntry, timeZoneInfo);

        var holidayResponse = new HolidayResponse(holidayEntry, timeZoneInfo);

        return Ok(holidayResponse);
    }
}