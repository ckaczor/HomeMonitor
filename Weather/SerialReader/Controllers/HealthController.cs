using Microsoft.AspNetCore.Mvc;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.SerialReader.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly TimeSpan _checkTimeSpan = TimeSpan.FromSeconds(5);

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            return SerialReader.BoardStarted ? Ok() : Conflict();
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            var lastReading = SerialReader.LastReading;
            var timeSinceLastReading = DateTimeOffset.UtcNow - lastReading;

            return timeSinceLastReading <= _checkTimeSpan ? Ok(lastReading) : BadRequest(lastReading);
        }
    }
}
