using ChrisKaczor.HomeMonitor.Calendar.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Controllers;

[Route("calendar")]
[ApiController]
public class CalendarController(IConfiguration configuration, HttpClient httpClient) : ControllerBase
{
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<CalendarEntry>>> GetUpcoming([FromQuery] int? days)
    {
        var data = await httpClient.GetStringAsync(configuration["Calendar:PersonalUrl"]);

        var calendar = Ical.Net.Calendar.Load(data);

        var start = DateTimeOffset.Now.Date;
        var end = start.AddDays(days ?? 1);

        var calendarEntries = calendar
            .GetOccurrences(start, end)
            .Select(o => new CalendarEntry(o))
            .OrderBy(ce => ce.Start);

        return Ok(calendarEntries);
    }
}