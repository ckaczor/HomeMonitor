using ChrisKaczor.HomeMonitor.Calendar.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Controllers;

[Route("calendar")]
[ApiController]
public class CalendarController(IConfiguration configuration, HttpClient httpClient) : ControllerBase
{
    private async Task<IEnumerable<CalendarEntry>> GetCalendarEntries(string calendarUrl, int days, bool isHoliday)
    {
        var data = await httpClient.GetStringAsync(calendarUrl);

        var calendar = Ical.Net.Calendar.Load(data);

        var start = DateTimeOffset.Now.Date;
        var end = start.AddDays(days);

        var calendarEntries = calendar
            .GetOccurrences(start, end)
            .Select(o => new CalendarEntry(o, isHoliday))
            .OrderBy(ce => ce.Start);

        return calendarEntries;
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<CalendarEntry>>> GetUpcoming([FromQuery] int days = 1, [FromQuery] bool includeHolidays = false)
    {
        var calendarEntries = await GetCalendarEntries(configuration["Calendar:PersonalUrl"]!, days, false);

        if (!includeHolidays) 
            return Ok(calendarEntries);

        var holidayEntries = await GetCalendarEntries(configuration["Calendar:HolidayUrl"]!, days, true);
        calendarEntries = calendarEntries.Concat(holidayEntries).OrderBy(c => c.Start);

        return Ok(calendarEntries);
    }
}