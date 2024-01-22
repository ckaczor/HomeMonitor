using ChrisKaczor.HomeMonitor.Power.Service.Data;
using ChrisKaczor.HomeMonitor.Power.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChrisKaczor.HomeMonitor.Power.Service.Controllers;

[Route("[controller]")]
[ApiController]
public class StatusController(Database database) : ControllerBase
{
    [HttpGet("recent")]
    public async Task<ActionResult<PowerStatus>> GetRecent()
    {
        return await database.GetRecentStatus();
    }

    [HttpGet("history-grouped")]
    public async Task<ActionResult<List<PowerStatusGrouped>>> GetHistoryGrouped(DateTimeOffset start, DateTimeOffset end, int bucketMinutes = 2)
    {
        return (await database.GetStatusHistoryGrouped(start, end, bucketMinutes)).ToList();
    }
}