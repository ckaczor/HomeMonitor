using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using ChrisKaczor.HomeMonitor.Weather.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReadingsController : ControllerBase
    {
        private readonly Database _database;

        public ReadingsController(Database database)
        {
            _database = database;
        }

        [HttpGet("recent")]
        public async Task<ActionResult<WeatherReading>> GetRecent()
        {
            return await _database.GetRecentReading();
        }

        [HttpGet("history")]
        public async Task<ActionResult<List<WeatherReading>>> GetHistory(DateTimeOffset start, DateTimeOffset end)
        {
            return (await _database.GetReadingHistory(start, end)).ToList();
        }

        [HttpGet("value-history")]
        public async Task<ActionResult<List<WeatherValue>>> GetValueHistory(WeatherValueType weatherValueType, DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 2)
        {
            return (await _database.GetReadingValueHistory(weatherValueType, start, end, bucketMinutes)).ToList();
        }

        [HttpGet("history-grouped")]
        public async Task<ActionResult<List<WeatherReadingGrouped>>> GetHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 2)
        {
            return (await _database.GetReadingHistoryGrouped(start, end, bucketMinutes)).ToList();
        }
    }
}