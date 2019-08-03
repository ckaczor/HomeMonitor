using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}