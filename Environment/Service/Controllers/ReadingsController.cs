using ChrisKaczor.HomeMonitor.Environment.Service.Data;
using ChrisKaczor.HomeMonitor.Environment.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Controllers;

[Route("[controller]")]
[ApiController]
public class ReadingsController(Database database) : ControllerBase
{
    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<Readings>>> GetRecent([FromQuery] string? tz)
    {
        var recentReadings = await database.GetRecentReadings();

        if (string.IsNullOrWhiteSpace(tz))
            return Ok(recentReadings);

        try
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tz);

            foreach (var recentReading in recentReadings)
            {
                recentReading.Time = recentReading.Time.ToOffset(timeZoneInfo.GetUtcOffset(recentReading.Time));
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok(recentReadings);
    }

    [HttpGet("history-grouped")]
    public async Task<ActionResult<List<ReadingsGrouped>>> GetHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 5)
    {
        return (await database.GetReadingsHistoryGrouped(start, end, bucketMinutes)).ToList();
    }

    [HttpGet("aggregate")]
    public async Task<ActionResult<List<ReadingsAggregate>>> GetAggregate(DateTimeOffset start, DateTimeOffset end)
    {
        return (await database.GetReadingsAggregate(start, end)).ToList();
    }
}