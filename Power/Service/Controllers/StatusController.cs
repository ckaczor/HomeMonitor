using ChrisKaczor.HomeMonitor.Power.Service.Data;
using ChrisKaczor.HomeMonitor.Power.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Power.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly Database _database;

        public StatusController(Database database)
        {
            _database = database;
        }

        [HttpGet("history-grouped")]
        public async Task<ActionResult<List<PowerStatusGrouped>>> GetHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 2)
        {
            return (await _database.GetStatusHistoryGrouped(start, end, bucketMinutes)).ToList();
        }
    }
}