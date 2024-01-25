using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using ChrisKaczor.HomeMonitor.Weather.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Controllers;

[Route("[controller]")]
[ApiController]
public class ReadingsController(Database database) : ControllerBase
{
    [HttpGet("recent")]
    public async Task<ActionResult<WeatherReading>> GetRecent([FromQuery] string tz)
    {
        var recentReading = await database.GetRecentReading();

        if (string.IsNullOrWhiteSpace(tz))
            return recentReading;

        try
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(tz);

            recentReading.Timestamp = recentReading.Timestamp.ToOffset(timeZoneInfo.GetUtcOffset(recentReading.Timestamp));
            recentReading.GpsTimestamp = recentReading.GpsTimestamp.ToOffset(timeZoneInfo.GetUtcOffset(recentReading.GpsTimestamp));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return recentReading;
    }

    [HttpGet("history")]
    public async Task<ActionResult<List<WeatherReading>>> GetHistory(DateTimeOffset start, DateTimeOffset end)
    {
        return (await database.GetReadingHistory(start, end)).ToList();
    }

    [HttpGet("aggregate")]
    public async Task<ActionResult<WeatherAggregate>> GetHistoryAggregate(DateTimeOffset start, DateTimeOffset end)
    {
        var readings = (await database.GetReadingHistory(start, end)).ToList();

        return readings.Any() ? new WeatherAggregate(readings) : null;
    }

    [HttpGet("value-history")]
    public async Task<ActionResult<List<WeatherValue>>> GetValueHistory(WeatherValueType weatherValueType, DateTimeOffset start, DateTimeOffset end)
    {
        return (await database.GetReadingValueHistory(weatherValueType, start, end)).ToList();
    }

    [HttpGet("value-history-grouped")]
    public async Task<ActionResult<List<WeatherValueGrouped>>> GetValueHistoryGrouped(WeatherValueType weatherValueType, DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 2)
    {
        return (await database.GetReadingValueHistoryGrouped(weatherValueType, start, end, bucketMinutes)).ToList();
    }

    [HttpGet("history-grouped")]
    public async Task<ActionResult<List<WeatherReadingGrouped>>> GetHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 2)
    {
        return (await database.GetReadingHistoryGrouped(start, end, bucketMinutes)).ToList();
    }

    [HttpGet("wind-history-grouped")]
    public async Task<ActionResult<List<WindHistoryGrouped>>> GetWindHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 60)
    {
        return (await database.GetWindHistoryGrouped(start, end, bucketMinutes)).ToList();
    }
}