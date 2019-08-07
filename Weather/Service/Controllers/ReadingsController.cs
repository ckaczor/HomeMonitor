using ChrisKaczor.HomeMonitor.Weather.Models;
using ChrisKaczor.HomeMonitor.Weather.Service.Data;
using Microsoft.AspNetCore.Mvc;
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
    }
}